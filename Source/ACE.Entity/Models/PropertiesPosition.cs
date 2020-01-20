using System;

namespace ACE.Entity.Models
{
    public class PropertiesPosition
    {
        public uint ObjCellId { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public float OriginZ { get; set; }
        public float AnglesW { get; set; }
        public float AnglesX { get; set; }
        public float AnglesY { get; set; }
        public float AnglesZ { get; set; }

        public PropertiesPosition Clone()
        {
            var result = new PropertiesPosition
            {
                ObjCellId = ObjCellId,
                OriginX = OriginX,
                OriginY = OriginY,
                OriginZ = OriginZ,
                AnglesW = AnglesW,
                AnglesX = AnglesX,
                AnglesY = AnglesY,
                AnglesZ = AnglesZ,
            };

            return result;
        }
    }
}
