using System;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class LandSurf : IUnpackable
    {
        public uint Type { get; private set; }
        //public PalShift PalShift { get; } = new PalShift(); // This is used if Type == 1 (which we haven't seen yet)
        public TexMerge TexMerge { get; } = new TexMerge();

        public void Unpack(BinaryReader reader)
        {
            Type = reader.ReadUInt32(); // This is always 0

            if (Type == 1)
                throw new NotImplementedException();
            TexMerge.Unpack(reader);
        }
    }
}
