using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Services.Contracts;
using SecureWebApi.Tests.TestHelpers.Builders;
using SecureWebApi.Tests.TestHelpers.Extensions;
using Xunit;


namespace SecureWebApi.Tests.Endpoints.Users.Restricted
{
    public class BasicAuthenticationAccessTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private static readonly User Level0User = TestUserFactory.CreateUser(0);
        private static readonly User Level1User = TestUserFactory.CreateUser(1);
        private static readonly User Level7User = TestUserFactory.CreateUser(7);
        private static readonly User[] RegisteredUsers = {Level0User, Level1User, Level7User};

        public BasicAuthenticationAccessTests(WebApplicationFactory<Startup> fixture) => _client = fixture
            .WithWebHostBuilder(builder => builder
                .ConfigureTestServices(services => services
                    .RemoveAll<IUserService>()
                    .AddScoped<IUserService>(_ => new AuthenticatedUserService(RegisteredUsers))))
            .CreateClient();

        [Theory]
        [InlineData("/users/restricted/basic/level-0-access")]
        [InlineData("/users/restricted/basic/level-1-access")]
        [InlineData("/users/restricted/basic/level-7-access")]
        public async Task UnauthorizedUser_Should_Not_Have_Access_To_Anything_Restricted(string path)
        {
            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, path)
                .Build();

            await _client
                .SendAsync(message)
                .AssertStatusCode(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(0, "/users/restricted/basic/level-0-access", true)]
        [InlineData(0, "/users/restricted/basic/level-1-access", false)]
        [InlineData(0, "/users/restricted/basic/level-7-access", false)]
        [InlineData(1, "/users/restricted/basic/level-0-access", true)]
        [InlineData(1, "/users/restricted/basic/level-1-access", true)]
        [InlineData(1, "/users/restricted/basic/level-7-access", false)]
        [InlineData(7, "/users/restricted/basic/level-0-access", true)]
        [InlineData(7, "/users/restricted/basic/level-1-access", true)]
        [InlineData(7, "/users/restricted/basic/level-7-access", true)]
        public async Task LevelX_ClearanceUser_Should_Have_Access_To_(int level, string path, bool shouldHaveAccess)
        {
            var user = RegisteredUsers.Single(x => x.ClearanceLevel == level);
            
            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, path)
                .WithBasicAuth(user.Username, user.Password)
                .Build();
            
            await _client
                .SendAsync(message)
                .AssertStatusCode(shouldHaveAccess ? HttpStatusCode.OK : HttpStatusCode.Forbidden);
        }
    }
}