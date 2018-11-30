using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public static class ObjectProviderExtension
    {
        public static ISignalRService<TDto, TRequest> GetSignalRService<TDto, TRequest>(this IAppContainer container, TRequest request)
            where TRequest : IHubRequest<TDto>
        {
            var explicitArg = new ExplicitArguments();
            explicitArg.Set(request);
            return container.ObjectProvider.GetInstance<ISignalRService<TDto, TRequest>>(explicitArg);
        }

        public static ISignalRService<TDto, TRequest> GetSignalRService<TDto, TRequest>(this IAppContainer container)
            where TRequest : IHubRequest<TDto>
        {
            return container.ObjectProvider.GetInstance<ISignalRService<TDto, TRequest>>();
        }
    }
}
