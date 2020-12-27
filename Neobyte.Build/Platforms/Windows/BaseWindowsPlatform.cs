using Neobyte.Build.Framework;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Neobyte.Build.Platforms.Windows
{
    static class WindowsSupport
    {
        static WindowsSupport()
        {
            switch (RuntimeInformation.OSArchitecture)
            {
                case Architecture.Arm:
                    WindowsSupport.HostPrefix = "arm";
                    break;

                case Architecture.Arm64:
                    WindowsSupport.HostPrefix = "arm64";
                    break;

                case Architecture.X64:
                    WindowsSupport.HostPrefix = "x64";
                    break;

                case Architecture.X86:
                    WindowsSupport.HostPrefix = "x86";
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public static string MapTargetArchitecture(TargetArchitecture architecture)
        {
            switch (architecture)
            {
                case TargetArchitecture.Arm64:
                    return "arm64";
                case TargetArchitecture.X64:
                    return "x64";
            }

            throw new ArgumentOutOfRangeException(nameof(architecture));
        }

        public static string HostPrefix { get; }
    }

    abstract class BaseWindowsPlatform
        : PlatformBase
    {
        protected BaseWindowsPlatform(Profile profile, TargetArchitecture architecture)
            : base(profile, architecture)
        {
        }

        protected void InitializeBasePath(string? version)
        {
            if (WindowsSdkProvider.IsSupported)
            {
                if (string.IsNullOrEmpty(version))
                {
                    throw new Exception($@"Windows SDK with version ""{version}"" is not available");
                }

                var location = WindowsSdkProvider.Location;

                var available = WindowsSdkProvider.Versions;

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

                var suffix = WindowsSupport.MapTargetArchitecture(this.Architecture);

                this.LibraryPaths = new[]
                {
                    Path.Combine(location, "Lib", version, "um", suffix),
                    Path.Combine(location, "Lib", version, "ucrt", suffix),
                };

                var binPrefix = WindowsSupport.HostPrefix;

                this.ResourceCompilerExecutable = new FileInfo(Path.Combine(location, "bin", version, binPrefix, "rc.exe"));
            }
        }

        public FileInfo? ResourceCompilerExecutable { get; private set; }
    }
}
