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
        public readonly TargetFlavor Flavor;

        public TargetDescriptor(
            TargetPlatform platform,
            TargetArchitecture architecture,
            TargetToolchain toolchain,
            TargetConfiguration configuration,
            TargetFlavor flavor)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolchain = toolchain;
            this.Configuration = configuration;
            this.Flavor = flavor;
        }

        public override string ToString()
        {
            if (this.Flavor == TargetFlavor.Game)
            {
                return $@"{this.Platform}-{this.Toolchain}-{this.Architecture}-{this.Configuration}";
            }
            else
            {
                return $@"{this.Platform}-{this.Toolchain}-{this.Architecture}-{this.Configuration}-{this.Flavor}";
            }
        }
    }
}
