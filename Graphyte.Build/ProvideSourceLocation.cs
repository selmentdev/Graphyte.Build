using System;
using System.Runtime.CompilerServices;

namespace Graphyte.Build
{
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = false)]
    public sealed class ProvideSourceLocation : Attribute
    {
        public readonly string File;
        public readonly int Line;

        public ProvideSourceLocation(
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0)
        {
            this.File = file;
            this.Line = line;
        }

        public override string ToString()
        {
            return $@"{this.File}({this.Line})";
        }
    }
}
