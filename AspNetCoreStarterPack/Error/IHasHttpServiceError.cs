using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Error
{
    public interface IHasHttpServiceError
    {
        HttpServiceError HttpServiceError { get; }
    }
}
