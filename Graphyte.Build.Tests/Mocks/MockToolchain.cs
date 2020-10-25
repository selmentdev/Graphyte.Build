using Graphyte.Build.Toolchains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Tests.Mocks
{
    public class MockToolchain
        : BaseToolchain
    {
        public override bool IsHostSupported => true;

        public override ToolchainType Type => ToolchainType.Create("Mock");

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
