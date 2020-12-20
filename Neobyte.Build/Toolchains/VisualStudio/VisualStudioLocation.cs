namespace Neobyte.Build.Toolchains.VisualStudio
{
    enum VisualStudioProductId
    {
        Enterprise,
        Community,
        Professional,
        BuildTools
    }

    /// <summary>
    /// Represents Visual Studio installaction information.
    /// </summary>
    readonly struct VisualStudioLocation
    {
        /// <summary>
        /// Gets installaction location.
        /// </summary>
        public readonly string Location;

        /// <summary>
        /// Gets name of VS installation.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets target toolkit of VS installation. For example, `v142`.
        /// </summary>
        public readonly string Toolkit;

        /// <summary>
        /// Gets toolset version of VS installation. For example, `14.1.5.1`.
        /// </summary>
        public readonly string Toolset;

        /// <summary>
        /// Gets version of VS installation.
        /// </summary>
        public readonly string ProductVersion;

        public readonly VisualStudioProductId ProductId;

        /// <summary>
        /// Creates new instance of VisualStudioLocation.
        /// </summary>
        /// <param name="location">An location instance.</param>
        /// <param name="name">A name of instance.</param>
        /// <param name="toolkit">A toolkit of instance.</param>
        /// <param name="toolset">A toolset of instance.</param>
        /// <param name="version">A version of instance.</param>
        public VisualStudioLocation(
            string location,
            string name,
            string toolkit,
            string toolset,
            string productVersion,
            VisualStudioProductId productId)
        {
            this.Location = location;
            this.Name = name;
            this.Toolkit = toolkit;
            this.Toolset = toolset;
            this.ProductVersion = productVersion;
            this.ProductId = productId;
        }
    }
}