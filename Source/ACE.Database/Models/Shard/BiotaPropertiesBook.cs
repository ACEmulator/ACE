using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int MaxNumPages { get; set; }
        public int MaxNumCharsPerPage { get; set; }

        public Biota Object { get; set; }
    }
}
