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

            this.CompilerExecutable = $@"{location}/bin/clang";

            this.LinkerExecutable = $@"{location}/bin/lld";

            this.LibrarianExecutable = $@"{location}/bin/llvm-ar";

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
    }
}
