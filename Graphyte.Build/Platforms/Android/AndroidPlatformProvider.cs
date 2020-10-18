using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Graphyte.Build.Platforms.Android
{
    public sealed class AndroidPlatformProvider
    {
        private readonly struct AbiMapping
        {
            public readonly Architecture Architecture;
            public readonly string Toolchain;
            public readonly string Platform;
            public readonly int Bitness;
            public readonly string LibDirectory;

            public AbiMapping(
                Architecture architecture,
                string toolchain,
                string platform,
                int bitness,
                string libDirectory)
            {
                this.Architecture = architecture;
                this.Toolchain = toolchain;
                this.Platform = platform;
                this.Bitness = bitness;
                this.LibDirectory = libDirectory;
            }
        }

        public string SdkPath { get; private set; }
        public string NdkPath { get; private set; } = @"E:\Downloads\android-ndk-r21d-windows-x86_64\android-ndk-r21d";

        public int MinApiLevel { get; private set; }
        public int MaxApiLevel { get; private set; }

        public IReadOnlyDictionary<string, int> SystemLibraries { get; private set; }

        public AndroidPlatformProvider()
        {
            this.SdkPath = Environment.GetEnvironmentVariable("ANDROID_SDK");
            this.NdkPath = Environment.GetEnvironmentVariable("ANDROID_NDK");

            if (this.SdkPath != null && Directory.Exists(this.SdkPath) &&
                this.NdkPath != null && Directory.Exists(this.NdkPath))
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

        private static (int min, int max) GetMinMaxApiVersion(string ndk_path)
        {
            var content = File.ReadAllBytes(Path.Combine(ndk_path, "meta", "platforms.json"));
            using var document = JsonDocument.Parse(content, new JsonDocumentOptions()
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip,
            });

            var properties = document.RootElement.EnumerateObject().ToArray();
            var min_prop = properties.FirstOrDefault(x => x.Name == "min");
            var max_prop = properties.FirstOrDefault(x => x.Name == "max");

            return (min: min_prop.Value.GetInt32(), max: max_prop.Value.GetInt32());
        }

        private static IReadOnlyDictionary<string, int> GetSystemLibs(string ndk_path)
        {
            var content = File.ReadAllBytes(Path.Combine(ndk_path, "meta", "system_libs.json"));
            using var document = JsonDocument.Parse(content, new JsonDocumentOptions()
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip,
            });

            return document.RootElement.EnumerateObject()
                .ToImmutableDictionary(
                    x => x.Name.ToString(),
                    x => Convert.ToInt32(x.Value.ToString()));
        }
    }
}
