using System;

namespace Neobyte.Build.Core
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class TypesProviderAttribute
        : Attribute
    {
    }
}
