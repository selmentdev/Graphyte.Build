using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Graphyte.Build.Platforms.Windows
{
    public static class WindowsSdkProvider
    {
        private static readonly string[] CPlusPlusSdkOptions = new[]
        {
            "OptionId.DesktopCPPx64",
            "OptionId.DesktopCPParm64",
            "OptionId.SigningTools",
            "OptionId.UWPCPP",
        };

        public static readonly string[] Versions;
        public static readonly string Location;
        public static readonly bool IsSupported;

        static WindowsSdkProvider()
        {
            WindowsSdkProvider.IsSupported = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (WindowsSdkProvider.IsSupported)
            {
                var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                var roots = root.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows Kits\Installed Roots");

                var kitsroot10name = roots.GetValueNames().Where(x => x.StartsWith(@"KitsRoot10")).First();

                var sdkNames = roots.GetSubKeyNames().Where(x => x.StartsWith("10."));
                var sdksFound = new List<string>();

                foreach (var sdkName in sdkNames)
                {
                    var sdk = roots.OpenSubKey(sdkName);
                    var installedOptions = sdk.OpenSubKey("Installed Options");
                    var options = installedOptions.GetValueNames();

                    var all = WindowsSdkProvider.CPlusPlusSdkOptions.All(x => options.Contains(x));

                    if (all)
                    {
                        sdksFound.Add(sdkName);
                    }
                }

                WindowsSdkProvider.Location = (string)roots.GetValue(kitsroot10name);
                WindowsSdkProvider.Versions = sdksFound.ToArray();
            }
            else
            {
                WindowsSdkProvider.Location = string.Empty;
                WindowsSdkProvider.Versions = new string[0];
            }
        }
    }
}
