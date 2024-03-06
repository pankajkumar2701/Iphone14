using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Iphone14.Models;
using Iphone14.Services;
using Iphone14.Entities;
using System.Text;

namespace Iphone14.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication and authorization.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for user registration, login, and token refreshing.
    /// </remarks>
    [AllowAnonymous]
    [Route("api/user-account")]
    public class UserAccountController(IUserAuthenticationService _userAuthenticationService) : Controller
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model">User registration information.</param>
        /// <returns>Returns the user ID upon successful registration.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                    return BadRequest("Password and confirm-password does not match");
                // Check if the email is unique (you might want to add more validation)
                if (_userAuthenticationService.UserExist(model.TenantId, model.UserName, model.Email) == false)
                {
                    // Hash the password and save the user to the database
                    var (passwordHash, salt) = HashPassword(model.Password);
                    User user = new User
                    {
                        Id = new Guid(),
                        UserName = model.UserName,
                        EmailId = model.Email,
                        PasswordHash = passwordHash,
                        Saltkey = salt,
                        TenantId = model.TenantId,
                        Name = model.Name
                    };
                    _userAuthenticationService.CreateUser(user);
                    return Ok(new { user.Id });
                }
                else
                {
                    return BadRequest("Email or userName is already taken.");
                }
            }

            return BadRequest("Invalid model data.");
        }

        /// <summary>
        /// Perform user login.
        /// </summary>
        /// <param name="model">User login credentials.</param>
        /// <returns>Returns JWT and refresh token upon successful login.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userAuthenticationService.GetUser(null, model.UserName);
                if (user != null && ValidatePassword(model.Password, user.PasswordHash, user.Saltkey))
                {
                    //role data
                    var data = new List<ClaimRoleModel>();
                    if (!user.IsSuperAdmin)
                    {
                        data = _userAuthenticationService.GetUserACL(user.TenantId, user.Id);
                    }
                    // Password is valid, perform login logic (you might want to use a cookie or JWT)
                    Claim[] claims = new Claim[]
                    {
                        new ("name", user.Name??string.Empty),
                        new ("userName", user.UserName),
                        new ("userId", user.Id.ToString()),
                        new ("email", user.EmailId ?? string.Empty),
                        new ("tenantId", user?.TenantId.ToString()??string.Empty),
                        new ("isSuperAdmin", Convert.ToString(user.IsSuperAdmin) ?? "false"),
                        new ("userInRole", JsonConvert.SerializeObject(data))
                    };
                    var token = GenerateJSONWebToken(claims, AppSetting.TokenExpirationtime);
                    var key = generateSecretKey();
                    Claim[] newClaims = claims.Append(new Claim("secretKey", key)).ToArray();
                    var reftoken = GenerateJSONWebToken(newClaims, AppSetting.TokenExpirationtime  * 2);
                    _userAuthenticationService.CreateRefreshTokenByUser(user.TenantId, user.Id, key);
                    return Ok(new{
                        token,
                        refreshtoken = reftoken,
                        tokenType = "Bearer"
                    });
                }

                return Unauthorized("Invalid login attempt.");
            }

            return BadRequest("Invalid model.");
        }

        /// <summary>
        /// Refresh the authentication token.
        /// </summary>
        /// <param name="refreshtoken">Refresh token information.</param>
        /// <returns>Returns new JWT and refresh token upon successful token refresh.</returns>
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] Token refreshtoken)
        {
            IActionResult response = Unauthorized();
            var handler = new JwtSecurityTokenHandler();
            var usertoken = handler.ReadToken(refreshtoken.RefreshToken) as JwtSecurityToken;
            var userid = usertoken.Claims.First(claim => claim.Type == "userId").Value;
            var tenantid = usertoken.Claims.First(claim => claim.Type == "tenantId").Value;
            var tokenstring = usertoken.Claims.First(claim => claim.Type == "secretKey").Value;
            var isSuperAdmin = usertoken.Claims.First(claim => claim.Type == "isSuperAdmin").Value;
            var userInRole = usertoken.Claims.First(claim => claim.Type == "userInRole").Value;
            var tokenId = ValidateRefreshToken(usertoken, tokenstring, Guid.Parse(tenantid), Guid.Parse(userid));
            if (tokenId != Guid.Empty)
            {
                var user = _userAuthenticationService.GetUser(Guid.Parse(userid), string.Empty);
                Claim[] claims = new Claim[]
                {
                    new ("name", user.Name??string.Empty),
                    new ("userName", user.UserName),
                    new ("userId", user.Id.ToString()),
                    new ("email", user.EmailId ?? string.Empty),
                    new ("tenantId", user?.TenantId.ToString()??string.Empty),
                    new ("isSuperAdmin", isSuperAdmin),
                    new ("userInRole",userInRole)
                };
                var token = GenerateJSONWebToken(claims, AppSetting.TokenExpirationtime);
                var key = generateSecretKey();
                Claim[] newClaims = claims.Append(new Claim("secretKey", key)).ToArray();
                var reftoken = GenerateJSONWebToken(newClaims, AppSetting.TokenExpirationtime  * 2);
                _userAuthenticationService.UpdateRefreshTokenByUser(user.TenantId, tokenId, user.Id, key);
                if (claims != null)
                {
                    return Ok(new{
                        token,
                        refreshtoken = reftoken,
                        tokenType = "Bearer"
                    });
                }
            }

            return response;
        }

        private Guid ValidateRefreshToken(JwtSecurityToken token, string tokenstring, Guid tenantid, Guid userid)
        {
            var userTokenId = _userAuthenticationService.RefreshTokenExist(tenantid, tokenstring, userid);
            if (userTokenId == null || userTokenId == Guid.Empty)
            {
                throw new SecurityTokenException("Invalid token!");
            }

            var expDate = token.ValidTo;
            if (expDate < DateTime.UtcNow.AddMinutes(1))
            {
                throw new SecurityTokenException("Invalid token!");
            }

            return (Guid)userTokenId;
        }

        static (string, string) HashPassword(string password)
        {
            var saltBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);
            var hashedBytes = SHA256.HashData(saltedPassword);
            return (Convert.ToBase64String(hashedBytes), Convert.ToBase64String(saltBytes));
        }

        static bool ValidatePassword(string password, string storedHash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);
            byte[] hashedBytes = SHA256.HashData(saltedPassword);
            string computedHash = Convert.ToBase64String(hashedBytes);
            return storedHash == computedHash;
        }

        private readonly static string jwtKey = !string.IsNullOrEmpty(AppSetting.JwtKey) ? AppSetting.JwtKey : string.Empty;

        protected static string GenerateJSONWebToken(Claim[] claims, int expiryTime)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(expiryTime);
            var token = new JwtSecurityToken(AppSetting.JwtIssuer, AppSetting.JwtIssuer, claims,  expires: expirationTime.ToUniversalTime(),  signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string generateSecretKey()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}