using System;

namespace Graphyte.Build
{
    public enum ArchitectureType
    {
        X64,
        X86,
        ARM,
        ARM64,
        PPC64,
    }

    public static class ArchitectureExtensions
    {
        public static bool Is64Bit(this ArchitectureType self)
        {
            switch (self)
            {
                case ArchitectureType.X64:
                case ArchitectureType.ARM64:
                case ArchitectureType.PPC64:
                    return true;

                case ArchitectureType.X86:
                case ArchitectureType.ARM:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
