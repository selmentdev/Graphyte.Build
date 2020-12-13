using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Graphyte.Build.Toolchains.VisualStudio
{
    public static class VisualStudioToolchainProvider
    {
        /// <summary>
        /// Gets collection of Visual Studio locations.
        /// </summary>
        public static VisualStudioLocation[] Instances { get; }

        /// <summary>
        /// Creates new instance of VisualStudioToolchainProvider.
        /// </summary>
        static VisualStudioToolchainProvider()
        {
            VisualStudioToolchainProvider.Instances = VisualStudioToolchainProvider.Discover();
        }

        /// <summary>
        /// Represents toolkit mapping.
        /// </summary>
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

        /// <summary>
        /// All known toolkit mappings.
        /// </summary>
        private static readonly VersionToolkitMapping[] g_VersionToolkitMappings = new[]
        {
            new VersionToolkitMapping("2017", "v141"),
            new VersionToolkitMapping("2019", "v142"),
        };

        /// <summary>
        /// Discovers all available Visual Studio locations.
        /// </summary>
        /// <returns></returns>
        private static VisualStudioLocation[] Discover()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programFiles = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

                if (programFiles != null)
                {
                    var vswhere = Path.Combine(
                        programFiles,
                        "Microsoft Visual Studio",
                        "Installer",
                        "vswhere.exe");

                    var process = Process.Start(new ProcessStartInfo()
                    {
                        FileName = vswhere,
                        Arguments = "-utf8 -latest -products * -format json",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                    });

                    var content = process.StandardOutput.ReadToEnd();
                    var document = JsonDocument.Parse(content);

                    return document.RootElement.EnumerateArray()
                        .Select(ParseVisualStudioLocation)
                        .ToArray();
                }
            }

            return Array.Empty<VisualStudioLocation>();
        }

        /// <summary>
        /// Parses Visual Studio instance information from JSON element.
        /// </summary>
        /// <param name="instance">An JSON representation of Visual Studio instance.</param>
        /// <returns>The parsed information.</returns>
        private static VisualStudioLocation ParseVisualStudioLocation(JsonElement instance)
        {
            var properties = instance.EnumerateObject()
                .ToDictionary(x => x.Name, x => x.Value);

            var name = properties["displayName"].ToString();
            var path = properties["installationPath"].ToString();
            var version = properties["catalog"].EnumerateObject()
                .First(x => x.Name == "productLineVersion").Value.ToString();

            var toolkit = g_VersionToolkitMappings.First(x => x.Version == version).Toolkit;

            var toolset = GetToolsVersion(path, toolkit);

            return new VisualStudioLocation(
                location: path,
                name: name,
                version: version,
                toolkit: toolkit,
                toolset: toolset);
        }

        /// <summary>
        /// Gets tools version for given Visual Studio installation and toolkit version.
        /// </summary>
        /// <param name="path">A path to Visual Studio installation.</param>
        /// <param name="toolkit">A toolkit version.</param>
        /// <returns>The version of Visual Studio toolset.</returns>
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
