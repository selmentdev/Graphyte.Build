using Neobyte.Build.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Neobyte.Build.Platforms.Android
{
    public sealed class AndroidSdkProvider
    {
        private readonly struct AbiMapping
        {
            public readonly TargetArchitecture Architecture;
            public readonly string Toolchain;
            public readonly string Platform;
            public readonly string LibPath;
            public readonly int Bitness;

            public AbiMapping(TargetArchitecture architecture, string toolchain, string platform, string libPath, int bitness) : this()
            {
                this.Architecture = architecture;
                this.Toolchain = toolchain;
                this.Platform = platform;
                this.LibPath = libPath;
                this.Bitness = bitness;
            }
        }

        public DirectoryInfo? SdkPath { get; private set; }
        public DirectoryInfo? NdkPath { get; private set; }

        public int MinApiLevel { get; private set; }
        public int MaxApiLevel { get; private set; }

        public IReadOnlyDictionary<string, int>? SystemLibraries { get; private set; }

        private static DirectoryInfo? GetPathFromEnvVar(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);

            if (value != null)
            {
                return new DirectoryInfo(value);
            }

            return null;
        }

        public AndroidSdkProvider()
        {
            this.SdkPath = GetPathFromEnvVar("ANDROID_SDK");
            this.NdkPath = GetPathFromEnvVar("ANDROID_NDK");

            if (this.SdkPath != null && this.SdkPath.Exists &&
                this.NdkPath != null && this.NdkPath.Exists)
            {

                Trace.WriteLine($@"NDK Path: {this.NdkPath}");

                (this.MinApiLevel, this.MaxApiLevel) = GetMinMaxApiVersion(this.NdkPath);
                Trace.WriteLine($@"MinApiLevel = {this.MinApiLevel}, MaxApiLevel = {this.MaxApiLevel}");

                this.SystemLibraries = GetSystemLibs(this.NdkPath);

                foreach (var item in this.SystemLibraries)
                {
                    Trace.WriteLine($@"system lib: ""{item.Key}"", version: ""{item.Value}""");
                }
            }
        }

        private static readonly JsonDocumentOptions g_ReaderOptions = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        private static (int MinVersion, int MaxVersion) GetMinMaxApiVersion(DirectoryInfo ndkPath)
        {
            var content = File.ReadAllBytes(Path.Combine(ndkPath.FullName, "meta", "platforms.json"));
            using var document = JsonDocument.Parse(content, g_ReaderOptions);

            var properties = document.RootElement.EnumerateObject().ToArray();
            var min_prop = properties.FirstOrDefault(x => x.Name == "min");
            var max_prop = properties.FirstOrDefault(x => x.Name == "max");

            return (MinVersion: min_prop.Value.GetInt32(), MaxVersion: max_prop.Value.GetInt32());
        }

        private static IReadOnlyDictionary<string, int> GetSystemLibs(DirectoryInfo ndkPath)
        {
            var content = File.ReadAllBytes(Path.Combine(ndkPath.FullName, "meta", "system_libs.json"));
            using var document = JsonDocument.Parse(content, g_ReaderOptions);

            return document.RootElement.EnumerateObject()
                .ToImmutableDictionary(
                    x => x.Name.ToString(),
                    x => Convert.ToInt32(x.Value.ToString()));
        }
    }
}
