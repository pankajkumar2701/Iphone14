namespace Iphone14.Services
{
    using System;
    using Iphone14.Data;
    using Iphone14.Entities;
    using Iphone14.Models;

    public interface IUserAuthenticationService
    {
        bool CreateUser(User model);
        User? GetUser(Guid? id, string username);
        bool UserExist(Guid tenantId, string username, string emailId);
        bool CreateRefreshTokenByUser(Guid tenantId, Guid userId, string token);
        bool UpdateRefreshTokenByUser(Guid tenantId, Guid refreshTokenId, Guid userId, string token);
        Guid? RefreshTokenExist(Guid tenantId, string token, Guid userId);
        List<ClaimRoleModel> GetUserACL(Guid tenantId, Guid userId);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private Iphone14Context _dbContext;
        public UserAuthenticationService(Iphone14Context dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateUser(User model)
        {
            if (model != null)
            {
                model.CreatedOn = DateTime.UtcNow;
                model.UpdatedOn = DateTime.UtcNow;
                _dbContext.User.Add(model);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public User? GetUser(Guid? id, string username)
        {
            if (id == null && !string.IsNullOrEmpty(username))
                return _dbContext.User.FirstOrDefault(u => u.UserName == username);
            return _dbContext.User.FirstOrDefault(u => u.Id == id);
        }

        public bool UserExist(Guid tenantId, string username, string emailId)
        {
            return _dbContext.User.Any(u => u.EmailId == emailId || u.UserName == username && u.TenantId == tenantId);
        }

        public bool UpdateRefreshTokenByUser(Guid tenantId, Guid refreshTokenId, Guid userId, string token)
        {
            UserToken model = new UserToken
            {
                UserId = userId,
                TenantId = tenantId,
                RefershToken = token,
                CreatedOn = DateTime.UtcNow
            };
            _dbContext.UserToken.Update(model);
            _dbContext.SaveChanges();
            return true;
        }

        public bool CreateRefreshTokenByUser(Guid tenantId, Guid userId, string token)
        {
            UserToken model = new UserToken
            {
                Id = new Guid(),
                UserId = userId,
                TenantId = tenantId,
                RefershToken = token,
                CreatedOn = DateTime.UtcNow
            };
            _dbContext.UserToken.Add(model);
            _dbContext.SaveChanges();
            return true;
        }

        public Guid? RefreshTokenExist(Guid tenantId, string token, Guid userId)
        {
            return _dbContext.UserToken.FirstOrDefault(u => u.TenantId == tenantId && u.RefershToken == token && u.UserId == userId)?.Id;
        }

        public List<ClaimRoleModel> GetUserACL(Guid tenantId, Guid userId)
        {
            List<ClaimRoleModel> data = new List<ClaimRoleModel>();
            var roles = _dbContext.UserInRole.Where(w => w.TenantId == tenantId && w.UserId == userId).ToList();
            if (roles.Count != 0)
                data = _dbContext.Entity.Where(e => e.TenantId == tenantId).Select(entity => new ClaimRoleModel { EntityName = entity.Name, Action = _dbContext.RoleEntitlement.Where(re => re.TenantId == tenantId && re.EntityId == entity.Id && roles.Select(s => s.RoleId).Contains(re.RoleId)).Select(re => re.Entitlement).ToList() }).ToList();
            return data;
        }
    }
}