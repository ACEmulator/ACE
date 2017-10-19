using System;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_contract_tracker")]
    public class AceContractTracker : BaseAceProperty, ICloneable
    {
        [DbField("contractId", (int)MySqlDbType.UInt32, IsCriteria = true, Update = false)]
        public uint ContractId { get; set; }

        [DbField("version", (int)MySqlDbType.UInt32)]
        public uint Version { get; set; } = 0;

        [DbField("stage", (int)MySqlDbType.UInt32)]
        public uint Stage { get; set; } = 0;

        [DbField("timeWhenDone", (int)MySqlDbType.UInt64)]
        public ulong TimeWhenDone { get; set; } = 0;

        [DbField("timeWhenRepeats", (int)MySqlDbType.UInt64)]
        public ulong TimeWhenRepeats { get; set; } = 0;

        [DbField("deleteContract", (int)MySqlDbType.UInt32)]
        public uint DeleteContract { get; set; } = 0;

        [DbField("setAsDisplayContract", (int)MySqlDbType.UInt32)]
        public uint SetAsDisplayContract { get; set; } = 0;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
