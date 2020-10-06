using System;

namespace Graphyte.Build
{
    public enum Architecture
    {
        X64,
        X86,
        ARM,
        ARM64,
        PPC64,
    }

    public static class ArchitectureExtensions
    {
        public static bool Is64Bit(this Architecture self)
        {
            switch (self)
            {
                case Architecture.X64:
                case Architecture.ARM64:
                case Architecture.PPC64:
                    return true;

                case Architecture.X86:
                case Architecture.ARM:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
