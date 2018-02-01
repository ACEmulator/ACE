using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Reads and stores the XP Tables from the client_portal.dat (file 0x0E000018).
    /// </summary>
    public class XpTable : IUnpackable
    {
        public ExperienceExpenditureChart AbilityXpChart { get; } = new ExperienceExpenditureChart();
        public ExperienceExpenditureChart VitalXpChart { get; } = new ExperienceExpenditureChart();
        public ExperienceExpenditureChart TrainedSkillXpChart { get; } = new ExperienceExpenditureChart();
        public ExperienceExpenditureChart SpecializedSkillXpChart { get; } = new ExperienceExpenditureChart();
        public LevelingChart LevelingXpChart { get; } = new LevelingChart();

        public void Unpack(BinaryReader reader)
        {
            reader.BaseStream.Position += 4; // Skip the ID. We know what it is.

            // The counts for each "Table" are at the top of the file.
            int abilityCount            = reader.ReadInt32();
            int vitalCount              = reader.ReadInt32();
            int trainedSkillCount       = reader.ReadInt32();
            int specializedSkillCount   = reader.ReadInt32();
            uint levelCount             = reader.ReadUInt32();

            ReadExperienceChart(AbilityXpChart, abilityCount, reader);
            ReadExperienceChart(VitalXpChart, vitalCount, reader);
            ReadExperienceChart(TrainedSkillXpChart, trainedSkillCount, reader);
            ReadExperienceChart(SpecializedSkillXpChart, specializedSkillCount, reader);

            // The level table is a little different since it has UInt64 data types.
            ulong prevRank = 0;
            reader.BaseStream.Position += 8; // skip level 0

            // Start from 1 because dat includes level 0.
            for (uint i = 1; i <= levelCount; i++)
            {
                CharacterLevel characterLevel = new CharacterLevel();
                characterLevel.Level = i;
                characterLevel.TotalXp = reader.ReadUInt64();
                characterLevel.FromPreviousLevel = characterLevel.TotalXp - prevRank;
                // Store this to use it in the next loop...
                prevRank = characterLevel.TotalXp;
                LevelingXpChart.Levels.Add(characterLevel);
            }

            // The final section is skill credits... It has the same count as levels.
            int cumulativeSkillPoints = 0;
            reader.BaseStream.Position += 4; // skip level 0
            for (int i = 0; i < levelCount; i++)
            {
                int skillPoint = reader.ReadInt32();
                cumulativeSkillPoints += skillPoint;
                LevelingXpChart.Levels[i].GrantsSkillPoint = skillPoint == 1;
                LevelingXpChart.Levels[i].CumulativeSkillPoints = cumulativeSkillPoints;
            }
        }

        public static XpTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E000018))
                return (XpTable)DatManager.PortalDat.FileCache[0x0E000018];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E000018);

            XpTable xpTable = new XpTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                xpTable.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[0x0E000018] = xpTable;

            return xpTable;
        }

        /// <summary>
        /// Reads the experience chart from the client_portal.dat. datReader is passed by reference to ensure we keep the proper offset.
        /// </summary>
        private static void ReadExperienceChart(ExperienceExpenditureChart xpChart, int rankCounts, BinaryReader reader)
        {
            uint prevRank = 0;

            // less than OR equals as the chart includes the zero rank, as well.
            for (int i = 0; i <= rankCounts; i++) 
            {
                ExperienceExpenditure rank = new ExperienceExpenditure();
                rank.Rank = i;
                rank.TotalXp = reader.ReadUInt32();
                rank.XpFromPreviousRank = rank.TotalXp - prevRank;
                // Store this to use it in the next loop so we can calc the difference...
                prevRank = rank.TotalXp;
                xpChart.Ranks.Add(rank);
            }
        }
    }
}
