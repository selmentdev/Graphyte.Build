namespace Graphyte.Build.Toolchains.VisualStudio
{
    public sealed class VisualStudioToolchainSettings
        : BaseToolchainSettings
    {
        public string Version { get; set; } = "v142";

        public bool AddressSanitizer { get; set; } = false;
        public bool StaticAnalyzer { get; set; } = false;
    }
}
