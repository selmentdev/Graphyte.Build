using Graphyte.Build.Platforms.Android;
using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Linux
{
    public sealed class AndroidPlatform
        : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private static readonly ArchitectureType[] g_SupportedArchitectures = new[]
        {
            ArchitectureType.ARM,
            ArchitectureType.ARM64,
            ArchitectureType.X64,
            ArchitectureType.X86,
        };

        public override ArchitectureType[] Architectures => AndroidPlatform.g_SupportedArchitectures;

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            switch (platformKind)
            {
                case PlatformKind.Mobile:
                    return true;
                case PlatformKind.Desktop:
                case PlatformKind.Console:
                case PlatformKind.Server:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(platformKind));
        }

        private AndroidPlatformSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<AndroidPlatformSettings>();
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.SharedLibrary:
                    return $@"lib{name}.so";
                case TargetType.StaticLibrary:
                    return $@"lib{name}.a";
                case TargetType.HeaderLibrary:
                case TargetType.Application:
                    return name;
                case TargetType.Default:
                    break;
            };

            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
    }
}
