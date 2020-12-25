using System;

namespace Neobyte.Build.Framework
{
    public enum TargetFlavor
    {
        Game,
        Editor,
        Client,
        Server,
    }

    public static partial class TargetExtensions
    {
        public static TargetFlavor[] Flavors => Enum.GetValues<TargetFlavor>();
    }
}
