using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceUser: IIdentity
    {
        string UserId { get; }
        string Username { get; }
        string UserRole { get; }
        IEnumerable<Claim> Claims { get; }
        void AddClaim(Claim claim);
    }
}
