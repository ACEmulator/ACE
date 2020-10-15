using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Factories.Tables
{
    public static class CantripChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ChanceTable<int> T1_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.95f ),
            ( 1, 0.05f ),
        };

        private static ChanceTable<int> T2_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.90f ),
            ( 1, 0.10f ),
        };

        private static ChanceTable<int> T3_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.725f ),
            ( 1, 0.250f ),
            ( 2, 0.025f ),
        };

        private static ChanceTable<int> T4_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.62f ),
            ( 1, 0.32f ),
            ( 2, 0.055f ),
            ( 3, 0.005f ),
        };

        private static ChanceTable<int> T5_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.40f ),
            ( 1, 0.42f ),
            ( 2, 0.155f ),
            ( 3, 0.024f ),
            ( 4, 0.001f ),
        };

        private static ChanceTable<int> T6_NumCantrips = new ChanceTable<int>()
        {
            ( 0, 0.25f ),
            ( 1, 0.40f ),
            ( 2, 0.25f ),
            ( 3, 0.08f ),
            ( 4, 0.019f ),
            ( 5, 0.001f ),
        };

        private static ChanceTable<int> T7_T8_NumCantrips = new ChanceTable<int>()
        {
            ( 1, 0.81f ),
            ( 2, 0.17f ),
            ( 3, 0.016f ),
            ( 4, 0.004f ),
        };

        private static readonly List<ChanceTable<int>> _numCantrips = new List<ChanceTable<int>>()
        {
            T1_NumCantrips,
            T2_NumCantrips,
            T3_NumCantrips,
            T4_NumCantrips,
            T5_NumCantrips,
            T6_NumCantrips,
            T7_T8_NumCantrips,
            T7_T8_NumCantrips,
        };

        public static int RollNumCantrips(TreasureDeath profile)
        {
            return numCantrips[profile.Tier - 1].Roll(profile.LootQualityMod);
        }


        private static ChanceTable<int> T1_T2_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 1.0f )
        };

        private static ChanceTable<int> T3_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.97f ),
            ( 2, 0.03f ),
        };

        private static ChanceTable<int> T4_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.90f ),
            ( 2, 0.10f ),
        };

        private static ChanceTable<int> T5_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.85f ),
            ( 2, 0.15f ),
        };

        private static ChanceTable<int> T6_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.80f ),
            ( 2, 0.20f ),
        };

        private static ChanceTable<int> T7_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.15f ),
            ( 2, 0.60f ),
            ( 3, 0.25f )
        };

        private static ChanceTable<int> T8_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 0.02f ),
            ( 2, 0.46f ),
            ( 3, 0.42f ),
            ( 4, 0.10f )
        };

        private static readonly List<ChanceTable<int>> _cantripLevels = new List<ChanceTable<int>>()
        {
            T1_T2_CantripLevel,
            T1_T2_CantripLevel,
            T3_CantripLevel,
            T4_CantripLevel,
            T5_CantripLevel,
            T6_CantripLevel,
            T7_CantripLevel,
            T8_CantripLevel,
        };

        public static int RollCantripLevel(TreasureDeath profile)
        {
            return cantripLevels[profile.Tier - 1].Roll(profile.LootQualityMod);
        }


        private static List<ChanceTable<int>> numCantrips = _numCantrips;

        public static void ApplyNumCantripsMod(bool showResults = true)
        {
            // scales NumCantrips, no chance vs. chance, relative to each other
            var cantrip_drop_rate = (float)Math.Max(0.0f, PropertyManager.GetDouble("cantrip_drop_rate").Item);

            if (cantrip_drop_rate != 1.0f)
            {
                var newTable = new List<ChanceTable<int>>();

                foreach (var entry in _numCantrips)
                {
                    var newEntry = ScaleNumCantrips(entry, cantrip_drop_rate);
                    newTable.Add(newEntry);
                }
                numCantrips = newTable;
            }
            else
                numCantrips = _numCantrips;

            if (showResults)
            {
                log.Info($"ApplyNumCantripsMod({cantrip_drop_rate})");
                log.Info("");

                ShowTables(numCantrips);
            }
        }

        public static ChanceTable<int> ScaleNumCantrips(ChanceTable<int> numCantrips, float cantrip_drop_rate)
        {
            var newTable = new ChanceTable<int>();

            foreach (var entry in numCantrips)
            {
                if (entry.result != 0)
                    newTable.Add((entry.result, entry.chance * cantrip_drop_rate));
            }

            var totalChance = newTable.Sum(i => i.chance);

            if (totalChance > 1.0f)
            {
                newTable = Rescale(newTable);
                totalChance = 1.0f;
            }

            var finalTable = new ChanceTable<int>();
            finalTable.Add((0, 1.0f - totalChance));
            finalTable.AddRange(newTable);

            return finalTable;
        }

        private static List<ChanceTable<int>> cantripLevels = _cantripLevels;

        public static void ApplyCantripLevelsMod(bool showResults = true)
        {
            // scales CantripLevels, relative to each other
            var minor_cantrip_drop_rate = (float)Math.Max(0.0f, PropertyManager.GetDouble("minor_cantrip_drop_rate").Item);
            var major_cantrip_drop_rate = (float)Math.Max(0.0f, PropertyManager.GetDouble("major_cantrip_drop_rate").Item);
            var epic_cantrip_drop_rate = (float)Math.Max(0.0f, PropertyManager.GetDouble("epic_cantrip_drop_rate").Item);
            var legendary_cantrip_drop_rate = (float)Math.Max(0.0f, PropertyManager.GetDouble("legendary_cantrip_drop_rate").Item);

            if (minor_cantrip_drop_rate != 1.0f || major_cantrip_drop_rate != 1.0f || epic_cantrip_drop_rate != 1.0f || legendary_cantrip_drop_rate != 1.0f)
            {
                var newTable = new List<ChanceTable<int>>();

                foreach (var entry in _cantripLevels)
                {
                    var newEntry = ScaleCantripLevels(entry, minor_cantrip_drop_rate, major_cantrip_drop_rate, epic_cantrip_drop_rate, legendary_cantrip_drop_rate);
                    newTable.Add(newEntry);
                }
                cantripLevels = newTable;
            }
            else
                cantripLevels = _cantripLevels;

            if (showResults)
            {
                log.Info($"ApplyCantripLevelsMod({minor_cantrip_drop_rate}, {major_cantrip_drop_rate}, {epic_cantrip_drop_rate}, {legendary_cantrip_drop_rate})");
                log.Info("");

                ShowTables(cantripLevels);
            }
        }

        public static ChanceTable<int> ScaleCantripLevels(ChanceTable<int> cantripLevel, float minor_cantrip_drop_rate, float major_cantrip_drop_rate, float epic_cantrip_drop_rate, float legendary_cantrip_drop_rate)
        {
            var newTable = new ChanceTable<int>();

            foreach (var entry in cantripLevel)
            {
                var modifier = 1.0f;

                switch (entry.result)
                {
                    case 1:
                        modifier = minor_cantrip_drop_rate;
                        break;
                    case 2:
                        modifier = major_cantrip_drop_rate;
                        break;
                    case 3:
                        modifier = epic_cantrip_drop_rate;
                        break;
                    case 4:
                        modifier = legendary_cantrip_drop_rate;
                        break;
                }
                newTable.Add((entry.result, entry.chance * modifier));
            }

            return Rescale(newTable);
        }

        public static ChanceTable<int> Rescale(ChanceTable<int> table, float target = 1.0f)
        {
            var total = table.Sum(i => i.chance);

            if (total == target)
                return table;

            // get scalar
            var scalar = target / total;

            var rescaled = new ChanceTable<int>();

            foreach (var entry in table)
                rescaled.Add((entry.result, entry.chance * scalar));

            return rescaled;
        }

        static CantripChance()
        {
            ApplyNumCantripsMod(false);
            ApplyCantripLevelsMod(false);
        }

        private static void ShowTables(List<ChanceTable<int>> tables)
        {
            for (var i = 0; i < tables.Count; i++)
            {
                var table = tables[i];

                log.Info($"Tier {i + 1}:");
                log.Info("");

                foreach (var entry in table)
                    log.Info($"{entry.result}: {GetPercent(entry.chance)}");

                log.Info("");
            }
        }

        private static string GetPercent(float pct)
        {
            return $"{Math.Round(pct * 100, 2, MidpointRounding.AwayFromZero)}%";
        }
    }
}
