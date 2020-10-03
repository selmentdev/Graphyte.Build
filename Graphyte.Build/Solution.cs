using System;
using System.Collections.Generic;

namespace Graphyte.Build
{
    public class Solution
    {
        #region Fields
        private readonly List<Project> m_Projects = new List<Project>();
        private readonly List<TargetTuple> m_TargetTuples = new List<TargetTuple>();
        private readonly List<BuildType> m_BuildTypes = new List<BuildType>();
        private readonly List<ConfigurationType> m_ConfigurationTypes = new List<ConfigurationType>();
        #endregion

        #region Properties
        public IReadOnlyList<Project> Projects => this.m_Projects;
        public IReadOnlyList<TargetTuple> TargetTuples => this.m_TargetTuples;
        public IReadOnlyList<BuildType> BuildTypes => this.m_BuildTypes;
        public IReadOnlyList<ConfigurationType> ConfigurationTypes => this.m_ConfigurationTypes;
        #endregion

        #region Solution Name
        private string m_Name = null;
        public string Name
        {
            get => this.m_Name ?? this.GetType().Name;
            set => this.m_Name = value;
        }
        #endregion

        #region Solution Setup
        protected void AddProject(Project project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            this.m_Projects.Add(project);
        }

        protected void AddTargetTuple(PlatformType platform, ArchitectureType architecture)
        {
            this.m_TargetTuples.Add(new TargetTuple(platform, architecture));
        }

        protected void AddBuildType(BuildType build)
        {
            this.m_BuildTypes.Add(build);
        }

        protected void AddConfigurationType(ConfigurationType configuration)
        {
            this.m_ConfigurationTypes.Add(configuration);
        }
        #endregion

        #region Configuration
        protected internal virtual void PreConfigureTarget(Target target, IContext context)
        {
        }

        protected internal virtual void PostConfigureTarget(Target target, IContext context)
        {
        }
        #endregion
    }
}
