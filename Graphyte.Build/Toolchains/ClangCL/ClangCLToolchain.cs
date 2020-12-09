using System.Collections.Generic;

namespace Graphyte.Build.Toolchains.ClangCL
{
    public sealed class ClangCLToolchainSettings
        : BaseToolchainSettings
    {
        public string Location { get; set; }
        public string Version { get; set; }
    }

    public sealed class ClangCLToolchain
        : BaseToolchain
    {
        public ClangCLToolchain(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<ClangCLToolchainSettings>();

            var location = this.m_Settings.Location;

            this.CompilerExecutable = $@"{location}/bin/clang";

            this.LinkerExecutable = $@"{location}/bin/lld";

            this.LibrarianExecutable = $@"{location}/bin/llvm-ar";
        }

        private readonly ClangCLToolchainSettings m_Settings;

        public override ToolchainType ToolchainType => ToolchainType.ClangCL;

        public override string FormatDefine(string value)
        {
            return $@"/D{value}";
        }

        public override string FormatLink(string value)
        {
            return value;
        }

        public override string FormatIncludePath(string value)
        {
            return $@"/I""{value}""";
        }

        public override string FormatLibraryPath(string value)
        {
            return $@"/LIBPATH:""{value}""";
        }

        public override string FormatCompilerInputFile(string input)
        {
            return $@"/c ""{input}""";
        }

        public override string FormatCompilerOutputFile(string output)
        {
            return $@"/Fo""{output}""";
        }

        public override string FormatLinkerInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLinkerOutputFile(string output)
        {
            return $@"/OUT:""{output}""";
        }

        public override string FormatLibrarianInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLibrarianOutputFile(string output)
        {
            return $@"/OUT:""{output}""";
        }

        public override IEnumerable<string> GetCompilerCommandLine(Target target)
        {
            yield break;
        }
    }
}
