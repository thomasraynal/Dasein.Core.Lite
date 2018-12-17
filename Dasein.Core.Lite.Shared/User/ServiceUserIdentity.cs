using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public class ServiceUserIdentity : ClaimsIdentity
    {
        public ServiceUserIdentity(String userName, string authenticationType) : base(authenticationType)
        {
            InitializeServiceUserIdentity(userName);
        }

        public ServiceUserIdentity(String userName, IEnumerable<Claim> claims) : base(claims)
        {
            InitializeServiceUserIdentity(userName);
        }

        public ServiceUserIdentity(String userName, IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
            InitializeServiceUserIdentity(userName);
        }

        private void InitializeServiceUserIdentity(String userName)
        {
            var userNameClaim = new Claim(ClaimTypes.Name, userName);
            this.AddOrReplaceClaim(userNameClaim);
        }

    }
}
