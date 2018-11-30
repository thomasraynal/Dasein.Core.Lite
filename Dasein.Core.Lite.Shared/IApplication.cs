using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public interface IApplication
    {
        string Name { get; set; }
        int Version { get; set; }
    }
}
