using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Graphyte.Build
{
    [Serializable]
    public sealed class Profile
    {
        /// <summary>
        /// Prefix used when generating solution and project files.
        /// </summary>
        public string Prefix { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlatformType Platform { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ArchitectureType Architecture { get; set; }

        public bool EnableAddressSanitizer { get; set; }
        public bool EnableThreadSanitizer { get; set; }
        public bool EnableUndefinedBehaviorSanitizer { get; set; }
    }



    public static class ProfileSerializer
    {
        private static readonly JsonSerializerOptions m_SerializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            IgnoreNullValues = true,
        };

        public static Profile Deserialize(string content)
        {
            return (Profile)JsonSerializer.Deserialize(content, typeof(Profile), m_SerializationOptions);
        }

        public static string Serialize(Profile profile)
        {
            return JsonSerializer.Serialize(profile, typeof(Profile), m_SerializationOptions);
        }
    }
}
