using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dasein.Core.Lite
{
    public class Host<TStartup> : DefaultHostBase<TStartup> where TStartup : class
    {
    }

    public abstract class DefaultHostBase<TStartup> where TStartup : class
    {
        public const String serviceConfigFile = "config.json";

        public virtual IConfigurationBuilder Configure(IConfigurationBuilder builder)
        {
            return builder;
        }

        public IWebHost Build(params string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(serviceConfigFile, true, true)
                .AddCommandLine(args);

            builder = Configure(builder);

            var config = builder.Build();

            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<TStartup>()
                .UseKestrel()
                .Build();
        }
    }
}
