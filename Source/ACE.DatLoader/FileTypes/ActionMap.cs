using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using static System.Net.Mime.MediaTypeNames;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is the client_portal.dat file 0x26000000; It's related to keyboard configuration.
    /// </summary>
    [DatFileType(DatFileType.ActionMap)]
    public class ActionMap : FileType
    {
        public Dictionary<uint, Dictionary<uint, ActionMapValue>> InputMaps { get; } = new Dictionary<uint, Dictionary<uint, ActionMapValue>>();
        public uint StringTable { get; set; } // DID Value to lookup all the hashes in -- will be 0x23000005
        public Dictionary<uint, InputMapConflictsValue> ConflictingMaps { get; } = new Dictionary<uint, InputMapConflictsValue>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            reader.ReadByte(); // bucket/size
            byte count = reader.ReadByte();
            for (var i = 0; i < count; i++)
            {
                var key = reader.ReadUInt32();
                reader.ReadByte(); // bucket/size
                byte valuesCount = reader.ReadByte();
                Dictionary<uint, ActionMapValue> values = new Dictionary<uint, ActionMapValue>();
                for (var j = 0; j < valuesCount; j++)
                {
                    uint valuesKey = reader.ReadUInt32();
                    ActionMapValue value = new ActionMapValue();
                    value.Unpack(reader);
                    values.Add(valuesKey, value);
                }
                InputMaps.Add(key, values);
            }

            StringTable = reader.ReadUInt32(); // Will be 0x23000005

            reader.ReadByte(); // bucket/size
            byte conflictCount = reader.ReadByte();
            for (var i = 0; i < conflictCount; i++) {
                var key = reader.ReadUInt32();
                InputMapConflictsValue conflictValue = new InputMapConflictsValue();
                conflictValue.Unpack(reader);
                ConflictingMaps.Add(key, conflictValue);
            }
        }

    
    }
}
