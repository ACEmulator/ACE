using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x12. 
    /// </summary>
    [DatFileType(DatFileType.Scene)]
    public class Scene : FileType
    {
        public List<ObjectDesc> Objects { get; } = new List<ObjectDesc>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Objects.Unpack(reader);
        }
    }
}
