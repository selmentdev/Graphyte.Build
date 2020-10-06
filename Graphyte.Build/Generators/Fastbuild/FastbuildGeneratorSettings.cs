namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGeneratorSettings
        : BaseGeneratorSettings
    {
        public bool? UnityBuild { get; set; }
        public bool? Distributed { get; set; }
        public string CachePath { get; set; }
    }
}
