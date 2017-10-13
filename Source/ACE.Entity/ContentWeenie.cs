using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ACE.Entity
{
    [DbTable("ace_content_weenie")]
    public class ContentWeenie : ChangeTrackedObject
    {
        public ContentWeenie()
        {
            ContentWeenieGuid = Guid.NewGuid();
            IsDirty = true;
            HasEverBeenSavedToDatabase = false;
        }
        
        public Guid ContentWeenieGuid { get; set; }

        [DbField("contentWeenieGuid", (int)MySqlDbType.Binary, Update = false, IsCriteria = true)]
        public byte[] ContentWeenieGuid_Binder
        {
            get { return ContentWeenieGuid.ToByteArray(); }
            set { ContentWeenieGuid = new Guid(value); }
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

        /// <summary>
        /// this value is functionally immutable.  do not set expecting it to be updated in the database, only ever inserted.
        /// </summary>
        [DbField("weenieId", (int)MySqlDbType.UInt32, Update = false)]
        public uint WeenieId { get; set; }

        private string _comment;

        [DbField("comment", (int)MySqlDbType.Text)]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; IsDirty = true; }
        }
    }
}
