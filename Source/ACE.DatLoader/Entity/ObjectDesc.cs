using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class ObjectDesc
    {
        public uint ObjId { get; set; }
        public Position BaseLoc { get; set; }
        public float Freq { get; set; }
        public float DisplaceX { get; set; }
        public float DisplaceY { get; set; }
        public float MinScale { get; set; }
        public float MaxScale { get; set; }
        public float MaxRotation { get; set; }
        public float MinSlope { get; set; }
        public float MaxSlope { get; set; }
        public uint Align { get; set; }
        public uint Orient { get; set; }
        public uint WeenieObj { get; set; }

        public static ObjectDesc Read(DatReader datReader)
        {
            ObjectDesc obj = new ObjectDesc();

            obj.ObjId = datReader.ReadUInt32();
            obj.BaseLoc = PositionExtensions.ReadPosition(datReader);
            obj.Freq = datReader.ReadSingle();
            obj.DisplaceX = datReader.ReadSingle();
            obj.DisplaceY = datReader.ReadSingle();
            obj.MinScale = datReader.ReadSingle();
            obj.MaxScale = datReader.ReadSingle();
            obj.MaxRotation = datReader.ReadSingle();
            obj.MinSlope = datReader.ReadSingle();
            obj.MaxSlope = datReader.ReadSingle();
            obj.Align = datReader.ReadUInt32();
            obj.Orient = datReader.ReadUInt32();
            obj.WeenieObj = datReader.ReadUInt32();

            return obj;
        }
    }
}
