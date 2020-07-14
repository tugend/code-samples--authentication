using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureWebApi.Services.Contracts;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Endpoints.Users.Authenticate.Basic
{
    [ApiExplorerSettings(GroupName = RestrictedApiGroupName)]
    [ApiController]
    [Route("users/authenticate/basic")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Verify the given password is correct.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Authenticate(Request request)
        {
            if (!await _userService.TryAuthenticate(request.Username, request.Password, out var user))
            {
                return new BadRequestObjectResult("Username or password is incorrect");
            }

            var response = new Response(userId: user.Id, username: user.Username);
            return Ok(response);
        }
    }
}