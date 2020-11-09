using Graphyte.Build.Platforms;
using Graphyte.Build.Resolving;
using Graphyte.Build.Tests.Mocks;
using Graphyte.Build.Toolchains;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyCycle
    {
        public class SampleSolution : Solution
        {
            public SampleSolution()
            {
                this.AddProject(new A());
                this.AddProject(new B());
                this.AddProject(new C());
                this.AddProject(new D());
                this.AddProject(new E());
                this.AddProject(new F());
                this.AddProject(new G());
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
                    target.AddPublicDependency<D>();
                }
            }

            public class D : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<E>();
                }
            }

            public class E : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<F>();
                }
            }

            public class F : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<G>();
                }
            }

            public class G : Project
            {
                public override void Configure(Target target)
                {
                    target.TargetType = TargetType.SharedLibrary;
                    target.AddPublicDependency<A>();
                }
            }
        }

        [TestMethod]
        public void Cycle()
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
