using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.Pipeline;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace Dasein.Core.Lite.Shared
{

    public class SignalRServiceBuilder<TDto, TRequest> where TRequest : IHubRequest<TDto>
    {
        public static SignalRServiceBuilder<TDto, TRequest> Create()
        {
            return new SignalRServiceBuilder<TDto, TRequest>();
        }

        public ISignalRService<TDto, TRequest> Build(TRequest request)
        {
            var args = new ExplicitArguments();

            args.Set(request);

            var service = AppCore.Instance.ObjectProvider.GetInstance<ISignalRService<TDto, TRequest>>(args);

            service.Initialize();

            return service;
        }

    }
}
