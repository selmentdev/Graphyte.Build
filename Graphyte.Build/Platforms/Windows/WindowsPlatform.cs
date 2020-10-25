using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class WindowsPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static readonly ArchitectureType[] g_SupportedArchitectures = new[]
        {
            ArchitectureType.X64,
            ArchitectureType.ARM64,
        };
        public override ArchitectureType[] Architectures => WindowsPlatform.g_SupportedArchitectures;

        public override PlatformType Type => PlatformType.Windows;

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

        private WindowsPlatformSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<WindowsPlatformSettings>();
        }

        public override void PreConfigureTarget(Target target)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
            Trace.Assert(target.TargetType != TargetType.Default);
            Trace.WriteLine($@"{target.Name} {this.AdjustTargetName(target.Name, target.TargetType)} {target.TargetType} {target.ModuleType}");
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
