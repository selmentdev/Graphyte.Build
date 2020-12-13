using Graphyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Toolchains.Clang
{
    public sealed class ClangToolchain
        : ToolchainBase
    {
        public ClangToolchainSettings Settings { get; }
        public TargetPlatform Platform { get; }

        public ClangToolchain(
            Profile profile,
            TargetPlatform platform,
            TargetArchitecture architecture,
            ClangToolchainSettings settings)
            : base(profile, architecture)
        {
            this.Settings = settings;

            this.Platform = platform;

            var location = this.Settings.Location;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.CompilerExecutable = $@"{location}/bin/clang.exe";
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

        public override string FormatDefine(string value)
        {
            return $@"-D{value}";
        }

        public override string FormatLink(string value)
        {
            return $@"-l{value}";
        }

        public override string FormatIncludePath(string value)
        {
            return $@"-I""{value}""";
        }

        public override string FormatLibraryPath(string value)
        {
            return $@"-L""{value}""";
        }

        public override string FormatCompilerInputFile(string input)
        {
            return $@"-c ""{input}""";
        }

        public override string FormatCompilerOutputFile(string output)
        {
            return $@"-o ""{output}""";
        }

        public override string FormatLinkerGroupStart => "-Wl,--start-group ";
        public override string FormatLinkerGroupEnd => " -Wl,--end-group";

        public override TargetToolchain Toolchain => TargetToolchain.Clang;

        public override string FormatLinkerInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLinkerOutputFile(string output)
        {
            return $@"-o ""{output}""";
        }

        public override string FormatLibrarianInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLibrarianOutputFile(string output)
        {
            return $@"""{output}""";
        }

        public override IEnumerable<string> GetCompilerCommandLine(TargetRules target)
        {
            yield return "-std=c++20";
            //yield return "-fconcepts"; -- already in std=c++20

            yield return "-fdiagnostics-color=always";

            if (target.Descriptor.Platform != TargetPlatform.Windows && target.Descriptor.Platform != TargetPlatform.UniversalWindows)
            {
                yield return "-fpic";
                yield return "-stdlib=libc++";
            }

            yield return "-Wno-#pragma-messages";

            yield return "-mavx";
            yield return "-msse4.1";
            yield return "-msse3";
            yield return "-mssse3";
            yield return "-msse2";
            yield return "-m64";
        }
    }
}
