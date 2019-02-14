using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.Pipeline;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.Http.Connections;

namespace Dasein.Core.Lite.Shared
{

    public class SignalRServiceBuilder<TDto, TRequest> where TRequest : class, IHubRequest<TDto>
    {
        public static SignalRServiceBuilder<TDto, TRequest> Create()
        {
            return new SignalRServiceBuilder<TDto, TRequest>();
        }

        private ISignalRService<TDto, TRequest> BuildInternal(params object[] parameters)
        {
            var args = new ExplicitArguments();

            foreach (var @param in parameters)
            {
                args.Set(@param.GetType(), @param);
            }

            if (!args.Defaults.ContainsKey(typeof(Action<HttpConnectionOptions>)))
            {
                args.Defaults.Add(typeof(Action<HttpConnectionOptions>), new Action<HttpConnectionOptions>((_) => { }));
            }

            if (!args.Defaults.ContainsKey(typeof(HttpTransportType)))
            {
                args.Defaults.Add(typeof(HttpTransportType), HttpTransportType.WebSockets | HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents);
            }

            var service = AppCore.Instance.ObjectProvider.GetInstance<ISignalRService<TDto, TRequest>>(args);

            service.BuildInternal();

            return service;
        }

        public ISignalRService<TDto, TRequest> Build(TRequest request)
        {
            return BuildInternal(request);
        }

        public ISignalRService<TDto, TRequest> Build(TRequest request, HttpTransportType transports, Action<HttpConnectionOptions> configureHttpConnection)
        {
            return BuildInternal(request, transports, configureHttpConnection);
        }

        public ISignalRService<TDto, TRequest> Build(TRequest request, HttpTransportType transports)
        {
            return BuildInternal(request, transports);
        }

        public ISignalRService<TDto, TRequest> Build(TRequest request, Action<HttpConnectionOptions> configureHttpConnection)
        {
            return BuildInternal(request, configureHttpConnection);
        }

    }
}
