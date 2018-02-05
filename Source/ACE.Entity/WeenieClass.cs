using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_weenie_class")]
    public class WeenieClass
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt32, ListGet = true)]
        public uint WeenieClassId { get; set; }

        [DbField("weenieClassDescription", (int)MySqlDbType.Text, IsCriteria = true, ListGet = true)]
        public string WeenieClassDescription { get; set; }
    }
}
