using System;
using Microsoft.AspNetCore.Builder;
using StructureMap;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Dasein.Core.Lite.Shared;

namespace Dasein.Core.Lite
{
    public abstract class ServiceStartupBase<TConfiguration> : ICanLog where TConfiguration : ServiceConfigurationBase
    {

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ServiceConfiguration = Configuration.GetSection(ServiceConstants.serviceConfiguration).Get<TConfiguration>();
            
            services.AddSingleton(ServiceConfiguration);
            services.AddSingleton<IServiceConfiguration>(ServiceConfiguration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(ServiceConfiguration.Version, 0);
            });

            ConfigureServicesInternal(services);

            return ConfigureIoC(services);
        }

        public TConfiguration ServiceConfiguration { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IHostingEnvironment HostingEnvironment { get; private set; }

        public ServiceStartupBase(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            HostingEnvironment = env;

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
            ConfigureInternal(app);
        }

        protected virtual void ConfigureInternal(IApplicationBuilder app)
        {
        }
    }
}
