using Graphyte.Build.Toolchains;

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
