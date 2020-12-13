using Graphyte.Build.Framework;

namespace Graphyte.Build.Generators.FastBuild
{
    sealed class FastBuildGenerator : Generator
    {
        private readonly FastBuildGeneratorSettings m_Settings;

        public FastBuildGenerator(Profile profile)
        {
            this.m_Settings = profile.GetSection<FastBuildGeneratorSettings>();
            _ = this.m_Settings;
        }

        public override GeneratorType GeneratorType => GeneratorType.FastBuild;
    }
}
