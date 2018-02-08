using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Reads and stores the XP Tables from the client_portal.dat (file 0x0E000018).
    /// </summary>
    [DatFileType(DatFileType.XpTable)]
    public class XpTable : IUnpackable
    {
        private const uint FILE_ID = 0x0E000018;

        public List<uint> AbilityXpList { get; } = new List<uint>();
        public List<uint> VitalXpList { get; } = new List<uint>();
        public List<uint> TrainedSkillXpList { get; } = new List<uint>();
        public List<uint> SpecializedSkillXpList { get; } = new List<uint>();

        /// <summary>
        /// The XP needed to reach each level
        /// </summary>
        public List<ulong> CharacterLevelXPList { get; } = new List<ulong>();

        /// <summary>
        /// Number of credits gifted at each level
        /// </summary>
        public List<uint> CharacterLevelSkillCreditList { get; } = new List<uint>();

        public void Unpack(BinaryReader reader)
        {
            reader.BaseStream.Position += 4; // Skip the ID. We know what it is.

            // The counts for each "Table" are at the top of the file.
            int abilityCount            = reader.ReadInt32();
            int vitalCount              = reader.ReadInt32();
            int trainedSkillCount       = reader.ReadInt32();
            int specializedSkillCount   = reader.ReadInt32();

            uint levelCount             = reader.ReadUInt32();

            for (int i = 0; i <= abilityCount; i++)
                AbilityXpList.Add(reader.ReadUInt32());

            for (int i = 0; i <= vitalCount; i++)
                VitalXpList.Add(reader.ReadUInt32());

            for (int i = 0; i <= trainedSkillCount; i++)
                TrainedSkillXpList.Add(reader.ReadUInt32());

            for (int i = 0; i <= specializedSkillCount; i++)
                SpecializedSkillXpList.Add(reader.ReadUInt32());

            for (int i = 0; i <= levelCount; i++)
                CharacterLevelXPList.Add(reader.ReadUInt64());

            for (int i = 0; i <= levelCount; i++)
                CharacterLevelSkillCreditList.Add(reader.ReadUInt32());
        }

        public static XpTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(FILE_ID, out var result))
                return (XpTable)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(FILE_ID);

            var obj = new XpTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[FILE_ID] = obj;

            return obj;
        }
    }
}
