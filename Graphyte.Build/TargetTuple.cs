using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public struct TargetTuple
    {
        public PlatformType Platform;
        public ArchitectureType Architecture;

        public TargetTuple(PlatformType platform, ArchitectureType architecture)
        {
            this.Platform = platform;
            this.Architecture = architecture;
        }
    }
}
