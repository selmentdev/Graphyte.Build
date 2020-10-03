using System.Collections.Generic;
using System.Diagnostics;

namespace Graphyte.Build.Resolving
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public sealed partial class ResolvedTarget
    {
        #region Debug View
        private string GetDebuggerDisplay()
        {
            return this.SourceTarget.Name;
        }
        #endregion

        #region Properties
        public ResolvedSolution Solution { get; }
        public Target SourceTarget { get; }

        public string Name => this.SourceTarget.Name;

        public List<ResolvedTarget> PublicDependencies { get; } = new List<ResolvedTarget>();
        public List<ResolvedTarget> PrivateDependencies { get; } = new List<ResolvedTarget>();

        public List<string> PublicIncludePaths { get; } = new List<string>();
        public List<string> PrivateIncludePaths { get; } = new List<string>();

        public List<string> PublicLibraryPaths { get; } = new List<string>();
        public List<string> PrivateLibraryPaths { get; } = new List<string>();

        public List<string> PublicLibraries { get; } = new List<string>();
        public List<string> PrivateLibraries { get; } = new List<string>();

        public List<string> PublicDefines { get; } = new List<string>();
        public List<string> PrivateDefines { get; } = new List<string>();
        #endregion

        #region Constructors
        public ResolvedTarget(ResolvedSolution solution, Target target)
        {
            this.Solution = solution;
            this.SourceTarget = target;
        }
        #endregion
    }
}
