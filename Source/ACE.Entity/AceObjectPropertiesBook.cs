using System;
using ACE.Common;
using MySql.Data.MySqlClient;
namespace ACE.Entity
{
    [DbTable("ace_object_properties_book")]
    public class AceObjectPropertiesBook : BaseAceProperty, ICloneable
    {
        [DbField("page", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public uint Page { get; set; }

        [DbField("authorName", (int)MySqlDbType.VarChar)]
        public string AuthorName { get; set; }

        [DbField("authorAccount", (int)MySqlDbType.VarChar)]
        public string AuthorAccount { get; set; }

        [DbField("authorId", (int)MySqlDbType.UInt32)]
        public uint AuthorId { get; set; } = 0xFFFFFFFF;

        [DbField("pageText", (int)MySqlDbType.VarChar)]
        public string PageText { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
