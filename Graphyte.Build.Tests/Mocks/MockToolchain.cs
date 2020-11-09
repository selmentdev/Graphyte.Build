using Graphyte.Build.Toolchains;

namespace Graphyte.Build.Tests.Mocks
{
    public class MockToolchain
        : BaseToolchain
    {
        public MockToolchain(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
        }

        public override ToolchainType ToolchainType => ToolchainType.Create("Mock");

        public override void PostConfigureTarget(Target target)
        {
        }

        public override void PreConfigureTarget(Target target)
        {
        }
    }
}
