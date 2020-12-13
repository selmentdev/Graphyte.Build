using Graphyte.Build.Framework;
using Graphyte.Build.Toolchains;

namespace Graphyte.Build.Platforms
{
    public abstract class PlatformFactory
    {
        public TargetPlatform TargetPlatform { get; }
        public TargetArchitecture TargetArchitecture { get; }
        public TargetToolchain TargetToolchain { get; }

        protected PlatformFactory(
            TargetPlatform targetPlatform,
            TargetArchitecture targetArchitecture,
            TargetToolchain targetToolchain)
        {
            this.TargetPlatform = targetPlatform;
            this.TargetArchitecture = targetArchitecture;
            this.TargetToolchain = targetToolchain;
        }

        public abstract Platform CreatePlatform(Profile profile);
        public abstract Toolchain CreateToolchain(Profile profile);

        public override string ToString()
        {
            return $@"{this.TargetPlatform}-{this.TargetToolchain}-{this.TargetArchitecture}";
        }
    }
}
