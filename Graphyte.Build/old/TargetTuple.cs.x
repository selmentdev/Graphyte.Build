using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Graphyte.Build
{
    [Serializable]
    [JsonConverter(typeof(TargetTupleJsonConverter))]
    public struct TargetTuple
    {
        public PlatformType Platform { get; set; }
        public ArchitectureType Architecture { get; set; }
        public ToolsetType Toolset { get; set; }

        public TargetTuple(PlatformType platform, ArchitectureType architecture, ToolsetType toolset)
        {
            this.Platform = platform;
            this.Architecture = architecture;
            this.Toolset = toolset;
        }

        public TargetTuple(string value)
        {
            var parts = value.Split('-');

            this.Platform = Enum.Parse<PlatformType>(parts[0]);
            this.Architecture = Enum.Parse<ArchitectureType>(parts[1]);
            this.Toolset = ToolsetType.Create(parts[2]);
        }

        public override string ToString()
        {
            return $@"{this.Platform}-{this.Architecture}-{this.Toolset}";
        }
    }

    public class TargetTupleJsonConverter : JsonConverter<TargetTuple>
    {
        public override TargetTuple Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new TargetTuple(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, TargetTuple value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
