using Graphyte.Build.Platforms;
using Graphyte.Build.Toolchains;

namespace Graphyte.Build
{
    public readonly struct TargetTuple
    {
        public readonly PlatformType Platform;
        public readonly ArchitectureType Architecture;
        public readonly ToolchainType Toolchain;
        public readonly ConfigurationType Configuration;

        public TargetTuple(
            PlatformType platform,
            ArchitectureType architecture,
            ToolchainType toolchain,
            ConfigurationType configuration)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolchain = toolchain;
            this.Configuration = configuration;
        }
    }
}
