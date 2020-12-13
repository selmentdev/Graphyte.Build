using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Platforms.Android
{
    sealed class AndroidPlatform : Platform
    {
        private readonly AndroidPlatformSettings m_Settings;

        public AndroidPlatform(Profile profile, TargetArchitecture targetArchitecture, AndroidPlatformSettings settings)
            : base(profile, targetArchitecture)
        {
            this.m_Settings = settings;
            _ = this.m_Settings;
        }

        public override TargetPlatform TargetPlatform => TargetPlatform.Android;

        public override bool IsPlatformKind(TargetPlatformKind platformKind)
        {
            switch (platformKind)
            {
                case TargetPlatformKind.Mobile:
                    return true;
                case TargetPlatformKind.Desktop:
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
                    return $@"lib{name}.so";
                case ModuleType.StaticLibrary:
                    return $@"lib{name}.a";
                case ModuleType.ExternLibrary:
                case ModuleType.Application:
                    return name;
                case ModuleType.Default:
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(moduleType));
        }
    }
}
