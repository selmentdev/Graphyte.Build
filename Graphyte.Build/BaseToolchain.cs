using System.Collections.Generic;

namespace Graphyte.Build
{
    public abstract class BaseToolchainSettings
        : BaseProfileSection
    {
    }

    public abstract class BaseToolchain
    {
        protected BaseToolchain(
            Profile profile,
            ArchitectureType architectureType)
        {
            this.m_Profile = profile;
            this.ArchitectureType = architectureType;
        }

        protected Profile m_Profile;

        public ArchitectureType ArchitectureType { get; }

        public abstract ToolchainType ToolchainType { get; }

        public string[] IncludePaths { get; protected set; }

        public string[] LibraryPaths { get; protected set; }

        public string RootPath { get; protected set; }

        public string CompilerExecutable { get; protected set; }

        public string[] CompilerExtraFiles { get; protected set; }

        public string LinkerExecutable { get; protected set; }

        public string LibrarianExecutable { get; protected set; }

        public virtual string FormatLinkerGroupStart => string.Empty;
        public virtual string FormatLinkerGroupEnd => string.Empty;

        public abstract string FormatDefine(string value);
        public abstract string FormatLink(string value);
        public abstract string FormatIncludePath(string value);
        public abstract string FormatLibraryPath(string value);

        public abstract string FormatCompilerInputFile(string input);
        public abstract string FormatCompilerOutputFile(string output);

        public abstract string FormatLinkerInputFile(string input);
        public abstract string FormatLinkerOutputFile(string output);

        public abstract string FormatLibrarianInputFile(string input);
        public abstract string FormatLibrarianOutputFile(string output);

        public abstract IEnumerable<string> GetCompilerCommandLine(Target target);
    }
}
