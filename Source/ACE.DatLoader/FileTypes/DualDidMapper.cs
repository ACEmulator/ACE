using System.Collections.Generic;
using System.IO;

using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// EnumMapper files are 0x27 in the client_portal.dat
    /// They contain a list of Weenie IDs and their W_Class. The client uses these for items such as tracking spell components (to know if the player has all required to cast a spell).
    ///
    /// A description of each DualDidMapper is in DidMapper entry 0x25000005 (WEENIE_CATEGORIES)
    /// 27000000 - Materials
    /// 27000001 - Gems
    /// 27000002 - SpellComponents
    /// 27000003 - ComponentPacks
    /// 27000004 - TradeNotes
    /// </summary>
    [DatFileType(DatFileType.DualDidMapper)]
    public class DualDidMapper : FileType
    {
        // Thanks to OptimShi for decoding these structures!
        // The client/server designation is guessed based on the content in each list.

        // The keys in these two Dictionaries are common. So ClientEnumToId[key] = ClientEnumToName[key].
        public NumberingType ClientIDNumberingType;   // bitfield designating how the numbering is organized. Not really needed for our usage.
        public Dictionary<uint, uint> ClientEnumToID = new Dictionary<uint, uint>();       // m_EnumToID

        public NumberingType ClientNameNumberingType; // bitfield designating how the numbering is organized. Not really needed for our usage.
        public Dictionary<uint, string> ClientEnumToName = new Dictionary<uint, string>(); // m_EnumToName

        // The keys in these two Dictionaries are common. So ServerEnumToId[key] = ServerEnumToName[key].
        public NumberingType ServerIDNumberingType; // bitfield designating how the numbering is organized. Not really needed for our usage.
        public Dictionary<uint, uint> ServerEnumToID = new Dictionary<uint, uint>();       // m_EnumToID

        public NumberingType ServerNameNumberingType; // bitfield designating how the numbering is organized. Not really needed for our usage.
        public Dictionary<uint, string> ServerEnumToName = new Dictionary<uint, string>(); // m_EnumToName

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ClientIDNumberingType = (NumberingType)reader.ReadByte();
            uint numClientEnumToIDs = reader.ReadCompressedUInt32();
            for (var i = 0; i < numClientEnumToIDs; i++)
                ClientEnumToID.Add(reader.ReadUInt32(), reader.ReadUInt32());

            ClientNameNumberingType = (NumberingType)reader.ReadByte();
            uint numClientEnumToNames = reader.ReadCompressedUInt32();
            for (var i = 0; i < numClientEnumToNames; i++)
                ClientEnumToName.Add(reader.ReadUInt32(), reader.ReadPString(1));

            ServerIDNumberingType = (NumberingType)reader.ReadByte();
            uint numServerEnumToIDs = reader.ReadCompressedUInt32();
            for (var i = 0; i < numServerEnumToIDs; i++)
                ServerEnumToID.Add(reader.ReadUInt32(), reader.ReadUInt32());

            ServerNameNumberingType = (NumberingType)reader.ReadByte();
            uint numServerEnumToNames = reader.ReadCompressedUInt32();
            for (var i = 0; i < numServerEnumToNames; i++)
                ServerEnumToName.Add(reader.ReadUInt32(), reader.ReadPString(1));
        }
    }
}
