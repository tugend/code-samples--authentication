using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureWebApi.Endpoints.Users.Authenticate.JsonWebTokens;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Services.Contracts;
using SecureWebApi.Tests.TestHelpers.Builders;
using SecureWebApi.Tests.TestHelpers.Extensions;
using Xunit;
using static System.Net.Mime.MediaTypeNames.Application;
using static System.Text.Encoding;


namespace SecureWebApi.Tests.Endpoints.Users.Restricted
{
    public class JwtAndBasicAuthenticatedControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private static readonly User Level0User = TestUserFactory.CreateUser(0);
        private static readonly User Level1User = TestUserFactory.CreateUser(1);
        private static readonly User Level7User = TestUserFactory.CreateUser(7);
        private static readonly User[] RegisteredUsers = {Level0User, Level1User, Level7User};

        public JwtAndBasicAuthenticatedControllerTests(WebApplicationFactory<Startup> fixture) => _client = fixture
            .WithWebHostBuilder(builder => builder
                .ConfigureTestServices(services => services
                    .RemoveAll<IUserService>()
                    .AddScoped<IUserService>(_ => new AuthenticatedUserService(RegisteredUsers))))
            .CreateClient();

        [Theory]
        [InlineData(1, "users/restricted/jwt+basic/level-1-access")]
        [InlineData(7, "users/restricted/jwt+basic/level-1-access")]
        public async Task User_Should_Receive_A_Jwt_Token_That_Allow_Access(int level, string path)
        {
            var user = RegisteredUsers.Single(x => x.ClearanceLevel == level);
            var response = await RequestJwtToken(user.Username, user.Password);

            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, path)
                .WithJwtAuth(response.Token)
                .Build();
                
            await _client
                .SendAsync(message)
                .AssertStatusCode(HttpStatusCode.OK);
        }
       
        [Theory]
        [InlineData(1, "users/restricted/jwt+basic/level-1-access")]
        [InlineData(7, "users/restricted/jwt+basic/level-1-access")]
        public async Task BasicAuthenticatedUser_Should_Allow_Access(int level, string path)
        {
            var user = RegisteredUsers.Single(x => x.ClearanceLevel == level);

            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, path)
                .WithBasicAuth(user.Username, user.Password)
                .Build();
            
            await _client
                .SendAsync(message)
                .AssertStatusCode(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task UnauthorizedUser_Should_Not_Get_Access()
        {
            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, "users/restricted/jwt+basic/level-1-access")
                .Build();
            
            await _client
                .SendAsync(message)
                .AssertStatusCode(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task User_With_Too_Low_Clearance_Should_Not_Get_Access()
        {
            var message = HttpRequestMessageBuilder
                .Create(HttpMethod.Get, "users/restricted/jwt+basic/level-1-access")
                .WithBasicAuth(Level0User.Username, Level0User.Password)
                .Build();
            
            await _client
                .SendAsync(message)
                .AssertStatusCode(HttpStatusCode.Unauthorized);
        }

        private async Task<Response> RequestJwtToken(string username, string password)
        {
            var request = new Request(username: username, password: password);
            var serializedContent = System.Text.Json.JsonSerializer.Serialize(request);
            var jsonStringContent = new StringContent(serializedContent, UTF8, Json);

            return await _client
                .PostAsync("/users/authenticate/jwt", jsonStringContent)
                .AssertStatusCode(HttpStatusCode.OK)
                .Deserialize<Response>();
        }
    }
}