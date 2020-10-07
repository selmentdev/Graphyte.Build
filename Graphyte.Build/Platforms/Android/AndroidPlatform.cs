using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public sealed class AndroidPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public override bool IsSupported(TargetTuple targetTuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (targetTuple.Platform != Platform.Android)
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
                case TargetType.Application:
                    return name;
                case TargetType.Default:
                    break;
            };

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
    }
}
