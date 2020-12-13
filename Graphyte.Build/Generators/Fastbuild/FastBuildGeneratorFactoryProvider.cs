using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Generators.FastBuild
{
    static class FastBuildHostSupport
    {
        public static bool IsSupported
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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
