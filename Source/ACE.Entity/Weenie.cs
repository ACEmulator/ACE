using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_weenie_class")]
    public class Weenie
    {
        [DbField("weenieClassId", (int)MySqlDbType.UInt32, IsCriteria = true)]
        public uint WeenieClassId { get; set; }

        [DbField("weenieClassDescription", (int)MySqlDbType.Text)]
        public string WeenieClassDescription { get; set; }
    }
}
