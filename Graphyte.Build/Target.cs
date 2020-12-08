using System;
using System.Collections.Generic;

namespace Graphyte.Build
{
    public sealed class Target
    {
        /// <summary>
        /// Source project for which current target is being configured.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Target tuple for current target.
        /// </summary>
        private readonly TargetTuple m_TargetTuple;

        public PlatformType PlatformType
            => this.m_TargetTuple.PlatformType;

        public ArchitectureType ArchitectureType
            => this.m_TargetTuple.ArchitectureType;

        public ToolchainType ToolchainType
            => this.m_TargetTuple.ToolchainType;

        public ConfigurationType ConfigurationType
            => this.m_TargetTuple.ConfigurationType;

        public ConfigurationFlavour ConfigurationFlavour
            => this.m_TargetTuple.ConfigurationFlavour;

        /// <summary>
        /// Type of configured target.
        /// </summary>
        public TargetType TargetType { get; set; } = TargetType.Default;

        /// <summary>
        /// Module type of target.
        /// </summary>
        public ModuleType ModuleType { get; set; } = ModuleType.Runtime;

        public Guid ProjectGuid { get; set; }

        private string m_Name = null;

        /// <summary>
        /// Name of target.
        /// </summary>
        public string Name
        {
            get => this.m_Name ?? this.Project.Name;
            set => this.m_Name = value;
        }

        public List<Type> PublicDependencies { get; } = new List<Type>();
        public List<Type> PrivateDependencies { get; } = new List<Type>();
        public List<Type> InterfaceDependencies { get; } = new List<Type>();

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

        public Target(Project project, TargetTuple targetTuple)
        {
            this.Project = project;
            this.m_TargetTuple = targetTuple;
            this.ProjectGuid = Tools.MakeGuid(this.Name);
        }

        public void AddPublicDependency<T>()
            where T : Project
        {
            this.PublicDependencies.Add(typeof(T));
        }

        public void AddPrivateDependency<T>()
            where T : Project
        {
            this.PrivateDependencies.Add(typeof(T));
        }

        public void AddInterfaceDependency<T>()
            where T : Project
        {
            this.InterfaceDependencies.Add(typeof(T));
        }

        #region Experimental
        public bool UseUnityFiles { get; set; }
        #endregion
    }
}
