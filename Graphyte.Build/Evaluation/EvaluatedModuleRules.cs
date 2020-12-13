using Graphyte.Build.Core;
using Graphyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Evaluation
{
    public sealed partial class EvaluatedModuleRules
    {
        public ModuleRules ModuleRules { get; }

        public EvaluatedTargetRules EvaluatedTargetRules { get; }

        public List<EvaluatedModuleRules> PublicDependencies { get; } = new List<EvaluatedModuleRules>();
        public List<EvaluatedModuleRules> PrivateDependencies { get; } = new List<EvaluatedModuleRules>();
        public List<string> PublicIncludePaths { get; } = new List<string>();
        public List<string> PrivateIncludePaths { get; } = new List<string>();
        public List<string> PublicLibraryPaths { get; } = new List<string>();
        public List<string> PrivateLibraryPaths { get; } = new List<string>();
        public List<string> PublicLibraries { get; } = new List<string>();
        public List<string> PrivateLibraries { get; } = new List<string>();
        public List<string> PublicDefines { get; } = new List<string>();
        public List<string> PrivateDefines { get; } = new List<string>();

        public EvaluatedModuleRules(ModuleRules moduleRules, EvaluatedTargetRules evaluatedTargetRules)
        {
            Validate(moduleRules);

            this.ModuleRules = moduleRules;

            this.EvaluatedTargetRules = evaluatedTargetRules;
        }

        public override string ToString()
        {
            return this.ModuleRules.ToString();
        }
    }
}

namespace Graphyte.Build.Evaluation
{
    public sealed partial class EvaluatedModuleRules
    {
        private static void Validate(ModuleRules moduleRules)
        {
            if (moduleRules.ModuleType == ModuleType.Default)
            {
                throw new Exception($@"{moduleRules} must specify module type");
            }

            if (moduleRules.ModuleKind == ModuleKind.Default)
            {
                throw new Exception($@"{moduleRules} must specify module kind");
            }

            if (moduleRules.ModuleLanguage == ModuleLanguage.Default)
            {
                throw new Exception($@"{moduleRules} must specify module language");
            }
        }
    }
}

namespace Graphyte.Build.Evaluation
{
    public sealed partial class EvaluatedModuleRules
    {
        private void ValidateDependencyCycle(Stack<EvaluatedModuleRules> trace)
        {
            if (trace.Contains(this))
            {
                var message = new StringBuilder();
                message.AppendLine($@"Cycle detected when resolving target {this}");

                foreach (var item in trace)
                {
                    message.AppendLine($@"required by {item}");
                }

                throw new Exception(message.ToString());
            }
        }

        private bool m_IsResolved = false;

