using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains.Clang;

namespace Graphyte.Build.Platforms.Linux
{
    [ProfileSection]
    public sealed class LinuxPlatformSettings
    {
        public ClangToolchainSettings Clang { get; set; }
    }
}
