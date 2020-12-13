using Graphyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
