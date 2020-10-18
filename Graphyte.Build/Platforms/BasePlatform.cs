namespace Graphyte.Build.Platforms
{
    /// <summary>
    /// Represents abstract platform definition.
    /// </summary>
    public abstract class BasePlatform
    {
        /// <summary>
        /// Gets value indicating whether definition is supported on host machine.
        /// </summary>
        public abstract bool IsHostSupported { get; }

        /// <summary>
        /// Gets value indicating whether target tuple is supported by this platform.
        /// </summary>
        /// <param name="targetTuple">Provides a target tuple.</param>
        /// <returns>Returns <c>true</c> when target tuple is supported, <c>false</c> otherwise</returns>
        public abstract bool IsSupported(TargetTuple targetTuple);

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
