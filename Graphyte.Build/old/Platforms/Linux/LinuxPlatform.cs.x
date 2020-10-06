using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public class LinuxPlatform : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public override bool IsSupported(TargetTuple tuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (tuple.Platform != PlatformType.Linux)
            {
                return false;
            }

            if (tuple.Toolset != ToolsetType.MSVC && tuple.Toolset != ToolsetType.ClangCL)
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
                TargetType.Application => $@"{name}.elf",
                _ => throw new ArgumentOutOfRangeException(nameof(targetType)),
            };
        }
    }
}
