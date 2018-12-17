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
        public static IServiceCollection AddSwagger<TConfiguration>(this ServiceStartupBase<TConfiguration> startup, IServiceCollection services) where TConfiguration : ServiceConfigurationBase
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{startup.ServiceConfiguration.Version}", new Info { Title = startup.ServiceConfiguration.Name, Version = $"v{startup.ServiceConfiguration.Version}" });
            });
        }

        public static IApplicationBuilder UseSwagger<TConfiguration>(this ServiceStartupBase<TConfiguration> startup, IApplicationBuilder app) where TConfiguration : ServiceConfigurationBase
        {
            app.UseSwagger();
            return app.UseSwaggerUI(options =>
             {
                 options.SwaggerEndpoint($"/swagger/v{startup.ServiceConfiguration.Version}/swagger.json", $"API [{startup.ServiceConfiguration.Name}] v.{startup.ServiceConfiguration.Version}");
             });
        }
    }
}
