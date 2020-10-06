using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public class UniwersalWindowsPlatform : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public override bool IsSupported(TargetTuple tuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (tuple.Platform != PlatformType.UWP)
            {
                return false;
            }

            if (tuple.Toolset != ToolsetType.MSVC)
            {
                return false;
            }

            return true;
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            return targetType switch
            {
                TargetType.SharedLibrary => $@"lib{name}.dll",
                TargetType.StaticLibrary => $@"lib{name}.lib",
                TargetType.HeaderLibrary => name,
                TargetType.Application => $@"{name}.exe",
                _ => throw new ArgumentOutOfRangeException(nameof(targetType)),
            };
        }
    }
}
