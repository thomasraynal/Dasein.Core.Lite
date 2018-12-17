using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public interface IHashProvider
    {
        byte[] ComputeHash(byte[] data);
        int SaltSize { get; }
    }
}
