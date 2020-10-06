using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Graphyte.Build.Platforms.Windows
{
    public readonly struct VisualStudioLocation
    {
        public readonly string Location;
        public readonly string Name;
        public readonly string Version;
        public readonly string Toolkit;
        public readonly string Toolset;

        public VisualStudioLocation(
            string location,
            string name,
            string version,
            string toolkit,
            string toolset)
        {
            this.Location = location;
            this.Name = name;
            this.Version = version;
            this.Toolkit = toolkit;
            this.Toolset = toolset;
        }
    }

    public sealed class VisualStudioProvider
    {
        public VisualStudioLocation[] Instances { get; }

        public VisualStudioProvider()
        {
            this.Instances = this.DiscoverInstances();
        }

        private readonly struct VersionToolkitMapping
        {
            public readonly string Version;
            public readonly string Toolkit;

            public VersionToolkitMapping(string version, string toolkit)
            {
                this.Version = version;
                this.Toolkit = toolkit;
            }
        }

        private static readonly VersionToolkitMapping[] g_VersionToolkitMapping = new VersionToolkitMapping[]
        {
            new VersionToolkitMapping("2017", "v141"),
            new VersionToolkitMapping("2019", "v142"),
        };

        private VisualStudioLocation[] DiscoverInstances()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var msvcPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

                if (msvcPath != null)
                {
                    var vswherePath = Path.Combine(
                            msvcPath,
                            "Microsoft Visual Studio",
                            "Installer",
                            "vswhere.exe");

                    var vswhere = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = vswherePath,
                            Arguments = "-utf8 -latest -products * -format json",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                        },
                    };

                    if (vswhere.Start())
                    {
                        var content = vswhere.StandardOutput.ReadToEnd();
                        var document = JsonDocument.Parse(content);

                        return document.RootElement.EnumerateArray().Select(ParseVisualStudioLocation).ToArray();
                    }
                }
            }

            return new VisualStudioLocation[0];
        }

        private static VisualStudioLocation ParseVisualStudioLocation(JsonElement instance)
        {
            var properties = instance.EnumerateObject()
                .ToDictionary(x => x.Name, x => x.Value);

            var name = properties["displayName"].ToString();
            var path = properties["installationPath"].ToString();
            var version = properties["catalog"].EnumerateObject()
                .First(x => x.Name == "productLineVersion").Value.ToString();

            var toolkit = g_VersionToolkitMapping.First(x => x.Version == version).Toolkit;

            var toolset = GetToolsVersion(path, toolkit);

            return new VisualStudioLocation(
                location: path,
                name: name,
                version: version,
                toolkit: toolkit,
                toolset: toolset);
        }

        private static string GetToolsVersion(string path, string toolkit)
        {
            var prefixPath = Path.Combine(path, "VC", "Auxiliary", "Build");
            var defaultPath = Path.Combine(prefixPath, "Microsoft.VCToolsVersion.default.txt");
            var versionPath = Path.Combine(prefixPath, $"Microsoft.VCToolsVersion.{toolkit}.default.txt");

            var selectedPath = File.Exists(versionPath) ? versionPath : defaultPath;

            var content = File.ReadAllLines(selectedPath);
            return content[0];
        }
    }
}
