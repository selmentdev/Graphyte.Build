using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class WindowsPlatformSettings
        : BasePlatformSettings
    {
        public string WindowsSdkVersion { get; set; }
    }

    sealed class WindowsPlatform
        : BaseWindowsPlatform
    {
        public WindowsPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<WindowsPlatformSettings>();
            var version = this.m_Settings.WindowsSdkVersion;

            this.InitializeBasePaths(version);
        }

        public override PlatformType PlatformType => PlatformType.Windows;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Desktop:
                case PlatformKind.Mobile:
                case PlatformKind.Server:
                    return true;
                case PlatformKind.Console:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        public override void PreConfigureTarget(Target target)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
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

        private readonly WindowsPlatformSettings m_Settings;
    }

}
