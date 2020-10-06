using System;
using System.Collections.Generic;

namespace Graphyte.Build
{
    public sealed partial class Target
    {
        public Project Project { get; }
        public TargetType Type { get; set; }
        public string Name { get; set; }
        public Guid ProjectGuid { get; set; }
        public ComponentType Component { get; }
        public RuntimeKind Runtime { get; set; } = RuntimeKind.Release;

        public List<object> Options { get; } = new List<object>();

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

        public List<string> PublicDefines { get; } = new List<string>();
        public List<string> PrivateDefines { get; } = new List<string>();
        public List<string> InterfaceDefines { get; } = new List<string>();

        public Target(Project project)
        {
            this.Project = project;
            this.Name = project.Name;
            this.Component = project.ComponentType;
            this.ProjectGuid = Tools.Utils.MakeGuid(this.Name);
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
