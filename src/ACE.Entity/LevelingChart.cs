using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Entity
{
    public class LevelingChart
    {
        [JsonProperty("levelChart")]
        public List<CharacterLevel> Levels { get; set; } = new List<CharacterLevel>();
    }
}
