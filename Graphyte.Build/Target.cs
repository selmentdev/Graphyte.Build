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
        public TargetTuple TargetTuple { get; }

        public TargetType TargetType { get; set; } = TargetType.Default;

        public ComponentType ComponentType { get; set; } = ComponentType.GameApplication;

        public Target(Project project, TargetTuple targetTuple)
        {
            this.Project = project;
            this.TargetTuple = targetTuple;
        }
    }
}
