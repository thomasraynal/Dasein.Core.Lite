using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack
{
    public interface IAppContainer
    {
        IContainer ObjectProvider { get; }
        T Get<T>();
        IEnumerable<T> GetAll<T>();
    }
}
