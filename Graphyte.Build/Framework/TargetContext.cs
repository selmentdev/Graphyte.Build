using Graphyte.Build.Platforms;
using Graphyte.Build.Toolchains;

namespace Graphyte.Build.Framework
{
    public sealed class TargetContext
    {
        public TargetContext(Platform platform, Toolchain toolchain)
        {
            this.Platform = platform;
            this.Toolchain = toolchain;
        }

        public Platform Platform { get; private set; }

        public Toolchain Toolchain { get; private set; }
    }
}
