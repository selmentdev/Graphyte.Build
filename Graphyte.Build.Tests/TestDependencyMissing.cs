using Graphyte.Build.Platforms;
using Graphyte.Build.Resolving;
using Graphyte.Build.Tests.Mocks;
using Graphyte.Build.Toolchains;
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
                this.AddProject(new A());
                this.AddProject(new B());
            }

            public class A : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<B>();
                }
            }

            public class B : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<C>();
                }
            }

            public class C : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                }
            }
        }

        [TestMethod]
        public void MissingDependency()
        {
            var solution = new SampleSolution();
            var targetTuple = new TargetTuple(
                PlatformType.Windows,
                ArchitectureType.X64,
                ToolchainType.MSVC,
                ConfigurationType.Debug);

            var resolved = new ResolvedSolution(solution, targetTuple);

            Assert.ThrowsException<ResolvingException>(() =>
            {
                resolved.Configure(new MockToolchain(), new MockGenerator(), new MockPlatform());
                resolved.Resolve();
            });
        }
    }
}
