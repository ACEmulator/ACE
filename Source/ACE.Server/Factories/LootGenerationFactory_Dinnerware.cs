using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
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

        private static readonly Dictionary<WeenieClassName, int> damageTable = new Dictionary<WeenieClassName, int>()
        {
            { WeenieClassName.cup,            8  },
            { WeenieClassName.chalice,        10 },
            { WeenieClassName.ewer,           10 },
            { WeenieClassName.mug,            10 },
            { WeenieClassName.flagon,         12 },
            { WeenieClassName.goblet,         14 },
            { WeenieClassName.tankard,        14 },
            { WeenieClassName.bowl,           18 },
            { WeenieClassName.dinnerplate,    20 },
            { WeenieClassName.ornamentalbowl, 20 },
            { WeenieClassName.stoup,          22 },
        };

        private static void MutateDinnerware(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll = null)
        {
            // material type
            wo.MaterialType = (MaterialType)GetMaterialType(wo, profile.Tier);

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

            // "mutate" damage - these were always the same values
            // it would almost make sense to just put these on the base weenies,
            // however if they appeared in vendor shops w/out these values, maybe loot system added them?

            // "Empty Flask" is the only Dinnerware to not get these mutations

            var wcid = (WeenieClassName)wo.WeenieClassId;

            if (wcid != WeenieClassName.flasksimple)
            {
                if (damageTable.TryGetValue(wcid, out var damage))
                    wo.Damage = damage;

                if (wcid != WeenieClassName.dinnerplate)
                    wo.W_DamageType = DamageType.Bludgeon;
                else
                    wo.W_DamageType = DamageType.Slash;

                wo.DamageVariance = 0.25f;
                wo.WeaponTime = 10;

                // spells
                if (isMagical)
                    AssignMagic(wo, profile, roll);
            }

            // item value
            MutateDinnerware_ItemValue(wo);

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

