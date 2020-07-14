using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureWebApi.Endpoints.Users.Authenticate.JsonWebTokens;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Services.Contracts;
using SecureWebApi.Tests.TestHelpers.Extensions;
using Xunit;
using static System.Net.Mime.MediaTypeNames.Application;
using static System.Text.Encoding;

namespace SecureWebApi.Tests.Endpoints.Users.Authenticate.JsonWebTokens
{
    public class AuthenticateControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private readonly User _registeredUser;

        public AuthenticateControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _registeredUser = TestUserFactory.CreateUser();

            _client = fixture
                .WithWebHostBuilder(builder => builder
                    .ConfigureTestServices(services => services
                        .RemoveAll<IUserService>()
                        .AddScoped<IUserService>(_ => new AuthenticatedUserService(_registeredUser))))
                .CreateClient();
        }

        [Fact]
        public async Task Should_Reject_Given_Invalid_Password()
        {
            var jsonStringContent = ToJsonContent(
                new Request(
                    username: _registeredUser.Username, 
                    password: "wrong password"));

            await _client
                .PostAsync("/users/authenticate/jwt", jsonStringContent)
                .AssertStatusCode(HttpStatusCode.BadRequest)
                .AssertContent("Username or password is incorrect");
        }

        [Fact]
        public async Task Should_Receive_A_Jwt_Token_Given_Valid_Password()
        {
            var jsonStringContent = ToJsonContent(
                new Request(
                    username: _registeredUser.Username, 
                    password: _registeredUser.Password));

            var response = await _client
                .PostAsync("/users/authenticate/jwt", jsonStringContent)
                .AssertStatusCode(HttpStatusCode.OK)
                .Deserialize<Response>();

            Assert.NotNull(response.Token);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) jwtSecurityTokenHandler.ReadToken(response.Token);
            jsonToken.Subject.Should().Be(_registeredUser.Id);
        }

        private static StringContent ToJsonContent(Request content)
        {
            var serializedContent = JsonSerializer.Serialize(content);
            return new StringContent(serializedContent, UTF8, Json);
        }
    }
}