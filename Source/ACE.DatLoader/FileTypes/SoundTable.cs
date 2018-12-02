using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// SoundTable files contain a listing of which Wav types to play in response to certain events.
    /// They are located in the client_portal.dat and are files starting with 0x20
    /// </summary>
    [DatFileType(DatFileType.SoundTable)]
    public class SoundTable:FileType
    {
        public uint Unknown; // As the name implies, not sure what this is

        // Not quite sure what this is for, but it's the same in every file.
        public List<SoundTableData> SoundHash { get; } = new List<SoundTableData>();

        // The uint key corresponds to an Enum.Sound
        public Dictionary<uint, SoundData> Data { get; } = new Dictionary<uint, SoundData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Unknown = reader.ReadUInt32();
            SoundHash.Unpack(reader);
            Data.UnpackPackedHashTable(reader);
        }
    }
}
