using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class GeneratorTable
    {
        [JsonPropertyName("delay")]
        public float Delay { get; set; }

        [JsonPropertyName("frame")]
        public Frame Frame { get; set; } = new Frame();


        [JsonPropertyName("initCreate")]
        public uint InitCreate { get; set; }

        [JsonPropertyName("maxNum")]
        public uint MaxNumber { get; set; }

        [JsonPropertyName("objcell_id")]
        public uint ObjectCell { get; set; }

        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("ptid")]
        public uint PaletteId { get; set; }

        [JsonPropertyName("shade")]
        public float Shade { get; set; }

        [JsonPropertyName("slot")]
        public uint Slot { get; set; }

        [JsonPropertyName("stackSize")]
        public int StackSize { get; set; }

        [JsonPropertyName("type")]
        public uint WeenieClassId { get; set; }

        [JsonPropertyName("whenCreate")]
        public uint WhenCreate { get; set; }

        [JsonPropertyName("whereCreate")]
        public uint WhereCreate { get; set; }

        [JsonIgnore]
        public RegenerationType WhenCreateEnum
        {
            get
            {
                return (RegenerationType)WhenCreate;
            }
            set
            {
                WhenCreate = (uint)value;
            }
        }

        [JsonIgnore]
        public RegenerationLocation WhereCreateEnum
        {
            get
            {
                return (RegenerationLocation)WhereCreate;
            }
            set
            {
                WhereCreate = (uint)value;
            }
        }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
