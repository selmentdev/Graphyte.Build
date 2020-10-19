namespace Graphyte.Build.Platforms
{
    /// <summary>
    /// Represents abstract platform definition.
    /// </summary>
    public abstract class BasePlatform
        : ISupportQuery
    {
        public abstract bool IsHostSupported { get; }

        public abstract bool IsTargetTupleSupported(TargetTuple targetTuple);

        /// <summary>
        /// Pre-configures target.
        /// </summary>
        /// <param name="traget">A target to pre-configure.</param>
        public virtual void PreConfigureTarget(Target traget)
        {
        }

        /// <summary>
        /// Post-configures target.
        /// </summary>
        /// <param name="target">A target to post-configure.</param>
        public virtual void PostConfigureTarget(Target target)
        {
        }

        /// <summary>
        /// Adjusts target name.
        /// </summary>
        /// <param name="name">A target name.</param>
        /// <param name="targetType">A target type.</param>
        /// <returns>The adjusted target name.</returns>
        public abstract string AdjustTargetName(string name, TargetType targetType);

        /// <summary>
        /// Returns name of platform.
        /// </summary>
        public string Name => this.GetType().Name;
    }
}
