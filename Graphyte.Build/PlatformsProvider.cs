using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build
{
    public abstract class BasePlatformFactory
    {
        protected BasePlatformFactory(
            PlatformType platformType,
            ArchitectureType architectureType,
            ToolchainType toolchainType)
        {
            this.PlatformType = platformType;
            this.ArchitectureType = architectureType;
            this.ToolchainType = toolchainType;
        }

        public ArchitectureType ArchitectureType { get; }
        public PlatformType PlatformType { get; }
        public ToolchainType ToolchainType { get; }

        public abstract BasePlatform CreatePlatform(Profile profile);
        public abstract BaseToolchain CreateToolchain(Profile profile);

        public override string ToString()
        {
            return $@"{this.PlatformType}-{this.ArchitectureType}-{this.ToolchainType}";
        }
    }

    public interface IPlatformProvider
    {
        IEnumerable<BasePlatformFactory> Provide();
    }
}

namespace Graphyte.Build
{
    public sealed class PlatformsProvider
    {
        public BasePlatformFactory[] Platforms { get; }

        public PlatformsProvider()
        {
            var providers = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(this.ValidType)
                .Select(x => Activator.CreateInstance(x))
                .Cast<IPlatformProvider>();

            this.Platforms = providers
                .SelectMany(x => x.Provide())
                .ToArray();
        }

        private bool ValidType(Type type)
        {
            return typeof(IPlatformProvider).IsAssignableFrom(type)
                && type.IsClass
                && !type.IsAbstract
                && type.IsSealed
                ;
        }
    }
}
