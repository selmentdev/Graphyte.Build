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

        public override string FormatDefine(string value)
        {
            return value;
        }

        public override string FormatIncludePath(string value)
        {
            return value;
        }

        public override string FormatLibraryPath(string value)
        {
            return value;
        }

        public override string FormatLink(string value)
        {
            return value;
        }
    }
}
