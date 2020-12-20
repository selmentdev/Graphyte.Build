using System;

namespace Neobyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ProfileSectionAttribute
        : Attribute
    {
    }
}
