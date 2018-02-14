using System.IO;

namespace ACE.Entity
{
    public class Appearance
    {
        public uint Eyes { get; set; }
        public uint Nose { get; set; }
        public uint Mouth { get; set; }
        public uint HairColor { get; set; }
        public uint EyeColor { get; set; }
        public uint HairStyle { get; set; }
        public uint HeadgearStyle { get; set; }
        public uint HeadgearColor { get; set; }
        public uint ShirtStyle { get; set; }
        public uint ShirtColor { get; set; }
        public uint PantsStyle { get; set; }
        public uint PantsColor { get; set; }
        public uint FootwearStyle { get; set; }
        public uint FootwearColor { get; set; }
        public double SkinHue { get; set; }
        public double HairHue { get; set; }
        public double HeadgearHue { get; set; }
        public double ShirtHue { get; set; }
        public double PantsHue { get; set; }
        public double FootwearHue { get; set; }

        public void Unpack(BinaryReader reader)
        {
            Eyes            = reader.ReadUInt32();
            Nose            = reader.ReadUInt32();
            Mouth           = reader.ReadUInt32();
            HairColor       = reader.ReadUInt32();
            EyeColor        = reader.ReadUInt32();
            HairStyle       = reader.ReadUInt32();
            HeadgearStyle   = reader.ReadUInt32();
            HeadgearColor   = reader.ReadUInt32();
            ShirtStyle      = reader.ReadUInt32();
            ShirtColor      = reader.ReadUInt32();
            PantsStyle      = reader.ReadUInt32();
            PantsColor      = reader.ReadUInt32();
            FootwearStyle   = reader.ReadUInt32();
            FootwearColor   = reader.ReadUInt32();
            SkinHue         = reader.ReadDouble();
            HairHue         = reader.ReadDouble();
            HeadgearHue     = reader.ReadDouble();
            ShirtHue        = reader.ReadDouble();
            PantsHue        = reader.ReadDouble();
            FootwearHue     = reader.ReadDouble();
        }
    }
}
