using ACE.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.Database
{
    public class ChartDatabase : IChartDatabase
    {
        private string basePath = AppDomain.CurrentDomain.BaseDirectory;

        // cache block for these things - no need to keep reloading them
        private static Dictionary<string, ExperienceExpenditureChart> charts = new Dictionary<string, ExperienceExpenditureChart>();

        private static object chartMutex = new object();

        private LevelingChart levelingChart = null;

        public ChartDatabase() { }

        public ChartDatabase(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                throw new ArgumentException("Invalid base path for ChartDatabase");
            }

            this.basePath = basePath;
        }

        public ExperienceExpenditureChart GetAbilityXpChart()
        {
            return LoadRankChart(@".\abilityXp.json");
        }

        public LevelingChart GetLevelingXpChart()
        {
            return LoadLevelChartFromFile(@"levels.json");
        }
        
        public ExperienceExpenditureChart GetSpecializedSkillXpChart()
        {
            return LoadRankChart(@".\skillXp-Specialized.json");
        }

        public ExperienceExpenditureChart GetTrainedSkillXpChart()
        {
            return LoadRankChart(@".\skillXp-Trained.json");
        }

        public ExperienceExpenditureChart GetVitalXpChart()
        {
            return LoadRankChart(@".\vitalXp.json");
        }

        private LevelingChart LoadLevelChartFromFile(string filename)
        {
            if (levelingChart != null)
            {
                // check cache and short circuit
                return levelingChart;
            }

            lock (chartMutex)
            {
                if (levelingChart != null)
                {
                    // double check for initial race condition (check/lock/check pattern)
                    return levelingChart;
                }

                var fullPath = Path.Combine(this.basePath, filename);

                if (!File.Exists(fullPath))
                {
                    throw new ArgumentException("Invalid filename specified for loading leveling data.");
                }

                string contents = File.ReadAllText(fullPath);

                levelingChart = JsonConvert.DeserializeObject<LevelingChart>(contents);
            }

            return levelingChart;
        }

        private ExperienceExpenditureChart LoadRankChart(string filename)
        {
            var fullPath = Path.Combine(this.basePath, filename);

            if (charts.ContainsKey(fullPath))
            {
                // check cache and short circuit
                return charts[fullPath];
            }

            lock (chartMutex)
            {
                if (charts.ContainsKey(fullPath))
                {
                    // double check for initial race condition (check/lock/check pattern)
                    return charts[fullPath];
                }
                if (!File.Exists(fullPath))
                {
                    throw new ArgumentException("Invalid filename specified for loading xp rank data.");
                }

                string contents = File.ReadAllText(fullPath);

                charts.Add(fullPath, JsonConvert.DeserializeObject<ExperienceExpenditureChart>(contents));
            }

            return charts[fullPath];
        }
    }
}
