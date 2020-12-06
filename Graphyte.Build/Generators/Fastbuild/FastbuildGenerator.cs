using Graphyte.Build.Evaluation;
using Graphyte.Build.Platforms.Windows;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGeneratorSettings
        : BaseGeneratorSettings
    {
        public bool? UnityBuild { get; set; }
        public bool? Distributed { get; set; }
        public string CachePath { get; set; }
    }

    sealed class FastbuildGenerator
        : BaseGenerator
    {
        public FastbuildGenerator(Profile profile)
            : base(profile)
        {
            this.m_Settings = profile.GetSection<FastbuildGeneratorSettings>();
        }

        public override GeneratorType GeneratorType => GeneratorType.FastBuild;

        private readonly FastbuildGeneratorSettings m_Settings;

        private static void EmitGeneratedBanner(TextWriter output)
        {
            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine("; THIS FILE WAS MACHINE GENERATED");
            output.WriteLine(";");
        }

        private static void EmitToolchainDefinition(TextWriter output, BaseToolchain toolchain)
        {
            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Toolchain definition: {toolchain.ToolchainType} {toolchain.ArchitectureType}");

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Compilers");

            output.WriteLine($@"Compiler('Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}') {{");
            output.WriteLine($@"    .Root = '{toolchain.RootPath}'");
            output.WriteLine($@"    .Executable = '{toolchain.CompilerExecutable}'");
            output.WriteLine($@"    .ExtraFiles = {{");

            foreach (var extra in toolchain.CompilerExtraFiles)
            {
                output.WriteLine($@"        '{extra}',");
            }

            output.WriteLine($@"    }}");
            output.WriteLine($@"}}");

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Toolchain Paths");
            output.WriteLine($@".Toolchain_{toolchain.ToolchainType}_{toolchain.ArchitectureType}_IncludePaths = {{");
            foreach (var path in toolchain.IncludePaths)
            {
                output.WriteLine($@"    '{path}',");
            }
            output.WriteLine($@"}}");
            output.WriteLine($@".Toolchain_{toolchain.ToolchainType}_{toolchain.ArchitectureType}_LibraryPaths = {{");
            foreach (var path in toolchain.LibraryPaths)
            {
                output.WriteLine($@"    '{path}',");
            }
            output.WriteLine($@"}}");
        }

        private static void EmitPlatformDefinitions(TextWriter output, BasePlatform platform)
        {
            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Platform definition: {platform.PlatformType} {platform.ArchitectureType}");

            if (platform is BaseWindowsPlatform windowsPlatform)
            {
                output.WriteLine(";---------------------------------------------------------------------------------------------------");
                output.WriteLine($@"; Resource Compiler");
                output.WriteLine($@"Compiler('Compiler-{windowsPlatform.PlatformType}-{windowsPlatform.ArchitectureType}-ResourceCompiler') {{");
                output.WriteLine($@"    .Executable = '{windowsPlatform.ResourceCompilerExecutable}'");
                output.WriteLine($@"    .CompilerFamily = 'custom'");
                output.WriteLine($@"}}");
            }

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Platform Paths");

            output.WriteLine($@".Platform_{platform.PlatformType}_{platform.ArchitectureType}_IncludePaths = {{");
            foreach (var path in platform.IncludePaths)
            {
                output.WriteLine($@"    '{path}',");
            }
            output.WriteLine($@"}}");

            output.WriteLine($@".Platform_{platform.PlatformType}_{platform.ArchitectureType}_LibraryPaths = {{");
            foreach (var path in platform.LibraryPaths)
            {
                output.WriteLine($@"    '{path}',");
            }
            output.WriteLine($@"}}");
        }

        public override void Generate(
            string outputPath,
            PlatformType platformType,
            ToolchainType toolchainType,
            Solution solution,
            EvaluatedSolution[] evaluatedSolutions)
        {
            Directory.CreateDirectory(outputPath);

            var outputPrefix = $@"{solution.Name}-{platformType}-{toolchainType}";
            var baseFile = Path.Combine(outputPath, $@"{outputPrefix}.bff");

            using var output = File.CreateText(baseFile);
            EmitGeneratedBanner(output);

            Trace.WriteLine($@"Generating {baseFile}");

            Trace.WriteLine(solution);
            Trace.Indent();

            foreach (var s in evaluatedSolutions)
            {
                EmitToolchainDefinition(output, s.Toolchain);
                EmitPlatformDefinitions(output, s.Platform);
            }

            foreach (var s in evaluatedSolutions)
            {
                Trace.WriteLine($@"Evaluated: {s.Platform.PlatformType}-{s.Platform.ArchitectureType}-{s.Toolchain.ToolchainType}");
                Trace.WriteLine($@"  - Generate platform/toolchain variables...");
                Trace.WriteLine($@"  - Generate architecture variables...");


                Trace.Indent();

                foreach (var r in s.ResolvedSolutions)
                {
                    output.WriteLine($@"; {r.TargetTuple}");
                    foreach (var t in r.Targets)
                    {
                        output.WriteLine($@"{t.Name}");
                    }
                }

                Trace.Unindent();
            }

            Trace.Unindent();
        }
    }
}
