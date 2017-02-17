using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Entity
{
    public class ExperienceExpenditureChart
    {
        [JsonProperty("ranks")]
        public List<ExperienceExpenditure> Ranks { get; set; } = new List<ExperienceExpenditure>();
    }
}
