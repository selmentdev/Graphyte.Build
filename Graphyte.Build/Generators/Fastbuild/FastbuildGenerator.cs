using Graphyte.Build.Evaluation;
using Graphyte.Build.Platforms.Windows;
using Graphyte.Build.Resolving;
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

        private static void EmitGeneratedBanner(StreamWriter output)
        {
            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine("; THIS FILE WAS MACHINE GENERATED");
            output.WriteLine(";");
        }

        private static void EmitToolchainDefinition(
            StreamWriter output,
            EvaluatedSolution solution)
        {
            var toolchain = solution.Toolchain;
            var platform = solution.Platform;

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Toolchain definition: {toolchain.ToolchainType} {toolchain.ArchitectureType} {platform.PlatformType}");

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Compilers");

            output.WriteLine($@"Compiler('Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}') {{");
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
            output.WriteLine($@".{variablePrefix}_ToolchainIncludePaths = ''");

            foreach (var path in toolchain.IncludePaths)
            {
                output.WriteLine($@"    + ' {toolchain.FormatIncludePath(path)}'");
            }

            output.WriteLine($@".{variablePrefix}_ToolchainLibraryPaths = ''");

            foreach (var path in toolchain.LibraryPaths)
            {
                output.WriteLine($@"    + ' {toolchain.FormatLibraryPath(path)}'");
            }
        }

        private static void EmitPlatformDefinitions(
            StreamWriter output,
            EvaluatedSolution solution)
        {
            var toolchain = solution.Toolchain;
            var platform = solution.Platform;

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Platform definition: {toolchain.ToolchainType} {toolchain.ArchitectureType} {platform.PlatformType}");

            if (platform is BaseWindowsPlatform windowsPlatform)
            {
                output.WriteLine(";---------------------------------------------------------------------------------------------------");
                output.WriteLine($@"; Resource Compiler");
                output.WriteLine($@"Compiler('Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-ResourceCompiler') {{");
                output.WriteLine($@"    .Executable = '{windowsPlatform.ResourceCompilerExecutable}'");
                output.WriteLine($@"    .CompilerFamily = 'custom'");
                output.WriteLine($@"}}");
            }

            output.WriteLine(";---------------------------------------------------------------------------------------------------");
            output.WriteLine($@"; Platform Paths");

            output.WriteLine($@".{variablePrefix}_PlatformIncludePaths = ''");
            foreach (var path in platform.IncludePaths)
            {
                output.WriteLine($@"    + ' {toolchain.FormatIncludePath(path)}'");
            }

            output.WriteLine($@".{variablePrefix}_PlatformLibraryPaths = ''");
            foreach (var path in platform.LibraryPaths)
            {
                output.WriteLine($@"    + ' {toolchain.FormatLibraryPath(path)}'");
            }
        }

        private static void EmitProjectPlatformToolchainCommonPart(
            StreamWriter output,
            EvaluatedSolution solution)
        {
            var toolchain = solution.Toolchain;
            var platform = solution.Platform;

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            output.WriteLine($@".{variablePrefix}_CommonPlatformToolchain = [");
            output.WriteLine($@"    .Compiler = 'Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}'");

            if (platform.PlatformType == PlatformType.Windows || platform.PlatformType == PlatformType.UniversalWindows)
            {
                output.WriteLine($@"    .ResCompiler = 'Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-ResourceCompiler'");
            }
            else
            {
                output.WriteLine($@"    .ResCompiler = ''");
            }
            output.WriteLine($@"    .Linker = '{toolchain.LinkerExecutable}'");
            output.WriteLine($@"    .Librarian = '{toolchain.LibrarianExecutable}'");

            output.WriteLine($@"]");
        }

        private static void EmitProjectDefinition(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            var platform = solution.Platform;
            var toolchain = solution.Toolchain;

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            var configurationPart = (target.SourceTarget.ConfigurationFlavour == ConfigurationFlavour.None)
                ? target.SourceTarget.ConfigurationType.ToString()
                : $@"{target.SourceTarget.ConfigurationType}{target.SourceTarget.ConfigurationFlavour}";

            var targetFileName = platform.AdjustTargetName(target.Name, target.SourceTarget.TargetType);

            output.WriteLine($@"Library('{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}') {{");
            output.WriteLine($@"    Using(.{variablePrefix}_CommonPlatformToolchain)");
            output.WriteLine($@"    .CompilerOptions = ''");
            output.WriteLine($@"        + .{variablePrefix}_PlatformIncludePaths");
            output.WriteLine($@"        + .{variablePrefix}_ToolchainIncludePaths");
            foreach (var path in target.PrivateIncludePaths)
            {
                output.WriteLine($@"        + ' {toolchain.FormatIncludePath(path)}'");
            }

            foreach (var path in target.PrivateDefines)
            {
                output.WriteLine($@"        + ' {toolchain.FormatDefine(path)}'");
            }

            output.WriteLine($@"    .LibrarianOutput = '.generated/build/{variablePrefix}{configurationPart}/{targetFileName}'");
            output.WriteLine($@"    .LibrarianOptions = ''");
            output.WriteLine($@"        + ' /NOLOGO'");
            output.WriteLine($@"        + ' /OUT:""%2"" ""%1""'");

            foreach (var path in target.PrivateLibraryPaths)
            {
                output.WriteLine($@"        + ' {toolchain.FormatLibraryPath(path)}'");
            }

            foreach (var lib in target.PrivateLibraries)
            {
                output.WriteLine($@"        + ' {toolchain.FormatLink(lib)}'");
            }

            foreach (var dep in target.PrivateDependencies)
            {
                output.WriteLine($@"        + ' {dep.Name}'");
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
                EmitToolchainDefinition(output, s);
                EmitPlatformDefinitions(output, s);
                EmitProjectPlatformToolchainCommonPart(output, s);
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
                        EmitProjectDefinition(output, s, t);
                    }
                }

                Trace.Unindent();
            }

            Trace.Unindent();
        }
    }
}
