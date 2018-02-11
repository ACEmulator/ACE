using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x33. 
    /// </summary>
    [DatFileType(DatFileType.PhysicsScript)]
    public class PhysicsScript : FileType
    {
        public List<PhysicsScriptData> ScriptData { get; } = new List<PhysicsScriptData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ScriptData.Unpack(reader);
        }
    }
}
