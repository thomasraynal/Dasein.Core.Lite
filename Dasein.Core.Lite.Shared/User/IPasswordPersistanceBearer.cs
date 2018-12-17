using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IPasswordPersistanceBearer
    {
        String PasswordHash { get; }
        String PasswordSalt { get; }
    }
}
