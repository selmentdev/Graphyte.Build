using System;
using System.Linq;

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

    sealed class MsvcToolchain
        : BaseToolchain
    {
        private static string MapArchitectureType(ArchitectureType architectureType)
        {
            switch (architectureType)
            {
                case ArchitectureType.ARM:
                    return "arm";
                case ArchitectureType.ARM64:
                    return "arm64";
                case ArchitectureType.X64:
                    return "x64";
                case ArchitectureType.X86:
                    return "x86";
                case ArchitectureType.PPC64:
                    break;
            }

            throw new ArgumentOutOfRangeException(nameof(architectureType));
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

            var root = $@"{vsInstance.Location}/VC/Tools/MSVC/{vsInstance.Toolset}";

            var rootBin = $@"{root}/bin/HostX64/{architectureMnemonic}";

            this.CompilerExecutable = $@"{rootBin}/cl.exe";

            this.CompilerExtraFiles = new[]
            {
                $@"{rootBin}/1033/clui.dll",
                $@"{rootBin}/1033/mspft140ui.dll",
                $@"{rootBin}/atlprov.dll",
                $@"{rootBin}/c1.dll",
                $@"{rootBin}/c1xx.dll",
                $@"{rootBin}/c2.dll",
                $@"{rootBin}/msobj140.dll",
                $@"{rootBin}/mspdb140.dll",
                $@"{rootBin}/mspdbcore.dll",
                $@"{rootBin}/mspdbsrv.exe",
                $@"{rootBin}/mspft140.dll",
                $@"{rootBin}/msvcp140.dll",
                $@"{rootBin}/tbbmalloc.dll",
                $@"{rootBin}/vcruntime140.dll",
            };

            this.LinkerExecutable = $@"{rootBin}/link.exe";
            this.LibrarianExecutable = $@"{rootBin}/lib.exe";

            this.IncludePaths = new[]
            {
                $@"{vsInstance.Location}/VC/Tools/MSVC/{vsInstance.Version}/include",
                $@"{vsInstance.Location}/VC/Tools/MSVC/{vsInstance.Version}/atlmfc/include",
                $@"{vsInstance.Location}/VC/Auxiliary/VS/include",
            };
            this.LibraryPaths = new[]
            {
                $@"{vsInstance.Location}/VC/Tools/MSVC/{vsInstance.Version}/lib/{architectureMnemonic}"
            };
        }

        private readonly MsvcToolchainSettings m_Settings;

        public override ToolchainType ToolchainType => ToolchainType.MSVC;

        public override void PostConfigureTarget(Target target)
        {
        }

        public override void PreConfigureTarget(Target target)
        {
        }
    }
}
