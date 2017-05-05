using System;
using System.IO;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0F. 
    /// They contain, as the name may imply, a set of palettes (0x04 files)
    /// </summary>
    public class PaletteSet
    {
        public uint PaletteSetId { get; set; }
        public List<uint> PaletteList { get; set; } = new List<uint>();

        public static PaletteSet ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E000018))
            {
                return (PaletteSet)DatManager.PortalDat.FileCache[0x0E000018];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                PaletteSet p = new PaletteSet();
                p.PaletteSetId = datReader.ReadUInt32();

                uint numpalettesets = datReader.ReadUInt32();
                for (int i = 0; i < numpalettesets; i++)
                    p.PaletteList.Add(datReader.ReadUInt32());

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = p;
                return p;
            }
        }

        /// <summary>
        /// Returns the palette ID (uint, 0x04 file) from the Palette list based on the corresponding hue
        /// Hue is mostly (only?) used in Character Creation data.
        /// "Hue" referred to as "shade" in acclient.c
        /// </summary>
        public uint GetPaletteID(double hue)
        {
            // Make sure the PaletteList has valid data and the hue is within valid ranges
            if (this.PaletteList.Count == 0 || hue < 0 || hue > 1)
                return 0;

            // Hue is stored in DB as a percent of the total, so do some math to figure out the int position
            int palIndex = Convert.ToInt32(Convert.ToDouble(this.PaletteList.Count - 0.000001) * hue); // Taken from acclient.c (PalSet::GetPaletteID)
                                                                                                       // Since the hue numbers are a little odd, make sure we're in the bounds.
            if (palIndex < 0)
                palIndex = 0;
            if (palIndex > this.PaletteList.Count - 1)
                palIndex = this.PaletteList.Count - 1;
            return this.PaletteList[palIndex];
        }
    }
}
