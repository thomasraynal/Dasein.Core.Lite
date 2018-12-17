using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IToken
    {
        DateTime Expiration { get; }
        string Digest { get; }
    }
}
