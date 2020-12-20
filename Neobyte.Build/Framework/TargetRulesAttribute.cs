using System;
using System.Runtime.CompilerServices;

namespace Neobyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TargetRulesAttribute
        : Attribute
    {
        public readonly string Location;

        public TargetRulesAttribute([CallerFilePath] string location = "")
        {
            this.Location = location;
        }

        public override string ToString()
        {
            return this.Location;
        }
    }
}
