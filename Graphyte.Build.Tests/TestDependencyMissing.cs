using Graphyte.Build.Resolving;
using Graphyte.Build.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
            var platformProvider = new PlatformsProvider();

            var platformFactory = platformProvider.Platforms.FirstOrDefault(
                x => x.PlatformType == MockPlatformFactory.MockPlatform
                    && x.ToolchainType == MockPlatformFactory.MockToolchain
                    && x.ArchitectureType == ArchitectureType.X64);

            var profile = Profile.Parse("{}");

            var platform = platformFactory.CreatePlatform(profile);

            var toolchain = platformFactory.CreateToolchain(profile);

            var targetTuple = new TargetTuple(
                platformFactory.PlatformType,
                platformFactory.ArchitectureType,
                platformFactory.ToolchainType,
                ConfigurationType.Debug,
                ConfigurationFlavour.None);

            var solution = new SampleSolution();

            var resolved = new ResolvedSolution(solution, targetTuple);

            Assert.ThrowsException<ResolvingException>(() =>
            {
                resolved.Configure();
                resolved.Resolve();
            });
        }
    }
}
