using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class WindowsPlatform
        : BaseWindowsPlatform
    {
        public WindowsPlatform(
            Profile profile,
            TargetArchitecture targetArchitecture,
            WindowsPlatformSettings settings)
            : base(profile, targetArchitecture)
        {
            this.m_Settings = settings;
            var version = this.m_Settings.WindowsSdkVersion;

            this.InitializeBasePath(version);
        }

        private readonly WindowsPlatformSettings m_Settings;

        public override TargetPlatform TargetPlatform => TargetPlatform.Windows;

        public override bool IsPlatformKind(TargetPlatformKind platformKind)
        {
            switch (platformKind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Mobile:
                case TargetPlatformKind.Server:
                    return true;
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

            throw new ArgumentOutOfRangeException(nameof(moduleType));
        }
    }
}
