using System;
using System.Security.Cryptography;
using System.Text;

namespace Neobyte.Build.Core
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
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value))
                .AsSpan(0, 16);

            return new Guid(hash);
        }
    }
}
