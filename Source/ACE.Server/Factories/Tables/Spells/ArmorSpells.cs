using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Tables
{
    public static class ArmorSpells
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            // creature buffs

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

            SpellId.DirtyFightingMasterySelf1,
            SpellId.RecklessnessMasterySelf1,
            SpellId.SneakAttackMasterySelf1,
            SpellId.DualWieldMasterySelf1,

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

            // life buffs

            SpellId.ArmorSelf1,

            SpellId.BladeProtectionSelf1,
            SpellId.PiercingProtectionSelf1,
            SpellId.BludgeonProtectionSelf1,

            SpellId.FireProtectionSelf1,
            SpellId.ColdProtectionSelf1,
            SpellId.AcidProtectionSelf1,
            SpellId.LightningProtectionSelf1,

            SpellId.RegenerationSelf1,
            SpellId.RejuvenationSelf1,
            SpellId.ManaRenewalSelf1,

            // item buffs

            SpellId.Impenetrability1,

            SpellId.BladeBane1,
            SpellId.PiercingBane1,
            SpellId.BludgeonBane1,

            SpellId.FlameBane1,
            SpellId.FrostBane1,
            SpellId.AcidBane1,
            SpellId.LightningBane1,
        };

        private static readonly int NumTiers = 8;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];
        public static readonly List<SpellId> CreatureLifeTable = new List<SpellId>();

        static ArmorSpells()
        {
            // takes ~0.3ms
            BuildSpells();
        }

        private static void BuildSpells()
        {
            for (var i = 0; i < spells.Count; i++)
                Table[i] = new SpellId[NumTiers];

            for (var i = 0; i < spells.Count; i++)
            {
                var spell = spells[i];

                var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

                if (spellLevels == null)
                {
                    log.Error($"ArmorSpells - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumTiers)
                {
                    log.Error($"ArmorSpells - expected {NumTiers} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumTiers; j++)
                    Table[i][j] = spellLevels[j];

                // build a version of this table w/out item spells
                switch (spell)
                {
                    case SpellId.Impenetrability1:
                    case SpellId.BladeBane1:
                    case SpellId.PiercingBane1:
                    case SpellId.BludgeonBane1:
                    case SpellId.FlameBane1:
                    case SpellId.FrostBane1:
                    case SpellId.AcidBane1:
                    case SpellId.LightningBane1:
                        break;

                    default:
                        CreatureLifeTable.Add(spell);
                        break;
                }
            }
        }

        // alt

        // this table also applies to clothing w/ AL

        private static readonly List<(SpellId spellId, float chance)> armorSpells = new List<(SpellId, float)>()
        {
            ( SpellId.PiercingBane1,    0.15f ),
            ( SpellId.FlameBane1,       0.15f ),
            ( SpellId.FrostBane1,       0.15f ),
            ( SpellId.Impenetrability1, 1.00f ),
            ( SpellId.AcidBane1,        0.15f ),
            ( SpellId.BladeBane1,       0.15f ),
            ( SpellId.LightningBane1,   0.15f ),
            ( SpellId.BludgeonBane1,    0.15f ),
        };

        public static List<SpellId> Roll(TreasureDeath treasureDeath)
        {
            // this roll also applies to clothing w/ AL!
            // ie., shirts and pants would never have item spells on them,
            // but cloth gloves would

            // thanks to Sapphire Knight and Butterflygolem for helping to figure this part out!

            var spells = new List<SpellId>();

            foreach (var spell in armorSpells)
            {
                var rng = ThreadSafeRandom.NextInterval(treasureDeath.LootQualityMod);

                if (rng < spell.chance)
                    spells.Add(spell.spellId);
            }
            return spells;
        }
    }
}
