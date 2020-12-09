using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Toolchains.VisualStudio
{
    /// <summary>
    /// Represents Visual Studio Toolchain settings.
    /// </summary>
    public sealed class MsvcToolchainSettings
        : BaseToolchainSettings
    {
        /// <summary>
        /// Gets or sets version of toolkit.
        /// </summary>
        public string Toolkit { get; set; } = "v142";

        /// <summary>
        /// Gets or sets value indicating whether Address Sanitizer should be used.
        /// </summary>
        public bool AddressSanitizer { get; set; } = false;

        /// <summary>
        /// Gets or sets value indicating whether static analyzer should be used.
        /// </summary>
        public bool StaticAnalyzer { get; set; } = false;
    }

    public sealed class MsvcToolchain
        : BaseToolchain
    {
        private static string MapArchitectureType(ArchitectureType architectureType)
        {
            switch (architectureType)
            {
                case ArchitectureType.ARM64:
                    return "arm64";
                case ArchitectureType.X64:
                    return "x64";
            }

            throw new ArgumentOutOfRangeException(nameof(architectureType));
        }

        private static readonly string HostPathPrefix;

        static MsvcToolchain()
        {
            switch (RuntimeInformation.OSArchitecture)
            {
                case Architecture.Arm:
                    HostPathPrefix = "arm";
                    break;

                case Architecture.Arm64:
                    HostPathPrefix = "arm64";
                    break;

                case Architecture.X86:
                    HostPathPrefix = "Hostx86";
                    break;

                case Architecture.X64:
                    HostPathPrefix = "Hostx64";
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public MsvcToolchain(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
            this.m_Settings = profile.GetSection<MsvcToolchainSettings>();

            var vsInstances = VisualStudioToolchainProvider.Instances;

            var vsInstance = vsInstances.First(x => x.Toolkit == this.m_Settings.Toolkit);

            var architectureMnemonic = MapArchitectureType(this.ArchitectureType);

            var root = Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Toolset);

            this.RootPath = Path.Combine(root, "bin", HostPathPrefix, architectureMnemonic);

            this.CompilerExecutable = Path.Combine(this.RootPath, "cl.exe");

            this.CompilerExtraFiles = new[]
            {
                Path.Combine(this.RootPath, "1033", "clui.dll"),
                Path.Combine(this.RootPath, "1033", "mspft140ui.dll"),
                Path.Combine(this.RootPath, "atlprov.dll"),
                Path.Combine(this.RootPath, "c1.dll"),
                Path.Combine(this.RootPath, "c1xx.dll"),
                Path.Combine(this.RootPath, "c2.dll"),
                Path.Combine(this.RootPath, "msobj140.dll"),
                Path.Combine(this.RootPath, "mspdb140.dll"),
                Path.Combine(this.RootPath, "mspdbcore.dll"),
                Path.Combine(this.RootPath, "mspdbsrv.exe"),
                Path.Combine(this.RootPath, "mspft140.dll"),
                Path.Combine(this.RootPath, "msvcp140.dll"),
                Path.Combine(this.RootPath, "tbbmalloc.dll"),
                Path.Combine(this.RootPath, "vcruntime140.dll"),
            };

            this.LinkerExecutable = Path.Combine(this.RootPath, "link.exe");
            this.LibrarianExecutable = Path.Combine(this.RootPath, "lib.exe");

            this.IncludePaths = new[]
            {
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Toolset, "include"),
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Toolset, "atlmfc", "include"),
                Path.Combine(vsInstance.Location, "VC", "Auxiliary", "VS", "include"),
            };

            this.LibraryPaths = new[]
            {
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Toolset, "lib", architectureMnemonic),
            };
        }

        private readonly MsvcToolchainSettings m_Settings;

        public override ToolchainType ToolchainType => ToolchainType.MSVC;

        public override string FormatDefine(string value)
        {
            return $@"/D{value}";
        }

        public override string FormatLink(string value)
        {
            return value;
        }

        public override string FormatIncludePath(string value)
        {
            return $@"/I""{value}""";
        }

        public override string FormatLibraryPath(string value)
        {
            return $@"/LIBPATH:""{value}""";
        }

        public override string FormatCompilerInputFile(string input)
        {
            return $@"/c ""{input}""";
        }

        public override string FormatCompilerOutputFile(string output)
        {
            return $@"/Fo""{output}""";
        }

        public override string FormatLinkerInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLinkerOutputFile(string output)
        {
            return $@"/OUT:""{output}""";
        }

        public override string FormatLibrarianInputFile(string input)
        {
            return $@"""{input}""";
        }

        public override string FormatLibrarianOutputFile(string output)
        {
            return $@"/OUT:""{output}""";
        }

        public override IEnumerable<string> GetCompilerCommandLine(Target target)
        {
            yield return "/nologo"; // Don't display compiler logo banner

            // /ZI -- PDB + edit and continue
            // /Zi -- PDB
            yield return "/Z7"; // Generate debug symbols inside object files
            yield return "/bigobj";// Increase Number of Sections in .Obj file
            yield return "/diagnostics:caret";     // Use caret (^) to indicate error location
            yield return "/EHa";                   // Support asynchronous structured exception handling (SEH) with the native C++
            yield return "/errorReport:send";      // Automatically sends reports of internal compiler errors to Microsoft

            //yield return "experimental:preprocessor";     // Enable new compliant preprocessor features when available (not available in VS2019, v16.4.3)

            yield return "/FC";                    // Full Path of Source Code File in Diagnostics
            yield return "/fp:fast";               // ...reorder, combine, or simplify floating-point operations to optimize floating-point code for speed and space
            yield return "/Gd";                    // the __cdecl calling convention for all functions except C++ member functions and functions that are marked __stdcall, __fastcall, or __vectorcall
            yield return "/GF";                    // Eliminate Duplicate Strings
            yield return "/GR-";                   // Enable Run-Time Type Information
            yield return "/Gw";                    // Optimize Global Data
            yield return "/Gy";                    // Enable Function-Level Linking
            yield return "/JMC-";                  // Disable Just My Code
            yield return "/permissive-";           // Enable Standards conformance
            yield return "/std:c++latest";         // Specify Language Standard Version

            //+ ' /wd4263'
            //+ ' /wd4264'
            //+ ' /wd4275'

            yield return "/WX";                    // Treats all compiler warnings as errors.
            yield return "/Zc:__cplusplus";        // Enable the __cplusplus macro to report the supported standard
            yield return "/Zc:char8_t";            //
            yield return "/Zc:externConstexpr";    // Enable extern constexpr variables
            yield return "/Zc:forScope";           // Force Conformance in for Loop Scope
            yield return "/Zc:inline";             // Remove unreferenced COMDAT
            yield return "/Zc:referenceBinding";   // Enforce reference binding rules
            yield return "/Zc:sizedDealloc";       // Enable Global Sized Deallocation Functions
            yield return "/Zc:ternary";            // Enforce conditional operator rules
            yield return "/Zc:throwingNew";        // Assume operator new throws
            yield return "/Zc:wchar_t";            // wchar_t Is Native Type

            // yield return "GL"; (Whole Program Optimization)

            yield return "/MDd";                   // Use Debug DLL Run-Time Library
            yield return "/Od";                    // Disable optimizations
            yield return "/Ob0";                   // Disable Inline Function Expansion
            yield return "/GS";                    // Enable Buffer Security Check
            yield return "/sdl";                   // Enable Additional Security Checks
            yield return "/RTCsu";                 // Enable Run-Time Error Checks
            yield return "/arch:AVX";
        }
    }
}
