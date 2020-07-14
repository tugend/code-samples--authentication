using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using SecureWebApi.Infrastructure.Authentication.Basic;

namespace SecureWebApi.Infrastructure.Authentication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBasicAuthentication(this IServiceCollection services) =>
            services
                .AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
                .Services;

        public static IServiceCollection AddAuthenticationPolicies(this IServiceCollection services) =>
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SecurityClearance+1", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "1"));
                options.AddPolicy("SecurityClearance+2", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "2"));
                options.AddPolicy("SecurityClearance+3", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "3"));
                options.AddPolicy("SecurityClearance+4", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "4"));
                options.AddPolicy("SecurityClearance+5", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "5"));
                options.AddPolicy("SecurityClearance+6", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "6"));
                options.AddPolicy("SecurityClearance+7", policy => policy.RequireClaim(CustomClaimTypes.ClearanceLevel, "7"));
            });
    }
}