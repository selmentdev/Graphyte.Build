using Neobyte.Build.Core;
using Neobyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Neobyte.Build.Evaluation
{
    public sealed partial class EvaluatedModuleRules
    {
        public ModuleRules Module { get; }

        public EvaluatedTargetRules Target { get; }

        public List<EvaluatedModuleRules> PublicDependencies { get; } = new();
        public List<EvaluatedModuleRules> PrivateDependencies { get; } = new();
        public List<string> PublicIncludePaths { get; } = new();
        public List<string> PrivateIncludePaths { get; } = new();
        public List<string> PublicLibraryPaths { get; } = new();
        public List<string> PrivateLibraryPaths { get; } = new();
        public List<string> PublicLibraries { get; } = new();
        public List<string> PrivateLibraries { get; } = new();
        public List<string> PublicDefines { get; } = new();
        public List<string> PrivateDefines { get; } = new();

        public EvaluatedModuleRules(ModuleRules module, EvaluatedTargetRules target)
        {
            Validate(module);

            this.Module = module;

            this.Target = target;
        }

        public override string ToString()
        {
            return this.Module.ToString();
        }
    }
}

namespace Neobyte.Build.Evaluation
{
    public sealed partial class EvaluatedModuleRules
    {
        private static void Validate(ModuleRules module)
        {
            if (module.Type == ModuleType.Default)
            {
                throw new Exception($@"{module} must specify module type");
            }

            if (module.Kind == ModuleKind.Default)
            {
                throw new Exception($@"{module} must specify module kind");
            }

            if (module.Language == ModuleLanguage.Default)
            {
                throw new Exception($@"{module} must specify module language");
            }
        }
    }
}

namespace Neobyte.Build.Evaluation
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

            this.PrivateIncludePaths.Import(this.Module.PublicIncludePaths);
            this.PrivateIncludePaths.Import(this.Module.PrivateIncludePaths);
            this.PublicIncludePaths.Import(this.Module.PublicIncludePaths);
            this.PublicIncludePaths.Import(this.Module.InterfaceIncludePaths);


            //
            // Library paths.
            //

            this.PrivateLibraryPaths.Import(this.Module.PublicLibraryPaths);
            this.PrivateLibraryPaths.Import(this.Module.PrivateLibraryPaths);
            this.PublicLibraryPaths.Import(this.Module.PublicLibraryPaths);
            this.PublicLibraryPaths.Import(this.Module.InterfaceLibraryPaths);


            //
            // Libraries.
            //

            this.PrivateLibraries.Import(this.Module.PublicLibraries);
            this.PrivateLibraries.Import(this.Module.PrivateLibraries);
            this.PublicLibraries.Import(this.Module.PublicLibraries);
            this.PublicLibraries.Import(this.Module.InterfaceLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(this.Module.PublicDefines);
            this.PrivateDefines.Import(this.Module.PrivateDefines);
            this.PublicDefines.Import(this.Module.PublicDefines);
            this.PublicDefines.Import(this.Module.InterfaceDefines);
        }

        public void Resolve(ModuleResolver resolver)
        {
            resolver.BeginTracking(this);

            if (this.m_IsResolved == false)
            {
                this.m_IsResolved = true;

                this.ImportProperties();


                var dependenciesPublic = this.Module.PublicDependencies.Select(resolver.Resolve).ToArray();
                var dependenciesPrivate = this.Module.PrivateDependencies.Select(resolver.Resolve).ToArray();
                var dependenciesInterface = this.Module.InterfaceDependencies.Select(resolver.Resolve).ToArray();

                // BUG:
                //      Validate if specified dependency is specified exactly once.

                Array.ForEach(dependenciesPublic, x => x.Resolve(resolver));
                Array.ForEach(dependenciesPrivate, x => x.Resolve(resolver));
                Array.ForEach(dependenciesInterface, x => x.Resolve(resolver));

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

            resolver.EndTracking(this);
        }

        private void ResolvePublicDependency(EvaluatedModuleRules dependency)
        {
            this.PrivateDependencies.Import(dependency);
            this.PublicDependencies.Import(dependency);

            if (dependency.Module.Type.IsImportable())
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

            if (dependency.Module.Type.IsImportable())
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

            if (dependency.Module.Type.IsImportable())
            {
                this.PublicDependencies.Import(dependency.PublicDependencies);
            }

            this.PublicIncludePaths.Import(dependency.PublicIncludePaths);

            this.PublicLibraryPaths.Import(dependency.PublicLibraryPaths);

            this.PublicLibraries.Import(dependency.PublicLibraries);

            this.PublicDefines.Import(dependency.PublicDefines);
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
            Trace.WriteLine($@"{this.Target}-{this.Target.Descriptor}-{this}");
        }
    }
}
