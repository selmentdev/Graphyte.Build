using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public class ConfigurationContext
    {
        public PlatformType Platform { get; }
        public ArchitectureType Architecture { get; }
        public BuildType Build { get; }
        public ConfigurationType Configuration { get; }

        public override string ToString()
        {
            return $@"{this.Platform} {this.Architecture} {this.Build} {this.Configuration}";
        }
    }
}
