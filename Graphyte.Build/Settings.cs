using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Graphyte.Build
{
    public interface ISettingsSection
    {
    }

    public abstract class PlatformSettings : ISettingsSection
    {
    }

    public abstract class GeneratorSettings : ISettingsSection
    {
    }

    public abstract class ToolchainSettings : ISettingsSection
    {
    }

    public sealed class Settings
    {
        internal readonly Dictionary<Type, ISettingsSection> m_Sections;

        private static IEnumerable<Type> DiscoverSettingsSections()
        {
            var baseType = typeof(ISettingsSection);
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes()).ToArray();
            var result = allTypes
                .Where(x => baseType.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
            return result;
        }


        private Settings(Dictionary<Type, ISettingsSection> sections)
        {
            this.m_Sections = sections;
        }

        private static readonly JsonReaderOptions g_DefaultOptions = new JsonReaderOptions()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        private static readonly JsonSerializerOptions g_Options = new JsonSerializerOptions()
        { 
        };


        public static Settings Parse(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var span = bytes.AsSpan<byte>();
            return Settings.Parse(span);
        }

        public static Settings Parse(ReadOnlySpan<byte> buffer)
        {
            var reader = new Utf8JsonReader(buffer, g_DefaultOptions);
            var types = Settings.DiscoverSettingsSections();
            var sections = new Dictionary<Type, ISettingsSection>();

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
                            var value = JsonSerializer.Deserialize(ref reader, type);
                            sections.Add(type, (ISettingsSection)value);
                        }
                        else if (reader.TokenType == JsonTokenType.EndObject)
                        {
                        }
                        else
                        {
                            throw new JsonException($@"{reader.TokenType}");
                        }
                    }
                }
            }

            return new Settings(sections);
        }

        public T Get<T>()
            where T : ISettingsSection
        {
            if (this.m_Sections.TryGetValue(typeof(T), out var section))
            {
                return (T)section;
            }

            throw new Exception();
        }

        public IEnumerable<T> GetAll<T>()
            where T : ISettingsSection
        {
            var type = typeof(T);

            foreach (var kv in this.m_Sections)
            {
                if (kv.Key.IsSubclassOf(type))
                {
                    yield return (T)kv.Value;
                }
            }
        }
    }
}
