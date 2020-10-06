using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Graphyte.Build.Profiles
{
#if false
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ProfileSectionNameAttribute : System.Attribute
    {
        public string Name { get; set; }
    }
#endif

    public abstract class BaseProfileSection
    {
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }

    public sealed class Profile
    {
        public IDictionary<Type, BaseProfileSection> Sections { get; }

        private Profile(IDictionary<Type, BaseProfileSection> sections)
        {
            this.Sections = sections;
        }

        public Profile()
        {
            this.Sections = new Dictionary<Type, BaseProfileSection>();
        }

        private static IReadOnlyDictionary<string, Type> DiscoverProfileSections()
        {
            var result = new Dictionary<string, Type>();
            var sectionType = typeof(BaseProfileSection);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(sectionType) && x.IsClass && !x.IsAbstract);

            foreach (var type in types)
            {
                result.Add(type.Name, type);
            }

            return result;
        }

        private static readonly JsonReaderOptions g_ReaderOptions = new JsonReaderOptions()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        private static readonly JsonWriterOptions g_WriterOptions = new JsonWriterOptions()
        {
            Indented = true,
        };

        private static Span<byte> ReadContent(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }
            else
            {
                using var newStream = new MemoryStream();
                stream.CopyTo(newStream);
                return newStream.ToArray();
            }
        }

        public static Profile Parse(Stream stream)
        {
            var content = ReadContent(stream);
            return Profile.Parse(content);
        }

        public static Profile Parse(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content).AsSpan<byte>();
            return Profile.Parse(bytes);
        }

        public static Profile Parse(ReadOnlySpan<byte> content)
        {
            var reader = new Utf8JsonReader(content, g_ReaderOptions);
            var types = Profile.DiscoverProfileSections();

#if DEBUG
            Debug.WriteLine("Discovering profile sections...");
            foreach (var type in types)
            {
                Debug.WriteLine($@"  Section ""{type.Value.FullName}""");
            }
            Debug.WriteLine("Done.");
#endif

            var sections = new Dictionary<Type, BaseProfileSection>();

            if (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            var name = reader.GetString();
                            var type = types[name];
                            var value = (BaseProfileSection)JsonSerializer.Deserialize(ref reader, type);

#if TRACE
                            if (value.Properties != null)
                            {
                                foreach (var property in value.Properties)
                                {
                                    Trace.WriteLine($@"Warning: ""{type.Name}"": unknown property ""{property.Key}""");
                                }
                            }
#endif

                            sections.Add(type, value);
                        }
                        else if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            // This is ok
                        }
                        else
                        {
                            throw new JsonException();
                        }
                    }
                }
                else
                {
                    throw new JsonException();
                }
            }

            return new Profile(sections);
        }

        public void Serialize(Stream stream)
        {
            using var writer = new Utf8JsonWriter(stream, g_WriterOptions);
            writer.WriteStartObject();

            foreach (var section in this.Sections)
            {
                writer.WritePropertyName(section.Key.Name);

                JsonSerializer.Serialize(writer, section.Value, section.Value.GetType());
            }

            writer.WriteEndObject();
        }

        public T GetSection<T>()
            where T : BaseProfileSection
        {
            var sectionType = typeof(T);

            foreach (var section in this.Sections)
            {
                if (section.Key.IsSubclassOf(sectionType))
                {
                    return (T)section.Value;
                }
            }

            return null;
        }

        public IEnumerable<T> GetSections<T>()
            where T : BaseProfileSection
        {
            var sectionType = typeof(T);

            foreach (var section in this.Sections)
            {
                if (section.Key.IsSubclassOf(sectionType))
                {
                    yield return (T)section.Value;
                }
            }
        }
    }
}
