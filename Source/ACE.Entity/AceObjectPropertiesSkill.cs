using System;
using ACE.Common;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ACE.Entity
{
    [DbTable("ace_object_properties_skill")]
    public class AceObjectPropertiesSkill : BaseAceProperty, ICloneable
    {
        private uint _xpSpent = 0;
        private ushort _skillPoints = 0;
        private ushort _skillStatus = 0;

        [JsonIgnore]
        [DbField("skillId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public ushort SkillId { get; set; }

        /// <summary>
        /// typed property for exposure in the API
        /// </summary>
        [JsonProperty("skill")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Skill Skill_Typed
        {
            get { return (Skill)SkillId; }
            set { SkillId = (ushort)value; }
        }

        [JsonIgnore]
        [DbField("skillStatus", (int)MySqlDbType.UInt16)]
        public ushort SkillStatus
        {
            get
            {
                return _skillStatus;
            }
            set
            {
                _skillStatus = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// typed property for exposure in the API
        /// </summary>
        [JsonProperty("skillStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SkillStatus SkillStatus_Typed
        {
            get { return (SkillStatus)_skillStatus; }
            set { _skillStatus = (ushort)value; }
        }

        [JsonProperty("ranks")]
        [DbField("skillPoints", (int)MySqlDbType.UInt16)]
        public ushort SkillPoints
        {
            get
            {
                return _skillPoints;
            }
            set
            {
                _skillPoints = value;
                IsDirty = true;
            }
        }

        [JsonProperty("experienceSpent")]
        [DbField("skillXpSpent", (int)MySqlDbType.UInt32)]
        public uint SkillXpSpent
        {
            get
            {
                return _xpSpent;
            }
            set
            {
                _xpSpent = value;
                IsDirty = true;
            }
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
