using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Platforms.Windows
{
    abstract class BaseWindowsPlatform
        : BasePlatform
    {
        protected BaseWindowsPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
        }

        protected static string MapArchitectureType(ArchitectureType architectureType)
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

        protected void InitializeBasePaths(string version)
        {
            if (WindowsSdkProvider.IsSupported)
            {
                var location = WindowsSdkProvider.Location;

                var available = WindowsSdkProvider.Versions;

                if (string.IsNullOrEmpty(version))
                {
                    throw new Exception($@"Windows SDK with version ""{version}"" is not available");
                }

                if (!available.Contains(version))
                {
                    throw new Exception($@"Windows SDK with version ""{version}"" is not available");
                }

                this.IncludePaths = new[]
                {
                    $@"{location}\Include\{version}\shared",
                    $@"{location}\Include\{version}\ucrt",
                    $@"{location}\Include\{version}\um",
                    $@"{location}\Include\{version}\winrt",
                    $@"{location}\Include\{version}\cppwinrt",
                };

                var suffix = MapArchitectureType(this.ArchitectureType);

                this.LibraryPaths = new[]
                {
                    $@"{location}\Lib\{version}\um\{suffix}",
                    $@"{location}\Lib\{version}\ucrt\{suffix}",
                };

                this.ResourceCompilerExecutable = $@"{location}/bin/{version}/x64/rc.exe";
            }
        }

        public string ResourceCompilerExecutable { get; private set; }
    }
}
