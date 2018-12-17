using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface IAuthenticationForUser
    {
        [Post("/auth/user")]
        Task<IServiceUserToken> AuthenticateUser(Credentials credentials);
    }
}
