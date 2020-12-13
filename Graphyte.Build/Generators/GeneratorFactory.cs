using Graphyte.Build.Framework;
using System;

namespace Graphyte.Build.Generators
{
    public abstract class GeneratorFactory
    {
        protected GeneratorFactory(GeneratorType generatorType, Version version)
        {
            this.GeneratorType = generatorType;
            this.Version = version;
        }

        public GeneratorType GeneratorType { get; }

        public Version Version { get; }

        public abstract Generator Create(Profile profile);

        public override string ToString()
        {
            return $@"{this.GeneratorType}-{this.Version}";
        }
    }
}
