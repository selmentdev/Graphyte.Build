using Graphyte.Build.Resolving;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyMissing
    {
        public class SampleSolution : Solution
        {
            public SampleSolution()
            {
                //this.AddTargetTuple(PlatformType.Windows, ArchitectureType.X64);

                this.AddBuildType(BuildType.Developer);

                this.AddConfigurationType(ConfigurationType.Debug);

                this.AddProject(new A());
                this.AddProject(new B());
            }

            public class A : Project
            {
                public override void Configure(Target target, IContext context)
                {
                    target.Type = TargetType.SharedLibrary;
                    target.AddPublicDependency<B>();
                }
            }

            public class B : Project
            {
                public override void Configure(Target target, IContext context)
                {
                    target.Type = TargetType.SharedLibrary;
                    target.AddPublicDependency<C>();
                }
            }

            public class C : Project
            {
                public override void Configure(Target target, IContext context)
                {
                    target.Type = TargetType.SharedLibrary;
                }
            }
        }

        [TestMethod]
        public void MissingDependency()
        {
            var solution = new SampleSolution();
            var context = new Context(
                PlatformType.Windows,
                ArchitectureType.X64,
                ToolsetType.Default,
                BuildType.Developer,
                ConfigurationType.Debug);

            var resolved = new ResolvedSolution(solution, context);

            Assert.ThrowsException<ResolverException>(() => resolved.Resolve());
        }
    }
}
