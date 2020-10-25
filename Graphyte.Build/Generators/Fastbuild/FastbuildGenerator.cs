using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Graphyte.Build.Generators.Fastbuild
{
    public sealed class FastbuildGenerator : BaseGenerator
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

        private FastbuildGeneratorSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<FastbuildGeneratorSettings>();
        }
    }
}
