using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

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
                case ArchitectureType.ARM64:
                    return "arm64";
                case ArchitectureType.X64:
                    return "x64";
            }

            throw new ArgumentOutOfRangeException(nameof(architectureType));
        }

        private static string GetBinPrefix()
        {
            switch (RuntimeInformation.OSArchitecture)
            {
                case Architecture.Arm:
                    return "arm";

                case Architecture.Arm64:
                    return "arm64";

                case Architecture.X64:
                    return "x64";

                case Architecture.X86:
                    return "x86";

                default:
                    break;
            }

            throw new NotImplementedException();
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
                    Path.Combine(location, "Include", version, "shared"),
                    Path.Combine(location, "Include", version, "ucrt"),
                    Path.Combine(location, "Include", version, "um"),
                    Path.Combine(location, "Include", version, "winrt"),
                    Path.Combine(location, "Include", version, "cppwinrt"),
                };

                var suffix = MapArchitectureType(this.ArchitectureType);

                this.LibraryPaths = new[]
                {
                    Path.Combine(location, "Lib", version, "um", suffix),
                    Path.Combine(location, "Lib", version, "ucrt", suffix),
                };

                var binPrefix = GetBinPrefix();

                this.ResourceCompilerExecutable = Path.Combine(location, "bin", version, binPrefix, "rc.exe");
            }
        }

        public string ResourceCompilerExecutable { get; private set; }
    }
}
