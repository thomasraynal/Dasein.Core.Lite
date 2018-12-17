using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ServiceUser : ClaimsIdentity, IServiceUser
    {
        public ServiceUser()
        {
        }

        public ServiceUser(IEnumerable<Claim> claims) : base(claims)
        {
        }

        public ServiceUser(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
        }

        public ServiceUser(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(claims, authenticationType, nameType, roleType)
        {
        }

        public ServiceUser(IIdentity identity, IEnumerable<Claim> claims) : base(identity, claims)
        {
        }

        public ServiceUser(IIdentity identity) : base(identity)
        {
        }

        public ServiceUser(IIdentity identity, IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(identity, claims, authenticationType, nameType, roleType)
        {
        }

        public string UserId
        {
            get
            {
                var id = Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                if (null == id) return null;

                return id.Value;
            }
        }
        
        public string Username
        {
            get
            {
                var id = Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

                if (null == id) return null;

                return id.Value;
            }
        }

        public string UserRole
        {
            get
            {
                var id = Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                if (null == id) return null;

                return id.Value;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ServiceUser)) return false;
            return (obj as ServiceUser).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Claims.Select((id) => id.GetHashCode() * 397).Aggregate((id1, id2) => id1 ^ id2);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} | {1}", Username, UserRole);
        }
    }
}
