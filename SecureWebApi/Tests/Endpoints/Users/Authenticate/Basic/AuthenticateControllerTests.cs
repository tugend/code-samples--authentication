using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureWebApi.Endpoints.Users.Authenticate.Basic;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Services.Contracts;
using SecureWebApi.Tests.TestHelpers.Extensions;
using Xunit;
using static System.Net.Mime.MediaTypeNames.Application;
using static System.Text.Encoding;

namespace SecureWebApi.Tests.Endpoints.Users.Authenticate.Basic
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
        public async Task Should_Reject_Wrong_Password()
        {
            var jsonStringContent = ToJsonContent(new Request(_registeredUser.Username, "wrong password"));

            await _client
                .PostAsync("/users/authenticate/basic", jsonStringContent)
                .AssertStatusCode(HttpStatusCode.BadRequest)
                .AssertContent("Username or password is incorrect");
        }

        [Fact]
        public async Task Should_Accept_Correct_Password()
        {
            var jsonStringContent = ToJsonContent(new Request(_registeredUser.Username, _registeredUser.Password));

            await _client
                .PostAsync("/users/authenticate/basic", jsonStringContent)
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertContent(new {UserId = _registeredUser.Id, _registeredUser.Username});
        }

        private static StringContent ToJsonContent(Request content)
        {
            var serializedContent = JsonSerializer.Serialize(content);
            return new StringContent(serializedContent, UTF8, Json);
        }
    }
}