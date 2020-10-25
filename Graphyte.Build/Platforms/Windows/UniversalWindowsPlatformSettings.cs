using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class UniversalWindowsPlatformSettings
        : BasePlatformSettings
    {
        public string WindowsSdkVersion { get; set; }
    }
}
