using Microsoft.AspNetCore.Mvc;
using SecureWebApi.Infrastructure.Authentication.Jwt;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Endpoints.Users.Restricted
{
    [ApiExplorerSettings(GroupName = RestrictedApiGroupName)]
    [ApiController]
    [Route("users/restricted/jwt+basic")]
    public class JwtAndBasicAuthenticatedController : ControllerBase
    {
        [JwtAuthorize(1)]
        [HttpGet("level-1-access")]
        public string JwtOrBasicTopTopSecret() => "Another common secret, requires clearance level 1 and jwt OR basic authentication";
    }
}