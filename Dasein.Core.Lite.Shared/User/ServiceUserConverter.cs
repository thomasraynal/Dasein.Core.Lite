using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ServiceUserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType != typeof(IServiceUserToken) && (objectType == typeof(IServiceUser) || (objectType == typeof(ServiceUser)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClaimsIdentityLite>(reader);
            if (source == null) return null;


            var target = new ServiceUser(source.ToClaimIdentity());
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ServiceUser)value;

            var target = new ClaimsIdentityLite()
            {
                Claims = source.Claims.Select(claim => new ClaimLite(claim.Type, claim.Value)).ToList()
            };

            serializer.Serialize(writer, target);
        }
    }
}
