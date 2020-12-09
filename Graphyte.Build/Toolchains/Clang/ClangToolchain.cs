using System.Collections.Generic;

namespace Graphyte.Build.Toolchains.Clang
{
    public sealed class ClangToolchainSettings
        : BaseToolchainSettings
    {
        public string Location { get; set; }
        public string Version { get; set; }
        public bool AddressSanitizer { get; set; }
        public bool ThreadSanitizer { get; set; }
        public bool MemorySanitizer { get; set; }
        public bool UndefinedBehaviorSanitizer { get; set; }

        public bool TimeTrace { get; set; }

        public bool PgoOptimize { get; set; }
        public bool PgoProfile { get; set; }
        public string PgoDirectory { get; set; }
        public string PgoPrefix { get; set; }
    }

    public sealed class ClangToolchain
        : BaseToolchain
    {
        public ClangToolchain(
            Profile profile,
            ArchitectureType architectureType,
            PlatformType platformType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<ClangToolchainSettings>();

            this.m_PlatformType = platformType;

            var location = this.m_Settings.Location;

            this.CompilerExecutable = $@"{location}/bin/clang.exe";

            this.LinkerExecutable = $@"{location}/bin/lld.exe";

            this.LibrarianExecutable = $@"{location}/bin/llvm-ar.exe";

            this.IncludePaths = new string[] {};
            
            this.LibraryPaths = new string[] {};
        }

        private readonly PlatformType m_PlatformType;

        public override ToolchainType ToolchainType => ToolchainType.Clang;

        private readonly ClangToolchainSettings m_Settings;

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

        public override IEnumerable<string> GetCompilerCommandLine(Target target)
        {
            yield return "-std=c++20";
            //yield return "-fconcepts"; -- already in std=c++20

            yield return "-fdiagnostics-color=always";

            if (target.PlatformType != PlatformType.Windows && target.PlatformType != PlatformType.UniversalWindows)
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
