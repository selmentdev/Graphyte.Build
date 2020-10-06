using System;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public class WindowsPlatform : BasePlatform
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public override bool IsSupported(TargetTuple tuple)
        {
            if (!this.IsHostSupported)
            {
                return false;
            }

            if (tuple.Platform != PlatformType.Windows)
            {
                return false;
            }

            if (tuple.Toolset == ToolsetType.GCC)
            {
                return false;
            }

            return true;
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            return targetType switch
            {
                TargetType.SharedLibrary => $@"lib{name}.dll",
                TargetType.StaticLibrary => $@"lib{name}.lib",
                TargetType.HeaderLibrary => name,
                TargetType.Application => $@"{name}.exe",
                _ => throw new ArgumentOutOfRangeException(nameof(targetType)),
            };
        }

        public override void PreConfigureTarget(Target target, IContext context)
        {
            target.PrivateDefines.Add("_WIN32");
            target.PrivateDefines.Add("_WIN32_WINNT=0x0A00");
            target.PrivateDefines.Add("WINAPI_FAMILY=WINAPI_FAMILY_DESKTOP_APP");
            target.PrivateDefines.Add("GX_WINDOWS_SDK_BUILD_VERSION=$WindowsSdkBuildVersion$");
            target.PrivateDefines.Add("__WINDOWS__");
            target.PrivateDefines.Add("__STDC_WANT_LIB_EXT1__=1");
            target.PrivateDefines.Add("__STDINT_MACROS");
            target.PrivateDefines.Add("__STDINT_LIMITS");
            target.PrivateDefines.Add("__STDC_CONSTANT_MACROS");
            target.PrivateDefines.Add("__STDC_FORMAT_MACROS");
            target.PrivateDefines.Add("__STDC_LIMIT_MACROS");
            target.PrivateDefines.Add("_UNICODE");
            target.PrivateDefines.Add("UNICODE");
            target.PrivateDefines.Add("FMT_SHARED=1");     // this one comes from library
            target.PrivateDefines.Add("FMT_EXCEPTIONS=0"); // this one comes from library

            target.PrivateIncludePaths.Add(@"$WindowsSdkLocation$\Include\$WindowsSdkVersion$\shared");
            target.PrivateIncludePaths.Add(@"$WindowsSdkLocation$\Include\$WindowsSdkVersion$\ucrt");
            target.PrivateIncludePaths.Add(@"$WindowsSdkLocation$\Include\$WindowsSdkVersion$\um");
            target.PrivateIncludePaths.Add(@"$WindowsSdkLocation$\Include\$WindowsSdkVersion$\winrt");
            target.PrivateIncludePaths.Add(@"$WindowsSdkLocation$\Include\$WindowsSdkVersion$\cppwinrt");

            target.PrivateDefines.Add("_HAS_EXCEPTIONS=0");
            target.PrivateDefines.Add("_HAS_ITERATOR_DEBUGGING=0");
            target.PrivateDefines.Add("_SCL_SECURE=0");
            target.PrivateDefines.Add("_SECURE_SCL=0");
            target.PrivateDefines.Add("_CRT_SECURE_INVALID_PARAMETER=");

            target.PrivateIncludePaths.Add(@"$VsToolsLocation$/VC/Tools/MSVC/$VsToolsVersion$/include");
            target.PrivateIncludePaths.Add(@"$VsToolsLocation$/VC/Tools/MSVC/$VsToolsVersion$/atlmfc/include");
            target.PrivateIncludePaths.Add(@"$VsToolsLocation$/VC/Auxiliary/VS/include");

            // Post-configure!
            if (target.Type == TargetType.SharedLibrary)
            {
                target.PrivateDefines.Add("_WINDLL");
            }

            // TODO: Determine runtime type!
            // if (target.RuntimeType == RuntimeType.Debug)
            //   target.PrivateLibraries.Add("msvcprtd.lib");
            // else
            //   target.PrivateLibraries.Add("msvcprt.lib");

            if (context.Architecture == ArchitectureType.X64)
            {
                target.PrivateLibraryPaths.Add(@"$VsToolsLocation$/VC/Tools/MSVC/$VsToolsVersion$/lib/x64");
                target.PrivateLibraryPaths.Add(@"$WindowsSdkLocation$\Lib\$WindowsSdkVersion$\um\x64");
                target.PrivateLibraryPaths.Add(@"$WindowsSdkLocation$\Lib\$WindowsSdkVersion$\ucrt\x64");
            }
            else if (context.Architecture == ArchitectureType.Arm64)
            {
                target.PrivateLibraryPaths.Add(@"$VsToolsLocation$/VC/Tools/MSVC/$VsToolsVersion$/lib/arm64");
                target.PrivateLibraryPaths.Add(@"$WindowsSdkLocation$\Lib\$WindowsSdkVersion$\um\arm64");
                target.PrivateLibraryPaths.Add(@"$WindowsSdkLocation$\Lib\$WindowsSdkVersion$\ucrt\arm64");
            }
            else
            {
                throw new ConfigurationFailedException($@"Platform {this.Name} does not support architecture {context.Architecture}");
            }
        }

        public override void PostConfigureTarget(Target target, IContext context)
        {
        }
    }
}
