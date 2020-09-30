using Graphyte.Build.Resolving;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyPropertyResolving
    {
        public class ConfigurableProject : Project
        {
            private TargetType m_TargetType;

            public ConfigurableProject(TargetType type)
            {
                this.m_TargetType = type;
            }

            public override void Configure(Target target, IContext context)
            {
                target.Type = this.m_TargetType;

                target.PublicIncludePaths.Add($@"include-path/{this.Name}/public");
                target.PrivateIncludePaths.Add($@"include-path/{this.Name}/private");
                target.InterfaceIncludePaths.Add($@"include-path/{this.Name}/interface");

                target.PublicLibraryPaths.Add($@"library-path/{this.Name}/public");
                target.PrivateLibraryPaths.Add($@"library-path/{this.Name}/private");
                target.InterfaceLibraryPaths.Add($@"library-path/{this.Name}/interface");

                target.PublicLibraries.Add($@"{this.Name}-public.lib");
                target.PrivateLibraries.Add($@"{this.Name}-private.lib");
                target.InterfaceLibraries.Add($@"{this.Name}-interface.lib");

                target.PublicDefines[$@"PUBLIC_DEFINE_{this.Name}"] = this.Name;
                target.PrivateDefines[$@"PRIVATE_DEFINE_{this.Name}"] = this.Name;
                target.InterfaceDefines[$@"INTERFACE_DEFINE_{this.Name}"] = this.Name;
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
            private DependencyType m_DependencyType;

            public ImmediateProject(TargetType type, DependencyType dependency)
                : base(type)
            {
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target, IContext context)
            {
                base.Configure(target, context);

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
            private DependencyType m_DependencyType;

            public RootProject(TargetType type, DependencyType dependency)
                :base(type)
            {
                this.m_DependencyType = dependency;
            }

            public override void Configure(Target target, IContext context)
            {
                base.Configure(target, context);

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
                TargetType.SharedLibrary,
                DependencyType.Public);

            var context = new Context(
                PlatformType.Windows,
                ArchitectureType.X64,
                BuildType.Developer,
                ConfigurationType.Debug);

            var resolved = new ResolvedSolution(solution, context);

            resolved.Resolve();

            ConvertToString(resolved);
        }

        private static void ConvertToString(ResolvedSolution solution)
        {
            using var stream = System.IO.File.Create("d:/output2.json");
            using var writer = new System.Text.Json.Utf8JsonWriter(stream, new JsonWriterOptions()
            {
                Indented = true
            });

            writer.WriteStartObject();

            foreach (var target in solution.Targets)
            {
                writer.WriteStartObject(target.Name);
                SerializeTarget(writer, target);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        private static void SerializeTarget(Utf8JsonWriter writer, ResolvedTarget target)
        {
            writer.WriteString("TargetType", target.SourceTarget.Type.ToString());

            SerializeList(writer, nameof(target.PublicIncludePaths), target.PublicIncludePaths);
            SerializeList(writer, nameof(target.PrivateIncludePaths), target.PrivateIncludePaths);

            SerializeList(writer, nameof(target.PublicLibraryPaths), target.PublicLibraryPaths);
            SerializeList(writer, nameof(target.PrivateLibraryPaths), target.PrivateLibraryPaths);

            SerializeList(writer, nameof(target.PublicLibraries), target.PublicLibraries);
            SerializeList(writer, nameof(target.PrivateLibraries), target.PrivateLibraries);

            SerializeList(writer, nameof(target.PublicDependencies), target.PublicDependencies.Select(x => x.Name));
            SerializeList(writer, nameof(target.PrivateDependencies), target.PrivateDependencies.Select(x => x.Name));

            SerializeDictionary(writer, nameof(target.PublicDefines), target.PublicDefines);
            SerializeDictionary(writer, nameof(target.PrivateDefines), target.PrivateDefines);
        }

        private static void SerializeList(Utf8JsonWriter writer, string name, IEnumerable<string> items)
        {
            writer.WriteStartArray(name);

            foreach (var item in items)
            {
                writer.WriteStringValue(item);
            }

            writer.WriteEndArray();
        }

        private static void SerializeDictionary(Utf8JsonWriter writer, string name, IEnumerable<KeyValuePair<string, string>> items)
        {
            writer.WriteStartObject(name);

            foreach (var item in items)
            {
                writer.WriteString(item.Key, item.Value);
            }

            writer.WriteEndObject();
        }
    }
}
