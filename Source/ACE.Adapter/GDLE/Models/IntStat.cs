using Lifestoned.DataModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class IntStat
    {
        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonIgnore]
        public string[] MultiSelectRaw { get; set; }

        [JsonIgnore]
        public string[] MultiSelect
        {
            get
            {
                int value = Value;
                List<int> list = new List<int>();
                for (int j = 0; j <= 31; j++)
                {
                    uint num = (uint)Math.Pow(2.0, j);
                    if ((num & value) != 0)
                    {
                        list.Add((int)num);
                    }
                }

                return list.Select((int i) => i.ToString()).ToArray();
            }
            set
            {
                MultiSelectRaw = value;
                Value = 0;
                if (value != null && value.Length >= 1)
                {
                    foreach (string s in value)
                    {
                        Value += int.Parse(s);
                    }
                }
            }
        }

        [JsonIgnore]
        public string PropertyIdBinder => ((IntPropertyId)Key).GetName();

        [JsonIgnore]
        public ItemType? ItemTypeBoundValue
        {
            get
            {
                return (ItemType)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public WeenieType? WeenieTypeBoundValue
        {
            get
            {
                return (WeenieType)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public CreatureType? CreatureTypeBoundValue
        {
            get
            {
                return (CreatureType)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public ArmorType? ArmorTypeBoundValue
        {
            get
            {
                return (ArmorType)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public WieldRequirements? WieldRequirementsBoundValue
        {
            get
            {
                return (WieldRequirements)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public PaletteTemplate? PaletteTemplateBoundValue
        {
            get
            {
                return (PaletteTemplate)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public int? UiEffects { get; set; }

        [JsonIgnore]
        public Material? Material_Binder
        {
            get
            {
                return (Material)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public HeritageGroup? HeritageBinder
        {
            get
            {
                return (HeritageGroup)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public WeaponType? WeaponTypeBoundValue
        {
            get
            {
                return (WeaponType)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public SkillId? SkillIdBoundValue
        {
            get
            {
                return (SkillId)Value;
            }
            set
            {
                Value = (int)value.Value;
            }
        }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
