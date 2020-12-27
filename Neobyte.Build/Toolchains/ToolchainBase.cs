using Neobyte.Build.Framework;
using System;
using System.Collections.Generic;

namespace Neobyte.Build.Toolchains
{
    public abstract class ToolchainBase
    {
        protected ToolchainBase(Profile profile, TargetArchitecture architecture)
        {
            this.Profile = profile;
            this.Architecture = architecture;
        }

        public Profile Profile { get; }

        public TargetArchitecture Architecture { get; }

        public abstract TargetToolchain Toolchain { get; }

        public string[] IncludePaths { get; protected set; } = Array.Empty<string>();

        public string[] LibraryPaths { get; protected set; } = Array.Empty<string>();

        public string RootPath { get; protected set; } = string.Empty;

        public string CompilerExecutable { get; protected set; } = string.Empty;

        public string[] CompilerExtraFiles { get; protected set; } = Array.Empty<string>();

        public string LinkerExecutable { get; protected set; } = string.Empty;

        public string LibrarianExecutable { get; protected set; } = string.Empty;

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

        public abstract IEnumerable<string> GetCompilerCommandLine(TargetRules target);
    }
}
