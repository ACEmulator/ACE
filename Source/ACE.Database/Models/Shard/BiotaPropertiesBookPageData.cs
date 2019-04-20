using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesBookPageData
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint PageId { get; set; }
        public uint AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAccount { get; set; }
        public bool IgnoreAuthor { get; set; }
        public string PageText { get; set; }

        public Biota Object { get; set; }
    }
}
