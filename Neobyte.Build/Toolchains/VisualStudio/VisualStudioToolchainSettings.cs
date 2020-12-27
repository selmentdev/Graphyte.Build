namespace Neobyte.Build.Toolchains.VisualStudio
{
    public sealed class VisualStudioToolchainSettings
    {
        public string? Toolkit { get; set; } = "v142";

        public bool? AddressSanitizer { get; set; }

        public bool? StaticAnalyzer { get; set; }
    }
}
