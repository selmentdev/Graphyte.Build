using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class WindowsPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public override bool IsSupported(TargetTuple targetTuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (targetTuple.Platform != Platform.Windows)
            {
                return false;
            }

            switch (targetTuple.Compiler)
            {
                case Compiler.ClangCL:
                case Compiler.MSVC:
                case Compiler.Clang:
                case Compiler.Intel:
                    return true;
                case Compiler.GCC:
                    return false;
                case Compiler.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(targetTuple));
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.SharedLibrary:
                    return $@"lib{name}.dll";
                case TargetType.StaticLibrary:
                    return $@"lib{name}.lib";
                case TargetType.HeaderLibrary:
                    return name;
                case TargetType.Application:
                    return $@"{name}.exe";
                case TargetType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
    }
}
