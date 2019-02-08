using Dasein.Core.Lite.Shared;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Dasein.Core.Lite.Shared
{

    public class ApiServiceBuilder<TServiceContract> where TServiceContract : class
    {
        private string _apiRoot;
        private CompositeHttpClientHandler _handler;

        private ApiServiceBuilder(string apiRoot)
        {
            _apiRoot = apiRoot;
            _handler = new CompositeHttpClientHandler();
        }

        internal void AddHandler(Action<HttpRequestHeaders> handler)
        {
            _handler.AddHandler(handler);
        }

        public static ApiServiceBuilder<TServiceContract> Build(string apiRoot)
        {
            if (String.IsNullOrWhiteSpace(apiRoot)) throw new ArgumentException($"A root url must be provided for service {typeof(TServiceContract)}", "apiRoot");

            var builder = new ApiServiceBuilder<TServiceContract>(apiRoot);
            return builder;
        }

        public TServiceContract Create(HttpClient client = null, RefitSettings refitSettings = null)
        {
            if (null == client)
            {
                client = new HttpClient(_handler)
                {
                    Timeout = TimeSpan.FromMinutes(60),
                    BaseAddress = new Uri(_apiRoot)
                };
            }

            if (null == refitSettings)
            {
                var settings = AppCore.Instance.Get<JsonSerializerSettings>();

                refitSettings = new RefitSettings()
                {
                    JsonSerializerSettings = settings
                };
            }
            
            return RestService.For<TServiceContract>(client, refitSettings);
        }
    }
}
