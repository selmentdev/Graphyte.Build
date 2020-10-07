using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public sealed class LinuxPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public override bool IsSupported(TargetTuple targetTuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (targetTuple.Platform != Platform.Linux)
            {
                return false;
            }

            if (targetTuple.Compiler != Compiler.Clang)
            {
                return false;
            }

            return true;
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.SharedLibrary:
                    return $@"lib{name}.so";
                case TargetType.StaticLibrary:
                    return $@"lib{name}.a";
                case TargetType.HeaderLibrary:
                    return name;
                case TargetType.Application:
                    return $@"{name}.elf";
                case TargetType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
    }
}
