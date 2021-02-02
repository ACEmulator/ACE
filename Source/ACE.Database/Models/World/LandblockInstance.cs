using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class LandblockInstance
    {
        public LandblockInstance()
        {
            LandblockInstanceLink = new HashSet<LandblockInstanceLink>();
        }

        public uint Guid { get; set; }
        public int? Landblock { get; set; }
        public uint WeenieClassId { get; set; }
        public uint ObjCellId { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public float OriginZ { get; set; }
        public float AnglesW { get; set; }
        public float AnglesX { get; set; }
        public float AnglesY { get; set; }
        public float AnglesZ { get; set; }
        public bool IsLinkChild { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<LandblockInstanceLink> LandblockInstanceLink { get; set; }
    }
}
