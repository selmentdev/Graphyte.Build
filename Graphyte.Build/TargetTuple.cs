namespace Graphyte.Build
{
    public readonly struct TargetTuple
    {
        public readonly PlatformType PlatformType;
        public readonly ArchitectureType ArchitectureType;
        public readonly ToolchainType ToolchainType;
        public readonly ConfigurationType ConfigurationType;
        public readonly ConfigurationFlavour ConfigurationFlavour;

        public TargetTuple(
            PlatformType platformType,
            ArchitectureType architectureType,
            ToolchainType toolchainType,
            ConfigurationType configurationType,
            ConfigurationFlavour configurationFlavour)
        {
            this.PlatformType = platformType;
            this.ArchitectureType = architectureType;
            this.ToolchainType = toolchainType;
            this.ConfigurationType = configurationType;
            this.ConfigurationFlavour = configurationFlavour;
        }

        public override string ToString()
        {
            if (this.ConfigurationFlavour != ConfigurationFlavour.None)
            {
                return $@"{this.PlatformType}-{this.ToolchainType}-{this.ArchitectureType}-{this.ConfigurationType}-{this.ConfigurationFlavour}";
            }
            else
            {
                return $@"{this.PlatformType}-{this.ToolchainType}-{this.ArchitectureType}-{this.ConfigurationType}";
            }
        }
    }
}
