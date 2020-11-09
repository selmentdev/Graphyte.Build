using Graphyte.Build.Platforms;
using System.Collections;
using System.Collections.Generic;

namespace Graphyte.Build.Tests.Mocks
{
    sealed class MockPlatform
        : BasePlatform
    {
        public MockPlatform(
            Profile profile,
            ArchitectureType architectureType)
            : base(
                  profile,
                  architectureType)
        {
        }

        public override PlatformType PlatformType => PlatformType.Create("Mock");

        public override bool IsPlatformKind(PlatformKind platformKind)
        {
            return true;
        }

        public override string AdjustTargetName(string name, TargetType targetType)
        {
            return name;
        }
    }

    sealed class MockPlatformFactory
        : BasePlatformFactory
    {
        public static ToolchainType MockToolchain = ToolchainType.Create("Mock");
        public static PlatformType MockPlatform = PlatformType.Create("Mock");

        public MockPlatformFactory()
            : base(MockPlatform, ArchitectureType.X64, MockToolchain)
        {
        }

        public override BasePlatform CreatePlatform(Profile profile)
        {
            return new MockPlatform(profile, this.ArchitectureType);
        }

        public override BaseToolchain CreateToolchain(Profile profile)
        {
            return new MockToolchain(profile, this.ArchitectureType);
        }
    }

    sealed class MockPlatformProvider
        : IPlatformsProvider
    {
        public IEnumerable<BasePlatformFactory> Provide()
        {
            yield return new MockPlatformFactory();
        }
    }
}
