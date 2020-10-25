using System.Runtime.InteropServices;

namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGenerator
        : BaseGenerator
    {
        public override bool IsHostSupported
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case System.Runtime.InteropServices.Architecture.X64:
                            return true;
                        default:
                            break;
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case System.Runtime.InteropServices.Architecture.X64:
                            return true;
                        default:
                            break;
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case System.Runtime.InteropServices.Architecture.Arm64:
                        case System.Runtime.InteropServices.Architecture.X64:
                            return true;
                        default:
                            break;
                    }
                }

                return false;
            }
        }

        public override GeneratorType Type => GeneratorType.FastBuild;

        public override void PreConfigureTarget(Target target)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
        }

        private FastbuildGeneratorSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<FastbuildGeneratorSettings>();
        }
    }
}
