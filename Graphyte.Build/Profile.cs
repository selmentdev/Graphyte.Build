using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Graphyte.Build
{
    public abstract class BaseProfileSection
    {
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }

    public sealed class Profile
    {
        #region Types discovery
        /// <summary>
        /// Discovers available profile sections.
        /// </summary>
        /// <returns>The sequence of profile section types.</returns>
        private static IEnumerable<Type> DiscoverProfileSections()
        {
            // Get all types implementing profile section.
            //
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(typeof(BaseProfileSection)) && x.IsClass && !x.IsAbstract);

            foreach (var type in types)
            {
                if (!type.IsSealed)
                {
                    throw new InvalidOperationException($@"Type ""{type.FullName}"" must be either abstract or sealed");
                }
                yield return type;
            }
        }
        #endregion

        #region Parsing
        private static readonly JsonReaderOptions g_JsonReaderOptions = new JsonReaderOptions()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        private static Span<byte> ReadContents(Stream stream)
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

        public static Profile Parse(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content).AsSpan();
            return Profile.Parse(bytes);
        }

        public static Profile Parse(Stream stream)
        {
            var bytes = ReadContents(stream);
            return Profile.Parse(bytes);
        }

        public static Profile Parse(ReadOnlySpan<byte> content)
        {
            return new Profile(content);
        }

        private static List<BaseProfileSection> ParseSections(ReadOnlySpan<byte> content)
        {
            var reader = new Utf8JsonReader(content, g_JsonReaderOptions);
            var types = Profile.DiscoverProfileSections();

            var result = new List<BaseProfileSection>();

            if (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            var name = reader.GetString();
                            var type = types.First(x => x.Name == name);

                            var section = (BaseProfileSection)JsonSerializer.Deserialize(ref reader, type);

#if TRACE
                            if (section.Properties != null)
                            {
                                foreach (var property in section.Properties)
                                {
                                    Trace.WriteLine($@"Warning: ""{type.Name}"" - unknown property ""{property.Key}""");
                                }
                            }
#endif

                            result.Add(section);
                        }
                        else if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            // Do nothing
                        }
                        else
                        {
                            throw new JsonException($@"Expected property name or end object token");
                        }
                    }
                }
                else
                {
                    throw new JsonException($@"Expected start object token");
                }
            }

            return result;
        }

        private Profile(ReadOnlySpan<byte> content)
        {
            this.Sections = Profile.ParseSections(content);
        }
        #endregion

        #region Fields
        public IReadOnlyList<BaseProfileSection> Sections { get; }
        #endregion

        #region Methods
        public T GetSection<T>()
        {
            var type = typeof(T);
            if (type.IsAbstract)
            {
                throw new InvalidOperationException($@"Cannot get section of abstract type""{type.FullName}""");
            }

            return this.Sections.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetSections<T>()
        {
            return this.Sections.OfType<T>();
        }
        #endregion
    }
}
