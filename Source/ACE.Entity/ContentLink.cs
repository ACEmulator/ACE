using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    /// <summary>
    /// a 1-way association of content.  if a 2-way link is desired, it will have to be added
    /// in both places explicitly
    /// </summary>
    [DbTable("ace_content_link")]
    public class ContentLink : ChangeTrackedObject
    {
        /// <summary>
        /// content identifier
        /// </summary>
        public Guid ContentGuid { get; set; }

        [DbField("contentGuid1", (int)MySqlDbType.Binary, ListGet = true, ListDelete = true)]
        public byte[] ContentGuid_Binder
        {
            get { return ContentGuid.ToByteArray(); }
            set { ContentGuid = new Guid(value); }
        }

        /// <summary>
        /// associated content identifier
        /// </summary>
        public Guid AssociatedContentGuid { get; set; }

        [DbField("contentGuid2", (int)MySqlDbType.Binary)]
        public byte[] AssociatedContentGuid_Binder
        {
            get { return AssociatedContentGuid.ToByteArray(); }
            set { AssociatedContentGuid = new Guid(value); }
        }
    }
}
