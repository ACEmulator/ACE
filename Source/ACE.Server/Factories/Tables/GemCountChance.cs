using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class GemCountChance
    {
        // gem code -> tier -> (count, chance)

        // gem code is (byte)(TsysMutationData >> 8)

        private static readonly Dictionary<byte, Dictionary<int, ChanceTable<int>>> gemCodes = new Dictionary<byte, Dictionary<int, ChanceTable<int>>>();

        static GemCountChance()
        {
            using (var ctx = new WorldDbContext())
            {
                var gemDists = ctx.TreasureGemCount.Where(i => i.Chance > 0);

                foreach (var gemDist in gemDists)
                {
                    if (!gemCodes.TryGetValue(gemDist.GemCode, out var tiers))
                    {
                        tiers = new Dictionary<int, ChanceTable<int>>();
                        gemCodes[gemDist.GemCode] = tiers;
                    }
                    if (!tiers.TryGetValue(gemDist.Tier, out var chances))
                    {
                        chances = new ChanceTable<int>();
                        tiers[gemDist.Tier] = chances;
                    }
                    chances.Add(gemDist.Count, gemDist.Chance);
                }
            }
        }

        private static ChanceTable<int> GetChanceTable(byte gemCode, int tier)
        {
            // temporary code
            if (tier > 6) tier = 6;

            if (gemCodes.TryGetValue(gemCode, out var tiers) && tiers.TryGetValue(tier, out var chanceTable))
                return chanceTable;
            else
                return null;
        }

        public static int Roll(byte gemCode, int tier)
        {
            var chanceTable = GetChanceTable(gemCode, tier);

            if (chanceTable != null)
                return chanceTable.Roll();
            else
                return 0;
        }
    }
}
