using Graphyte.Build.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build
{
    public abstract class BaseGeneratorFactory
    {
        protected BaseGeneratorFactory(
            GeneratorType generatorType,
            Version version)
        {
            this.GeneratorType = generatorType;
            this.Version = version;
        }

        public GeneratorType GeneratorType { get; }
        public Version Version { get; }

        public abstract BaseGenerator Create(Profile profile);

        public override string ToString()
        {
            return $@"{this.GeneratorType}-{this.Version}";
        }
    }
}

namespace Graphyte.Build
{
    public interface IGeneratorsProvider
    {
        IEnumerable<BaseGeneratorFactory> Provide();
    }
}

namespace Graphyte.Build
{
    public sealed class GeneratorsProvider
    {
        public BaseGeneratorFactory[] Generators { get; }

        public GeneratorsProvider()
        {
            var providers = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(this.ValidType)
                .Select(x => Activator.CreateInstance(x))
                .Cast<IGeneratorsProvider>();

            this.Generators = providers
                .SelectMany(x => x.Provide())
                .ToArray();
        }

        private bool ValidType(Type type)
        {
            return type.IsClass
                && !type.IsAbstract
                && type.IsSealed
                && typeof(IGeneratorsProvider).IsAssignableFrom(type);
        }
    }
}
