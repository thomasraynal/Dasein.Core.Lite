using Dasein.Core.Lite.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Hosting
{
    public static class ServiceStartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IServiceConfiguration serviceConfiguration)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{serviceConfiguration.Version}", new Info { Title = serviceConfiguration.Name, Version = $"v{serviceConfiguration.Version}" });
            });
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IServiceConfiguration serviceConfiguration)
        {
            app.UseSwagger();
            return app.UseSwaggerUI(options =>
             {
                 options.SwaggerEndpoint($"/swagger/v{serviceConfiguration.Version}/swagger.json", $"API [{serviceConfiguration.Name}] v.{serviceConfiguration.Version}");
             });
        }
    }
}
