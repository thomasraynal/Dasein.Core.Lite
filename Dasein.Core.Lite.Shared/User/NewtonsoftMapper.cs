using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class NewtonsoftMapper : IJsonMapper
    {
        public string Serialize(object obj)
        {
            var serializer = AppCore.Instance.Get<JsonSerializer>();
            return JsonConvert.SerializeObject(obj, serializer.Converters.ToArray());
        }

        public T Parse<T>(string json)
        {
            var serializer = AppCore.Instance.Get<JsonSerializer>();
            return JsonConvert.DeserializeObject<T>(json, serializer.Converters.ToArray());
        }
    }
}
