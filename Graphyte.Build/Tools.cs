using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Graphyte.Build
{
    public static class Tools
    {
        public static Guid MakeGuid(string value)
        {
            var provider = SHA256.Create();
            var hash = provider.ComputeHash(Encoding.ASCII.GetBytes(value)).AsSpan(0, 16);
            return new Guid(hash);
        }
    }

    public static class ImportExtensions
    {
        public static List<T> Import<T>(this List<T> collection, T item)
        {
            if(!collection.Contains(item))
            {
                collection.Add(item);
            }

            return collection;
        }

        public static List<T> Import<T>(this List<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!collection.Contains(item))
                {
                    collection.Add(item);
                }
            }

            return collection;
        }
    }
}
