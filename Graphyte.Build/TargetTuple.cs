using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public readonly struct TargetTuple
    {
        public readonly PlatformKind Kind;
        public readonly PlatformType Platform;
        public readonly ArchitectureType Architecture;

        public TargetTuple(PlatformType platform, ArchitectureType architecture)
        {
            this.Kind = PlatformKind.None;
            this.Platform = platform;
            this.Architecture = architecture;
        }
    }
}
