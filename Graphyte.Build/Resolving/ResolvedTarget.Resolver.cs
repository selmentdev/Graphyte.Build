using Graphyte.Build.Extensions;
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
        
        public void ImportProperties()
        {
            //
            // Include paths.
            //

            this.PrivateIncludePaths.Import(this.SourceTarget.PublicIncludePaths);
            this.PublicIncludePaths.Import(this.SourceTarget.PublicIncludePaths);
            this.PrivateIncludePaths.Import(this.SourceTarget.PrivateIncludePaths);
            this.PublicIncludePaths.Import(this.SourceTarget.InterfaceIncludePaths);


            //
            // Library paths.
            //

            this.PrivateLibraryPaths.Import(this.SourceTarget.PublicLibraryPaths);
            this.PublicLibraryPaths.Import(this.SourceTarget.PublicLibraryPaths);
            this.PrivateLibraryPaths.Import(this.SourceTarget.PrivateLibraryPaths);
            this.PublicLibraryPaths.Import(this.SourceTarget.InterfaceLibraryPaths);


            //
            // Libraries.
            //

            this.PrivateLibraries.Import(this.SourceTarget.PublicLibraries);
            this.PublicLibraries.Import(this.SourceTarget.PublicLibraries);
            this.PrivateLibraries.Import(this.SourceTarget.PrivateLibraries);
            this.PublicLibraries.Import(this.SourceTarget.InterfaceLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(this.SourceTarget.PublicDefines);
            this.PublicDefines.Import(this.SourceTarget.PublicDefines);
            this.PrivateDefines.Import(this.SourceTarget.PrivateDefines);
            this.PublicDefines.Import(this.SourceTarget.InterfaceDefines);
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

                this.ResolveDependencies();
            }

            trace.Pop();
        }


        private void ResolveDependencies()
        {
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

            foreach (var dependency in this.m_Dependencies)
            {
                switch (dependency.Value)
                {
                    case DependencyType.Public:
                        this.ResolvePublicDependency(dependency.Key);
                        break;
                    case DependencyType.Private:
                        this.ResolvePrivateDependency(dependency.Key);
                        break;
                    case DependencyType.Interface:
                        this.ResolveInterfaceDependency(dependency.Key);
                        break;
                }
            }
        }

        private void ResolvePublicDependency(ResolvedTarget dependency)
        {
            //
            // Include paths.
            //

            this.PrivateIncludePaths.Import(dependency.PublicIncludePaths);
            this.PublicIncludePaths.Import(dependency.PublicIncludePaths);


            //
            // Library paths.
            //

            this.PrivateLibraryPaths.Import(dependency.PublicLibraryPaths);
            this.PublicLibraryPaths.Import(dependency.PublicLibraryPaths);


            //
            // Libraries.
            //

            this.PrivateLibraries.Import(dependency.PublicLibraries);
            this.PublicLibraries.Import(dependency.PublicLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(dependency.PublicDefines);
            this.PublicDefines.Import(dependency.PublicDefines);
        }

        private void ResolvePrivateDependency(ResolvedTarget dependency)
        {
            //
            // Include paths.
            //

            this.PrivateIncludePaths.Import(dependency.PublicIncludePaths);


            //
            // Library paths.
            //

            this.PrivateLibraryPaths.Import(dependency.PublicLibraryPaths);


            //
            // Libraries.
            //

            this.PrivateLibraries.Import(dependency.PublicLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(dependency.PublicDefines);
        }

        private void ResolveInterfaceDependency(ResolvedTarget dependency)
        {
            //
            // Include paths.
            //

            this.PublicIncludePaths.Import(dependency.PublicIncludePaths);


            //
            // Library paths.
            //

            this.PublicLibraryPaths.Import(dependency.PublicLibraryPaths);


            //
            // Libraries.
            //

            this.PublicLibraries.Import(dependency.PublicLibraries);


            //
            // Defines.
            //

            this.PrivateDefines.Import(dependency.PublicDefines);
        }

        #endregion
    }
}
