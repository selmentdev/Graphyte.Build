using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Linux
{
    sealed class LinuxPlatform : Platform
    {
        public LinuxPlatform(Profile profile, TargetArchitecture targetArchitecture, LinuxPlatformSettings settings)
            : base(profile, targetArchitecture)
        {
            this.m_Settings = settings;
            _ = this.m_Settings;

            this.IncludePaths = Array.Empty<string>();
            this.LibraryPaths = Array.Empty<string>();
        }

        private readonly LinuxPlatformSettings m_Settings;

        public override TargetPlatform TargetPlatform => TargetPlatform.Linux;

        public override bool IsPlatformKind(TargetPlatformKind platformKind)
        {
            switch (platformKind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Server:
                    return true;
                case TargetPlatformKind.Mobile:
                case TargetPlatformKind.Console:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        public override string AdjustModuleName(string name, ModuleType moduleType)
        {
            switch (moduleType)
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

            throw new ArgumentOutOfRangeException(nameof(moduleType));
        }
    }
}
