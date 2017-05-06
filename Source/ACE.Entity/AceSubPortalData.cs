using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_portal_object")]
    [DbGetList("ace_portal_object", 15, "weenieClassId")]
    public class AceSubPortalData
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public ushort WeenieClassId { get; set; }

        public Position DestPosition
        {
            get
            {
                if (DestLandblockId == null)
                    return null;
                else
                    return new Position(DestLandblockId.Value, DestPosX.Value, DestPosY.Value, DestPosZ.Value, DestQX, DestQY, DestQZ.Value, DestQW.Value);
            }
        }

        [DbField("destLandblockId", (int)MySqlDbType.UInt32)]
        public uint? DestLandblockId { get; set; }

        [DbField("destX", (int)MySqlDbType.Float)]
        public float? DestPosX { get; set; }

        [DbField("destY", (int)MySqlDbType.Float)]
        public float? DestPosY { get; set; }

        [DbField("destZ", (int)MySqlDbType.Float)]
        public float? DestPosZ { get; set; }

        // Should always be zero
        public float DestQX { get { return 0; } }

        // Should always be zero
        public float DestQY { get { return 0; } }

        [DbField("destQZ", (int)MySqlDbType.Float)]
        public float? DestQZ { get; set; }

        [DbField("destQW", (int)MySqlDbType.Float)]
        public float? DestQW { get; set; }

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
