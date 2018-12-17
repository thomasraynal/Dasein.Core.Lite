using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public class DefaultCryptoService : ICryptoService
    {
        private readonly IHashProvider _hashProvider;

        public DefaultCryptoService(IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
        }

        public void CreateHash(byte[] data, out byte[] hash, out byte[] salt)
        {
            salt = CreateSalt();
            hash = ComputeHash(data, salt);
        }

        public byte[] ComputeHash(byte[] data, byte[] salt)
        {
            var saltedData = Concatenate(data, salt);
            var hashBytes = _hashProvider.ComputeHash(saltedData);

            return hashBytes;
        }

        private byte[] CreateSalt()
        {
            var randomBytes = new byte[_hashProvider.SaltSize];
            using (var randomGenerator = new RNGCryptoServiceProvider())
            {
                randomGenerator.GetBytes(randomBytes);
                return randomBytes;
            }
        }

        private byte[] Concatenate(byte[] a, byte[] b)
        {
            var result = new byte[a.Length + b.Length];

            Buffer.BlockCopy(a, 0, result, 0, a.Length);
            Buffer.BlockCopy(b, 0, result, a.Length, b.Length);

            return result;
        }
    }
}
