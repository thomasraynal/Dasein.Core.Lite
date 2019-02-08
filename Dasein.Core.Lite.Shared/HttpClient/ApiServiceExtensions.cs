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

        public static ApiServiceBuilder<TServiceContract> AddHeader<TServiceContract>(this ApiServiceBuilder<TServiceContract> builder, String header, Func<String> getValue) where TServiceContract : class
        {
            builder.AddHandler((headers) =>
            {
                var value = getValue();
                headers.Add(header, value);
            });

            return builder;
        }

        public static ApiServiceBuilder<TServiceContract> AddAuthorizationHeader<TServiceContract>(this ApiServiceBuilder<TServiceContract> builder,  Func<String> getToken) where TServiceContract : class
        {
            builder.AddHandler((headers) =>
            {
                var token = getToken();
                headers.Authorization = new AuthenticationHeaderValue(BearerScheme, token);
            });

            return builder;
        }
    }
}
