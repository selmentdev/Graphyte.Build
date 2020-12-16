using Neobyte.Build.Framework;

namespace Neobyte.Build.Generators.FastBuild
{
    [ProfileSection]
    public sealed class FastBuildGeneratorSettings
    {
        public bool? UnityBuild { get; set; }

        public bool? Distributed { get; set; }

        public bool? Monitor { get; set; }

        public bool? Report { get; set; }

        public string CachePath { get; set; }
    }
}
