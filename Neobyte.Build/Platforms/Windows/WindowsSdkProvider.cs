using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace Neobyte.Build.Platforms.Windows
{
    public static class WindowsSdkProvider
    {
        private static readonly string[] CPlusPlusSdkOptions = new[]
        {
            "OptionId.DesktopCPPx64",
            "OptionId.DesktopCPParm64",
            "OptionId.SigningTools",
            "OptionId.UWPCPP",
            "OptionId.SigningTools",
        };

        public static readonly string[] Versions = Array.Empty<string>();
        public static readonly string Location = string.Empty;
        public static readonly bool IsSupported = false;

        static WindowsSdkProvider()
        {
            if (OperatingSystem.IsWindows())
            {
                using var rootKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                if (rootKey != null)
                {
                    using var installedRootsKey = rootKey
                        .OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows Kits\Installed Roots");

                    if (installedRootsKey != null)
                    {
                        var windowsRootKeyName10 = installedRootsKey
                            .GetValueNames()
                            .Where(x => x.StartsWith(@"KitsRoot10"))
                            .Single();

                        var location = (string?)installedRootsKey.GetValue(windowsRootKeyName10);

                        if (location != null)
                        {
                            Location = location;
                        }

                        var sdkNames = installedRootsKey
                            .GetSubKeyNames()
                            .Where(x => x.StartsWith("10."));

                        var sdksFound = GetMatchingVersions(installedRootsKey, sdkNames);
                        Versions = sdksFound.ToArray();
                    }
                }
            }
        }

        [SupportedOSPlatform("windows")]
        private static IEnumerable<string> GetMatchingVersions(RegistryKey rootKey, IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                using var sdkKey = rootKey.OpenSubKey(name);

                if (sdkKey != null)
                {
                    using var optionsKey = sdkKey.OpenSubKey("Installed Options");

                    if (optionsKey != null)
                    {
                        var options = optionsKey.GetValueNames();

                        var matches = CPlusPlusSdkOptions.All(x => options.Contains(x));

                        if (matches)
                        {
                            yield return name;
                        }
                    }
                }
            }
        }
    }
}
