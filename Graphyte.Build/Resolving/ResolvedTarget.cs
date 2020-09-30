using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        private readonly Dictionary<ResolvedTarget, DependencyType> m_Dependencies = new Dictionary<ResolvedTarget, DependencyType>();
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
