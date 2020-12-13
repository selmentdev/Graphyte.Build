namespace Graphyte.Build.Toolchains.VisualStudio
{
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
        /// Gets version of VS installation.
        /// </summary>
        public readonly string Version;

        /// <summary>
        /// Gets target toolkit of VS installation. For example, `v142`.
        /// </summary>
        public readonly string Toolkit;

        /// <summary>
        /// Gets toolset version of VS installation. For example, `14.1.5.1`.
        /// </summary>
        public readonly string Toolset;

        /// <summary>
        /// Creates new instance of VisualStudioLocation.
        /// </summary>
        /// <param name="location">An location instance.</param>
        /// <param name="name">A name of instance.</param>
        /// <param name="version">A version of instance.</param>
        /// <param name="toolkit">A toolkit of instance.</param>
        /// <param name="toolset">A toolset of instance.</param>
        public VisualStudioLocation(
            string location,
            string name,
            string version,
            string toolkit,
            string toolset)
        {
            this.Location = location;
            this.Name = name;
            this.Version = version;
            this.Toolkit = toolkit;
            this.Toolset = toolset;
        }
    }
}
