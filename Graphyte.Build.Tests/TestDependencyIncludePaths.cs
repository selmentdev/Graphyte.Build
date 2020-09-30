using Graphyte.Build.Resolving;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyPropertyResolving
    {
        public class LeafProject : Project
        {
            private TargetType m_TargetType;

            public LeafProject(TargetType type)
            {
                this.m_TargetType = type;
            }

            public override void Configure(Target target, IContext context)
            {
                target.Type = this.m_TargetType;

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class ImmediateProject : Project
        {
            private TargetType m_TargetType;
            private DependencyType m_DependencyType;

            public ImmediateProject(TargetType type, DependencyType dependency)
            {
                this.m_TargetType = type;
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target, IContext context)
            {
                target.Type = this.m_TargetType;

                if (this.m_DependencyType == DependencyType.Public)
                {
                    target.AddPublicDependency<LeafProject>();
                }
                else if (this.m_DependencyType == DependencyType.Private)
                {
                    target.AddPrivateDependency<LeafProject>();
                }
                else if (this.m_DependencyType == DependencyType.Interface)
                {
                    target.AddInterfaceDependency<LeafProject>();
                }

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
            }
        }

        public class RootProject : Project
        {
            private TargetType m_TargetType;
            private DependencyType m_DependencyType;

            public RootProject(TargetType type, DependencyType dependency)
            {
                this.m_TargetType = type;
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target, IContext context)
            {
                target.Type = this.m_TargetType;

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

                target.PublicIncludePaths.Add($@"include/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include/{this.Name}/interface");
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
                this.AddTargetTuple(PlatformType.Windows, ArchitectureType.X64);
                this.AddBuildType(BuildType.Developer);
                this.AddConfigurationType(ConfigurationType.Debug);

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
                TargetType.HeaderLibrary,
                DependencyType.Interface);

            var context = new Context(
                PlatformType.Windows,
                ArchitectureType.X64,
                BuildType.Developer,
                ConfigurationType.Debug);

            var resolved = new ResolvedSolution(solution, context);

            resolved.Resolve();

            foreach (var target in resolved.Targets)
            {
                DebugLogProperties(target);
            }
        }

        private static void DebugLogProperties(ResolvedTarget target)
        {
            Debug.WriteLine($@"{target.SourceTarget.Name} includes: {string.Join(';', target.PrivateIncludePaths)}");
            Debug.WriteLine($@"{target.SourceTarget.Name} interface-includes: {string.Join(';', target.PublicIncludePaths)}");
        }
    }
}
