using System;

namespace Neobyte.Build.Framework
{
    public enum TargetConfiguration
    {
        Debug,
        DebugGame,
        Development,
        Testing,
        Release,
    }

    public static partial class TargetExtensions
    {
        public static TargetConfiguration[] Configurations => Enum.GetValues<TargetConfiguration>();
    }
}
