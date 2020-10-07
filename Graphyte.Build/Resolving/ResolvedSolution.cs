using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphyte.Build.Resolving
{
    public sealed class ResolvedSolution
    {
        public Solution Solution { get; }
        public TargetTuple TargetTuple { get; }

        private readonly List<ResolvedTarget> m_Targets = new List<ResolvedTarget>();
        public IReadOnlyList<ResolvedTarget> Targets => this.m_Targets;

        public ResolvedSolution(Solution solution, TargetTuple targetTuple)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.TargetTuple = targetTuple;
        }

        public void Resolve()
        {
            foreach (var project in this.Solution.Projects)
            {
                var target = new Target(project, this.TargetTuple);

                this.Solution.PreConfigure(target);
                project.Configure(target);
                this.Solution.PostConfigure(target);

                var resolved = new ResolvedTarget(this, target);
                this.m_Targets.Add(resolved);
            }

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

        public ResolvedTarget FindTargetByProjectName(string name)
        {
            var found = this.m_Targets.FirstOrDefault(x => x.Name == name);

            return found ??
                throw new ResolvingException(
                    $@"Cannot resolve target ""{name}"" for solution ""{this.Solution.Name}""");
        }
    }
}
