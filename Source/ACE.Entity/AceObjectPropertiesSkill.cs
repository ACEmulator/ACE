using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_skill")]
    [DbGetList("ace_object_properties_skill", 26, "AceObjectId")]
    public class AceObjectPropertiesSkill
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint AceObjectId { get; set; }

        [DbField("skillId", (int)MySqlDbType.UInt16)]
        public ushort SkillId { get; set; }

        [DbField("skillStatus", (int)MySqlDbType.UInt16)]
        public ushort SkillStatus { get; set; }

        [DbField("skillPoints", (int)MySqlDbType.UInt16)]
        public ushort SkillPoints { get; set; }

        [DbField("skillXpSpent", (int)MySqlDbType.UInt32)]
        public uint SkillXpSpent { get; set; }
    }
}
