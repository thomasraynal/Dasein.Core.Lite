using Jose;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class JwtExtension
    {
        private static IServiceConfiguration Configuration
        {
            get
            {
                return AppCore.Instance.Get<IServiceConfiguration>();
            }
        }

        private static JwtSettings Settings
        {
            get
            {
                return new JwtSettings().RegisterMapper(new NewtonsoftMapper());
            }
        }

        public static string EncodeJWtToken<T>(this T payload, string issuer = null, string subject = null, string audience = null, long expiration = -1, long notBefore = -1, long issuedAt = -1)
        {
            var headers = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(issuer)) headers.Add("iss", issuer);
            if (!string.IsNullOrEmpty(subject)) headers.Add("sub", subject);
            if (!string.IsNullOrEmpty(audience)) headers.Add("aud", audience);
            if (expiration > -1) headers.Add("exp", expiration);
            if (notBefore > -1) headers.Add("nbf", notBefore);
            if (issuedAt > -1) headers.Add("iat", issuedAt);

            if (headers.Count == 0) headers = null;

            return JWT.Encode(payload, Encoding.UTF8.GetBytes(Configuration.Key), JwsAlgorithm.HS256, headers, Settings);
        }

        public static T DecodeJWtToken<T>(this string token)
        {
            return JWT.Decode<T>(token, Encoding.UTF8.GetBytes(Configuration.Key), JwsAlgorithm.HS256, Settings);
        }
    }
}
