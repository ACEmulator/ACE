using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x04. 
    /// </summary>
    [DatFileType(DatFileType.Palette)]
    public class Palette : FileType
    {
        /// <summary>
        /// Color data is stored in ARGB format
        /// </summary>
        public List<uint> Colors { get; } = new List<uint>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Colors.Unpack(reader);
        }
    }
}
