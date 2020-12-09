using Graphyte.Build.Evaluation;
using Graphyte.Build.Platforms.Windows;
using Graphyte.Build.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGeneratorSettings
        : BaseGeneratorSettings
    {
        public bool? UnityBuild { get; set; }
        public bool? Distributed { get; set; }
        public bool? Monitor { get; set; }
        public bool? Report { get; set; }
        public string CachePath { get; set; }
    }

    static class FastbuildTemplate
    {
        public static void EmitHeaderLibrary()
        {
        }

        public static void EmitUnity(
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

            var targetName = $@"{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}";

            output.WriteLine($@"Unity('Unity-{targetName}') {{");
            // {
            //   .UnityInputPath          ; (optional) Path (or paths) to find files
            //   .UnityInputExcludePath   ; (optional) Path (or paths) in which to ignore files
            //   .UnityInputExcludePattern; (optional) Wildcard pattern(s) of files/folders to exclude
            //   .UnityInputPattern       ; (optional) Pattern(s) of files to find (default *.cpp)
            //   .UnityInputPathRecurse   ; (optional) Recurse when searching for files (default true)
            //   .UnityInputFiles         ; (optional) Explicit list of files to include
            //   .UnityInputExcludedFiles ; (optional) Explicit list of excluded files (partial, root-relative or full path)
            //   .UnityInputIsolatedFiles ; (optional) List of files to exclude from unity, but still compile (partial end or root-relative)
            //   .UnityInputObjectLists   ; (optional) ObjectList(s) to use as input
            //   .UnityInputIsolateWritableFiles ; (optional) Build writable files individually (default false)
            //   .UnityInputIsolateWritableFilesLimit ; (optional) Disable isolation when many files are writable (default 0)
            //   .UnityInputIsolateListFile ; (optional) Text file containing list of files to isolate
            //   .UnityOutputPath         ; Path to output generated Unity files
            //   .UnityOutputPattern      ; (optional) Pattern of output Unity file names (default Unity*.cpp)
            //   .UnityNumFiles           ; (optional) Number of Unity files to generate (default 1)
            //   .UnityPCH                ; (optional) Precompiled Header file to add to generated Unity files
            //   .PreBuildDependencies    ; (optional) Force targets to be built before this Unity (Rarely needed,
            //                            ; but useful when a Unity should contain generated code)
            //   .Hidden                  ; (optional) Hide a target from -showtargets (default false)
            //   .UseRelativePaths_Experimental ; (optional) Use relative paths for generated Unity files
            // }
            output.WriteLine($@"}}");
        }

        public static void EmitObjectList(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            // ObjectList( alias )         ; Alias
            // {
            //   ; options for compilation
            //   .Compiler                 ; Compiler to use
            //   .CompilerOptions          ; Options for compiler
            //   .CompilerOutputPath       ; Path to store intermediate objects
            //   .CompilerOutputExtension  ; (optional) Specify the file extension for generated objects (default .obj or .o)
            //   .CompilerOutputKeepBaseExtension ; (optional) Append extension instead of replacing it (default: false)
            //   .CompilerOutputPrefix     ; (optional) Specify a prefix for generated objects (default none)
            // 
            //   ; Specify inputs for compilation
            //   .CompilerInputPath           ; (optional) Path to find files in
            //   .CompilerInputPattern        ; (optional) Pattern(s) to use when finding files (default *.cpp)
            //   .CompilerInputPathRecurse    ; (optional) Recurse into dirs when finding files (default true)
            //   .CompilerInputExcludePath    ; (optional) Path(s) to exclude from compilation
            //   .CompilerInputExcludedFiles  ; (optional) File(s) to exclude from compilation (partial, root-relative of full path)
            //   .CompilerInputExcludePattern ; (optional) Pattern(s) to exclude from compilation
            //   .CompilerInputFiles          ; (optional) Explicit array of files to build
            //   .CompilerInputFilesRoot      ; (optional) Root path to use for .obj path generation for explicitly listed files
            //   .CompilerInputUnity          ; (optional) Unity to build (or Unities)
            //   .CompilerInputAllowNoFiles   ; (optional) Don't fail if no inputs are found
            //   .CompilerInputObjectLists    ; (optional) ObjectList(s) whos output should be used as an input
            //   
            //   ; Cache & Distributed compilation control
            //   .AllowCaching             ; (optional) Allow caching of compiled objects if available (default true)
            //   .AllowDistribution        ; (optional) Allow distributed compilation if available (default true)
            // 
            //   ; Custom preprocessor support
            //   .Preprocessor             ; (optional) Compiler to use for preprocessing
            //   .PreprocessorOptions      ; (optional) Args to pass to compiler if using custom preprocessor
            //   
            //   ; Additional compiler options
            //   .CompilerForceUsing       ; (optional) List of objects to be used with /FU
            // 
            //   ; (optional) Properties to control precompiled header use
            //   .PCHInputFile             ; (optional) Precompiled header (.cpp) file to compile
            //   .PCHOutputFile            ; (optional) Precompiled header compilation output
            //   .PCHOptions               ; (optional) Options for compiler for precompiled header
            // 
            //   ; Additional options
            //   .PreBuildDependencies     ; (optional) Force targets to be built before this ObjectList (Rarely needed,
            //                             ; but useful when a ObjectList relies on generated code).
            //   .Hidden                   ; (optional) Hide a target from -showtargets (default false)
            // }
        }

        public static void EmitTest(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            // Test( alias )      // (optional) Alias
            // {
            //   // Options
            //   .TestExecutable          // The executable file to run that will execute the tests
            //   .TestOutput              // Output file for captured test output
            // 
            //   // Additional inputs
            //   .TestInput               // (optional) Input file(s) to pass to executable
            //   .TestInputPath           // (optional) Path to find files in
            //   .TestInputPattern        // (optional) Pattern(s) to use when finding files (default *.*)
            //   .TestInputPathRecurse    // (optional) Recurse into dirs when finding files (default true)
            //   .TestInputExcludePath    // (optional) Path(s) to exclude
            //   .TestInputExcludedFiles  // (optional) File(s) to exclude from compilation (partial, root-relative of full path)
            //   .TestInputExcludePattern // (optional) Pattern(s) to exclude
            // 
            //   // Other
            //   .TestArguments           // (optional) Arguments to pass to test executable
            //   .TestWorkingDir          // (optional) Working dir for test execution
            //   .TestTimeOut             // (optional) TimeOut (in seconds) for test (default: 0, no timeout)
            //   .TestAlwaysShowOutput    // (optional) Show output of tests even when they don't fail (default: false)
            // 
            //    // Additional options
            //   .PreBuildDependencies    // (optional) Force targets to be built before this Test (Rarely needed,
            //                            // but useful when Test relies on externally generated files).
            // 
            //   .Environment             // (optional) Environment variables to use for local build
            //                            // If set, linker uses this environment
            //                            // If not set, linker uses .Environment from your Settings node
            // }
        }

        public static void EmitSourceList(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target,
            SourceList source,
            string name)
        {
            var platform = solution.Platform;
            var toolchain = solution.Toolchain;

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            var configurationPart = (target.SourceTarget.ConfigurationFlavour == ConfigurationFlavour.None)
                ? target.SourceTarget.ConfigurationType.ToString()
                : $@"{target.SourceTarget.ConfigurationType}{target.SourceTarget.ConfigurationFlavour}";

            var targetName = $@"{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}";


            if (source.MergeFiles == 0)
            {
                output.WriteLine($@"; Emit-explicit-sources: {targetName}-{name}");
                // Emit ObjectList

                output.WriteLine($@"ObjectList('Objects-{targetName}-{name}') {{");
                output.WriteLine($@"    .Compiler = 'Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}'");
                output.WriteLine($@"    .CompilerOptions = '{toolchain.FormatCompilerInputFile("%1")} {toolchain.FormatCompilerOutputFile("%2")}'");
                output.WriteLine($@"    .CompilerOutputPath = 'build/obj/{targetName}'");
                output.WriteLine($@"    .CompilerInputFiles = {{");
                foreach (var file in source.Files)
                {
                    output.WriteLine($@"        '{target.SourceTarget.Project.ProjectRootPath}/{file}'");
                }
                output.WriteLine($@"    }}");
                output.WriteLine($@"    .CompilerInputFilesRoot = '{target.SourceTarget.Project.ProjectRootPath}'");
                output.WriteLine($@"}}");
            }
            else
            {
                output.WriteLine($@"Unity('Unity-{targetName}-{name}') {{");
                output.WriteLine($@"    .UnityNumFiles = {source.MergeFiles}");

                if (source.InputPaths != null)
                {
                    output.WriteLine($@"    .UnityInputPath = {{");
                    foreach (var path in source.InputPaths)
                    {
                        output.WriteLine($@"        '{path}'");
                    }
                    output.WriteLine($@"    }}");
                }

                if (source.InputPattern != null)
                {
                    output.WriteLine($@"    .UnityInputPattern = {{ '{source.InputPattern}' }}");
                }

                if (source.ExcludePaths != null)
                {
                    output.WriteLine($@"    .UnityInputExcludePath = {{");
                    foreach(var path in source.ExcludePaths)
                    {
                        output.WriteLine($@"        '{path}'");
                    }
                    output.WriteLine($@"}}");
                }

                if (source.ExcludePattern != null)
                {
                    output.WriteLine($@"    .UnityInputExcludePattern = '{source.ExcludePattern}'");
                }

                output.WriteLine($@"    .UnityOutputPath = 'build/unity/{targetName}'");
                output.WriteLine($@"    .UnityOutputPattern = '{target.Name}-unity-*.cxx'");

                //   .UnityInputPath          ; (optional) Path (or paths) to find files
                //   .UnityInputExcludePath   ; (optional) Path (or paths) in which to ignore files
                //   .UnityInputExcludePattern; (optional) Wildcard pattern(s) of files/folders to exclude
                //   .UnityInputPattern       ; (optional) Pattern(s) of files to find (default *.cpp)
                //   .UnityInputPathRecurse   ; (optional) Recurse when searching for files (default true)
                //   .UnityInputFiles         ; (optional) Explicit list of files to include
                //   .UnityInputExcludedFiles ; (optional) Explicit list of excluded files (partial, root-relative or full path)
                //   .UnityInputIsolatedFiles ; (optional) List of files to exclude from unity, but still compile (partial end or root-relative)
                //   .UnityInputObjectLists   ; (optional) ObjectList(s) to use as input
                //   .UnityOutputPath         ; Path to output generated Unity files
                //   .UnityOutputPattern      ; (optional) Pattern of output Unity file names (default Unity*.cpp)
                //   .UnityPCH                ; (optional) Precompiled Header file to add to generated Unity files
                // }
                output.WriteLine($@"}}");

                output.WriteLine($@"ObjectList('Objects-{targetName}-{name}') {{");
                output.WriteLine($@"    .Compiler = 'Compiler-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}'");
                output.WriteLine($@"    .CompilerOptions = '{toolchain.FormatCompilerInputFile("%1")} {toolchain.FormatCompilerOutputFile("%2")}'");
                output.WriteLine($@"    .CompilerOutputPath = 'build/obj/{targetName}'");
                output.WriteLine($@"    .CompilerInputUnity = 'Unity-{targetName}-{name}'");
                output.WriteLine($@"}}");
            }

        }

        public static void EmitStaticLibrary(
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

            var sourcePath = target.SourceTarget.Project.ProjectRootPath;

            for (var i = 0; i < target.SourceTarget.Sources.Count; ++i)
            {
                var source = target.SourceTarget.Sources[i];
                EmitSourceList(output, solution, target, source, $@"Source-{i}");
            }

            if (target.SourceTarget.Sources.Count == 0)
            {
                // Otherwise, add default source list
                EmitSourceList(output, solution, target, new SourceList()
                {
                    MergeFiles = 1,
                    InputPaths = new[]
                    {
                        sourcePath,
                    },
                    InputPattern = "*.cxx",
                }, $@"Source");
            }

            // Library( alias )            ; (optional) Alias
            // {
            //   ; options for compilation
            //   .Compiler                 ; Compiler to use
            //   .CompilerOptions          ; Options for compiler
            //   .CompilerOutputPath       ; Path to store intermediate objects
            //   .CompilerOutputExtension  ; (optional) Specify the file extension for generated objects (default .obj or .o)
            //   .CompilerOutputPrefix     ; (optional) Specify a prefix for generated objects (default none)
            // 
            //   ; Options for librarian
            //   .Librarian                ; Librarian to collect intermediate objects
            //   .LibrarianOptions         ; Options for librarian
            //   .LibrarianType            ; (optional) Specify the librarian type. Valid options include:
            //                             ; auto, msvc, ar, ar-orbis, greenhills-ax
            //                             ; Default is 'auto' (use the librarian executable name to detect)
            //   .LibrarianOutput          ; Output path for lib file
            //   .LibrarianAdditionalInputs; (optional) Additional inputs to merge into library
            //   .LibrarianAllowResponseFile ; (optional) Allow response files to be used if not auto-detected (default: false)  
            //   .LibrarianForceResponseFile ; (optional) Force use of response files (default: false)
            // 
            //   ; Specify inputs for compilation
            //   .CompilerInputPath           ; (optional) Path to find files in
            //   .CompilerInputPattern        ; (optional) Pattern(s) to use when finding files (default *.cpp)
            //   .CompilerInputPathRecurse    ; (optional) Recurse into dirs when finding files (default true)
            //   .CompilerInputExcludePath    ; (optional) Path(s) to exclude from compilation
            //   .CompilerInputExcludedFiles  ; (optional) File(s) to exclude from compilation (partial, root-relative of full path)
            //   .CompilerInputExcludePattern ; (optional) Pattern(s) to exclude from compilation
            //   .CompilerInputFiles          ; (optional) Explicit array of files to build
            //   .CompilerInputFilesRoot      ; (optional) Root path to use for .obj path generation for explicitly listed files
            //   .CompilerInputUnity          ; (optional) Unity to build (or Unities)
            //   .CompilerInputObjectLists    ; (optional) ObjectList(s) whos output should be used as an input
            // 
            //   ; Cache & Distributed compilation control
            //   .AllowCaching             ; (optional) Allow caching of compiled objects if available (default true)
            //   .AllowDistribution        ; (optional) Allow distributed compilation if available (default true)
            //   
            //   ; Custom preprocessor support
            //   .Preprocessor             ; (optional) Compiler to use for preprocessing
            //   .PreprocessorOptions      ; (optional) Args to pass to compiler if using custom preprocessor
            //   
            //   ; Additional compiler options
            //   .CompilerForceUsing       ; (optional) List of objects to be used with /FU
            // 
            //   ; (optional) Properties to control precompiled header use
            //   .PCHInputFile             ; (optional) Precompiled header (.cpp) file to compile
            //   .PCHOutputFile            ; (optional) Precompiled header compilation output
            //   .PCHOptions               ; (optional) Options for compiler for precompiled header
            // 
            //   ; Additional options
            //   .PreBuildDependencies     ; (optional) Force targets to be built before this library (Rarely needed,
            //                             ; but useful when a library relies on generated code).
            // 
            //   .Environment              ; (optional) Environment variables to use for local build
            //                             ; If set, librarian uses this environment
            //                             ; If not set, librarian uses .Environment from your Settings node
            //   .Hidden                   ; (optional) Hide a target from -showtargets (default false)
            // }
        }

        public static void EmitSharedLibrary(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            // DLL( alias )               ; (optional) Alias
            // {
            //   .Linker                  ; Linker executable to use
            //   .LinkerOutput            ; Output from linker
            //   .LinkerOptions           ; Options to pass to linker
            //   .Libraries               ; Libraries to link into DLL
            //   .LinkerLinkObjects       ; (optional) Link objects used to make libs instead of libs (default true)
            //   .LinkerAssemblyResources ; (optional) List of assembly resources to use with %3
            //   
            //   .LinkerStampExe          ; (optional) Executable to run post-link to "stamp" executable in-place
            //   .LinkerStampExeArgs      ; (optional) Arguments to pass to LinkerStampExe
            //   .LinkerType              ; (optional) Specify the linker type. Valid options include: 
            //                            ; auto, msvc, gcc, snc-ps3, clang-orbis, greenhills-exlr, codewarrior-ld
            //                            ; Default is 'auto' (use the linker executable name to detect)
            //   .LinkerAllowResponseFile ; (optional) Allow response files to be used if not auto-detected (default: false)
            //   .LinkerForceResponseFile ; (optional) Force use of response files (default: false)
            // 
            //   ; Additional options
            //   .PreBuildDependencies    ; (optional) Force targets to be built before this DLL (Rarely needed,
            //                            ; but useful when DLL relies on externally generated files).
            // 
            //   .Environment             ; (optional) Environment variables to use for local build
            //                            ; If set, linker uses this environment
            //                            ; If not set, linker uses .Environment from your Settings node
            // }
        }

        public static void EmitExecutable(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            // Executable( alias )        ; (optional) Alias
            // {
            //   .Linker                  ; Linker executable to use
            //   .LinkerOutput            ; Output from linker
            //   .LinkerOptions           ; Options to pass to linker
            //   .Libraries               ; Libraries to link into executable
            //   .LinkerLinkObjects       ; (optional) Link objects used to make libs instead of libs (default false)
            //   .LinkerAssemblyResources ; (optional) List of assembly resources to use with %3
            //   
            //   .LinkerStampExe          ; (optional) Executable to run post-link to "stamp" executable in-place
            //   .LinkerStampExeArgs      ; (optional) Arguments to pass to LinkerStampExe 
            //   .LinkerType              ; (optional) Specify the linker type. Valid options include: 
            //                            ; auto, msvc, gcc, snc-ps3, clang-orbis, greenhills-exlr, codewarrior-ld
            //                            ; Default is 'auto' (use the linker executable name to detect)
            //   .LinkerAllowResponseFile ; (optional) Allow response files to be used if not auto-detected (default: false)
            //   .LinkerForceResponseFile ; (optional) Force use of response files (default: false)
            // 
            //   ; Additional options
            //   .PreBuildDependencies    ; (optional) Force targets to be built before this Executable (Rarely needed,
            //                            ; but useful when Executable relies on externally generated files).                           
            // 
            //   .Environment             ; (optional) Environment variables to use for local build
            //                            ; If set, linker uses this environment
            //                            ; If not set, linker uses .Environment from your Settings node
            // }
        }


        public static void EmitTarget(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            var targetType = target.SourceTarget.TargetType;

            if (targetType == TargetType.SharedLibrary)
            {
                EmitSharedLibrary(output, solution, target);
            }
            else if (targetType == TargetType.StaticLibrary)
            {
                EmitStaticLibrary(output, solution, target);
            }
            else if (targetType == TargetType.HeaderLibrary)
            {
                ;
            }
            else if (targetType == TargetType.Application)
            {
                EmitExecutable(output, solution, target);
            }
        }
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

        private string GetCommandLinePath()
        {
            var sb = new StringBuilder();

            if (this.m_Settings.UnityBuild.GetValueOrDefault(false))
            {
                sb.Append(" -nounity");
            }

            if (this.m_Settings.Distributed.GetValueOrDefault(true))
            {
                sb.Append(" -dist");
            }

            if (this.m_Settings.Monitor.GetValueOrDefault(true))
            {
                sb.Append(" -monitor");
            }

            if (this.m_Settings.Report.GetValueOrDefault(true))
            {
                sb.Append(" -report");
            }

            return sb.ToString();
        }

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

            if (toolchain.CompilerExtraFiles != null)
            {
                output.WriteLine($@"    .ExtraFiles = {{");

                foreach (var extra in toolchain.CompilerExtraFiles)
                {
                    output.WriteLine($@"        '{extra}',");
                }

                output.WriteLine($@"    }}");
            }

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

        private static void EmitSources(
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

            var sourcePath = target.SourceTarget.Project.ProjectRootPath;

            output.WriteLine($@"Unity('Unity-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}') {{");
            output.WriteLine($@"    .UnityInputPath = '{sourcePath}'");
            output.WriteLine($@"    .UnityInputPattern = {{ '*.cxx' }}");
            output.WriteLine($@"    .UnityOutputPath  = 'unity/{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");
            output.WriteLine($@"    .UnityOutputPattern = '{target.Name}-unity-*.cxx'");
            output.WriteLine($@"    .UnityNumFiles = 1");

            output.WriteLine($@"    .UnityInputExcludePattern = {{");
            output.WriteLine($@"        '*Linux.*.cxx'");
            output.WriteLine($@"        '*Android.*.cxx'");
            output.WriteLine($@"        '*Posix.*.cxx'");
            output.WriteLine($@"        '*UWP.*.cxx'");
            output.WriteLine($@"    }}");
            output.WriteLine($@"}}");

            output.WriteLine($@"ObjectList('Obj-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}') {{");
            output.WriteLine($@"    Using(.{variablePrefix}_CommonPlatformToolchain)");
            output.WriteLine($@"    .CompilerOutputPath = 'obj/{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");
            output.WriteLine($@"    .CompilerOptions = ''");
            output.WriteLine($@"        + ' {toolchain.FormatCompilerInputFile("%1")}'");
            output.WriteLine($@"        + ' {toolchain.FormatCompilerOutputFile("%2")}'");
            output.WriteLine($@"        + .{variablePrefix}_PlatformIncludePaths");
            output.WriteLine($@"        + .{variablePrefix}_ToolchainIncludePaths");

            foreach (var item in toolchain.GetCompilerCommandLine(target.SourceTarget))
            {
                output.WriteLine($@"        + ' {item}'");
            }

            foreach (var path in target.PrivateIncludePaths)
            {
                output.WriteLine($@"        + ' {toolchain.FormatIncludePath(path)}'");
            }

            foreach (var path in target.PrivateDefines)
            {
                output.WriteLine($@"        + ' {toolchain.FormatDefine(path)}'");
            }

            output.WriteLine($@"    .CompilerInputUnity = 'Unity-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");
            output.WriteLine($@"}}");
        }

        private static string MapToFunctionType(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Application:
                    return "Executable";

                case TargetType.HeaderLibrary:
                    return null;
                case TargetType.SharedLibrary:
                    return "DLL";
                case TargetType.StaticLibrary:
                    return "Library";
            }

            throw new NotSupportedException();
        }

        private static void EmitProjectDefinition(
            StreamWriter output,
            EvaluatedSolution solution,
            ResolvedTarget target)
        {
            FastbuildTemplate.EmitTarget(output, solution, target);
#if false
            var platform = solution.Platform;
            var toolchain = solution.Toolchain;
            var targetCompileCommands = toolchain.GetCompilerCommandLine(target.SourceTarget);

            var variablePrefix = $@"{toolchain.ToolchainType}_{toolchain.ArchitectureType}_{platform.PlatformType}";

            var configurationPart = (target.SourceTarget.ConfigurationFlavour == ConfigurationFlavour.None)
                ? target.SourceTarget.ConfigurationType.ToString()
                : $@"{target.SourceTarget.ConfigurationType}{target.SourceTarget.ConfigurationFlavour}";

            var targetFileName = platform.AdjustTargetName(target.Name, target.SourceTarget.TargetType);

            EmitSources(output, solution, target);

            var functionType = MapToFunctionType(target.SourceTarget.TargetType);

            output.WriteLine($@"; Location: {target.SourceTarget.Project.ProjectRootPath}");
            output.WriteLine($@"{functionType}('Target-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}') {{");
            output.WriteLine($@"    Using(.{variablePrefix}_CommonPlatformToolchain)");
            output.WriteLine($@"    .CompilerOutputPath = 'obj-1/{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");
            output.WriteLine($@"    .CompilerOptions = ''");
            output.WriteLine($@"        + ' {toolchain.FormatCompilerInputFile("%1")}'");
            output.WriteLine($@"        + ' {toolchain.FormatCompilerOutputFile("%2")}'");
            output.WriteLine($@"        + .{variablePrefix}_PlatformIncludePaths");
            output.WriteLine($@"        + .{variablePrefix}_ToolchainIncludePaths");

            foreach (var item in targetCompileCommands)
            {
                output.WriteLine($@"        + ' {item}'");
            }

            foreach (var path in target.PrivateIncludePaths)
            {
                output.WriteLine($@"        + ' {toolchain.FormatIncludePath(path)}'");
            }

            foreach (var path in target.PrivateDefines)
            {
                output.WriteLine($@"        + ' {toolchain.FormatDefine(path)}'");
            }

            if (target.SourceTarget.TargetType == TargetType.StaticLibrary)
            {
                output.WriteLine($@"    .CompilerInputObjectLists = {{");
                output.WriteLine($@"        'Obj-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");
                output.WriteLine($@"    }}");
            }

            var linker = (target.SourceTarget.TargetType == TargetType.SharedLibrary) ? "Linker" : "Librarian";

            output.WriteLine($@"    .{linker}Output    = '.generated/bin/{variablePrefix}{configurationPart}/{targetFileName}'");
            output.WriteLine($@"    .{linker}Options = ''");
            output.WriteLine($@"        + ' /NOLOGO'");
            output.WriteLine($@"        + ' /MACHINE:X64'");
            output.WriteLine($@"        + .{variablePrefix}_ToolchainLibraryPaths");
            output.WriteLine($@"        + .{variablePrefix}_PlatformLibraryPaths");

            output.WriteLine($@"        + ' {toolchain.FormatLinkerGroupStart}{toolchain.FormatLinkerInputFile("%1")}{toolchain.FormatLinkerGroupEnd}'");

            output.WriteLine($@"        + ' {toolchain.FormatLinkerOutputFile("%2")}'");

            if (target.SourceTarget.TargetType == TargetType.SharedLibrary)
            {
                output.WriteLine($@"        + ' /DLL'");
            }

            foreach (var path in target.PrivateLibraryPaths)
            {
                output.WriteLine($@"        + ' {toolchain.FormatLibraryPath(path)}'");
            }

            foreach (var lib in target.PrivateLibraries)
            {
                output.WriteLine($@"        + ' {toolchain.FormatLink(lib)}'");
            }

            output.WriteLine($@"    .Libraries = {{");

            foreach (var dep in target.PrivateDependencies)
            {
                output.WriteLine($@"        'Target-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{dep.Name}-{configurationPart}'");
            }

            output.WriteLine($@"        'Obj-{toolchain.ToolchainType}-{toolchain.ArchitectureType}-{platform.PlatformType}-{target.Name}-{configurationPart}'");

            output.WriteLine($@"    }}");

            output.WriteLine($@"}}");
#endif
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

            output.WriteLine($@".CommandLineBuild = '{this.GetCommandLinePath()}'");

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
                        if (t.SourceTarget.TargetType == TargetType.HeaderLibrary)
                        {
                            continue;
                        }

                        //output.WriteLine("{");
                        EmitProjectDefinition(output, s, t);
                        //output.WriteLine("}");
                    }
                }

                Trace.Unindent();
            }

            Trace.Unindent();
        }
    }
}
