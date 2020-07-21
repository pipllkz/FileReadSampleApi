using System;
using System.IO;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.SwaggerUI;


namespace FileReadSampleApi.Extentions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Set the comments path for the Swagger JSON and UI.
            var basePath = AppContext.BaseDirectory;
            var webapp = Path.Combine(basePath, "FileReadSampleApi.xml");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Read File Api", Version = "v1" });

                c.IncludeXmlComments(webapp);
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Title");
                c.DocExpansion(DocExpansion.List);
            });

            return app;
        }
    }
}
