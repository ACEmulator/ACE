using System;
using System.IO;
using System.Collections.Generic;
namespace ACE.DatLoader.Entity
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0F. 
    /// They contain, as the name may imply, a set of palettes (0x04 files)
    /// </summary>
    public class PaletteSet
    {
        public uint PaletteSetId { get; set; }
        public List<uint> PaletteList { get; set; } = new List<uint>();

        public static PaletteSet ReadFromDat(DatReader datReader)
        {
            PaletteSet p = new PaletteSet();
            p.PaletteSetId = datReader.ReadUInt32();

            uint numpalettesets = datReader.ReadUInt32();
            for (int i = 0; i < numpalettesets; i++)
                p.PaletteList.Add(datReader.ReadUInt32());

            return p;
        }
    }
}