using System;
using ACE.Common;

using Newtonsoft.Json;

namespace ACE.Entity
{
    //[DbTable("ace_object_properties_spell")]
    public class AceObjectPropertiesSpell : ICloneable
    {
        [JsonIgnore]
        //[DbField("aceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true, ListGet = true, ListDelete = true)]
        public uint AceObjectId { get; set; }

        [JsonProperty("spellId")]
        //[DbField("spellId", (int)MySqlDbType.UInt32)]
        public uint SpellId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
