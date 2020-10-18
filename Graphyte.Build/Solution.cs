using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphyte.Build
{
    /// <summary>
    /// Represents solution.
    /// </summary>
    public abstract class Solution
    {
        private readonly List<Project> m_Projects = new List<Project>();

        /// <summary>
        /// Gets list of projects for this solution.
        /// </summary>
        public IReadOnlyList<Project> Projects => this.m_Projects;

        private string m_Name = null;

        /// <summary>
        /// Gets or sets name of solution.
        /// </summary>
        public string Name
        {
            get => this.m_Name ?? this.GetType().Name;
            set => this.m_Name = value;
        }

        /// <summary>
        /// Adds project to solution.
        /// </summary>
        /// <param name="project">A project to add.</param>
        protected void AddProject(Project project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            this.m_Projects.Add(project);
        }

        /// <summary>
        /// Pre configures target.
        /// </summary>
        /// <param name="target">A target to pre-configure.</param>
        public virtual void PreConfigure(Target target)
        {
        }

        /// <summary>
        /// Post configures target.
        /// </summary>
        /// <param name="target">A target to post-configure.</param>
        public virtual void PostConfigure(Target target)
        {
        }
    }
}

namespace Graphyte.Build
{
    public sealed class SolutionProvider
    {
        private static Solution[] Discover()
        {
            var solutionType = typeof(Solution);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(solutionType) && x.IsClass && !x.IsAbstract && x.IsVisible)
                .Select(x => (Solution)Activator.CreateInstance(x))
                .ToArray();
        }

        private readonly Solution[] m_Solutions;

        public SolutionProvider()
        {
            this.m_Solutions = SolutionProvider.Discover();
        }

        public Solution[] GetSolutions()
        {
            return this.m_Solutions;
        }
    }
}
