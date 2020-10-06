namespace Graphyte.Build
{
    public readonly struct TargetTuple
    {
        public readonly Platform Platform;
        public readonly Architecture Architecture;
        public readonly Compiler Compiler;
        public readonly Configuration Configuration;

        public TargetTuple(
            Platform platform,
            Architecture architecture,
            Compiler compiler,
            Configuration configuration)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Compiler = compiler;
            this.Configuration = configuration;
        }
    }
}
