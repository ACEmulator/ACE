using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Database.Models.World;

namespace ACE.Server.Factories.Tables
{
    public static class CasterItemMaxMana
    {
        // using a lerp table similar to some QualityChance rolls,
        // however this was definitely not a quality chance based on tier,
        // as it scaled evenly based on itemMaxMana between 6-75, regardless of tier

        // also verified the ones on the end were not outliers, ie. appeared to be regular lootgen items

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<float> chanceTable = new List<float>()
        {
            0.100f,
            0.300f,
            0.550f,
            0.800f,
            0.900f,
            0.950f,
            0.970f,
            0.990f,
            0.995f,
            1.000f,
        };

        public static float Roll(TreasureDeath profile)
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            for (var i = 0; i < chanceTable.Count; i++)
            {
                var curChance = chanceTable[i];

                if (rng < curChance && curChance >= profile.LootQualityMod)
                {
                    var prevChance = i > 0 ? chanceTable[i - 1] : 0;

                    var dx = curChance - prevChance;
                    var dy = 1.0f / chanceTable.Count;

                    var interval = (rng - prevChance) / dx;

                    return (float)(dy * (interval + i));
                }
            }
            log.Error($"CasterItemMaxMana.Roll({profile.LootQualityMod}) - this shouldn't happen");
            return 0.0f;
        }
    }
}
