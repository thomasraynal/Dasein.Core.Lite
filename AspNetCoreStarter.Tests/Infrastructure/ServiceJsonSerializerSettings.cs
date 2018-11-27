using AspNetCoreStarterPack.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AspNetCoreStarter.Tests.Infrastructure
{
    public class ServiceJsonSerializerSettings : JsonSerializerSettings
    {
        public ServiceJsonSerializerSettings()
        {
           
            Culture = CultureInfo.InvariantCulture;
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            Converters.Add(new DateTimeJsonConverter());
            Converters.Add(new StringEnumConverter());
        }
    }
}
