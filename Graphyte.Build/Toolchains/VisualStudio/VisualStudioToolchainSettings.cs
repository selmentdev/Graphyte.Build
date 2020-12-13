namespace Graphyte.Build.Toolchains.VisualStudio
{
    public sealed class VisualStudioToolchainSettings
    {
        public string Toolkit { get; set; } = "v142";
        public bool AddressSanitizer { get; set; } = false;
        public bool StaticAnalyzer { get; set; } = false;
    }
}
