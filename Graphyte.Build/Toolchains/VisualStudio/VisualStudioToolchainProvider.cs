using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Graphyte.Build.Toolchains.VisualStudio
{
    /// <summary>
    /// Represents Visual Studio installaction information.
    /// </summary>
    public readonly struct VisualStudioLocation
    {
        /// <summary>
        /// Gets installaction location.
        /// </summary>
        public readonly string Location;

        /// <summary>
        /// Gets name of VS installation.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets version of VS installation.
        /// </summary>
        public readonly string Version;

        /// <summary>
        /// Gets target toolkit of VS installation.
        /// </summary>
        public readonly string Toolkit;

        /// <summary>
        /// Gets toolset version of VS installation.
        /// </summary>
        public readonly string Toolset;

        /// <summary>
        /// Creates new instance of VisualStudioLocation.
        /// </summary>
        /// <param name="location">An location instance.</param>
        /// <param name="name">A name of instance.</param>
        /// <param name="version">A version of instance.</param>
        /// <param name="toolkit">A toolkit of instance.</param>
        /// <param name="toolset">A toolset of instance.</param>
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

    public sealed class VisualStudioToolchainProvider
    {
        /// <summary>
        /// Gets collection of Visual Studio locations.
        /// </summary>
        public VisualStudioLocation[] Instances { get; }

        /// <summary>
        /// Creates new instance of VisualStudioToolchainProvider.
        /// </summary>
        public VisualStudioToolchainProvider()
        {
            this.Instances = VisualStudioToolchainProvider.Discover();
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

            return new VisualStudioLocation[0];
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
