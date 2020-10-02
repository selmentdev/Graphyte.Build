using Graphyte.Build.Resolving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Graphyte.Build.Dump
{
    public static class DumpResolvedSolution
    {
        public static void SaveToFile(string path, ResolvedSolution solution)
        {
            using var stream = File.Create(path);
            using var writer = new System.Text.Json.Utf8JsonWriter(stream, new JsonWriterOptions()
            {
                Indented = true
            });

            writer.WriteStartObject();

            foreach (var target in solution.Targets)
            {
                writer.WriteStartObject(target.Name);
                SerializeTarget(writer, target);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        private static void SerializeTarget(Utf8JsonWriter writer, ResolvedTarget target)
        {
            writer.WriteString("TargetType", target.SourceTarget.Type.ToString());

            SerializeList(writer, nameof(target.PublicIncludePaths), target.PublicIncludePaths);
            SerializeList(writer, nameof(target.PrivateIncludePaths), target.PrivateIncludePaths);

            SerializeList(writer, nameof(target.PublicLibraryPaths), target.PublicLibraryPaths);
            SerializeList(writer, nameof(target.PrivateLibraryPaths), target.PrivateLibraryPaths);

            SerializeList(writer, nameof(target.PublicLibraries), target.PublicLibraries);
            SerializeList(writer, nameof(target.PrivateLibraries), target.PrivateLibraries);

            SerializeList(writer, nameof(target.PublicDependencies), target.PublicDependencies.Select(x => x.Name));
            SerializeList(writer, nameof(target.PrivateDependencies), target.PrivateDependencies.Select(x => x.Name));

            SerializeList(writer, nameof(target.PublicDefines), target.PublicDefines);
            SerializeList(writer, nameof(target.PrivateDefines), target.PrivateDefines);
        }

        private static void SerializeList(Utf8JsonWriter writer, string name, IEnumerable<string> items)
        {
            writer.WriteStartArray(name);

            foreach (var item in items)
            {
                writer.WriteStringValue(item);
            }

            writer.WriteEndArray();
        }

        private static void SerializeDictionary(Utf8JsonWriter writer, string name, IEnumerable<KeyValuePair<string, string>> items)
        {
            writer.WriteStartObject(name);

            foreach (var item in items)
            {
                writer.WriteString(item.Key, item.Value);
            }

            writer.WriteEndObject();
        }
    }
}
