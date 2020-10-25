using System.Runtime.InteropServices;

namespace Graphyte.Build.Toolchains.VisualStudio
{
    public sealed class MSVCToolchain
        : BaseToolchain
    {
        public override bool IsHostSupported
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public override ToolchainType Type => ToolchainType.MSVC;

        private MSVCToolchainSettings m_Settings;

        public override void Initialize(Profile profile)
        {
            this.m_Settings = profile.GetSection<MSVCToolchainSettings>();
        }

        public override void PreConfigureTarget(Target target)
        {
        }

        public override void PostConfigureTarget(Target target)
        {
        }
    }
}
