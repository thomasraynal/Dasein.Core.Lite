using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public static class CryptoServiceExtensions
    {
        public static void CreateHash(this ICryptoService cryptoService, string data, out string hash, out string salt)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            cryptoService.CreateHash(dataBytes, out byte[] hashBytes, out byte[] saltBytes);

            hash = Convert.ToBase64String(hashBytes);
            salt = Convert.ToBase64String(saltBytes);
        }

        public static string ComputeHash(this ICryptoService cryptoService, string data, string salt)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var saltBytes = Convert.FromBase64String(salt);

            var hashBytes = cryptoService.ComputeHash(dataBytes, saltBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
