using System.Collections.Generic;

using ACE.Database.Models.World;

using ACE.Server.Factories.Tables;
using ACE.Server.Factories.Tables.Wcids;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateAetheria(TreasureDeath profile, bool mutate = true)
        {
            var wcid = AetheriaWcids.Roll(profile.Tier);

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)wcid);

            if (mutate)
                MutateAetheria(wo, profile);

            return wo;
        }

        private static void MutateAetheria(WorldObject wo, TreasureDeath profile)
        {
            wo.ItemMaxLevel = AetheriaChance.Roll_ItemMaxLevel(profile);

            wo.IconOverlayId = IconOverlay_ItemMaxLevel[wo.ItemMaxLevel.Value - 1];
        }

        private static WorldObject CreateCoalescedMana(TreasureDeath profile)
        {
            var wcid = CoalescedManaWcids.Roll(profile);

            return WorldObjectFactory.CreateNewWorldObject((uint)wcid);
        }

        public static List<uint> IconOverlay_ItemMaxLevel { get; } = new List<uint>()
        {
            0x6006C34,  // 1
            0x6006C35,  // 2
            0x6006C36,  // 3
            0x6006C37,  // 4
            0x6006C38,  // 5
        };
    }
}
