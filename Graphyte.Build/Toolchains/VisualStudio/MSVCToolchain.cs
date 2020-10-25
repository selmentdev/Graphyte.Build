using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Graphyte.Build.Toolchains.VisualStudio
{
    public sealed class MSVCToolchain
        : BaseToolchain
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private MSVCToolchainSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<MSVCToolchainSettings>();
        }
    }
}
