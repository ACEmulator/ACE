using System;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateDinnerware(TreasureDeath profile, bool isMagical, bool mutate = true)
        {
            var rng = ThreadSafeRandom.Next(0, LootTables.DinnerwareLootMatrix.Length - 1);

            var wcid = (uint)LootTables.DinnerwareLootMatrix[rng];

            var wo = WorldObjectFactory.CreateNewWorldObject(wcid);

            if (wo != null && mutate)
                MutateDinnerware(wo, profile, isMagical);

            return wo;
        }

        private static void MutateDinnerware(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll = null)
        {
            // dinnerware did not have its Damage / DamageVariance / WeaponSpeed mutated

            // material type
            wo.MaterialType = GetMaterialType(wo, profile.Tier);

            // item color
            MutateColor(wo);

            // gem count / gem material
            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 5);

            wo.GemType = RollGemType(profile.Tier);

            // workmanship
            wo.ItemWorkmanship = WorkmanshipChance.Roll(profile.Tier);

            // "Empty Flask" was the only dinnerware that never received spells
            if (isMagical && wo.WeenieClassId != (uint)WeenieClassName.flasksimple)
                AssignMagic(wo, profile, roll);

            // item value
            if (wo.HasMutateFilter(MutateFilter.Value))
                MutateValue(wo, profile.Tier, roll);

            // long desc
            wo.LongDesc = GetLongDesc(wo);
        }

        private static void MutateDinnerware_ItemValue(WorldObject wo)
        {
            var materialMod = LootTables.getMaterialValueModifier(wo);
            var gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);

            var baseValue = ThreadSafeRandom.Next(300, 600);

            var workmanship = wo.ItemWorkmanship ?? 1;

            wo.Value = (int)(baseValue * gemMaterialMod * materialMod * workmanship);
        }

        private static bool GetMutateDinnerwareData(uint wcid)
        {
            return LootTables.DinnerwareLootMatrix.Contains((int)wcid);
        }
    }
}

