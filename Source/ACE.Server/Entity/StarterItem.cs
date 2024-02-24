using System.Text.Json.Serialization;

namespace ACE.Server.Entity
{
    public class StarterItem
    {
        public StarterItem()
        {
            StackSize = 1;
        }

        [JsonPropertyName("weenieId")]
        public uint WeenieId { get; set; }

        [JsonPropertyName("stacksize")]
        public ushort StackSize { get; set; }
    }
}
