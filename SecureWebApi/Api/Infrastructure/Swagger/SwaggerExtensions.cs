using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Infrastructure.Swagger
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder SetupSwaggerUi(this IApplicationBuilder app) =>  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{PublicApiGroupName}/swagger.json", PublicApiDoc.Title);
                c.SwaggerEndpoint($"/swagger/{RestrictedApiGroupName}/swagger.json", RestrictedApiDoc.Title);
            });
        
        public static IServiceCollection AddSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: PublicApiGroupName, PublicApiDoc);
                c.SwaggerDoc(name: RestrictedApiGroupName, RestrictedApiDoc);
                UseFullNamespaceQualifications(c);
                RenderXmlComments(c);
            });

        private static void RenderXmlComments(SwaggerGenOptions c)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        }

        private static void UseFullNamespaceQualifications(SwaggerGenOptions c)
        {
            c.CustomSchemaIds(type => type.FullName);
        }
    }
}