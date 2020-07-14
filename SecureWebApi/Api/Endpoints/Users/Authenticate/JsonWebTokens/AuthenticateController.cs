using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecureWebApi.Infrastructure;
using SecureWebApi.Infrastructure.Authentication.Jwt;
using SecureWebApi.Services.Contracts;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Endpoints.Users.Authenticate.JsonWebTokens
{
    [ApiExplorerSettings(GroupName = RestrictedApiGroupName)]
    [ApiController]
    [Route("users/authenticate/jwt")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly string _secretKey;

        public AuthenticateController(IUserService userService, IOptions<AppSettings> settings)
        {
            _userService = userService;
            _secretKey = settings.Value.Secret;
        }

        [HttpPost]
        public async Task<IActionResult> GetJwtToken(Request request)
        {
            if (!await _userService.TryAuthenticate(request.Username, request.Password, out var user))
            {
                return new BadRequestObjectResult("Username or password is incorrect");
            }

            var token = JwtTokenGenerator.GenerateJwtToken(user.Id, _secretKey);
            var response = new Response(user.Id, user.Username, token);
            return Ok(response);
        }
    }
}