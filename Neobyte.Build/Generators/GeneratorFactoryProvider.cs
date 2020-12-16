using System.Collections.Generic;

namespace Neobyte.Build.Generators
{
    public abstract class GeneratorFactoryProvider
    {
        public abstract IEnumerable<GeneratorFactory> Provide();
    }
}
