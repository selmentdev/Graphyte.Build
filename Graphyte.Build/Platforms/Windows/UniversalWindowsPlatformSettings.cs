using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains.VisualStudio;

namespace Graphyte.Build.Platforms.Windows
{
    [ProfileSection]
    public sealed class UniversalWindowsPlatformSettings
    {
        public string WindowsSdkVersion { get; set; }
        public VisualStudioToolchainSettings VisualStudio { get; set; }
    }
}
