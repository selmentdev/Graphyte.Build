using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Generators.FastBuild
{
    sealed class FastBuildGeneratorFactory
        : GeneratorFactory
    {
        public FastBuildGeneratorFactory()
            : base(GeneratorType.FastBuild, new Version(1, 0))
        {
        }

        public override GeneratorBase Create(Profile profile)
        {
            return new FastBuildGenerator(profile);
        }
    }
}
