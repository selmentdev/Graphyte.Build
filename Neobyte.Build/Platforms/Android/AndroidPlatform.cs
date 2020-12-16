using Neobyte.Build.Framework;
using System;

namespace Neobyte.Build.Platforms.Android
{
    sealed class AndroidPlatform
        : PlatformBase
    {
        public AndroidPlatformSettings Settings { get; }

        public AndroidPlatform(Profile profile, TargetArchitecture architecture, AndroidPlatformSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;
        }

        public override TargetPlatform Platform => TargetPlatform.Android;

        public override bool IsPlatformKind(TargetPlatformKind kind)
        {
            switch (kind)
            {
                case TargetPlatformKind.Mobile:
                    return true;
                case TargetPlatformKind.Desktop:
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

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
