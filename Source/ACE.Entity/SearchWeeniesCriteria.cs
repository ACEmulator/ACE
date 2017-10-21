using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class SearchWeeniesCriteria
    {
        public SearchWeeniesCriteria()
        {
        }

        [JsonProperty("partialName")]
        public string PartialName { get; set; }

        [JsonProperty("weenieClassId")]
        public uint? WeenieClassId { get; set; }

        [JsonProperty("weenieType")]
        public WeenieType? WeenieType { get; set; }

        [JsonProperty("itemType")]
        public ItemType? ItemType { get; set; }

        [JsonProperty("contentGuid")]
        public Guid? ContentGuid { get; set; }

        [JsonProperty("userModified")]
        public bool? UserModified { get; set; }

        [JsonProperty("criteria")]
        public List<SearchWeenieProperty> PropertyCriteria { get; set; } = new List<SearchWeenieProperty>();
    }
}
