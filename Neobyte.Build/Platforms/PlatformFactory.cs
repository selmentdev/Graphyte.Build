using Neobyte.Build.Framework;

namespace Neobyte.Build.Platforms
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

        public abstract TargetContext CreateContext(Profile profile);

        public override string ToString()
        {
            return $@"{this.Platform}-{this.Toolchain}-{this.Architecture}";
        }
    }
}
