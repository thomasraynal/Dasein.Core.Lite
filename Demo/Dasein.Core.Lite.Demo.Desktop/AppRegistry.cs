using Dasein.Core.Lite.Demo.Desktop.Infrastructure;
using Dasein.Core.Lite.Demo.Server;
using Dasein.Core.Lite.Demo.Shared;
using Dasein.Core.Lite.Shared;
using Dasein.Service.Demo.Desktop;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Demo.Desktop
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            For<ILogger>().Use<VoidLogger>();
            For<IHubConfiguration>().Use<HubConfiguration>();
            For<ISignalRService<Price,PriceRequest>>().Use<PriceHubClient>();
            For<ISignalRService<TradeEvent, TradeEventRequest>>().Use<TradeEventHubClient>();

            var jsonSettings = new AppJsonSerializer();
            var jsonSerializer = JsonSerializer.Create(jsonSettings);

            For<JsonSerializerSettings>().Use(jsonSettings);
            For<JsonSerializer>().Use(jsonSerializer);

            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.WithDefaultConventions();
                scanner.ConnectImplementationsToTypesClosing(typeof(ISignalRService<,>));
            });
        }
    }
}
