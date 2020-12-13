using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Graphyte.Build.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ProfileSectionAttribute : Attribute
    {
    }
}

namespace Graphyte.Build.Framework
{
    public sealed class Profile
    {
        private static bool IsProfileSection(Type type)
        {
            return type.IsClass
                && !type.IsAbstract
                && type.IsSealed
                && type.IsDefined(typeof(ProfileSectionAttribute));
        }

        private static IEnumerable<Type> DiscoverSections()
        {
            var types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(Core.TypesProviderAttribute)))
                .SelectMany(x => x.GetTypes())
                .Where(IsProfileSection);
            return types;
        }

        private static readonly JsonReaderOptions g_ReaderOptions = new JsonReaderOptions()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        public Profile(ReadOnlySpan<byte> contents)
        {
            this.Sections = Parse(contents);
        }

        public Profile(string contents)
            : this(Encoding.UTF8.GetBytes(contents))
        {
        }

        public IReadOnlyList<object> Sections { get; }

        private static List<object> Parse(ReadOnlySpan<byte> contents)
        {
            var reader = new Utf8JsonReader(contents, Profile.g_ReaderOptions);
            var types = Profile.DiscoverSections();

            var result = new List<object>();

            if (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            var name = reader.GetString();
                            var type = types.FirstOrDefault(x => x.Name == name);

                            if (type == null)
                            {
                                throw new Exception($@"Cannot parse {name} profile section");
                            }

                            var section = JsonSerializer.Deserialize(ref reader, type);

                            result.Add(section);
                        }
                        else if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            // skip
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

        public T GetSection<T>()
        {
            var type = typeof(T);

            if (!IsProfileSection(type))
            {
                throw new Exception($@"Type ""{type.FullName}"" must be marked as profile section");
            }

            return this.Sections.OfType<T>().SingleOrDefault();
        }

        public IEnumerable<T> GetSections<T>()
        {
            return this.Sections.OfType<T>();
        }
    }
}
