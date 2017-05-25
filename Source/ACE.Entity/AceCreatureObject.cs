using ACE.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ACE.Entity
{
    [DbTable("vw_ace_creature_object")]
    [DbGetList("vw_ace_creature_object", 42, "weenieClassId")]
    public class AceCreatureObject : BaseAceObject, ICreatureStats
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public override uint WeenieClassId { get; set; }

        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32)]
        public override uint AceObjectId { get; set; }

        [DbField("level", (int)MySqlDbType.UInt32)]
        public uint Level { get; set; }

        [DbField("strength", (int)MySqlDbType.UInt32)]
        public uint Strength { get; set; }

        [DbField("endurance", (int)MySqlDbType.UInt32)]
        public uint Endurance { get; set; }

        [DbField("coordination", (int)MySqlDbType.UInt32)]
        public uint Coordination { get; set; }

        [DbField("quickness", (int)MySqlDbType.UInt32)]
        public uint Quickness { get; set; }

        [DbField("focus", (int)MySqlDbType.UInt32)]
        public uint Focus { get; set; }

        [DbField("self", (int)MySqlDbType.UInt32)]
        public uint Self { get; set; }

        [DbField("health", (int)MySqlDbType.UInt32)]
        public uint Health { get; set; }

        [DbField("stamina", (int)MySqlDbType.UInt32)]
        public uint Stamina { get; set; }

        [DbField("mana", (int)MySqlDbType.UInt32)]
        public uint Mana { get; set; }

        [DbField("luminance", (int)MySqlDbType.Byte)]
        public byte Luminance { get; set; }

        [DbField("lootTier", (int)MySqlDbType.Byte)]
        public byte LootTier { get; set; }

        public List<WeeniePaletteOverride> WeeniePaletteOverrides { get; set; } = new List<WeeniePaletteOverride>();

        public List<WeenieTextureMapOverride> WeenieTextureMapOverrides { get; set; } = new List<WeenieTextureMapOverride>();

        public List<WeenieAnimationOverride> WeenieAnimationOverrides { get; set; } = new List<WeenieAnimationOverride>();
    }
}
