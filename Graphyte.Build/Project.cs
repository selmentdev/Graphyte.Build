namespace Graphyte.Build
{
    /// <summary>
    /// Specifies source code language of project.
    /// </summary>
    public enum ProjectLanguage
    {
        C,
        CPlusPlus,
        CSharp,
    }

    public abstract class Project
    {
        private string m_Name;

        /// <summary>
        /// Gets project name.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.m_Name == null)
                {
                    return this.GetType().Name;
                }
                return this.m_Name;
            }

            protected set => this.m_Name = value;
        }

        protected string ProjectFileName { get; set; }

        /// <summary>
        /// Configures specified target.
        /// </summary>
        /// <param name="target">Provides target to configure.</param>
        public abstract void Configure(Target target);
    }
}
