using Neobyte.Build.Platforms;
using Neobyte.Build.Toolchains;

namespace Neobyte.Build.Framework
{
    public sealed class TargetContext
    {
        public TargetContext(PlatformBase platform, ToolchainBase toolchain)
        {
            this.Platform = platform;
            this.Toolchain = toolchain;
        }

        public PlatformBase Platform { get; private set; }

        public ToolchainBase Toolchain { get; private set; }
    }
}