        private void ImportProperties()
        {
            //
            // Include paths.
            //

            this.PrivateIncludePaths.Import(this.ModuleRules.PublicIncludePaths);
            this.PrivateIncludePaths.Import(this.ModuleRules.PrivateIncludePaths);
            this.PublicIncludePaths.Import(this.ModuleRules.PublicIncludePaths);
            this.PublicIncludePaths.Import(this.ModuleRules.InterfaceIncludePaths);


            //
            // Library paths.
            //

            this.PrivateLibraryPaths.Import(this.ModuleRules.PublicLibraryPaths);
            this.PrivateLibraryPaths.Import(this.ModuleRules.PrivateLibraryPaths);
            this.PublicLibraryPaths.Import(this.ModuleRules.PublicLibraryPaths);
            this.PublicLibraryPaths.Import(this.ModuleRules.InterfaceLibraryPaths);


            //
            // Libraries.
            //

            this.PrivateLibraries.Import(this.ModuleRules.PublicLibraries);
            this.PrivateLibraries.Import(this.ModuleRules.PrivateLibraries);
            this.PublicLibraries.Import(this.ModuleRules.PublicLibraries);
            this.PublicLibraries.Import(this.ModuleRules.InterfaceLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(this.ModuleRules.PublicDefines);
            this.PrivateDefines.Import(this.ModuleRules.PrivateDefines);
            this.PublicDefines.Import(this.ModuleRules.PublicDefines);
            this.PublicDefines.Import(this.ModuleRules.InterfaceDefines);
        }

        public void Resolve(Stack<EvaluatedModuleRules> trace)
        {
            this.ValidateDependencyCycle(trace);

            trace.Push(this);

            if (this.m_IsResolved == false)
            {
                this.m_IsResolved = true;

                this.ImportProperties();


                var dependenciesPublic = this.ModuleRules.PublicDependencies.Select(this.EvaluatedTargetRules.Find).ToArray();
                var dependenciesPrivate = this.ModuleRules.PrivateDependencies.Select(this.EvaluatedTargetRules.Find).ToArray();
                var dependenciesInterface = this.ModuleRules.InterfaceDependencies.Select(this.EvaluatedTargetRules.Find).ToArray();

                // BUG:
                //      Validate if specified dependency is specified exactly once.

                Array.ForEach(dependenciesPublic, x => x.Resolve(trace));
                Array.ForEach(dependenciesPrivate, x => x.Resolve(trace));
                Array.ForEach(dependenciesInterface, x => x.Resolve(trace));

                //
                // Import properties from dependencies.
                //
                // Rules:
                //      Public Dependency:
                //
                //          Imports source interface properties as private and interface properties of current target.
                //
                //      Private Dependency:
                //
                //          Imports source interface properties as private only properties of current target.
                //
                //      Interface Dependency:
                //
                //          Imports source interface properties as interface only properties of current target.
                //

                Array.ForEach(dependenciesPublic, this.ResolvePublicDependency);
                Array.ForEach(dependenciesPrivate, this.ResolvePrivateDependency);
                Array.ForEach(dependenciesInterface, this.ResolveInterfaceDependency);

            }

            trace.Pop();
        }

        private void ResolvePublicDependency(EvaluatedModuleRules dependency)
        {
            this.PrivateDependencies.Import(dependency);
            this.PublicDependencies.Import(dependency);

            if (dependency.ModuleRules.ModuleType.IsImportable())
            {
                this.PrivateDependencies.Import(dependency.PublicDependencies);
                this.PublicDependencies.Import(dependency.PublicDependencies);
            }

            this.PrivateIncludePaths.Import(dependency.PublicIncludePaths);
            this.PublicIncludePaths.Import(dependency.PublicIncludePaths);

            this.PrivateLibraryPaths.Import(dependency.PublicLibraryPaths);
            this.PublicLibraryPaths.Import(dependency.PublicLibraryPaths);

            this.PrivateLibraries.Import(dependency.PublicLibraries);
            this.PublicLibraries.Import(dependency.PublicLibraries);

            this.PrivateDefines.Import(dependency.PublicDefines);
            this.PublicDefines.Import(dependency.PublicDefines);
        }

        private void ResolvePrivateDependency(EvaluatedModuleRules dependency)
        {
            this.PrivateDependencies.Import(dependency);

            if (dependency.ModuleRules.ModuleType.IsImportable())
            {
                this.PrivateDependencies.Import(dependency.PublicDependencies);
            }

            this.PrivateIncludePaths.Import(dependency.PublicIncludePaths);

            this.PrivateLibraryPaths.Import(dependency.PublicLibraryPaths);

            this.PrivateLibraries.Import(dependency.PublicLibraries);

            this.PrivateDefines.Import(dependency.PublicDefines);
        }

        private void ResolveInterfaceDependency(EvaluatedModuleRules dependency)
        {
            this.PublicDependencies.Import(dependency);

            if (dependency.ModuleRules.ModuleType.IsImportable())
            {
                this.PublicDependencies.Import(dependency.PublicDependencies);
            }

            this.PublicIncludePaths.Import(dependency.PublicIncludePaths);

            this.PublicLibraryPaths.Import(dependency.PublicLibraryPaths);

            this.PublicLibraries.Import(dependency.PublicLibraries);

            this.PublicDefines.Import(dependency.PublicDefines);
        }
    }
}
