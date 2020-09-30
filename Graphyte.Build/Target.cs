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

        public List<string> PublicDependencies { get; } = new List<string>();
        public List<string> PrivateDependencies { get; } = new List<string>();
        public List<string> InterfaceDependencies { get; } = new List<string>();

        public List<string> PublicIncludePaths { get; } = new List<string>();
        public List<string> PrivateIncludePaths { get; } = new List<string>();
        public List<string> InterfaceIncludePaths { get; } = new List<string>();

        public List<string> PublicLibraryPaths { get; } = new List<string>();
        public List<string> PrivateLibraryPaths { get; } = new List<string>();
        public List<string> InterfaceLibraryPaths { get; } = new List<string>();

        public List<string> PublicLibraries { get; } = new List<string>();
        public List<string> PrivateLibraries { get; } = new List<string>();
        public List<string> InterfaceLibraries { get; } = new List<string>();

        public Dictionary<string, string> PublicDefines { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> PrivateDefines { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> InterfaceDefines { get; } = new Dictionary<string, string>();

        public Target(Project project)
        {
            this.Project = project;
            this.Name = project.Name;
        }

        public void AddPublicDependency<T>()
            where T : Project
        {
            this.PublicDependencies.Add(typeof(T).Name);
        }

        public void AddPrivateDependency<T>()
            where T : Project
        {
            this.PrivateDependencies.Add(typeof(T).Name);
        }

        public void AddInterfaceDependency<T>()
            where T : Project
        {
            this.InterfaceDependencies.Add(typeof(T).Name);
        }
    }
}
