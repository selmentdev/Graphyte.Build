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
    }
}
