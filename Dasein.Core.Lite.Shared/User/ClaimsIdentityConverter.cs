using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ClaimsIdentityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ClaimsIdentity) || objectType == typeof(IIdentity);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClaimsIdentityLite>(reader);
            if (source == null) return null;

            var claims = source.Claims.Select(claim => new Claim(claim.Type, claim.Value));
            var target = new ClaimsIdentity(claims, source.AuthenticationType);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ClaimsIdentity)value;

            var target = new ClaimsIdentityLite(source.Claims.Select(claim => new ClaimLite { Type = claim.Type, Value = claim.Value }), source.AuthenticationType);

            serializer.Serialize(writer, target);
        }
    }
}
