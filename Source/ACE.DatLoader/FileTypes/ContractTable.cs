using ACE.DatLoader.Entity;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This is the client_portal.dat file 0x0E00001D
    /// </summary>
    public class ContractTable
    {
        private const uint CONTRACT_TABLE_ID = 0x0E00001D;

        public uint Id { get; set; } // This should match CONTRACT_TABLE_ID
        public Dictionary<uint, Contract> Contracts { get; set; } = new Dictionary<uint, Contract>(); 

        public static ContractTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(CONTRACT_TABLE_ID))
            {
                return (ContractTable)DatManager.PortalDat.FileCache[CONTRACT_TABLE_ID];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(CONTRACT_TABLE_ID);
                ContractTable obj = new ContractTable();

                obj.Id = datReader.ReadUInt32();

                ushort num_contracts = datReader.ReadUInt16();
                ushort table_size = datReader.ReadUInt16(); // We don't need this since C# handles it's own memory
                for (ushort i = 0; i < num_contracts; i++)
                {
                    uint key = datReader.ReadUInt32();
                    Contract value = Contract.Read(datReader);
                    obj.Contracts.Add(key, value);
                }

                DatManager.PortalDat.FileCache[CONTRACT_TABLE_ID] = obj;
                return obj;
            }
        }
    }
}