using Dasein.Core.Lite.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite
{
    public static class ServiceContainerExtension
    {
        public static TService GetService<TService>(this IAppContainer container)
        {
            var middleware = (TService)container.ObjectProvider.TryGetInstance<IServiceProxy<TService>>();
            if (null != middleware) return middleware;
            return container.Get<TService>();
        }
    }
}
