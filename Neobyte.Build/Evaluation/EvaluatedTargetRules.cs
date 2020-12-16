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

        public EvaluatedTargetRules(Type type, TargetDescriptor descriptor, TargetContext context, Type[] modules)
        {
            this.Descriptor = descriptor;

            this.Context = context;

            this.Target = Activator.CreateInstance(type, descriptor, context) as TargetRules;

            this.Modules = modules.Select(this.CreateModuleRules).ToArray();

            var trace = new Stack<EvaluatedModuleRules>();

            foreach (var module in this.Modules)
            {
                module.Resolve(trace);
            }

            if (trace.Count > 0)
            {
                throw new Exception($@"Internal resolving error");
            }

#if DEBUG
            foreach (var module in this.Modules)
            {
                Debug.Assert(module.Target == this);
                Debug.Assert(module.Target.Target == this.Target);
            }
#endif
        }

        private EvaluatedModuleRules CreateModuleRules(Type type)
        {
            var rules = Activator.CreateInstance(type, this.Target) as ModuleRules;
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
    }
}
