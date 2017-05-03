using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_portal_object")]
    [DbGetList("vw_ace_portal_object", 14, "baseAceObjectId")]
    public class AcePortalObject : AceObject
    {
        [DbField("baseAceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public override uint AceObjectId { get; set; }

        [DbField("weenieClassId", (int)MySqlDbType.UInt16)]
        public override ushort WeenieClassId { get; set; }

        public Position DestPosition
        {
            get {
                if (DestLandblockId == null)
                    return null;
                else
                    return new Position(DestLandblockId.Value, DestPosX.Value, DestPosY.Value, DestPosZ.Value, DestQX, DestQY, DestQZ.Value, DestQW.Value);
            }
        }

        [DbField("destLandblockId", (int)MySqlDbType.UInt32)]
        private uint? DestLandblockId { get; set; }

        [DbField("destX", (int)MySqlDbType.Float)]
        private float? DestPosX { get; set; }

        [DbField("destY", (int)MySqlDbType.Float)]
        private float? DestPosY { get; set; }

        [DbField("destZ", (int)MySqlDbType.Float)]
        private float? DestPosZ { get; set; }

        // Should always be zero
        private float DestQX { get { return 0; } }

        // Should always be zero
        private float DestQY { get { return 0; } }

        [DbField("destQZ", (int)MySqlDbType.Float)]
        private float? DestQZ { get; set; }

        [DbField("destQW", (int)MySqlDbType.Float)]
        private float? DestQW { get; set; }

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
