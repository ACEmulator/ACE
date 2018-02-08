using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0D. 
    /// These are basically pre-fab regions for things like the interior of a dungeon.
    /// </summary>
    [DatFileType(DatFileType.Environment)]
    public class Environment : FileType
    {
        public Dictionary<uint, CellStruct> Cells { get; set; } = new Dictionary<uint, CellStruct>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32(); // this will match fileId

            Cells.Unpack(reader);
        }
    }
}
