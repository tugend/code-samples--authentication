using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecureWebApi.Infrastructure.Authentication.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly int _leastClearanceLevelRequired;

        public JwtAuthorize(int leastClearanceLevelRequired) => _leastClearanceLevelRequired = leastClearanceLevelRequired;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User) context.HttpContext.Items["User"];
            if (user != null && user.ClearanceLevel >= _leastClearanceLevelRequired) return; 
            
            // not logged in
            var result = new {message = "Unauthorized"};
            context.Result = new JsonResult(result)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}