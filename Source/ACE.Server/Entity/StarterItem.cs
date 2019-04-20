using Newtonsoft.Json;

namespace ACE.Server.Entity
{
    public class StarterItem
    {
        public StarterItem()
        {
            StackSize = 1;
        }

        [JsonProperty("weenieId")]
        public uint WeenieId { get; set; }

        [JsonProperty("stacksize")]
        public ushort StackSize { get; set; }
    }
}
