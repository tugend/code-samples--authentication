using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApi.Endpoints.Sanity.Hello;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Services.Contracts;
using SecureWebApi.Tests.Infrastructure;
using SecureWebApi.Tests.TestHelpers.Extensions;
using Xunit;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace SecureWebApi.Tests.Endpoints.Sanity
{
    public class TestControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly FixedSystemClock _fixedSystemClock = new FixedSystemClock();
        private readonly HttpClient _client;

        public TestControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture
                .WithWebHostBuilder(builder => builder
                    .ConfigureTestServices(services => services
                        .AddScoped<IUserService>(_ => new AuthenticatedUserService(new User[]{}))
                        .AddTransient<ISystemClock>(_ => _fixedSystemClock)))
                .CreateClient();
        }

        [Fact]
        public async Task HelloWorld()
        {
            await _client
                .GetAsync("/sanity/hello")
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertContent(new HelloResponse("Hello world!"));
        }
        
        [Fact]
        public async Task HelloWhatTimeIsIt()
        {
            var now = DateTimeOffset.Now;

            _fixedSystemClock.SetTime(now);

            await _client
                .GetAsync("/sanity/hello/what-time-is-it")
                .AssertStatusCode(HttpStatusCode.OK)
                .AssertContent(new HelloWhatTimeIsItResponse("Hello, here is the time!", now));
        }
    }
}