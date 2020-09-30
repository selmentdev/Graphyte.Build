using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Resolving
{
    public sealed class ResolvedSolution
    {
        public Solution Solution { get; }
        public ConfigurationContext Context { get; }

        private readonly List<ResolvedTarget> m_Targets = new List<ResolvedTarget>();
        public IReadOnlyList<ResolvedTarget> Targets => this.m_Targets;

        public ResolvedSolution(Solution solution, ConfigurationContext context)
        {
            this.Solution = solution;
            this.Context = context;
        }
    }
}
