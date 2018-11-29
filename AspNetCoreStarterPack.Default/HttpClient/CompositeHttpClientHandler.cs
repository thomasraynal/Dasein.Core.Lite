using AspNetCoreStarterPack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreStarterPack.Default
{
    public class CompositeHttpClientHandler : HttpClientHandler, ICanLog
    {
        private List<Action<HttpRequestHeaders>> _headerHandlers;

        public void AddHandler(Action<HttpRequestHeaders> handler)
        {
            _headerHandlers.Add(handler);
        }

        public CompositeHttpClientHandler()
        {
            _headerHandlers = new List<Action<HttpRequestHeaders>>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var handler in _headerHandlers)
            {
                handler(request.Headers);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                this.LogWarning($"Request failed with code [{(int)response.StatusCode}], reason [{response.ReasonPhrase}], message [{errorMessage}]");
            }

            return response;
        }
    }
}
