using Graphyte.Build.Resolving;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyResolving
    {
        // A (application)
        // A requires AA (shared)
        // A requires AB (static)
        // A requires AC (header)
        // AA requires AAA (shared)
        // AA requires AAB (static)
        // AA requires AAC (header)
        // AB requires ABA (shared)
        // AB requires ABB (static)
        // AB requiers ABC (header)
        // AC requires ACA (shared)
        // AC requires ACB (static)
        // AC requiers ACC (header)

        public class A : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.Application;

                target.AddPublicDependency<AA>();
                target.AddPrivateDependency<AB>();
                target.AddInterfaceDependency<AC>();

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AA : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.SharedLibrary;

                target.AddPrivateDependency<AAA>();
                target.AddInterfaceDependency<AAB>();
                target.AddPublicDependency<AAC>();

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AB : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.StaticLibrary;

                target.AddPublicDependency<ABA>();
                target.AddPrivateDependency<ABB>();
                target.AddInterfaceDependency<ABC>();

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AC : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.HeaderLibrary;

                target.AddInterfaceDependency<ACA>();
                target.AddPublicDependency<ACB>();
                target.AddPrivateDependency<ACC>();

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AAA : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.SharedLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AAB : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.StaticLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class AAC : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.HeaderLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ABA : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.SharedLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ABB : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.StaticLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ABC : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.HeaderLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ACA : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.SharedLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ACB : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.StaticLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ACC : Project
        {
            public override void Configure(Target target)
            {
                target.TargetType = TargetType.HeaderLibrary;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }


        public class SampleSolution : Solution
        {
            public SampleSolution()
            {
                this.AddProject(new A());
                this.AddProject(new AA());
                this.AddProject(new AB());
                this.AddProject(new AC());
                this.AddProject(new AAA());
                this.AddProject(new AAB());
                this.AddProject(new AAC());
                this.AddProject(new ABA());
                this.AddProject(new ABB());
                this.AddProject(new ABC());
                this.AddProject(new ACA());
                this.AddProject(new ACB());
                this.AddProject(new ACC());
            }
        }

        [TestMethod]
        public void SolutionResolving()
        {
            var solution = new SampleSolution();
            var targetTuple = new TargetTuple(
                PlatformType.Windows,
                ArchitectureType.X64,
                ToolchainType.MSVC,
                ConfigurationType.Debug,
                ConfigurationFlavour.None);

            var resolved = new ResolvedSolution(solution, targetTuple);

            resolved.Resolve();
        }
    }
}
