using System.Security.Cryptography;

namespace Dasein.Core.Lite.Shared
{
    public class DefaultHashProvider : IHashProvider
    {
        public int SaltSize
        {
            get { return 13; }
        }

        public byte[] ComputeHash(byte[] data)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(data);
            }
        }
    }
}
