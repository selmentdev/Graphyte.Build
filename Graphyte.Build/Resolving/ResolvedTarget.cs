using System.Collections.Generic;

namespace Graphyte.Build.Resolving
{
    public sealed partial class ResolvedTarget
    {
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

        public ResolvedTarget(ResolvedSolution resolvedSolution, Target target)
        {
            this.Solution = resolvedSolution;
            this.SourceTarget = target;
        }
    }
}
