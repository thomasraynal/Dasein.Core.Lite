using System;
using Microsoft.AspNetCore.Builder;
using StructureMap;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AspNetCoreStarterPack.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreStarterPack.Error;

namespace AspNetCoreStarterPack
{
    public abstract class ServiceStartupBase<TConfiguration> : ICanLog where TConfiguration : ServiceConfigurationBase
    {
        private readonly string serviceConfiguration = "serviceConfiguration";

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ServiceConfiguration = Configuration.GetSection(serviceConfiguration).Get<TConfiguration>();

            services.AddSingleton(ServiceConfiguration);
            services.AddSingleton<IServiceConfiguration>(ServiceConfiguration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(ServiceConfiguration.Version, 0);
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"{ServiceConfiguration.Name} v{ServiceConfiguration.Version}", new Info { Title = ServiceConfiguration.Name, Version = ServiceConfiguration.Version.ToString() });
            });

            ConfigureServicesInternal(services);

            return ConfigureIoC(services);
        }

        public TConfiguration ServiceConfiguration { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public ServiceStartupBase(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;

            AppCore.Instance.ObjectProvider.Configure(config =>
            {
                config.For<IConfiguration>().Use(Configuration);
            });
        }

        protected virtual void ConfigureServicesInternal(IServiceCollection services)
        {
        }

        protected virtual void OnApplicationStart()
        {
        }

        private IServiceProvider ConfigureIoC(IServiceCollection services)
        {

            ConfigureIoCInternal(AppCore.Instance.ObjectProvider);

            AppCore.Instance.ObjectProvider.Configure(config =>
            {
                config.Populate(services);
            });

            ServiceConfiguration.Container = AppCore.Instance;
            ServiceConfiguration.Root = Configuration;


            var loggerFactory = AppCore.Instance.Get<ILoggerFactory>();
            var globalLogger = loggerFactory.CreateLogger(GetType());

            AppCore.Instance.ObjectProvider.Configure((config) => config.For<ILogger>().Use(globalLogger));

            this.LogInformation($"Start service [{ServiceConfiguration.Name}]");

            OnApplicationStart();

            return AppCore.Instance.ObjectProvider.GetInstance<IServiceProvider>();

        }

        protected virtual void ConfigureIoCInternal(IContainer services)
        {

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v{ServiceConfiguration.Version}/swagger.json", $"API [{ServiceConfiguration.Name}] v.{ServiceConfiguration.Version}");
            });

            app.UseServiceExceptionHandler();

            ConfigureInternal(app);
        }

        protected virtual void ConfigureInternal(IApplicationBuilder app)
        {
        }
    }
}
