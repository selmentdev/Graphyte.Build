using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class WindowsPlatform
        : BaseWindowsPlatform
    {
        public WindowsPlatform(
            Profile profile,
            TargetArchitecture architecture,
            WindowsPlatformSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;
            var version = this.Settings.WindowsSdkVersion;

            this.InitializeBasePath(version);
        }

        public WindowsPlatformSettings Settings { get; }

        public override TargetPlatform Platform => TargetPlatform.Windows;

        public override bool IsPlatformKind(TargetPlatformKind kind)
        {
            switch (kind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Mobile:
                case TargetPlatformKind.Server:
                    return true;
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
