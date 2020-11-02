using System.Collections.Generic;

namespace Graphyte.Build.Platforms
{
    /// <summary>
    /// Represents abstract platform definition.
    /// </summary>
    public abstract class BasePlatform
    {
        public abstract bool IsHostSupported { get; }

        public abstract ArchitectureType[] Architectures { get; }

        public abstract PlatformType Type { get; }

        public abstract bool IsPlatformKind(PlatformKind platformKind);

        public abstract void Initialize(Profile profile);

        /// <summary>
        /// Pre-configures target.
        /// </summary>
        /// <param name="traget">A target to pre-configure.</param>
        public abstract void PreConfigureTarget(Target traget);

        /// <summary>
        /// Post-configures target.
        /// </summary>
        /// <param name="target">A target to post-configure.</param>
        public abstract void PostConfigureTarget(Target target);

        /// <summary>
        /// Adjusts target name.
        /// </summary>
        /// <param name="name">A target name.</param>
        /// <param name="targetType">A target type.</param>
        /// <returns>The adjusted target name.</returns>
        public abstract string AdjustTargetName(string name, TargetType targetType);

        public abstract string[] GetIncludePaths(ArchitectureType architectureType);
        public abstract string[] GetLibraryPaths(ArchitectureType architectureType);
    }
}
