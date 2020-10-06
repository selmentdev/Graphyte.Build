using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public sealed class WindowsSdkProvider
    {
        private static readonly string[] CPlusPlusSdkOptions = new[]
        {
            "OptionId.DesktopCPPx64",
            "OptionId.DesktopCPParm64",
            "OptionId.SigningTools",
            "OptionId.UWPCPP",
        };

        public readonly WindowsTargetPlatformVersion[] Versions;
        public readonly string Location;
        public readonly bool IsSupported;

        public WindowsSdkProvider()
        {
            this.IsSupported = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (this.IsSupported)
            {
                var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                var roots = root.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows Kits\Installed Roots");

                var kitsroot10name = roots.GetValueNames().Where(x => x.StartsWith(@"KitsRoot10")).First();

                var sdkNames = roots.GetSubKeyNames().Where(x => x.StartsWith("10."));
                var sdksFound = new List<WindowsTargetPlatformVersion>();

                foreach (var sdkName in sdkNames)
                {
                    var sdk = roots.OpenSubKey(sdkName);
                    var installedOptions = sdk.OpenSubKey("Installed Options");
                    var options = installedOptions.GetValueNames();

                    var all = WindowsSdkProvider.CPlusPlusSdkOptions.All(x => options.Contains(x));

                    if (all)
                    {
                        sdksFound.Add(WindowsTargetPlatformVersion.Create(sdkName));
                    }
                }

                this.Location = (string)roots.GetValue(kitsroot10name);
                this.Versions = sdksFound.ToArray();
            }
            else
            {
                this.Location = string.Empty;
                this.Versions = new WindowsTargetPlatformVersion[0];
            }
        }
    }
}
