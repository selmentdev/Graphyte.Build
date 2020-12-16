using Neobyte.Build.Framework;

namespace Neobyte.Build.Generators.FastBuild
{
    sealed class FastBuildGenerator
        : GeneratorBase
    {
        public FastBuildGeneratorSettings Settings { get; }

        public FastBuildGenerator(Profile profile)
        {
            this.Settings = profile.GetSection<FastBuildGeneratorSettings>();
        }

        public override GeneratorType GeneratorType => GeneratorType.FastBuild;
    }
}
