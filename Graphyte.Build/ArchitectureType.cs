using System;

namespace Graphyte.Build
{
    public enum ArchitectureType
    {
        X64,
        ARM64,
    }

    public static class ArchitectureExtensions
    {
        public static bool Is64Bit(this ArchitectureType self)
        {
            switch (self)
            {
                case ArchitectureType.X64:
                case ArchitectureType.ARM64:
                    return true;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
