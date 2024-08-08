using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class InputMapConflictsValue : IUnpackable
    {
        public uint InputMap { get; private set; }
        public List<uint> ConflictingInputMaps { get; private set; } = new List<uint>();

        public void Unpack(BinaryReader reader)
        {
            InputMap = reader.ReadUInt32();
            ConflictingInputMaps.Unpack(reader);
        }
    }
}
