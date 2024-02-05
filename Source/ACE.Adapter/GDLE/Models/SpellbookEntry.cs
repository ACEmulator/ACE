using Lifestoned.DataModel.Shared;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellbookEntry
    {
        [JsonPropertyName("key")]
        public int SpellId { get; set; }

        [JsonPropertyName("value")]
        public SpellCastingStats Stats { get; set; } = new SpellCastingStats();


        [JsonIgnore]
        public bool Deleted { get; set; }

        public string GetSpellDescription()
        {
            string input = ((SpellId)SpellId).ToString();
            return "(" + SpellId + ") " + Regex.Replace(input, "([A-Z0-9])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}
