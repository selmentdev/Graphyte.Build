using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;

namespace Graphyte.Build.Platforms
{
    public abstract class PlatformFactory
    {
        public TargetPlatform Platform { get; }
        public TargetArchitecture Architecture { get; }
        public TargetToolchain Toolchain { get; }

        protected PlatformFactory(
            TargetPlatform platform,
            TargetArchitecture architecture,
            TargetToolchain toolchain)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolchain = toolchain;
        }

        public abstract PlatformBase CreatePlatform(Profile profile);

        public abstract ToolchainBase CreateToolchain(Profile profile);

        public override string ToString()
        {
            return $@"{this.Platform}-{this.Toolchain}-{this.Architecture}";
        }
    }
}
