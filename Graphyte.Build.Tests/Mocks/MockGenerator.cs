using Graphyte.Build.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Tests.Mocks
{
    public sealed class MockGenerator
        : BaseGenerator
    {
        public override bool IsHostSupported => true;

        public override GeneratorType Type => GeneratorType.Create("Mock");

        public override void Initialize(Profile profile)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
        }

        public override void PreConfigureTarget(Target target)
        {
        }
    }
}
