using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Resolving
{
    public sealed partial class ResolvedTarget
    {
        private void ValidateDependencyCycle(Stack<ResolvedTarget> trace)
        {
            if (trace.Contains(this))
            {
                var message = new StringBuilder();
                message.AppendLine($@"Cycle detected when resolving target {this.Name}");

                foreach (var item in trace)
                {
                    message.AppendLine($@"  - required by {item.Name}");
                }

                throw new ResolvingException(message.ToString());
            }
        }

        private bool m_IsResolved = false;

        private void ImportProperties()
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


                this.ImportProperties();

                //
                // Find dependencies from target.
                //

                var publicDependencies = this.SourceTarget.PublicDependencies.Select(this.Solution.FindTargetByProjectName).ToArray();
                var privateDependencies = this.SourceTarget.PrivateDependencies.Select(this.Solution.FindTargetByProjectName).ToArray();
                var interfaceDependencies = this.SourceTarget.InterfaceDependencies.Select(this.Solution.FindTargetByProjectName).ToArray();

                //
                // Resolve dependencies recursively.
                //

                Array.ForEach(publicDependencies, x => x.Resolve(trace));
                Array.ForEach(privateDependencies, x => x.Resolve(trace));
                Array.ForEach(interfaceDependencies, x => x.Resolve(trace));

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

                Array.ForEach(publicDependencies, this.ResolvePublicDependency);
                Array.ForEach(privateDependencies, this.ResolvePrivateDependency);
                Array.ForEach(interfaceDependencies, this.ResolveInterfaceDependency);
            }

            trace.Pop();
        }

        private void ResolvePublicDependency(ResolvedTarget dependency)
        {
            this.PublicDependencies.Import(dependency);
            this.PrivateDependencies.Import(dependency);

            if (dependency.SourceTarget.TargetType.IsImportable())
            {
                this.PrivateDependencies.Import(dependency.PublicDependencies);
                this.PublicDependencies.Import(dependency.PublicDependencies);
            }


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
            this.PrivateDependencies.Import(dependency);

            if (dependency.SourceTarget.TargetType.IsImportable())
            {
                this.PrivateDependencies.Import(dependency.PublicDependencies);
            }


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
            this.PublicDependencies.Import(dependency);

            if (dependency.SourceTarget.TargetType.IsImportable())
            {
                this.PublicDependencies.Import(dependency.PublicDependencies);
            }


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

    }
}
