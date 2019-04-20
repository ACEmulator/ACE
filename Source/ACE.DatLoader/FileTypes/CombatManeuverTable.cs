using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x30. 
    /// </summary>
    [DatFileType(DatFileType.CombatTable)]
    public class CombatManeuverTable : FileType
    {
        public List<CombatManeuver> CMT { get; } = new List<CombatManeuver>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32(); // This should always equal the fileId

            CMT.Unpack(reader);
        }
    }
}
