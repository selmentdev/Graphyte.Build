using Graphyte.Build.Generators;
using Graphyte.Build.Platforms;
using Graphyte.Build.Toolchains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphyte.Build.Resolving
{
    /// <summary>
    /// Represents resolved solution.
    /// </summary>
    /// <remarks>
    /// Resolved solution has proper configuration, toolchain and generator provided.
    /// </remarks>
    public sealed class ResolvedSolution
    {
        /// <summary>
        /// Gets solution.
        /// </summary>
        public Solution Solution { get; }

        /// <summary>
        /// Gets target tuple.
        /// </summary>
        public TargetTuple TargetTuple { get; }

        private readonly List<ResolvedTarget> m_Targets = new List<ResolvedTarget>();

        /// <summary>
        /// Gets list of resolved targets.
        /// </summary>
        public IReadOnlyList<ResolvedTarget> Targets => this.m_Targets;

        /// <summary>
        /// Creates new instance of ResolvedSolution for given solution and target tuple.
        /// </summary>
        /// <param name="solution">A solution to resolve.</param>
        public ResolvedSolution(
            Solution solution,
            TargetTuple targetTuple)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.TargetTuple = targetTuple;


            //
            // Create targets
            //

            foreach (var project in this.Solution.Projects)
            {
                var target = new Target(project, this.TargetTuple);
                var resolved = new ResolvedTarget(this, target);
                this.m_Targets.Add(resolved);
            }
        }

        public void Configure()
        {
            var solution = this.Solution;

            foreach (var current in this.m_Targets)
            {
                var target = current.SourceTarget;

                solution.PreConfigure(target);

                target.Project.Configure(target);

                solution.PostConfigure(target);
            }
        }

        /// <summary>
        /// Resolves solution and all targets.
        /// </summary>
        public void Resolve()
        {
            var trace = new Stack<ResolvedTarget>();

            foreach (var target in this.m_Targets)
            {
                target.Resolve(trace);
            }

            if (trace.Count != 0)
            {
                throw new ResolvingException($@"Internal resolving error");
            }
        }

        /// <summary>
        /// Finds resolved target by project name.
        /// </summary>
        /// <param name="name">A project name.</param>
        /// <returns>The resolved target.</returns>
        public ResolvedTarget FindTargetByProjectName(string name)
        {
            var found = this.m_Targets.FirstOrDefault(x => x.Name == name);

            return found ??
                throw new ResolvingException(
                    $@"Cannot resolve target ""{name}"" for solution ""{this.Solution.Name}""");
        }
    }
}
