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
    }
}
