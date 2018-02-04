using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is the client_portal.dat file 0x0E00001D
    /// </summary>
    [DatFileType(DatFileType.ContractTable)]
    public class ContractTable : IUnpackable
    {
        private const uint FILE_ID = 0x0E00001D;

        public Dictionary<uint, Contract> Contracts { get; } = new Dictionary<uint, Contract>();

        public void Unpack(BinaryReader reader)
        {
            reader.BaseStream.Position += 4; // Skip the ID. We know what it is.

            ushort num_contracts = reader.ReadUInt16();
            /*ushort table_size = */reader.ReadUInt16(); // We don't need this since C# handles it's own memory

            for (ushort i = 0; i < num_contracts; i++)
            {
                uint key = reader.ReadUInt32();

                Contract value = new Contract();
                value.Unpack(reader);

                Contracts.Add(key, value);
            }
        }

        public static ContractTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(FILE_ID))
                return (ContractTable)DatManager.PortalDat.FileCache[FILE_ID];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(FILE_ID);

            var obj = new ContractTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[FILE_ID] = obj;

            return obj;
        }
    }
}
