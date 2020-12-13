using Graphyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Graphyte.Build.Evaluation
{
    public sealed class EvaluatedTargetRules
    {
        public TargetDescriptor TargetDescriptor { get; }

        public TargetContext TargetContext { get; }

        public TargetRules TargetRules { get; }

        public EvaluatedModuleRules[] ModuleRules { get; }

        public EvaluatedTargetRules(Type type, TargetDescriptor targetDescriptor, TargetContext targetContext, Type[] moduleRules)
        {
            this.TargetDescriptor = targetDescriptor;

            this.TargetContext = targetContext;

            this.TargetRules = Activator.CreateInstance(type, targetDescriptor, targetContext) as TargetRules;

            this.ModuleRules = moduleRules.Select(this.CreateModuleRules).ToArray();

            var trace = new Stack<EvaluatedModuleRules>();

            foreach (var module in this.ModuleRules)
            {
                module.Resolve(trace);
            }

            if (trace.Count > 0)
            {
                throw new Exception($@"Internal resolving error");
            }

#if DEBUG
            foreach (var module in this.ModuleRules)
            {
                Debug.Assert(module.EvaluatedTargetRules == this);
                Debug.Assert(module.EvaluatedTargetRules.TargetRules == this.TargetRules);
            }
#endif
        }

        private EvaluatedModuleRules CreateModuleRules(Type type)
        {
            var rules = Activator.CreateInstance(type, this.TargetRules) as ModuleRules;
            return new EvaluatedModuleRules(rules, this);
        }

        public EvaluatedModuleRules Find(Type type)
        {
            var found = this.ModuleRules.SingleOrDefault(x => x.ModuleRules.GetType() == type);

            return found ?? throw new Exception($@"Cannot resolve module of type ""{type}""");
        }

        public override string ToString()
        {
            return this.TargetRules.ToString();
        }
    }
}
