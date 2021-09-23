using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesBook
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int MaxNumPages { get; set; }
        public int MaxNumCharsPerPage { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
