using Graphyte.Build.Platforms;
using Graphyte.Build.Resolving;
using Graphyte.Build.Toolchains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Generators
{
    public class GeneratedSolution
    {
        public ResolvedSolution Solution { get; }
        public BasePlatform Platform { get; }
        public BaseToolchain Toolchain { get; }
        public BaseGenerator Generator { get; }

        private readonly List<GeneratedTarget> m_Targets = new List<GeneratedTarget>();
        public IReadOnlyList<GeneratedTarget> Targets => this.m_Targets;

        public GeneratedSolution(
            ResolvedSolution resolvedSolution,
            BasePlatform basePlatform,
            BaseToolchain baseToolchain,
            BaseGenerator baseGenerator)
        {
            this.Solution = resolvedSolution;
            this.Platform = basePlatform;
            this.Toolchain = baseToolchain;
            this.Generator = baseGenerator;
        }
    }
}
