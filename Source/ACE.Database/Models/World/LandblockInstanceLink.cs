using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class LandblockInstanceLink
    {
        public uint Id { get; set; }
        public uint ParentGuid { get; set; }
        public uint ChildGuid { get; set; }
        public DateTime LastModified { get; set; }

        public virtual LandblockInstance ParentGu { get; set; }
    }
}
