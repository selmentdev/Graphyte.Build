using System;

namespace Graphyte.Build.Platforms
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PlatformFactoryProviderAttribute
        : Attribute
    {
    }
}
