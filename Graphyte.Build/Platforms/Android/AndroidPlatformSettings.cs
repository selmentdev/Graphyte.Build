using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains.Clang;

namespace Graphyte.Build.Platforms.Android
{
    [ProfileSection]
    public sealed class AndroidPlatformSettings
    {
        public string SdkPath { get; set; }
        public string NdkPath { get; set; }
        public int TargetApiLevel { get; set; }

        public ClangToolchainSettings Clang { get; set; }
    }
}
