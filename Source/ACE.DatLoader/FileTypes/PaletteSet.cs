using System;
using System.IO;
using System.Collections.Generic;
namespace ACE.DatLoader.Entity
{
    /* These are client_portal.dat files starting with 0x24------ */
    public class PaletteSet
    {
        public uint PaletteSetId { get; set; }
        public List<uint> PaletteList { get; set; } = new List<uint>();

        public static PaletteSet ReadFromDat(string datFilePath, uint offset, uint size, uint sectorSize)
        {
            DatReader datReader = new DatReader(datFilePath, offset, size, sectorSize);

            PaletteSet p = new PaletteSet();
            p.PaletteSetId = datReader.ReadUInt32();

            uint numpalettesets = datReader.ReadUInt32();
            for (int i = 0; i < numpalettesets; i++)
                p.PaletteList.Add(datReader.ReadUInt32());

            return p;
        }
    }
}