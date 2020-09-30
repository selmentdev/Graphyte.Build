using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Resolving
{
    public sealed class ResolvedSolution
    {
        public Solution Solution { get; }
        public Context Context { get; }

        private readonly List<ResolvedTarget> m_Targets = new List<ResolvedTarget>();
        public IReadOnlyList<ResolvedTarget> Targets => this.m_Targets;

        public ResolvedSolution(Solution solution, Context context)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.Context = context ?? throw new ArgumentNullException(nameof(context));

            this.Validate();
        }

        private void Validate()
        {
            if (!this.Solution.TargetTuples.Contains(new TargetTuple(this.Context.Platform, this.Context.Architecture)))
            {
                throw new ResolverException($@"Solution does not support {this.Context.Platform} {this.Context.Architecture} target tuple");
            }

            if (!this.Solution.BuildTypes.Contains(this.Context.Build))
            {
                throw new ResolverException($@"Solution does not support {this.Context.Build} build type");
            }

            if (!this.Solution.ConfigurationTypes.Contains(this.Context.Configuration))
            {
                throw new ResolverException($@"Solution does not support {this.Context.Configuration} configuration type");
            }
        }

        public void Resolve()
        {
            var context = this.Context;
            var solution = this.Solution;

            foreach (var project in solution.Projects)
            {
                var target = new Target(project);
                solution.PreConfigureTarget(target, context);
                project.Configure(target, context);
                solution.PostConfigureTarget(target, context);

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
                throw new ResolverException($@"Internal resolving error");
            }
        }

        public ResolvedTarget FindTargetByProjectName(string name)
        {
            var found = this.m_Targets.FirstOrDefault(x => x.SourceTarget.Project.Name == name);

            if (found == null)
            {
                throw new ResolverException(
                    $@"Cannot resolve project target {name} for solution {this.Solution.Name} in configuration {this.Context}");
            }

            return found;
        }
    }
}
