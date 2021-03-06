using Graphyte.Build.Resolving;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyPropertyResolving
    {
        public enum DependencyType
        {
            Public,
            Private,
            Interface,
        }

        public class ConfigurableProject : Project
        {
            private readonly TargetType m_TargetType;

            public ConfigurableProject(TargetType type)
            {
                this.m_TargetType = type;
            }

            public override void Configure(Target target)
            {
                target.TargetType = this.m_TargetType;

                target.PublicIncludePaths.Add($@"include-path/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include-path/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include-path/{this.Name}/interface");

                target.PublicLibraryPaths.Add($@"library-path/{this.Name}/public");
                target.PrivateLibraryPaths.Add($@"library-path/{this.Name}/private");
                target.InterfaceLibraryPaths.Add($@"library-path/{this.Name}/interface");

                target.PublicLibraries.Add($@"{this.Name}-public.lib");
                target.PrivateLibraries.Add($@"{this.Name}-private.lib");
                target.InterfaceLibraries.Add($@"{this.Name}-interface.lib");

                target.PublicDefines.Add($@"PUBLIC_DEFINE_{this.Name}={this.Name}");
                target.PrivateDefines.Add($@"PUBLIC_DEFINE_{this.Name}={this.Name}");
                target.InterfaceDefines.Add($@"PUBLIC_DEFINE_{this.Name}={this.Name}");
            }
        }

        public class LeafProject : ConfigurableProject
        {

            public LeafProject(TargetType type)
                : base(type)
            {
            }
        }

        public class ImmediateProject : ConfigurableProject
        {
            private readonly DependencyType m_DependencyType;

            public ImmediateProject(TargetType type, DependencyType dependency)
                : base(type)
            {
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target)
            {
                base.Configure(target);

                if (this.m_DependencyType == DependencyType.Public)
                {
                    target.AddPublicDependency<LeafProject>();
                    target.AddPrivateDependency<LeafProject>();
                }
                else if (this.m_DependencyType == DependencyType.Private)
                {
                    target.AddPrivateDependency<LeafProject>();
                }
                else if (this.m_DependencyType == DependencyType.Interface)
                {
                    target.AddInterfaceDependency<LeafProject>();
                }
            }
        }

        public class RootProject : ConfigurableProject
        {
            private readonly DependencyType m_DependencyType;

            public RootProject(TargetType type, DependencyType dependency)
                : base(type)
            {
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target)
            {
                base.Configure(target);

                if (this.m_DependencyType == DependencyType.Public)
                {
                    target.AddPublicDependency<ImmediateProject>();
                }
                else if (this.m_DependencyType == DependencyType.Private)
                {
                    target.AddPrivateDependency<ImmediateProject>();
                }
                else if (this.m_DependencyType == DependencyType.Interface)
                {
                    target.AddInterfaceDependency<ImmediateProject>();
                }
            }
        }

        public class SampleSolution : Solution
        {
            public SampleSolution(
                TargetType immediateType,
                DependencyType immediateDependency,
                TargetType leafType,
                DependencyType leafDependency)
            {
                this.AddProject(new RootProject(TargetType.Application, immediateDependency));
                this.AddProject(new ImmediateProject(immediateType, leafDependency));
                this.AddProject(new LeafProject(leafType));
            }
        }

        [TestMethod]
        public void TestIncludePaths()
        {
            var solution = new SampleSolution(
                TargetType.SharedLibrary,
                DependencyType.Public,
                TargetType.SharedLibrary,
                DependencyType.Public);

            var targetTuple = new TargetTuple(
                            PlatformType.Windows,
                            ArchitectureType.X64,
                            ToolchainType.MSVC,
                            ConfigurationType.Debug,
                            ConfigurationFlavour.None);

            var resolved = new ResolvedSolution(solution, targetTuple);

            resolved.Resolve();

            //Dump.DumpResolvedSolution.SaveToFile("d:/output.json", resolved);
        }

    }
}
