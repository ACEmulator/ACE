using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SkyObject
    {
        public float BeginTime { get; set; }
        public float EndTime { get; set; }
        public float BeginAngle { get; set; }
        public float EndAngle { get; set; }
        public float TexVelocityX { get; set; }
        public float TexVelocityY { get; set; }
        public float TexVelocityZ { get; set; } = 0;
        public uint DefaultGFXObjectId { get; set; }
        public uint DefaultPESObjectId { get; set; }
        public uint Properties { get; set; }
        
        public static SkyObject Read(DatReader datReader)
        {
            SkyObject obj = new SkyObject();
            obj.BeginTime = datReader.ReadSingle();
            obj.EndTime = datReader.ReadSingle();
            obj.BeginAngle = datReader.ReadSingle();
            obj.EndAngle = datReader.ReadSingle();
            obj.TexVelocityX = datReader.ReadSingle();
            obj.TexVelocityY = datReader.ReadSingle();
            obj.DefaultGFXObjectId = datReader.ReadUInt32();
            obj.DefaultPESObjectId = datReader.ReadUInt32();
            obj.Properties = datReader.ReadUInt32();
            return obj;
        }
    }
}
