using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains.Clang;

namespace Neobyte.Build.Platforms.Android
{
    [ProfileSection]
    public sealed class AndroidPlatformSettings
    {
        public string? SdkPath { get; set; }

        public string? NdkPath { get; set; }

        public int? TargetApiLevel { get; set; }

        public ClangToolchainSettings? Clang { get; set; }
    }
}
