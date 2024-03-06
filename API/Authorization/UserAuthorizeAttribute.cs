using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Iphone14.Models;
using Newtonsoft.Json;

namespace Iphone14.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _entity;
        private readonly Entitlements _action;
        public UserAuthorizeAttribute(string entity, Entitlements action)
        {
            _entity = entity;
            _action = action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Guid.TryParse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId);
            bool.TryParse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "isSuperAdmin")?.Value, out bool isSuperAdmin);
            if (isSuperAdmin)
                return;
            var claimUserRole = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userInRole")?.Value;
            if (claimUserRole == null)
            {
                context.Result = new JsonResult(new { message = "You are not authorized to access" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            var roleObj = JsonConvert.DeserializeObject<List<ClaimRoleModel>>(claimUserRole);
            string entity = roleObj.FirstOrDefault(f => f.EntityName.ToLower() == _entity.ToLower())?.EntityName;
            var actions = roleObj.Where(f => f.EntityName.ToLower() == _entity.ToLower() && f.Action.Contains((int)_action)).FirstOrDefault();
            if (userId == Guid.Empty || string.IsNullOrEmpty(entity) || actions == null)
            {
                context.Result = new JsonResult(new { message = "You are not authorized to access" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            } return ; 

        }
    }
}