using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class ClaimsExtentions
    {
        public static void AddOrReplaceClaim(this ClaimsIdentity identity, Claim claim)
        {
            var toRemove = identity.Claims.Where(c => c.Type == claim.Type).ToList();

            foreach(var remove in toRemove)
            {
                identity.TryRemoveClaim(remove);
            }

            identity.AddClaim(claim);
        }
    }
}
