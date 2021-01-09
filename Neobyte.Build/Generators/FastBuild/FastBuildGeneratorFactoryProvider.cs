using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Neobyte.Build.Generators.FastBuild
{
    static class FastBuildHostSupport
    {
        public static bool IsSupported
        {
            get
            {
                if (OperatingSystem.IsWindows())
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case Architecture.X64:
                        case Architecture.X86:
                            return true;
                        default:
                            break;
                    }
                }
                else if (OperatingSystem.IsLinux())
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case Architecture.X64:
                        case Architecture.X86:
                            return true;
                        default:
                            break;
                    }
                }
                else if (OperatingSystem.IsMacOS())
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case Architecture.X64:
                        case Architecture.Arm64:
                            return true;
                        default:
                            break;
                    }
                }

                return false;
            }
        }
    }

    [GeneratorFactoryProvider]
    public sealed class FastBuildGeneratorFactoryProvider
        : GeneratorFactoryProvider
    {
        public override IEnumerable<GeneratorFactory> Provide()
        {
            if (FastBuildHostSupport.IsSupported)
            {
                yield return new FastBuildGeneratorFactory();
            }
        }
    }
}
