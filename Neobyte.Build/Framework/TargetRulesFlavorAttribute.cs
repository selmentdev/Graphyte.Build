using System;

namespace Neobyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TargetRulesFlavorAttribute
        : Attribute
    {
        public readonly TargetFlavor Flavor;

        public TargetRulesFlavorAttribute(TargetFlavor flavor)
        {
            this.Flavor = flavor;
        }
    }
}
