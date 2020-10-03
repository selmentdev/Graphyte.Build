using System;

namespace Graphyte.Build
{
    public enum ArchitectureType
    {
        X64,
        X86,
        Arm,
        Arm64,
        PowerPC64,
    }
}

namespace Graphyte.Build
{
    public static class ArchitectureTypeExtensions
    {
        public static bool Is64Bit(this ArchitectureType type)
        {
            switch (type)
            {
                case ArchitectureType.X86:
                case ArchitectureType.Arm:
                    return false;
                case ArchitectureType.Arm64:
                case ArchitectureType.X64:
                case ArchitectureType.PowerPC64:
                    return true;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
