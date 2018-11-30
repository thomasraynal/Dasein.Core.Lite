using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public static class ApiServiceExtensions
    {
        public const string BearerScheme = "Bearer";

        public static ApiServiceBuilder<TServiceContract> AddHeader<TServiceContract>(this ApiServiceBuilder<TServiceContract> builder, String header, Func<String> getToken) where TServiceContract : class
        {
            var token = getToken();
            builder.AddHandler((headers) =>
            {
                headers.Add(header, token);
            });

            return builder;
        }

        public static ApiServiceBuilder<TServiceContract> AddAuthorizationHeader<TServiceContract>(this ApiServiceBuilder<TServiceContract> builder,  Func<String> getToken) where TServiceContract : class
        {
            var token = getToken();
            builder.AddHandler((headers) =>
            {
                headers.Authorization = new AuthenticationHeaderValue(BearerScheme, token);
            });

            return builder;
        }
    }
}
