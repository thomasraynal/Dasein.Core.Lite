using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ClaimsIdentityLite
    {
        public ClaimsIdentity ToClaimIdentity()
        {
            return new ClaimsIdentity(Claims.Select(claim => new Claim(claim.Type, claim.Value)), AuthenticationType);
        }

        public ClaimsIdentityLite(ClaimsIdentity identity)
        {
            AuthenticationType = identity.AuthenticationType;
            Claims = identity.Claims.Select(claim => new ClaimLite { Type = claim.Type, Value = claim.Value }).ToList();
        }

        public ClaimsIdentityLite()
        {
        }

        public ClaimsIdentityLite(IEnumerable<ClaimLite> claims, string authenticationType)
        {
            AuthenticationType = authenticationType;
            Claims = claims.ToList();
        }

        public String Name
        {
            get
            {
                var name = Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
                if (null == name) return null;
                return name.Value;
            }
        }

        public string AuthenticationType { get; set; }
        public List<ClaimLite> Claims { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ClaimsIdentityLite && this.GetHashCode() == (obj as ClaimsIdentityLite).GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Claims.Select((id) => id.GetHashCode() * 397).Aggregate((id1, id2) => id1 ^ id2);
                return hashCode;
            }
        }
    }

    public class ClaimsPrincipalLite
    {

        public ClaimsPrincipal ToClaimPrincipal()
        {
            return new ClaimsPrincipal(Identities.Select(identity => new ClaimsIdentity(identity.Claims.Select(claim => new Claim(claim.Type, claim.Value)), identity.AuthenticationType)));
        }

        public ClaimsPrincipalLite(ClaimsPrincipal principal)
        {
            Identities = principal.Identities.Select(identity => new ClaimsIdentityLite(identity.Claims.Select(claim => new ClaimLite(claim.Type, claim.Value)).ToList(), identity.AuthenticationType)).ToList();
        }

        public ClaimsPrincipalLite()
        {
        }

        public ClaimsPrincipalLite(IEnumerable<ClaimsIdentityLite> identities)
        {
            Identities = identities.ToList();
        }
        
        public List<ClaimsIdentityLite> Identities { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ClaimsPrincipalLite && this.GetHashCode() == (obj as ClaimsPrincipalLite).GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Identities.Select((id)=>  id.GetHashCode() *397 ).Aggregate((id1,id2)=> id1 ^ id2);
                return hashCode;
            }
        }
    }

    public class ClaimLite
    {
        public ClaimLite()
        {
        }

        public ClaimLite(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ClaimLite && this.GetHashCode() == (obj as ClaimLite).GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                return hashCode;
            }
        }
    }

    public class ClaimsPrincipalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ClaimsPrincipal) || objectType == typeof(IPrincipal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClaimsPrincipalLite>(reader);
            if (source == null) return null;
            var target = new ClaimsPrincipal(source.Identities.Select(identity => new ClaimsIdentity(identity.Claims.Select(claim => new Claim(claim.Type, claim.Value)), identity.AuthenticationType)));
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ClaimsPrincipal)value;

            var target = new ClaimsPrincipalLite(source);

            serializer.Serialize(writer, target);
        }
    }
}
