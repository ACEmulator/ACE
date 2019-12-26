using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesBook
    {
        public uint ObjectId { get; set; }
        public int MaxNumPages { get; set; }
        public int MaxNumCharsPerPage { get; set; }

        public virtual Biota Object { get; set; }
    }
}
