namespace Graphyte.Build.Toolchains.VisualStudio
{
    /// <summary>
    /// Represents Visual Studio Toolchain settings.
    /// </summary>
    public sealed class VisualStudioToolchainSettings
        : BaseToolchainSettings
    {
        /// <summary>
        /// Gets or sets version of toolkit.
        /// </summary>
        public string Version { get; set; } = "v142";

        /// <summary>
        /// Gets or sets value indicating whether Address Sanitizer should be used.
        /// </summary>
        public bool AddressSanitizer { get; set; } = false;

        /// <summary>
        /// Gets or sets value indicating whether static analyzer should be used.
        /// </summary>
        public bool StaticAnalyzer { get; set; } = false;
    }
}
