using Dasein.Core.Lite.Shared;
using System;

namespace Dasein.Core.Service.Authentication
{
    public abstract class AuthenticationServiceBase
    {
        public IServiceConfiguration ServiceConfiguration
        {
            get
            {
                return AppCore.Instance.Get<IServiceConfiguration>();
            }
        }

        public DateTime TokenExpiration => DateTime.Now.AddMilliseconds(ServiceConfiguration.TokenExpiration);
    }
}
