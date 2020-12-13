using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class UniversalWindowsPlatform
        : BaseWindowsPlatform
    {
        public UniversalWindowsPlatform(
            Profile profile,
            TargetArchitecture architecture,
            UniversalWindowsPlatformSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;

            var version = this.Settings.WindowsSdkVersion;

            this.InitializeBasePath(version);
        }

        public UniversalWindowsPlatformSettings Settings { get; }

        public override TargetPlatform Platform => TargetPlatform.UniversalWindows;

        public override bool IsPlatformKind(TargetPlatformKind kind)
        {
            switch (kind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Mobile:
                    return true;
                case TargetPlatformKind.Console:
                case TargetPlatformKind.Server:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        public override string AdjustModuleName(string name, ModuleType type)
        {
            switch (type)
            {
                case ModuleType.SharedLibrary:
                    return $@"lib{name}.dll";
                case ModuleType.StaticLibrary:
                    return $@"lib{name}.lib";
                case ModuleType.ExternLibrary:
                    return name;
                case ModuleType.Application:
                    return $@"{name}.exe";
                case ModuleType.Default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
