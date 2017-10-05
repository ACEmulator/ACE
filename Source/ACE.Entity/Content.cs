using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using Newtonsoft.Json;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    /// <summary>
    /// ACE-constructed concept that allows for association of resources into a logical grouping
    /// for ease of finding information about some content and editing the associated resources
    /// </summary>
    public class Content
    {
        /// <summary>
        /// content identifier
        /// </summary>
        public Guid ContentId { get; set; }

        [DbField("contentId", (int)MySqlDbType.Binary, Update = false, IsCriteria = true, ListGet = true)]
        public byte[] ContentId_Binder
        {
            get { return ContentId.ToByteArray(); }
            set { ContentId = new Guid(value); }
        }

        /// <summary>
        /// user-defined name for the content that is searchable
        /// </summary>
        [DbField("contentName", (int)MySqlDbType.Text)]
        public string ContentName { get; set; }

        /// <summary>
        /// logical grouping for what type of content this is
        /// </summary>
        [DbField("contentType", (int)MySqlDbType.UByte)]
        public ContentType ContentType { get; set; }

        /// <summary>
        /// list of landblock upper words (ie, landblock x/y) containing dungeons associated with this quest.  please note, other content
        /// may be associated with them as well
        /// </summary>
        public List<ContentLandblock> AssociatedLandblocks { get; set; }

        /// <summary>
        /// list of weenies that are associated with this content.  NPCs, items turned in, found, or otherwise
        /// involved in this quest.  could also be traps, levers, or other static objects in the dungeon.
        /// also includes spawn generators associated with the quests.
        /// </summary>
        public List<ContentWeenie> Weenies { get; set; }

        /// <summary>
        /// list of URLs linking the content in other places (acpedia, asheron.wikia, etc)
        /// </summary>
        public List<ContentResource> ExternalResources { get; set; }

        /// <summary>
        /// list of associated content
        /// </summary>
        public List<ContentLink> AssociatedContent { get; set; }

        public string GenerateSqlScript()
        {
            StringBuilder b = new StringBuilder();

            // script will be keyed off the Guid

            return b.ToString();
        }
    }
}
