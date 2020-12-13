using System.Collections.Generic;

namespace Graphyte.Build.Generators
{
    public abstract class GeneratorFactoryProvider
    {
        public abstract IEnumerable<GeneratorFactory> Provide();
    }
}
