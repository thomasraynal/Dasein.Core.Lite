using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IServiceUserToken: IServiceUser, IToken
    {
    }
}
