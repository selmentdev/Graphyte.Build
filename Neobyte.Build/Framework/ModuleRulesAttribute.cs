using System;
using System.Runtime.CompilerServices;

namespace Neobyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleRulesAttribute
        : Attribute
    {
        public readonly string Location;

        public ModuleRulesAttribute([CallerFilePath] string location = "")
        {
            this.Location = location;
        }

        public override string ToString()
        {
            return this.Location;
        }
    }
}
