using Neobyte.Build.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Neobyte.Build.Evaluation
{
    public sealed class EvaluatedWorkspace
    {
        public Workspace Workspace { get; }

        public IEnumerable<EvaluatedTargetRules> Targets { get; }

        public EvaluatedWorkspace(Workspace workspace)
        {
            this.Workspace = workspace;

            this.Targets = Evaluate(workspace).ToArray();
        }

        public static IEnumerable<EvaluatedTargetRules> Evaluate(Workspace workspace)
        {
            foreach (var factory in workspace.Platforms)
            {
                var context = factory.CreateContext(workspace.Profile);

                foreach (var target in workspace.Targets)
                {
                    foreach (var flavor in workspace.Flavors)
                    {
                        foreach (var configuration in workspace.Configurations)
                        {
                            var descriptor = new TargetDescriptor(
                                factory.Platform,
                                factory.Architecture,
                                factory.Toolchain,
                                configuration,
                                flavor);

                            var evaluated = new EvaluatedTargetRules(
                                target,
                                descriptor,
                                context,
                                workspace.Modules);

                            yield return evaluated;
                        }
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
            foreach (var target in this.Targets)
            {
                target.Dump();
            }
        }
    }
}
