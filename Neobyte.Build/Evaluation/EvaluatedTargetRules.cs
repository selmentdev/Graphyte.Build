using Neobyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Neobyte.Build.Evaluation
{
    public sealed class EvaluatedTargetRules
    {
        public TargetDescriptor Descriptor { get; }

        public TargetContext Context { get; }

        public TargetRules Target { get; }

        /// <summary>
        /// Gets additional modules required by this target.
        /// </summary>
        public EvaluatedModuleRules[] Modules { get; }

        /// <summary>
        /// Gets module used to launch this target.
        /// </summary>
        public EvaluatedModuleRules? LaunchModule { get; }

        /// <summary>
        /// Gets resolved modules required to build this target.
        /// </summary>
        public EvaluatedModuleRules[] ResolvedModules { get; }

        public Workspace Workspace { get; }

        public EvaluatedTargetRules(
            TargetRulesFactory target,
            TargetDescriptor descriptor,
            TargetContext context,
            Workspace workspace)
        {
            this.Workspace = workspace;

            this.Descriptor = descriptor;

            this.Context = context;

            this.Target = target.Create(descriptor, context);

            using (var resolver = new ModuleResolver(this))
            {
                this.Modules = this.Target.Modules.Select(resolver.Resolve).ToArray();

                foreach (var module in this.Modules)
                {
                    module.Resolve(resolver);
                }

                var launchModule = this.Target.LaunchModule;
                if (launchModule != null)
                {
                    this.LaunchModule = resolver.Resolve(launchModule);

                    if (this.LaunchModule.Module.Type != ModuleType.Application)
                    {
                        throw new Exception($@"Launch module {this.LaunchModule} for target {this} must be application");
                    }

                    this.LaunchModule.Resolve(resolver);
                }

                this.ResolvedModules = resolver.GetModules().ToArray();
            }


#if DEBUG
            foreach (var module in this.Modules)
            {
                Debug.Assert(module.Target == this);
                Debug.Assert(module.Target.Target == this.Target);
            }
#endif
        }


        public override string ToString()
        {
            return this.Target.ToString();
        }

        public void Dump()
        {
            Trace.WriteLine($@"{this}-{this.Descriptor}");
            Trace.Indent();

            foreach (var module in this.Modules)
            {
                Trace.WriteLine(module);
            }

            Trace.Unindent();
        }
    }
}
