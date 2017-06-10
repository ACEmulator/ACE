using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_character")]
    [DbList("vw_ace_character", "accountId", "deleted")]
    [DbGetAggregate("vw_ace_character", 106, "MAX", "guid")]
    public class CachedCharacter
    {
        [DbField("guid", (int)MySqlDbType.UInt32)]
        public uint LowGuid { get; set; }

        public ObjectGuid Guid
        {
            get { return new ObjectGuid(LowGuid, GuidType.Player); } 
            set { LowGuid = value.Low; }
        }

        public byte SlotId { get; }

        [DbField("accountId", (int)MySqlDbType.UInt32)]
        public uint AccountId { get; set; }

        [DbField("name", (int)MySqlDbType.VarChar, IsCriteria = true)]
        public string Name { get; set; }

        [DbField("deleted", (int)MySqlDbType.Bit)]
        public bool Deleted { get; set; }

        [DbField("deleteTime", (int)MySqlDbType.UInt64)]
        public ulong DeleteTime { get; set; }

        public CachedCharacter() { }

        public CachedCharacter(ObjectGuid guid, byte slotId, string name, ulong deleteTime)
        {
            Guid = guid;
            SlotId = slotId;
            Name = name;
            DeleteTime = deleteTime;
        }
    }
}
