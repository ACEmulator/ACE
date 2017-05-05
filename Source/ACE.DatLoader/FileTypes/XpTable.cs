using ACE.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Reads and stores the XP Tables from the client_portal.dat (file 0x0E000018).
    /// </summary>
    public class XpTable
    {
        public LevelingChart LevelingXpChart;
        public ExperienceExpenditureChart AbilityXpChart;
        public ExperienceExpenditureChart VitalXpChart;
        public ExperienceExpenditureChart SpecializedSkillXpChart;
        public ExperienceExpenditureChart TrainedSkillXpChart;

        public static XpTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E000018))
            {
                return (XpTable)DatManager.PortalDat.FileCache[0x0E000018];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E000018);
                XpTable xp = new XpTable();

                datReader.Offset += 4; // Skip the ID. We know what it is.

                // The counts for each "Table" are at the top of the file.
                int abilityCount = datReader.ReadInt32();
                int vitalCount = datReader.ReadInt32();
                int trainedSkillCount = datReader.ReadInt32();
                int specializedSkillCount = datReader.ReadInt32();
                uint levelCount = datReader.ReadUInt32();

                xp.AbilityXpChart = ReadExperienceChart(abilityCount, ref datReader);
                xp.VitalXpChart = ReadExperienceChart(vitalCount, ref datReader);
                xp.TrainedSkillXpChart = ReadExperienceChart(trainedSkillCount, ref datReader);
                xp.SpecializedSkillXpChart = ReadExperienceChart(specializedSkillCount, ref datReader);

                // The level table is a little different since it has UInt64 data types.
                LevelingChart levelingXpChart = new LevelingChart();
                ulong prevRank = 0;
                datReader.Offset += 8; // skip level 0
                // Start from 1 because dat includes level 0.
                for (uint i = 1; i <= levelCount; i++)
                {
                    CharacterLevel characterLevel = new CharacterLevel();
                    characterLevel.Level = i;
                    characterLevel.TotalXp = datReader.ReadUInt64();
                    characterLevel.FromPreviousLevel = characterLevel.TotalXp - prevRank;
                    // Store this to use it in the next loop...
                    prevRank = characterLevel.TotalXp;
                    levelingXpChart.Levels.Add(characterLevel);
                }

                // The final section is skill credits... It has the same count as levels.
                int cumulativeSkillPoints = 0;
                datReader.Offset += 4; // skip level 0
                for (int i = 0; i < levelCount; i++)
                {
                    int skillPoint = datReader.ReadInt32();
                    cumulativeSkillPoints += skillPoint;
                    levelingXpChart.Levels[i].GrantsSkillPoint = skillPoint == 1;
                    levelingXpChart.Levels[i].CumulativeSkillPoints = cumulativeSkillPoints;
                }

                xp.LevelingXpChart = levelingXpChart;

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[0x0E000018] = xp;
                return xp;
            }
        }

        /// <summary>
        /// Reads the experience chart from the client_portal.dat. datReader is passed by reference to ensure we keep the proper offset.
        /// </summary>
        private static ExperienceExpenditureChart ReadExperienceChart(int rankCounts, ref DatReader datReader)
        {
            ExperienceExpenditureChart xpChart = new ExperienceExpenditureChart();
            uint prevRank = 0;
            // less than OR equals as the chart includes the zero rank, as well.
            for (int i = 0; i <= rankCounts; i++) 
            {
                ExperienceExpenditure rank = new ExperienceExpenditure();
                rank.Rank = i;
                rank.TotalXp = datReader.ReadUInt32();
                rank.XpFromPreviousRank = rank.TotalXp - prevRank;
                // Store this to use it in the next loop so we can calc the difference...
                prevRank = rank.TotalXp;
                xpChart.Ranks.Add(rank);
            }
            return xpChart;
        }
    }
}
