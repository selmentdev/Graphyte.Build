using Graphyte.Build.Evaluation;
using Graphyte.Build.Generators;
using System;
using System.Collections.Generic;

namespace Graphyte.Build.Tests.Mocks
{
    sealed class MockGenerator
        : BaseGenerator
    {
        public MockGenerator(Profile profile)
            : base(profile)
        {
        }

        public override GeneratorType GeneratorType => MockGeneratorFactory.Mock;

        public override void Generate(string outputPath, PlatformType platformType, ToolchainType toolchainType, Solution solution, EvaluatedSolution[] evaluatedSolutions)
        {
        }
    }

    sealed class MockGeneratorFactory
        : BaseGeneratorFactory
    {
        public static GeneratorType Mock = GeneratorType.Create("Mock");

        public MockGeneratorFactory()
            : base(MockGeneratorFactory.Mock, new Version(1, 0))
        {
        }

        public override BaseGenerator Create(Profile profile)
        {
            return new MockGenerator(profile);
        }
    }

    sealed class MockGeneratorsProvider
        : IGeneratorsProvider
    {
        public IEnumerable<BaseGeneratorFactory> Provide()
        {
            yield return new MockGeneratorFactory();
        }
    }
}
