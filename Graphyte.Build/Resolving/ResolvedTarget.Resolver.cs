using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Resolving
{
    public sealed partial class ResolvedTarget
    {
        #region Resolving
        private void ValidateDependencyCycle(Stack<ResolvedTarget> trace)
        {
            if(trace.Contains(this))
            {
                var message = new StringBuilder();
                message.AppendLine($@"Cycle detected when resolving target {this.SourceTarget.Name}");

                foreach (var item in trace)
                {
                    message.AppendLine($@"  - required by {item.SourceTarget.Name}");
                }

                throw new ResolverException(message.ToString());
            }
        }

        private bool m_IsResolved = false;

        public void FindDependencies()
        {
            foreach (var dependency in this.SourceTarget.Dependencies)
            {
                var targetDependency = this.Solution.FindTargetByProjectName(dependency.Key);
                this.m_Dependencies.Add(targetDependency, dependency.Value);
            }
        }

        public void Resolve(Stack<ResolvedTarget> trace)
        {
            this.ValidateDependencyCycle(trace);

            trace.Push(this);

            if (this.m_IsResolved == false)
            {
                this.m_IsResolved = true;

                foreach (var dependency in this.m_Dependencies)
                {
                    dependency.Key.Resolve(trace);
                }
            }

            trace.Pop();
        }
        #endregion
    }
}
