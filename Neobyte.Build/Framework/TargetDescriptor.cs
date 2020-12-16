namespace Neobyte.Build.Framework
{
    /// <summary>
    /// Descibres currently used target. Combined with TargetContext gives proper set of toolchains and rules.
    /// </summary>
    public readonly struct TargetDescriptor
    {
        public readonly TargetPlatform Platform;
        public readonly TargetArchitecture Architecture;
        public readonly TargetToolchain Toolchain;
        public readonly TargetConfiguration Configuration;

        public TargetDescriptor(
            TargetPlatform platform,
            TargetArchitecture architecture,
            TargetToolchain toolchain,
            TargetConfiguration configuration)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolchain = toolchain;
            this.Configuration = configuration;
        }

        public override string ToString()
        {
            return $@"{this.Platform}-{this.Toolchain}-{this.Architecture}-{this.Configuration}";
        }
    }
}
