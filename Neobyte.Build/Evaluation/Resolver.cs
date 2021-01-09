using Neobyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neobyte.Build.Evaluation
{
    //++
    // Resolving algorithm:
    //
    // 1. create module resolver with access to all types
    // 2. list all resolved modules in evaluated target
    // 3. import module properties from point 2.
    // 4. resolve dependencies recursively
    //  
    //--
    public sealed class ModuleResolver : IDisposable
    {
       private EvaluatedTargetRules m_Target;

        private Stack<EvaluatedModuleRules> m_BackTrace = new();

        private Dictionary<Type, EvaluatedModuleRules> m_Modules = new();

        private bool m_Disposed=false;

        public ModuleResolver(EvaluatedTargetRules targetRules)
        {
            this.m_Target = targetRules;
        }

        public void BeginTracking(EvaluatedModuleRules current)
        {
            this.ValidateDependencyCycle(current);
            this.m_BackTrace.Push(current);
        }

        public void EndTracking(EvaluatedModuleRules current)
        {
            var last = this.m_BackTrace.Pop();
            if (last != current)
            {
                throw new Exception("Internal resolving error");
            }
        }

        public IEnumerable<EvaluatedModuleRules> GetModules()
        {
            return this.m_Modules.Values;
        }

        private void ValidateDependencyCycle(EvaluatedModuleRules current)
        {
            if (this.m_BackTrace.Contains(current))
            {
                var message = new StringBuilder();
                message.AppendLine($@"Cycle detected when resolving target {this}");

                foreach (var item in this.m_BackTrace)
                {
                    message.AppendLine($@"required by {item}");
                }

                throw new Exception(message.ToString());
            }
        }

        public EvaluatedModuleRules Resolve(Type type)
        {
            if (this.m_Modules.TryGetValue(type, out var moduleRules))
            {
                return moduleRules;
            }

            var module = this
                .m_Target
                .Workspace
                .Modules
                .SingleOrDefault(x => x.Type == type)
                .Create(this.m_Target.Target);

            var result = new EvaluatedModuleRules(module, this.m_Target);

            this.m_Modules.Add(type, result);

            return result;
        }

        private void Dispose(bool disposing)
        {
            if (!this.m_Disposed)
            {
                if (disposing)
                {
                    if (this.m_BackTrace.Count > 0)
                    {
                        throw new Exception("Internal module resolver failure");
                    }
                }

                this.m_Disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
