using System;

namespace Neobyte.Build.Platforms
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PlatformFactoryProviderAttribute
        : Attribute
    {
    }
}
