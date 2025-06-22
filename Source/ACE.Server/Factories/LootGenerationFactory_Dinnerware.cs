using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.Factories.Tables.Wcids;
using ACE.Server.WorldObjects;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// This is only called by /testlootgen command
        /// The actual lootgen system doesn't use this.
        /// </summary>
        private static WorldObject CreateDinnerware(TreasureDeath profile, bool isMagical)
        {
            var treasureRoll = new TreasureRoll(TreasureItemType.ArtObject);
            treasureRoll.Wcid = GenericWcids.Roll(profile.Tier);

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)treasureRoll.Wcid);
            MutateDinnerware(wo, profile, isMagical, treasureRoll);

            return wo;
        }

        private static void MutateDinnerware(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll)
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
    }
}

