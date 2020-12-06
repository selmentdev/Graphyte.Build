using System;
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
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Version, "include"),
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Version, "atlmfc", "include"),
                Path.Combine(vsInstance.Location, "VC", "Auxiliary", "VS", "include"),
            };

            this.LibraryPaths = new[]
            {
                Path.Combine(vsInstance.Location, "VC", "Tools", "MSVC", vsInstance.Version, "lib", architectureMnemonic),
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
    }
}
