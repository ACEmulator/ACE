using System;

namespace ACE.Entity.Models
{
    public class PropertiesPosition
    {
        public uint ObjCellId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationW { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }

        public PropertiesPosition Clone()
        {
            var result = new PropertiesPosition
            {
                ObjCellId = ObjCellId,
                PositionX = PositionX,
                PositionY = PositionY,
                PositionZ = PositionZ,
                RotationW = RotationW,
                RotationX = RotationX,
                RotationY = RotationY,
                RotationZ = RotationZ,
            };

            return result;
        }
    }
}
