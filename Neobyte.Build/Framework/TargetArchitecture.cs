using System;

namespace Neobyte.Build.Framework
{
    public enum TargetArchitecture
    {
        X64,
        Arm64,
    }
}

namespace Neobyte.Build.Framework
{
    public static partial class TargetExtensions
    {
        public static bool Is64Bit(this TargetArchitecture self)
        {
            switch (self)
            {
                case TargetArchitecture.X64:
                case TargetArchitecture.Arm64:
                    return true;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }
}
