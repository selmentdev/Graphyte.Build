using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains.Clang;
using Neobyte.Build.Toolchains.VisualStudio;

namespace Neobyte.Build.Platforms.Windows
{
    [ProfileSection]
    public sealed class WindowsPlatformSettings
    {
        public string? WindowsSdkVersion { get; set; }

        public VisualStudioToolchainSettings? VisualStudio { get; set; }

        public ClangToolchainSettings? Clang { get; set; }

        public ClangCLToolchainSettings? ClangCL { get; set; }
    }
}
