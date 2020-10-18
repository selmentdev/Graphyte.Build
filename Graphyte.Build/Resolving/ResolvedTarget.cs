using System.Collections.Generic;

namespace Graphyte.Build.Resolving
{
    /// <summary>
    /// Represents resolved target.
    /// </summary>
    public sealed partial class ResolvedTarget
    {
        /// <summary>
        /// Gets owning resolved solution.
        /// </summary>
        public ResolvedSolution Solution { get; }

        /// <summary>
        /// Gets original target.
        /// </summary>
        public Target SourceTarget { get; }

        /// <summary>
        /// Gets target name.
        /// </summary>
        public string Name => this.SourceTarget.Name;

        /// <summary>
        /// Gets list of public dependencies.
        /// </summary>
        public List<ResolvedTarget> PublicDependencies { get; } = new List<ResolvedTarget>();

        /// <summary>
        /// Gets list of private dependencies.
        /// </summary>
        public List<ResolvedTarget> PrivateDependencies { get; } = new List<ResolvedTarget>();

        /// <summary>
        /// Getes list of public include paths.
        /// </summary>
        public List<string> PublicIncludePaths { get; } = new List<string>();

        /// <summary>
        /// Gets list of private include paths.
        /// </summary>
        public List<string> PrivateIncludePaths { get; } = new List<string>();

        /// <summary>
        /// Gets list of public library paths.
        /// </summary>
        public List<string> PublicLibraryPaths { get; } = new List<string>();

        /// <summary>
        /// Gets list of private library paths.
        /// </summary>
        public List<string> PrivateLibraryPaths { get; } = new List<string>();

        /// <summary>
        /// Gets list of public libraries.
        /// </summary>
        public List<string> PublicLibraries { get; } = new List<string>();

        /// <summary>
        /// Gets list of private libraries.
        /// </summary>
        public List<string> PrivateLibraries { get; } = new List<string>();

        /// <summary>
        /// Gets list of public defines.
        /// </summary>
        public List<string> PublicDefines { get; } = new List<string>();

        /// <summary>
        /// Gets list of private defines.
        /// </summary>
        public List<string> PrivateDefines { get; } = new List<string>();

        /// <summary>
        /// Creates new instance of resolved target.
        /// </summary>
        /// <param name="resolvedSolution">An owning resolved solution.</param>
        /// <param name="target">A source target.</param>
        public ResolvedTarget(ResolvedSolution resolvedSolution, Target target)
        {
            this.Solution = resolvedSolution;
            this.SourceTarget = target;
        }
    }
}
