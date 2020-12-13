using Graphyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Toolchains.Clang
{
    public sealed class ClangCLToolchain
        : ToolchainBase
    {
        public ClangCLToolchainSettings Settings { get; }

        public TargetPlatform Platform { get; }

        public ClangCLToolchain(
            Profile profile,
            TargetPlatform platform,
            TargetArchitecture architecture,
            ClangCLToolchainSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;

            this.Platform = platform;

            var location = this.Settings.Location;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.CompilerExecutable = $@"{location}/bin/clang-cl.exe";
                this.LinkerExecutable = $@"{location}/bin/lld.exe";
                this.LibrarianExecutable = $@"{location}/bin/llvm-ar.exe";
            }
            else
            {
                this.CompilerExecutable = $@"{location}/bin/clang";
                this.LinkerExecutable = $@"{location}/bin/lld";
                this.LibrarianExecutable = $@"{location}/bin/llvm-ar";
            }

            this.IncludePaths = Array.Empty<string>();

            this.LibraryPaths = Array.Empty<string>();
        }

        public override TargetToolchain Toolchain => TargetToolchain.ClangCL;

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

        public override string FormatLinkerGroupStart => "-Wl,--start-group ";
        public override string FormatLinkerGroupEnd => " -Wl,--end-group";

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

        public override IEnumerable<string> GetCompilerCommandLine(TargetRules target)
        {
            yield break;
        }
    }
}
