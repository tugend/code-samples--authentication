using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApi.Infrastructure;
using SecureWebApi.Infrastructure.Authentication;
using SecureWebApi.Infrastructure.Authentication.Jwt;
using SecureWebApi.Infrastructure.Swagger;
using SecureWebApi.Services.Contracts;

namespace SecureWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration
                .GetSection("AppSettings");

            // NOTE: NewtonsoftJson package instead of System.Text.Json
            // to support use of constructors when deserializing
            services
                .AddControllers().AddNewtonsoftJson().Services
                .AddBasicAuthentication()
                .AddAuthenticationPolicies()
                .AddSwagger();
            
            services
                .AddOptions<AppSettings>().Bind(appSettings)
                .ValidateDataAnnotations().Services
                .AddTransient<ISystemClock, SystemClock>()
                .AddTransient<IUserService>(_ => new AuthenticatedUserService());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger().SetupSwaggerUi();
            app.UseMiddleware<JwtMiddleware>();
            
            // NOTE: Order of UseAuthentication and UseAuthorization
            // matters, it only works in this sequence!
            app
                .UseRouting()
                .UseAuthentication().UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}