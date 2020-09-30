using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public sealed class Target
    {
        public Project Project { get; }
        public TargetType Type { get; set; }
        public string Name { get; set; }

        private readonly Dictionary<string, DependencyType> m_Dependencies = new Dictionary<string, DependencyType>();

        public IReadOnlyDictionary<string, DependencyType> Dependencies => this.m_Dependencies;

        public Target(Project project)
        {
            this.Project = project;
            this.Name = project.Name;
        }

        private void AddDependency(Type project, DependencyType dependencyType)
        {
            var name = project.Name;

            if (this.m_Dependencies.TryGetValue(name, out DependencyType existing))
            {
                throw new Exception($@"Cannot add {dependencyType} to project {name}, because it is already specified as {existing} dependency");
            }

            this.m_Dependencies.Add(name, dependencyType);
        }

        public void AddPublicDependency<T>()
            where T : Project
        {
            this.AddDependency(typeof(T), DependencyType.Public);
        }

        public void AddPrivateDependency<T>()
            where T : Project
        {
            this.AddDependency(typeof(T), DependencyType.Private);
        }

        public void AddInterfaceDependency<T>()
            where T : Project
        {
            this.AddDependency(typeof(T), DependencyType.Interface);
        }
    }
}
