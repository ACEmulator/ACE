using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.Factories.Tables.Wcids;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateAetheria(int tier, bool mutate = true)
        {
            int chance;
            uint aetheriaType;

            if (tier < 5) return null;

            // TODO: drop percentage tweaks between types within a given tier, if needed
            switch (tier)
            {
                case 5:
                    aetheriaType = Aetheria.AetheriaBlue;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1, 10);  // Example 50/50 split between color type
                    if (chance <= 5)
                        aetheriaType = Aetheria.AetheriaBlue;
                    else
                        aetheriaType = Aetheria.AetheriaYellow;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(1, 9); // Example 33% between color type
                    if (chance <= 3)
                        aetheriaType = Aetheria.AetheriaBlue;
                    else if (chance <= 6)
                        aetheriaType = Aetheria.AetheriaYellow;
                    else
                        aetheriaType = Aetheria.AetheriaRed;
                    break;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(aetheriaType) as Gem;

            if (wo != null && mutate)
                MutateAetheria(wo, tier);

            return wo;
        }

        public static readonly List<uint> IconOverlay_ItemMaxLevel = new List<uint>()
        {
            0x6006C34,  // 1
            0x6006C35,  // 2
            0x6006C36,  // 3
            0x6006C37,  // 4
            0x6006C38,  // 5
        };

        private static void MutateAetheria(WorldObject wo, int tier)
        {
            // Initial roll for an Aetheria level 1 through 3
            wo.ItemMaxLevel = 1;

            var rng = ThreadSafeRandom.Next(1, 7);

            if (rng > 4)
            {
                if (rng > 6)
                    wo.ItemMaxLevel = 3;
                else
                    wo.ItemMaxLevel = 2;
            }

            // Perform an additional roll check for a chance at a higher Aetheria level for tiers 6+
            if (tier > 5)
            {
                if (ThreadSafeRandom.Next(1, 50) == 1)
                {
                    wo.ItemMaxLevel = 4;
                    if (tier > 6 && ThreadSafeRandom.Next(1, 5) == 1)
                    {
                        wo.ItemMaxLevel = 5;
                    }
                }
            }
            wo.IconOverlayId = IconOverlay_ItemMaxLevel[wo.ItemMaxLevel.Value - 1];
        }

        private static WorldObject CreateCoalescedMana(TreasureDeath profile)
        {
            var wcid = CoalescedManaWcids.Roll(profile);

            return WorldObjectFactory.CreateNewWorldObject((uint)wcid);
        }


        private static WorldObject CreateAetheria_New(TreasureDeath profile, bool mutate = true)
        {
            var wcid = AetheriaWcids.Roll(profile.Tier);

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)wcid);

            if (mutate)
                MutateAetheria_New(wo, profile);

            return wo;
        }

        private static void MutateAetheria_New(WorldObject wo, TreasureDeath profile)
        {
            wo.ItemMaxLevel = AetheriaChance.Roll_ItemMaxLevel(profile);

            wo.IconOverlayId = IconOverlay_ItemMaxLevel[wo.ItemMaxLevel.Value - 1];
        }

        private static bool GetMutateAetheriaData(uint wcid)
        {
            return LootTables.AetheriaWcids.Contains(wcid);
        }
    }
}
