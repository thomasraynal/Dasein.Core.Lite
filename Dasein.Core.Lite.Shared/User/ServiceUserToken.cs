using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ServiceUserToken : ServiceUser, IServiceUserToken
    {

        public ServiceUserToken(IIdentity identity) : base(identity)
        {
        }

        public ServiceUserToken()
        {
        }

        public ServiceUserToken(IEnumerable<Claim> claims) : base(claims)
        {
        }

        public ServiceUserToken(IIdentity identity, IEnumerable<Claim> claims) : base(identity, claims)
        {
        }

        public ServiceUserToken(IIdentity identity, IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(identity, claims, authenticationType, nameType, roleType)
        {
        }

        public ServiceUserToken(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(claims, authenticationType, nameType, roleType)
        {
        }

        public ServiceUserToken(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
        }

        public DateTime Expiration
        {
            get
            {
                var expiration = Claims.FirstOrDefault(claim => claim.Type == ServiceUserConstants.Expiration).Value;
                return DateTime.Parse(expiration);
            }
        }

        public string Digest
        {
            get
            {
                var digest = Claims.FirstOrDefault(claim => claim.Type == ServiceUserConstants.Digest).Value;
                return digest;
            }
        }
    }
}
