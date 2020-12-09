using System.Collections.Generic;

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

        public override string FormatCompilerInputFile(string input)
        {
            return input;
        }

        public override string FormatCompilerOutputFile(string output)
        {
            return output;
        }

        public override string FormatLinkerInputFile(string input)
        {
            return input;
        }

        public override string FormatLinkerOutputFile(string output)
        {
            return output;
        }

        public override string FormatLibrarianInputFile(string input)
        {
            return input;
        }

        public override string FormatLibrarianOutputFile(string output)
        {
            return output;
        }

        public override IEnumerable<string> GetCompilerCommandLine(Target target)
        {
            yield break;
        }
    }
}
