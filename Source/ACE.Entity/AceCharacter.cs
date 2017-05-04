using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("vw_ace_character")]
    [DbGetList("vw_ace_character", 42, "guid")]
    public class AceCharacter
    {
        [DbField("guid", (int)MySqlDbType.UInt16, IsCriteria = true)]
        public uint Guid { get; set; }

        [DbField("name", (int)MySqlDbType.VarChar)]
        public string Name { get; set; }
    }
}
