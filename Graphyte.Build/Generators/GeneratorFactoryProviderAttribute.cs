using System;

namespace Graphyte.Build.Generators
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class GeneratorFactoryProviderAttribute : Attribute
    {
    }
}
