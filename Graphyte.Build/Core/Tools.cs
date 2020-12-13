using System;
using System.Security.Cryptography;
using System.Text;

namespace Graphyte.Build.Core
{
    public static partial class Tools
    {
        /// <summary>
        /// Computes predictive GUID of UTF8 string value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid MakeGuid(string value)
        {
            var provider = SHA256.Create();
            var hash = provider
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                .AsSpan(0, 16);

            return new Guid(hash);
        }
    }
}
