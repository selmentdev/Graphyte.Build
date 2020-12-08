using System;
using System.IO;
using System.Reflection;

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
        /// <summary>
        /// Gets project name.
        /// </summary>
        public string Name
            => this.GetType().Name;

        protected string ProjectFileName { get; set; }

        /// <summary>
        /// Gets project root path.
        /// </summary>
        public string ProjectRootPath { get; }

        /// <summary>
        /// Gets path to file declaring current project.
        /// </summary>
        public string ProjectFilePath { get; }

        /// <summary>
        /// Gets source location of project declaration.
        /// </summary>
        protected Project()
        {
            var type = this.GetType();
            var location = type.GetCustomAttribute<ProvideSourceLocation>();

            if (location is null)
            {
                throw new Exception($@"Project {type} must provide ProvideSourceLocation attribute");
            }

            this.ProjectRootPath = Path.GetDirectoryName(location.File);
            this.ProjectFilePath = location.File;
        }

        /// <summary>
        /// Configures specified target.
        /// </summary>
        /// <param name="target">Provides target to configure.</param>
        public abstract void Configure(Target target);
    }
}
