using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Linux
{
    sealed class LinuxPlatform
        : PlatformBase
    {
        public LinuxPlatform(Profile profile, TargetArchitecture architecture, LinuxPlatformSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;

            this.IncludePaths = Array.Empty<string>();
            this.LibraryPaths = Array.Empty<string>();
        }

        public LinuxPlatformSettings Settings { get; }

        public override TargetPlatform Platform => TargetPlatform.Linux;

        public override bool IsPlatformKind(TargetPlatformKind kind)
        {
            switch (kind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Server:
                    return true;
                case TargetPlatformKind.Mobile:
                case TargetPlatformKind.Console:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        public override string AdjustModuleName(string name, ModuleType type)
        {
            switch (type)
            {
                case ModuleType.SharedLibrary:
                    return $@"lib{name}.so";
                case ModuleType.StaticLibrary:
                    return $@"lib{name}.a";
                case ModuleType.ExternLibrary:
                    return name;
                case ModuleType.Application:
                    return $@"{name}.elf";
                case ModuleType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
