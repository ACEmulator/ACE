using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object")]
    [DbGetList("vw_ace_object", 2, "landblock")]
    public class AceObject : BaseAceObject
    {
        [DbField("AceObjectId", (int)MySqlDbType.UInt32, Update = false, IsCriteria = true)]
        public override uint AceObjectId { get; set; }
        
    }
}
