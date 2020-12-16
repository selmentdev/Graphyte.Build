using Neobyte.Build.Framework;
using System;

namespace Neobyte.Build.Generators
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

        public abstract GeneratorBase Create(Profile profile);

        public override string ToString()
        {
            return $@"{this.GeneratorType}-{this.Version}";
        }
    }
}
