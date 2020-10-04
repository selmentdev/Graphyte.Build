using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Graphyte.Build
{
    [Serializable]
    public sealed class Profile
    {
        public List<TargetTuple> Targets { get; set; }
        public bool EnableAddressSanitizer { get; set; }
        public bool EnableThreadSanitizer { get; set; }
        public bool EnableUndefinedBehaviorSanitizer { get; set; }
    }

    public static class ProfileSerializer
    {
        private static readonly JsonSerializerOptions m_SerializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            WriteIndented = true
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
