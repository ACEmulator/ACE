using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class HousePortal
    {
        public uint Id { get; set; }
        public uint HouseId { get; set; }
        public uint ObjCellId { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public float OriginZ { get; set; }
        public float AnglesW { get; set; }
        public float AnglesX { get; set; }
        public float AnglesY { get; set; }
        public float AnglesZ { get; set; }
        public DateTime LastModified { get; set; }
    }
}
