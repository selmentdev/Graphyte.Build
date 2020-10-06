namespace Graphyte.Build
{
    public interface IContext
    {
        PlatformType Platform { get; }
        ArchitectureType Architecture { get; }
        ToolsetType Toolset { get; }
        BuildType Build { get; }
        ConfigurationType Configuration { get; }
    }

    public class Context : IContext
    {
        public PlatformType Platform { get; }
        public ArchitectureType Architecture { get; }
        public ToolsetType Toolset { get; }
        public BuildType Build { get; }
        public ConfigurationType Configuration { get; }

        public Context(
            PlatformType platform,
            ArchitectureType architecture,
            ToolsetType toolset,
            BuildType build,
            ConfigurationType configuration)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolset = toolset;
            this.Build = build;
            this.Configuration = configuration;
        }

        public override string ToString()
        {
            return $@"{this.Platform} {this.Architecture} {this.Toolset} {this.Build} {this.Configuration}";
        }
    }
}
