using System;
using System.Security.Cryptography;
using System.Text;

namespace Graphyte.Build.Tools
{
    public static class Utils
    {
        public static Guid MakeGuid(string value)
        {
            var provider = SHA256.Create();
            var hash = provider.ComputeHash(Encoding.ASCII.GetBytes(value)).AsSpan(0, 16);
            return new Guid(hash);
        }
    }
}
