using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Endpoints.Users.Restricted
{
    [ApiExplorerSettings(GroupName = RestrictedApiGroupName)]
    [ApiController]
    [Route("users/restricted/basic")]
    public class BasicAuthenticatedController : ControllerBase
    {
        [Authorize]
        [HttpGet("level-0-access")]
        public string LittleSecret() => "A little secret, we just need to know who knows.";

        [Authorize(Policy = "SecurityClearance+1")]
        [HttpGet("level-1-access")]
        public string MajorSecret() => "A common secret, requires clearance level 1.";

        [Authorize(Policy = "SecurityClearance+7")]
        [HttpGet("level-7-access")]
        public string TopSecret() => "A top secret secret, restricted to only the highest clearance level.";
    }
}