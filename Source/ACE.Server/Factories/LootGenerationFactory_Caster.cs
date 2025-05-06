using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity.Mutations;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.Factories.Tables.Wcids;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        public static WorldObject CreateCaster(TreasureDeath profile, bool isMagical)
        {
            // this function is only used by test methods, and is not part of regular lootgen
            var treasureRoll = new TreasureRoll(TreasureItemType.Caster);
            treasureRoll.WeaponType = TreasureWeaponType.Caster;
            treasureRoll.Wcid = CasterWcids.Roll(profile.Tier);

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)treasureRoll.Wcid);
            MutateCaster(wo, profile, isMagical, treasureRoll);

            return wo;
        }

        private static void MutateCaster(WorldObject wo, TreasureDeath profile, bool isMagical, TreasureRoll roll)
        {
            // mutate ManaConversionMod
            var mutationFilter = MutationCache.GetMutation("Casters.caster.txt");
            mutationFilter.TryMutate(wo, profile.Tier);

            // mutate ElementalDamageMod / WieldRequirements
            var isElemental = wo.W_DamageType != DamageType.Undef;
            var scriptName = GetCasterScript(isElemental);

            mutationFilter = MutationCache.GetMutation(scriptName);
            mutationFilter.TryMutate(wo, profile.Tier);

            // this part was not handled by mutation filter
            if (wo.WieldRequirements == WieldRequirement.RawSkill)
            {
                if (wo.W_DamageType == DamageType.Nether)
                    wo.WieldSkillType = (int)Skill.VoidMagic;
                else
                    wo.WieldSkillType = (int)Skill.WarMagic;
            }

            // mutate WeaponDefense
            mutationFilter = MutationCache.GetMutation("Casters.weapon_defense.txt");
            mutationFilter.TryMutate(wo, profile.Tier);

            // material type
            var materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = materialType;

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

            // burden?

            // missile defense / magic defense
            wo.WeaponMissileDefense = MissileMagicDefense.Roll(profile.Tier);
            wo.WeaponMagicDefense = MissileMagicDefense.Roll(profile.Tier);

            // spells
            if (!isMagical)
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
            }
            else
            {
                // if a caster was from a MagicItem profile, it always had a SpellDID
                MutateCaster_SpellDID(wo, profile);

                AssignMagic(wo, profile, roll);
            }

            // item value
            //if (wo.HasMutateFilter(MutateFilter.Value))   // fixme: data
                MutateValue(wo, profile.Tier, roll);

            // long description
            wo.LongDesc = GetLongDesc(wo);
        }

        private static void MutateCaster_SpellDID(WorldObject wo, TreasureDeath profile)
        {
            var firstSpell = CasterSlotSpells.Roll(wo);

            var spellLevels = SpellLevelProgression.GetSpellLevels(firstSpell);

            if (spellLevels == null)
            {
                log.Error($"MutateCaster_SpellDID: couldn't find {firstSpell}");
                return;
            }

            if (spellLevels.Count != 8)
            {
                log.Error($"MutateCaster_SpellDID: found {spellLevels.Count} spell levels for {firstSpell}, expected 8");
                return;
            }

            var spellLevel = SpellLevelChance.Roll(profile.Tier);

            wo.SpellDID = (uint)spellLevels[spellLevel - 1];

            var spell = new Server.Entity.Spell(wo.SpellDID.Value);

            var castableMod = CasterSlotSpells.IsOrb(wo) ? 5.0f : 2.5f;

            wo.ItemManaCost = (int)(spell.BaseMana * castableMod);

            wo.ItemUseable = Usable.SourceWieldedTargetRemoteNeverWalk;
        }

        private static string GetCasterScript(bool isElemental = false)
        {
            var elementalStr = isElemental ? "elemental" : "non_elemental";

            return $"Casters.caster_{elementalStr}.txt";
        }
    }
}
