using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class GemSpells
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // all attribute / skill buffs
        private static readonly List<SpellId> creatureSpells = new List<SpellId>()
        {
            SpellId.StrengthSelf1,
            SpellId.EnduranceSelf1,
            SpellId.CoordinationSelf1,
            SpellId.QuicknessSelf1,
            SpellId.FocusSelf1,
            SpellId.WillpowerSelf1,

            SpellId.HeavyWeaponsMasterySelf1,
            SpellId.LightWeaponsMasterySelf1,
            SpellId.FinesseWeaponsMasterySelf1,
            SpellId.MissileWeaponsMasterySelf1,
            SpellId.TwoHandedMasterySelf1,

            SpellId.InvulnerabilitySelf1,
            SpellId.ImpregnabilitySelf1,
            SpellId.MagicResistanceSelf1,

            SpellId.CreatureEnchantmentMasterySelf1,
            SpellId.ItemEnchantmentMasterySelf1,
            SpellId.LifeMagicMasterySelf1,
            SpellId.WarMagicMasterySelf1,
            SpellId.VoidMagicMasterySelf1,
            SpellId.SummoningMasterySelf1,

            SpellId.ArcaneEnlightenmentSelf1,
            SpellId.DeceptionMasterySelf1,
            SpellId.HealingMasterySelf1,
            SpellId.JumpingMasterySelf1,
            SpellId.LockpickMasterySelf1,
            SpellId.ManaMasterySelf1,
            SpellId.SprintSelf1,

            SpellId.DualWieldMasterySelf1,
            SpellId.DirtyFightingMasterySelf1,
            SpellId.RecklessnessMasterySelf1,
            SpellId.SneakAttackMasterySelf1,

            SpellId.ShieldMasterySelf1,

            SpellId.AlchemyMasterySelf1,
            SpellId.CookingMasterySelf1,
            SpellId.FletchingMasterySelf1,

            SpellId.LeadershipMasterySelf1,
            SpellId.FealtySelf1,

            SpellId.ArcanumSalvagingSelf1,
            SpellId.ArmorExpertiseSelf1,
            SpellId.ItemExpertiseSelf1,
            SpellId.MagicItemExpertiseSelf1,
            SpellId.WeaponExpertiseSelf1,

            SpellId.MonsterAttunementSelf1,
            SpellId.PersonAttunementSelf1,
        };

        private static readonly List<SpellId> lifeSpells = new List<SpellId>()
        {
            SpellId.ArmorSelf1,

            SpellId.RegenerationSelf1,
            SpellId.RejuvenationSelf1,
            SpellId.ManaRenewalSelf1,

            SpellId.BladeProtectionSelf1,
            SpellId.PiercingProtectionSelf1,
            SpellId.BludgeonProtectionSelf1,

            SpellId.FireProtectionSelf1,
            SpellId.ColdProtectionSelf1,
            SpellId.AcidProtectionSelf1,
            SpellId.LightningProtectionSelf1,
        };

        private static readonly int NumTiers = 8;

        // original api
        public static readonly SpellId[][] GemCreatureSpellMatrix = new SpellId[NumTiers][];
        public static readonly SpellId[][] GemLifeSpellMatrix = new SpellId[NumTiers][];

        static GemSpells()
        {
            // takes ~0 ms
            BuildSpells(creatureSpells, GemCreatureSpellMatrix);
            BuildSpells(lifeSpells, GemLifeSpellMatrix);
        }

        private static void BuildSpells(List<SpellId> spells, SpellId[][] matrix)
        {
            for (var i = 0; i < NumTiers; i++)
                matrix[i] = new SpellId[spells.Count];

            for (var i = 0; i < spells.Count; i++)
            {
                var spell = spells[i];

                var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

                if (spellLevels == null)
                {
                    log.Error($"GemSpells - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumTiers)
                {
                    log.Error($"GemSpells - expected {NumTiers} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumTiers; j++)
                    matrix[j][i] = spellLevels[j];
            }
        }
    }
}
