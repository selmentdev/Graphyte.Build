using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Android
{
    public class AndroidPlatform : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public override bool IsSupported(TargetTuple tuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (tuple.Platform != PlatformType.Android)
            {
                return false;
            }

            if (tuple.Toolset != ToolsetType.Clang)
            {
                return false;
            }

            return true;
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            return targetType switch
            {
                TargetType.SharedLibrary => $@"lib{name}.so",
                TargetType.StaticLibrary => $@"lib{name}.a",
                TargetType.HeaderLibrary => name,
                TargetType.Application => name,
                _ => throw new ArgumentOutOfRangeException(nameof(targetType)),
            };
        }
    }
}
