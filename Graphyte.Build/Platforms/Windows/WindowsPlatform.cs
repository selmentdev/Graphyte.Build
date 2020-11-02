using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private string m_Location = null;
        private string m_Version = null;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<WindowsPlatformSettings>();

            if (WindowsSdkProvider.IsSupported)
            {
                this.m_Location = WindowsSdkProvider.Location;
                this.m_Version = this.m_Settings.WindowsSdkVersion;

                var available = WindowsSdkProvider.Versions;

                if (string.IsNullOrEmpty(this.m_Version))
                {
                    throw new Exception($@"Windows SDK with version ""{this.m_Version}"" is not available");
                }

                if (!available.Contains(this.m_Version))
                {
                    throw new Exception($@"Windows SDK with version ""{this.m_Version}"" is not available");
                }
            }
        }

        public override string[] GetIncludePaths(ArchitectureType architectureType)
        {
            return new[]
            {
                $@"{this.m_Location}\Include\{this.m_Version}\shared",
                $@"{this.m_Location}\Include\{this.m_Version}\ucrt",
                $@"{this.m_Location}\Include\{this.m_Version}\um",
                $@"{this.m_Location}\Include\{this.m_Version}\winrt",
                $@"{this.m_Location}\Include\{this.m_Version}\cppwinrt",
            };
        }

        public override string[] GetLibraryPaths(ArchitectureType architectureType)
        {
            switch (architectureType)
            {
                case ArchitectureType.X64:
                    return new[]
                    {
                        $@"{this.m_Location}\Lib\{this.m_Version}\um\x64",
                        $@"{this.m_Location}\Lib\{this.m_Version}\ucrt\x64",
                    };

                case ArchitectureType.X86:
                    return new[]
                    {
                        $@"{this.m_Location}\Lib\{this.m_Version}\um\x86",
                        $@"{this.m_Location}\Lib\{this.m_Version}\ucrt\x86",
                    };

                case ArchitectureType.ARM:
                    return new[]
                    {
                        $@"{this.m_Location}\Lib\{this.m_Version}\um\arm",
                        $@"{this.m_Location}\Lib\{this.m_Version}\ucrt\arm",
                    };

                case ArchitectureType.ARM64:
                    return new[]
                    {
                        $@"{this.m_Location}\Lib\{this.m_Version}\um\arm64",
                        $@"{this.m_Location}\Lib\{this.m_Version}\ucrt\arm64",
                    };

                case ArchitectureType.PPC64:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(architectureType));
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
