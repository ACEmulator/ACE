using ACE.DatLoader.Entity;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x40.
    /// It is essentially a map to a specific texture file (spritemap) that contains all the characters in this font.
    /// </summary>
    [DatFileType(DatFileType.Font)]
    public class Font : FileType
    {
        public uint MaxCharHeight;
        public uint MaxCharWidth;
        public uint NumCharacters;
        public List<FontCharDesc> CharDescs = new List<FontCharDesc>();
        public uint NumHorizontalBorderPixels;
        public uint NumVerticalBorderPixels;
        public uint BaselineOffset;
        public uint ForegroundSurfaceDataID; // This is a DataID to a Texture (0x06) type, if set
        public uint BackgroundSurfaceDataID; // This is a DataID to a Texture (0x06) type, if set

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            MaxCharHeight = reader.ReadUInt32();
            MaxCharWidth = reader.ReadUInt32();
            NumCharacters = reader.ReadUInt32();

            for(uint i = 0; i < NumCharacters; i++)
            {
                var fontCharDesc = new FontCharDesc();
                fontCharDesc.Unpack(reader);
                CharDescs.Add(fontCharDesc);
            }

            NumHorizontalBorderPixels = reader.ReadUInt32();
            NumVerticalBorderPixels = reader.ReadUInt32();
            BaselineOffset = reader.ReadUInt32();
            ForegroundSurfaceDataID = reader.ReadUInt32();
            BackgroundSurfaceDataID = reader.ReadUInt32();
        }
    }
}
