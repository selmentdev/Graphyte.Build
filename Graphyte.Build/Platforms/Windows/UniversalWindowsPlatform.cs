using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class UniversalWindowsPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static readonly ArchitectureType[] g_SupportedArchitectures = new[]
        {
            ArchitectureType.X64,
            ArchitectureType.ARM64,
        };
        public override ArchitectureType[] Architectures => UniversalWindowsPlatform.g_SupportedArchitectures;

        public override PlatformType Type => PlatformType.UniversalWindows;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Desktop:
                case PlatformKind.Mobile:
                    return true;
                case PlatformKind.Console:
                case PlatformKind.Server:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        private UniversalWindowsPlatformSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<UniversalWindowsPlatformSettings>();
        }

        public override string[] GetIncludePaths(ArchitectureType architectureType)
        {
            throw new NotImplementedException();
        }

        public override string[] GetLibraryPaths(ArchitectureType architectureType)
        {
            throw new NotImplementedException();
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
    }
}
