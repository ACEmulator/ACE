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
    [DbTable("ace_content_resource")]
    public class ContentResource : ChangeTrackedObject
    {
        public ContentResource()
        {
            ContentResourceGuid = Guid.NewGuid();
            IsDirty = true;
            HasEverBeenSavedToDatabase = false;
        }

        public Guid ContentResourceGuid { get; set; }

        [DbField("contentResourceGuid", (int)MySqlDbType.Binary, Update = false, IsCriteria = true)]
        public byte[] ContentResourceGuid_Binder
        {
            get { return ContentResourceGuid.ToByteArray(); }
            set { ContentResourceGuid = new Guid(value); }
        }

        /// <summary>
        /// content identifier
        /// </summary>
        public Guid ContentGuid { get; set; }

        [DbField("contentGuid", (int)MySqlDbType.Binary, Update = false, IsCriteria = true, ListGet = true, ListDelete = true)]
        public byte[] ContentId_Binder
        {
            get { return ContentGuid.ToByteArray(); }
            set { ContentGuid = new Guid(value); }
        }

        private string _name;

        [DbField("name", (int)MySqlDbType.Text)]
        public string Name
        {
            get { return _name; }
            set { _name = value; IsDirty = true; }
        }

        private string _resourceUri;

        [DbField("resourceUri", (int)MySqlDbType.Text)]
        public string ResourceUri
        {
            get { return _resourceUri; }
            set { _resourceUri = value; IsDirty = true; }
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
