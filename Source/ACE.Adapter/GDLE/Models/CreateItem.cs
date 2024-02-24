using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class CreateItem
    {
        [JsonPropertyName("wcid")]
        public uint? WeenieClassId { get; set; }

        [JsonPropertyName("palette")]
        public uint? Palette { get; set; }

        [JsonPropertyName("shade")]
        public double? Shade { get; set; }

        [JsonPropertyName("destination")]
        public uint? Destination { get; set; }

        [JsonIgnore]
        public Destination? Destination_Binder
        {
            get
            {
                return (Destination?)Destination;
            }
            set
            {
                Destination = (uint?)value;
            }
        }

        [JsonPropertyName("stack_size")]
        public int? StackSize { get; set; }

        [JsonPropertyName("try_to_bond")]
        public byte? TryToBond { get; set; }

        [JsonIgnore]
        public bool TryToBond_BooleanBinder
        {
            get
            {
                return TryToBond != 0;
            }
            set
            {
                TryToBond = (byte)(value ? 1u : 0u);
            }
        }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
