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
    [DbTable("ace_content")]
    public class Content : ChangeTrackedObject
    {
        public Content()
        {
        }

        /// <summary>
        /// content identifier.
        /// </summary>
        [JsonProperty("contentGuid")]
        public Guid? ContentGuid { get; set; }

        [JsonIgnore]
        [DbField("contentGuid", (int)MySqlDbType.Binary, Update = false, IsCriteria = true)]
        public byte[] ContentId_Binder
        {
            get { return ContentGuid.Value.ToByteArray(); }
            set { ContentGuid = new Guid(value); }
        }

        /// <summary>
        /// user-defined name for the content that is searchable
        /// </summary>
        [JsonProperty("contentName")]
        [DbField("contentName", (int)MySqlDbType.Text)]
        public string ContentName { get; set; }

        /// <summary>
        /// logical grouping for what type of content this is
        /// </summary>
        [JsonProperty("contentType")]
        public ContentType? ContentType { get; set; }

        [JsonIgnore]
        [DbField("contentType", (int)MySqlDbType.Byte)]
        public uint? ContentType_Binder
        {
            get { return (uint?)ContentType; }
            set { ContentType = (value == null ? (ContentType?)null : (ContentType)value); }
        }

        /// <summary>
        /// This is a mocked property that will set a flag in the database any time this object is altered.  this flag
        /// will allow us to detect objects that have changed post-installation and generate changesetss
        /// </summary>
        [JsonIgnore]
        [DbField("userModified", (int)MySqlDbType.Bit)]
        public virtual bool UserModified
        {
            get { return true; }
            set { } // method intentionally not implemented
        }

        /// <summary>
        /// list of landblock upper words (ie, landblock x/y) containing dungeons associated with this quest.  please note, other content
        /// may be associated with them as well
        /// </summary>
        [JsonProperty("associatedLandblocks")]
        public List<ContentLandblock> AssociatedLandblocks { get; set; } = new List<ContentLandblock>();

        /// <summary>
        /// list of weenies that are associated with this content.  NPCs, items turned in, found, or otherwise
        /// involved in this quest.  could also be traps, levers, or other static objects in the dungeon.
        /// also includes spawn generators associated with the quests.
        /// </summary>
        [JsonProperty("weenies")]
        public List<ContentWeenie> Weenies { get; set; } = new List<ContentWeenie>();

        /// <summary>
        /// list of URLs linking the content in other places (acpedia, asheron.wikia, etc)
        /// </summary>
        [JsonProperty("externalResources")]
        public List<ContentResource> ExternalResources { get; set; } = new List<ContentResource>();

        /// <summary>
        /// list of associated content
        /// </summary>
        [JsonProperty("associatedContent")]
        public List<ContentLink> AssociatedContent { get; set; } = new List<ContentLink>();
        
        public override void SetDirtyFlags()
        {
            base.SetDirtyFlags();

            AssociatedContent.ForEach(c => c.SetDirtyFlags());
            ExternalResources.ForEach(r => r.SetDirtyFlags());
            Weenies.ForEach(w => w.SetDirtyFlags());
            AssociatedLandblocks.ForEach(l => l.SetDirtyFlags());
        }

        public override void ClearDirtyFlags()
        {
            base.ClearDirtyFlags();

            AssociatedContent.ForEach(c => c.ClearDirtyFlags());
            ExternalResources.ForEach(r => r.ClearDirtyFlags());
            Weenies.ForEach(w => w.ClearDirtyFlags());
            AssociatedLandblocks.ForEach(l => l.ClearDirtyFlags());
        }
    }
}
