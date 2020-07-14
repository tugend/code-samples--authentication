using Microsoft.OpenApi.Models;

namespace SecureWebApi.Infrastructure.Swagger
{
    public static class ApiDefinitions
    {
        public const string PublicApiGroupName = "public";
        public static readonly OpenApiInfo PublicApiDoc = new OpenApiInfo 
            {
                Title = "Public API V1",
                Version = "v1",
                Description = "Any public APIs for free public consumption."
            };
        
        public const string RestrictedApiGroupName = "restricted";
        public static readonly OpenApiInfo RestrictedApiDoc = new OpenApiInfo 
            {
                Title = "Restricted API V1",
                Version = "v1",
                Description = "Any restricted APIs for authenticated and authorized consumption."
            };
    }
}