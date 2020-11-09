using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Generators.Fastbuild
{
    sealed class FastbuildGeneratorFactory
        : BaseGeneratorFactory
    {
        public FastbuildGeneratorFactory()
            : base(GeneratorType.FastBuild, new Version(1, 0))
        {
        }

        public override BaseGenerator Create(Profile profile)
        {
            return new FastbuildGenerator(profile);
        }
    }
}

namespace Graphyte.Build.Generators.Fastbuild
{
    sealed class FastbuildGeneratorsProvider
        : IGeneratorsProvider
    {
        public IEnumerable<BaseGeneratorFactory> Provide()
        {
            if (this.IsHostSupported)
            {
                yield return new FastbuildGeneratorFactory(); 
            }
        }

        private bool IsHostSupported
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
                        case Architecture.Arm64:
                        case Architecture.Arm:
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
                        case Architecture.X86:
                        case Architecture.Arm64:
                        case Architecture.Arm:
                            return true;
                        default:
                            break;
                    }
                }

                return false;
            }
        }
    }
}
