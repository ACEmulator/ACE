using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class MotionData
    {
        public byte Bitfield { get; set; }
        public byte Bitfield2 { get; set; }
        public List<AnimData> Anims { get; set; } = new List<AnimData>();
        public AceVector3 Velocity { get; set; }
        public AceVector3 Omega { get; set; }

        public static MotionData Read(DatReader datReader)
        {
            MotionData md = new Entity.MotionData();

            byte numAnims = datReader.ReadByte();
            md.Bitfield = datReader.ReadByte();
            md.Bitfield2 = datReader.ReadByte();
            datReader.AlignBoundary();

            for (byte i = 0; i < numAnims; i++) {
                AnimData animData = new AnimData();
                animData.AnimId = datReader.ReadUInt32();
                animData.LowFrame = datReader.ReadUInt32();
                animData.HighFrame = datReader.ReadUInt32();
                animData.Framerate = datReader.ReadSingle();
                md.Anims.Add(animData);
            }

            if ((md.Bitfield2 & 1) > 0)
                md.Velocity = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());

            if ((md.Bitfield2 & 2) > 0)
                md.Omega = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());

            return md;
        }
    }
}
