using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains.Clang;

namespace Neobyte.Build.Platforms.Linux
{
    [ProfileSection]
    public sealed class LinuxPlatformSettings
    {
        public ClangToolchainSettings Clang { get; set; }
    }
}
