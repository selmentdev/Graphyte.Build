using System.Runtime.InteropServices;

namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGeneratorSettings
        : BaseGeneratorSettings
    {
        public bool? UnityBuild { get; set; }
        public bool? Distributed { get; set; }
        public string CachePath { get; set; }
    }

    sealed class FastbuildGenerator
        : BaseGenerator
    {
        public FastbuildGenerator(Profile profile)
            : base(profile)
        {
            this.m_Settings = profile.GetSection<FastbuildGeneratorSettings>();
        }

        public override GeneratorType GeneratorType => GeneratorType.FastBuild;

        private readonly FastbuildGeneratorSettings m_Settings;
    }
}
