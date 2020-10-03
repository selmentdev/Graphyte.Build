namespace Graphyte.Build
{
    public interface IContext
    {
        PlatformType Platform { get; }
        ArchitectureType Architecture { get; }
        BuildType Build { get; }
        ConfigurationType Configuration { get; }
    }

    public class Context : IContext
    {
        public PlatformType Platform { get; }
        public ArchitectureType Architecture { get; }
        public BuildType Build { get; }
        public ConfigurationType Configuration { get; }

        public Context(
            PlatformType platform,
            ArchitectureType architecture,
            BuildType build,
            ConfigurationType configuration)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Build = build;
            this.Configuration = configuration;
        }

        public override string ToString()
        {
            return $@"{this.Platform} {this.Architecture} {this.Build} {this.Configuration}";
        }
    }
}
