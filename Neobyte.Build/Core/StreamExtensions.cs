using System.IO;

namespace Neobyte.Build.Core
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream self)
        {
            if (self is MemoryStream ms)
            {
                return ms.ToArray();
            }
            else
            {
                using var s = new MemoryStream();
                self.CopyTo(s);
                return s.ToArray();
            }
        }
    }
}
