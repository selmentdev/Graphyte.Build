using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Generators.FastBuild
{
    [GeneratorFactoryProvider]
    public sealed class FastBuildGeneratorFactoryProvider : GeneratorFactoryProvider
    {
        private static bool IsSupported
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

        public override IEnumerable<GeneratorFactory> Provide()
        {
            if (IsSupported)
            {
                yield return new FastBuildGeneratorFactory();
            }
        }
    }
}
