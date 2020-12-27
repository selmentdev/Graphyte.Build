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

        public EvaluatedModuleRules[] Modules { get; }

        public EvaluatedModuleRules? LaunchModule { get; }

        public EvaluatedTargetRules(
            TargetRulesFactory target,
            TargetDescriptor descriptor,
            TargetContext context,
            IEnumerable<ModuleRulesFactory> modules)
        {
            this.Descriptor = descriptor;

            this.Context = context;

            this.Target = target.Create(descriptor, context);

            this.Modules = modules.Select(this.CreateModuleRules).ToArray();

            var launchModule = this.Target.LaunchModule;
            if (launchModule != null)
            {
                this.LaunchModule = this.Find(launchModule);

                if (this.LaunchModule.Module.Type != ModuleType.Application)
                {
                    throw new Exception($@"Launch module {this.LaunchModule} for target {this} must be application");
                }
            }

            this.ResolveModules();


#if DEBUG
            foreach (var module in this.Modules)
            {
                Debug.Assert(module.Target == this);
                Debug.Assert(module.Target.Target == this.Target);
            }
#endif
        }

        private void ResolveModules()
        {
            var trace = new Stack<EvaluatedModuleRules>();

            foreach (var module in this.Modules)
            {
                module.Resolve(trace);
            }

            if (trace.Count > 0)
            {
                throw new Exception($@"Internal resolving error");
            }
        }

        private EvaluatedModuleRules CreateModuleRules(ModuleRulesFactory type)
        {
            var rules = type.Create(this.Target);
            return new EvaluatedModuleRules(rules, this);
        }

        public EvaluatedModuleRules Find(Type type)
        {
            var found = this.Modules.SingleOrDefault(x => x.Module.GetType() == type);

            return found ?? throw new Exception($@"Cannot resolve module of type ""{type}""");
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
