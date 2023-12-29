using Lifestoned.DataModel.Gdle;
using Lifestoned.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ACE.Adapter.GDLE.Models
{
    public class Emote
    {
        [JsonPropertyName("category")]
        public uint Category { get; set; }

        [JsonIgnore]
        [Display(Name = "Emote Category")]
        public EmoteCategory EmoteCategory
        {
            get
            {
                return (EmoteCategory)Category;
            }
            set
            {
                Category = (uint)value;
            }
        }

        [JsonIgnore]
        public EmoteType? NewEmoteType { get; set; }

        [JsonPropertyName("emotes")]
        public List<EmoteAction> Actions { get; set; }

        [JsonPropertyName("probability")]
        public float? Probability { get; set; }

        [JsonPropertyName("vendorType")]
        [EmoteCategory(EmoteCategory.Vendor)]
        public uint? VendorType { get; set; }

        [JsonPropertyName("quest")]
        [EmoteCategory(EmoteCategory.QuestFailure)]
        [EmoteCategory(EmoteCategory.QuestSuccess)]
        [EmoteCategory(EmoteCategory.ReceiveTalkDirect)]
        [EmoteCategory(EmoteCategory.TestSuccess)]
        [EmoteCategory(EmoteCategory.TestFailure)]
        [EmoteCategory(EmoteCategory.GotoSet)]
        [EmoteCategory(EmoteCategory.QuestNoFellow)]
        public string Quest { get; set; }

        [JsonPropertyName("classID")]
        [EmoteCategory(EmoteCategory.Refuse)]
        [EmoteCategory(EmoteCategory.Give)]
        public uint? ClassId { get; set; }

        [JsonPropertyName("style")]
        [EmoteCategory(EmoteCategory.HeartBeat)]
        public uint? Style { get; set; }

        [JsonPropertyName("substyle")]
        [EmoteCategory(EmoteCategory.HeartBeat)]
        public uint? SubStyle { get; set; }

        [JsonPropertyName("minhealth")]
        [EmoteCategory(EmoteCategory.WoundedTaunt)]
        public float? MinHealth { get; set; }

        [JsonPropertyName("maxhealth")]
        [EmoteCategory(EmoteCategory.WoundedTaunt)]
        public float? MaxHealth { get; set; }

        [JsonIgnore]
        public int? SortOrder { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }

        public static bool IsPropertyVisible(string propertyName, Emote emote)
        {
            PropertyInfo property = typeof(Emote).GetProperty(propertyName);
            List<EmoteCategoryAttribute> list = property.GetCustomAttributes(typeof(EmoteCategoryAttribute), inherit: false).Cast<EmoteCategoryAttribute>().ToList();
            if (list == null || list.Count < 1)
            {
                return true;
            }

            return list.Any((EmoteCategoryAttribute a) => a.CategoryList.Contains(emote.EmoteCategory));
        }
    }
}
