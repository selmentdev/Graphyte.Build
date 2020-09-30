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
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.Application;

                target.AddPublicDependency<AA>();
                target.AddPrivateDependency<AB>();
                target.AddInterfaceDependency<AC>();
            }
        }

        public class AA : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.SharedLibrary;

                target.AddPrivateDependency<AAA>();
                target.AddInterfaceDependency<AAB>();
                target.AddPublicDependency<AAC>();
            }
        }

        public class AB : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.StaticLibrary;

                target.AddPublicDependency<ABA>();
                target.AddPrivateDependency<ABB>();
                target.AddInterfaceDependency<ABC>();
            }
        }

        public class AC : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.HeaderLibrary;

                target.AddInterfaceDependency<ACA>();
                target.AddPublicDependency<ACB>();
                target.AddPrivateDependency<ACC>();
            }
        }

        public class AAA : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.SharedLibrary;
            }
        }

        public class AAB : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.StaticLibrary;
            }
        }

        public class AAC : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.HeaderLibrary;
            }
        }

        public class ABA : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.SharedLibrary;
            }
        }

        public class ABB : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.StaticLibrary;
            }
        }

        public class ABC : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.HeaderLibrary;
            }
        }

        public class ACA : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.SharedLibrary;
            }
        }

        public class ACB : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.StaticLibrary;
            }
        }

        public class ACC : Project
        {
            public override void Configure(Target target, IContext context)
            {
                target.Type = TargetType.HeaderLibrary;
            }
        }


        public class SampleSolution : Solution
        {
            public SampleSolution()
            {
                this.AddTargetTuple(PlatformType.Windows, ArchitectureType.X64);
                this.AddTargetTuple(PlatformType.UWP, ArchitectureType.X64);

                this.AddBuildType(BuildType.Developer);
                this.AddBuildType(BuildType.Testing);
                this.AddBuildType(BuildType.Retail);

                this.AddConfigurationType(ConfigurationType.Checked);
                this.AddConfigurationType(ConfigurationType.Profile);
                this.AddConfigurationType(ConfigurationType.Debug);
                this.AddConfigurationType(ConfigurationType.Release);

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
        public void ContextConfigurationNotSupportedBySolution()
        {
            var solution = new SampleSolution();
            var context = new Context(
                PlatformType.Windows,
                ArchitectureType.PowerPC64,
                BuildType.Developer,
                ConfigurationType.Debug);

            Assert.ThrowsException<ResolverException>(() =>
            {
                new ResolvedSolution(solution, context);
            });
        }

        [TestMethod]
        public void SolutionResolving()
        {
            var solution = new SampleSolution();
            var context = new Context(
                PlatformType.Windows,
                ArchitectureType.X64,
                BuildType.Developer,
                ConfigurationType.Debug);

            var resolved =new ResolvedSolution(solution, context);

            resolved.Resolve();
        }
    }
}
