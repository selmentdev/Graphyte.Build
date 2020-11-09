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
    }
}
