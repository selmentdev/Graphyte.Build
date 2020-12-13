using Graphyte.Build.Framework;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    abstract class BaseWindowsPlatform : Platform
    {
        protected BaseWindowsPlatform(Profile profile, TargetArchitecture targetArchitecture)
            : base(profile, targetArchitecture)

        {
        }

        protected static string MapTargetArchitecture(TargetArchitecture targetArchitecture)
        {
            switch (targetArchitecture)
            {
                case TargetArchitecture.Arm64:
                    return "arm64";
                case TargetArchitecture.X64:
                    return "x64";
            }

            throw new ArgumentOutOfRangeException(nameof(targetArchitecture));
        }

        private static string GetHostPrefix()
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

        protected void InitializeBasePath(string version)
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

                var suffix = MapTargetArchitecture(this.TargetArchitecture);

                this.LibraryPaths = new[]
                {
                    Path.Combine(location, "Lib", version, "um", suffix),
                    Path.Combine(location, "Lib", version, "ucrt", suffix),
                };

                var binPrefix = GetHostPrefix();

                this.ResourceCompilerExecutable = Path.Combine(location, "bin", version, binPrefix, "rc.exe");
            }
        }

        public string ResourceCompilerExecutable { get; private set; }
    }
}
