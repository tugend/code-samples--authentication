using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecureWebApi.Services.Contracts;

namespace SecureWebApi.Infrastructure.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock) => _userService = userService;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return AuthenticateResult.Fail("Missing authorization header");
            }
            
            if (!TryParseAuthHeader(authHeader, out var credentials))
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (!await _userService.TryAuthenticate(credentials.username, credentials.password, out var user))
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            Context.Items["User"] = user;
            
            var ticket = CreateTicket(Scheme.Name, user.Id, user.Username, user.ClearanceLevel);
            return AuthenticateResult.Success(ticket);
        }

        private static bool TryParseAuthHeader(string unparsedAuthHeader, out (string username, string password) credentials)
        {
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(unparsedAuthHeader);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentialsArray = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                credentials = (username: credentialsArray[0], password: credentialsArray[1]);

                return true;
            }
            catch
            {
                credentials = ("", "");
                return false;
            }
        } 
        
        private static AuthenticationTicket CreateTicket(string authenticationSchemeName, string userid, string username, int clearanceLevel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userid),
                new Claim(ClaimTypes.Name, username)
            };

            for(var level = 0; level <= clearanceLevel; level++)
            {
                claims.Add(new Claim(CustomClaimTypes.ClearanceLevel, level.ToString()));
            }
            
            var identity = new ClaimsIdentity(claims, authenticationSchemeName);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, authenticationSchemeName);
        }
    }
}