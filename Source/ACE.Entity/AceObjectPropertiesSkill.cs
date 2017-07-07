using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_skill")]
    public class AceObjectPropertiesSkill : BaseAceProperty, ICloneable
    {
        private uint _xpSpent = 0;
        private ushort _skillPoints = 0;
        private ushort _skillStatus = 0;
        
        [DbField("skillId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public ushort SkillId { get; set; }

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
