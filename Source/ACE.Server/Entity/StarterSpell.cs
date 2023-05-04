using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ACE.Server.Entity
{
    public class StarterSpell
    {
        [JsonPropertyName("spellId")]
        public uint SpellId { get; set; }

        /// <summary>
        /// not used, but in the json file for readability
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("specializedOnly"), JsonConverter(typeof(StringToBoolConverter))]
        public bool SpecializedOnly { get; set; }
    }

    public class StringToBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var boolString = reader.GetString();

                return Convert.ToBoolean(boolString);
            }
            else if (reader.TokenType == JsonTokenType.True)
            {
                return true;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return false;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }

    }
}
