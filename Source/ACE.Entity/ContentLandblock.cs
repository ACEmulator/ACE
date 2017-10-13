using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_content_landblock")]
    public class ContentLandblock : ChangeTrackedObject
    {
        public ContentLandblock()
        {
            ContentLandblockGuid = Guid.NewGuid();
            IsDirty = true;
            HasEverBeenSavedToDatabase = false;
        }

        public Guid ContentLandblockGuid { get; set; }

        [DbField("contentLandblockGuid", (int)MySqlDbType.Binary, Update = false, IsCriteria = true)]
        public byte[] ContentLandblockGuid_Binder
        {
            get { return ContentLandblockGuid.ToByteArray(); }
            set { ContentLandblockGuid = new Guid(value); }
        }

        /// <summary>
        /// content identifier
        /// </summary>
        public Guid ContentGuid { get; set; }

        [DbField("contentGuid", (int)MySqlDbType.Binary, Update = false, ListGet = true, ListDelete = true)]
        public byte[] ContentGuid_Binder
        {
            get { return ContentGuid.ToByteArray(); }
            set { ContentGuid = new Guid(value); }
        }

        public LandblockId LandblockId { get; set; }

        /// <summary>
        /// this value is functionally immutable.  do not set expecting it to be updated in the database, only ever inserted.
        /// </summary>
        [DbField("landblockId", (int)MySqlDbType.UInt32, Update = false)]
        public uint LandblockId_Binder
        {
            get { return LandblockId.Raw; }
            set { this.LandblockId = new LandblockId(value); }
        }

        private string _comment;

        [DbField("comment", (int)MySqlDbType.Text)]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; IsDirty = true; }
        }
    }
}
