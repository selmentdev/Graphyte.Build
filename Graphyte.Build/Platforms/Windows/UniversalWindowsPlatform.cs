using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Windows
{
    sealed class UniversalWindowsPlatform
        : BaseWindowsPlatform
    {
        public UniversalWindowsPlatform(
            Profile profile,
            TargetArchitecture targetArchitecture,
            UniversalWindowsPlatformSettings settings)
            : base(profile, targetArchitecture)
        {
            this.m_Settings = settings;

            var version = this.m_Settings.WindowsSdkVersion;

            this.InitializeBasePath(version);
        }

        private readonly UniversalWindowsPlatformSettings m_Settings;

        public override TargetPlatform TargetPlatform => TargetPlatform.UniversalWindows;

        public override bool IsPlatformKind(TargetPlatformKind platformKind)
        {
            switch (platformKind)
            {
                case TargetPlatformKind.Desktop:
                case TargetPlatformKind.Mobile:
                    return true;
                case TargetPlatformKind.Console:
                case TargetPlatformKind.Server:
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
