using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Graphyte.Build.Resolving
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "}")]
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
        public ConfiguredTarget SourceTarget { get; }
        #endregion

        #region Constructors
        public ResolvedTarget(ResolvedSolution solution, ConfiguredTarget target)
        {
            this.Solution = solution;
            this.SourceTarget = target;
        }
        #endregion
    }
}
