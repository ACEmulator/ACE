using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x34. 
    /// </summary>
    [DatFileType(DatFileType.PhysicsScriptTable)]
    public class PhysicsScriptTable : FileType
    {
        public Dictionary<uint, PhysicsScriptTableData> ScriptTable { get; set; } = new Dictionary<uint, PhysicsScriptTableData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ScriptTable.Unpack(reader);
        }
    }
}
