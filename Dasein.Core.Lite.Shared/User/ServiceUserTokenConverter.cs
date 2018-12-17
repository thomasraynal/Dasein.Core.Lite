using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    internal class ServiceUserTokenLite
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string AuthenticationType { get; set; }
        public string Digest { get; set; }
        public DateTime Expiration { get; set; }
        public List<ClaimLite> Claims { get; set; }
    }

    public class ServiceUserTokenConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IServiceUserToken) || objectType.GetInterfaces().Any(@interface => @interface == typeof(IServiceUserToken));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClaimsIdentityLite>(reader);
            if (source == null) return null;


            var target = new ServiceUserToken(source.ToClaimIdentity());
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ServiceUserToken)value;

            var target = new ServiceUserTokenLite()
            {
                Claims = source.Claims.Select(claims => new ClaimLite(claims.Type, claims.Value)).ToList(),
                AuthenticationType = source.AuthenticationType,
                Expiration = source.Expiration,
                UserId = source.UserId,
                UserName = source.Username,
                UserRole = source.UserRole,
                Digest = source.Digest
            };

            serializer.Serialize(writer, target);
        }
    }
}
