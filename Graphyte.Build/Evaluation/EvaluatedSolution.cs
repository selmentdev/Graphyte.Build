using Graphyte.Build.Resolving;

namespace Graphyte.Build.Evaluation
{
    public sealed class EvaluatedSolution
    {
        public BasePlatform Platform { get; }
        public BaseToolchain Toolchain { get; }

        public Solution Solution { get; }
        public Profile Profile { get; }
        public ResolvedSolution[] ResolvedSolutions { get; }

        public EvaluatedSolution(
            BasePlatform platform,
            BaseToolchain toolchain,
            Solution solution,
            Profile profile,
            ResolvedSolution[] resolvedSolutions)
        {
            this.Platform = platform;
            this.Toolchain = toolchain;
            this.Solution = solution;
            this.Profile = profile;
            this.ResolvedSolutions = resolvedSolutions;
        }
    }
}
