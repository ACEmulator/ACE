using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_character")]
    [DbGetAggregate("vw_ace_character", 106, "MAX", "guid")]
    public class CachedCharacter
    {
        [DbField("guid", (int)MySqlDbType.UInt32)]
        public uint FullGuid { get; set; }

        public ObjectGuid Guid
        {
            get
            {
                return new ObjectGuid(FullGuid);
            }
            set
            {
                FullGuid = value.Full;
            }
        }

        public byte SlotId { get; set; }

        [DbField("accountId", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint AccountId { get; set; }

        [DbField("name", (int)MySqlDbType.VarChar, IsCriteria = true)]
        public string Name { get; set; }

        [DbField("deleted", (int)MySqlDbType.Bit, ListGet = true)]
        public bool Deleted { get; set; }

        [DbField("deleteTime", (int)MySqlDbType.UInt64)]
        public ulong DeleteTime { get; set; }

        [DbField("loginTimestamp", (int)MySqlDbType.Double)]
        public double LoginTimestamp { get; set; }

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
