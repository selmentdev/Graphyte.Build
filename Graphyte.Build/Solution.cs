using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphyte.Build
{
    public abstract class Solution
    {
        private readonly List<Project> m_Projects = new List<Project>();
        public IReadOnlyList<Project> Projects => this.m_Projects;

        private string m_Name = null;
        public string Name
        {
            get => this.m_Name ?? this.GetType().Name;
            set => this.m_Name = value;
        }

        protected void AddProject(Project project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            this.m_Projects.Add(project);
        }

        public virtual void PreConfigure(Target target)
        {
        }

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
                .Where(x => x.IsSubclassOf(solutionType) && x.IsClass && !x.IsAbstract && x.IsVisible && x.IsSealed)
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
