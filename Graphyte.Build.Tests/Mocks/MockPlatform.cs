using Graphyte.Build.Platforms;

namespace Graphyte.Build.Tests.Mocks
{
    public sealed class MockPlatform
        : BasePlatform
    {
        public override bool IsHostSupported => true;

        private static readonly ArchitectureType[] g_SupportedArchitectures = new[]
        {
            ArchitectureType.X64,
        };

        public override ArchitectureType[] Architectures => g_SupportedArchitectures;

        public override PlatformType Type => PlatformType.Create("Mock");

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            return name;
        }

        public override void Initialize(Profile profile)
        {
        }

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            return true;
        }

        public override void PostConfigureTarget(Target target)
        {
        }

        public override void PreConfigureTarget(Target traget)
        {
        }
    }
}
