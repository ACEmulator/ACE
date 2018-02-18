using System;
using ACE.Common;

using Newtonsoft.Json;

namespace ACE.Entity
{
    //[DbTable("ace_object_properties_book")]
    public class AceObjectPropertiesBook : BaseAceProperty, ICloneable
    {
        [JsonProperty("page")]
        //[DbField("page", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public uint Page { get; set; }

        [JsonProperty("authorName")]
        //[DbField("authorName", (int)MySqlDbType.VarChar)]
        public string AuthorName { get; set; }

        [JsonProperty("authorAccount")]
        //[DbField("authorAccount", (int)MySqlDbType.VarChar)]
        public string AuthorAccount { get; set; }

        [JsonProperty("authorId")]
        //[DbField("authorId", (int)MySqlDbType.UInt32)]
        public uint AuthorId { get; set; } = 0xFFFFFFFF;

        [JsonProperty("ignoreAuthor")]
        //[DbField("ignoreAuthor", (int)MySqlDbType.UInt32)]
        public uint IgnoreAuthor { get; set; }

        [JsonProperty("pageText")]
        //[DbField("pageText", (int)MySqlDbType.VarChar)]
        public string PageText { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
