using System.Collections.Generic;

namespace Graphyte.Build.Core
{
    public static class CollectionExtensions
    {
        public static List<T> Import<T>(this List<T> self, T item)
        {
            if (!self.Contains(item))
            {
                self.Add(item);
            }

            return self;
        }

        public static  List<T> Import<T>(this List<T> self, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!self.Contains(item))
                {
                    self.Add(item);
                }
            }

            return self;
        }
    }
}
