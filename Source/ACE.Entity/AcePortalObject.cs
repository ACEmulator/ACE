using ACE.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [DbTable("ace_portal_object")]
    [DbGetList("ace_portal_object", 14, "weenieClassId")]
    public class AcePortalObject
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        public Position DestPosition
        {
            get { return new Position(DestLandblockId, DestPosX, DestPosY, DestPosZ, DestQX, DestQY, DestQZ, DestQW); }
        }

        [DbField("destLandblockId", (int)MySqlDbType.UInt32)]
        public uint DestLandblockId { get; set; }

        [DbField("destX", (int)MySqlDbType.Float)]
        public float DestPosX { get; set; }

        [DbField("destY", (int)MySqlDbType.Float)]
        public float DestPosY { get; set; }

        [DbField("destZ", (int)MySqlDbType.Float)]
        public float DestPosZ { get; set; }

        public float DestQX { get { return 0; } }

        public float DestQY { get { return 0; } }

        [DbField("destQZ", (int)MySqlDbType.Float)]
        public float DestQZ { get; set; }

        [DbField("destQW", (int)MySqlDbType.Float)]
        public float DestQW { get; set; }

        [DbField("min_lvl", (int)MySqlDbType.UInt32)]
        public uint MinLvl { get; set; }

        [DbField("max_lvl", (int)MySqlDbType.UInt32)]
        public uint MaxLvl { get; set; }

        [DbField("societyId", (int)MySqlDbType.UByte)]
        public byte SocietyId { get; set; }

        [DbField("isTieable", (int)MySqlDbType.UByte)]
        public byte IsTieable { get; set; }

        [DbField("isRecallable", (int)MySqlDbType.UByte)]
        public byte IsRecallable { get; set; }

        [DbField("isSummonable", (int)MySqlDbType.UByte)]
        public byte IsSummonable { get; set; }
    }
}
