using Neobyte.Build.Framework;
using Neobyte.Build.Toolchains.VisualStudio;

namespace Neobyte.Build.Platforms.Windows
{
    [ProfileSection]
    public sealed class UniversalWindowsPlatformSettings
    {
        public string? WindowsSdkVersion { get; set; }

        public VisualStudioToolchainSettings? VisualStudio { get; set; }
    }
}
