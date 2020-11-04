using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class MeleeSpells
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            SpellId.StrengthSelf1,
            SpellId.EnduranceSelf1,
            SpellId.CoordinationSelf1,
            SpellId.QuicknessSelf1,     // added, according to spellSelectionGroup6

            SpellId.BloodDrinkerSelf1,
            SpellId.DefenderSelf1,
            SpellId.HeartSeekerSelf1,
            SpellId.SwiftKillerSelf1,

            SpellId.DirtyFightingMasterySelf1,
            SpellId.DualWieldMasterySelf1,
            SpellId.RecklessnessMasterySelf1,
            SpellId.SneakAttackMasterySelf1,
        };

        private static readonly int NumTiers = 8;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];
        public static readonly List<SpellId> CreatureLifeTable = new List<SpellId>();

        static MeleeSpells()
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
                    log.Error($"MeleeSpells - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumTiers)
                {
                    log.Error($"MeleeSpells - expected {NumTiers} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumTiers; j++)
                    Table[i][j] = spellLevels[j];

                // build a version of this table w/out item spells
                switch (spell)
                {
                    case SpellId.BloodDrinkerSelf1:
                    case SpellId.DefenderSelf1:
                    case SpellId.HeartSeekerSelf1:
                    case SpellId.SwiftKillerSelf1:
                        break;

                    default:
                        CreatureLifeTable.Add(spell);
                        break;
                }
            }
        }

        // alt

        private static readonly List<(SpellId spellId, float chance)> weaponMeleeSpells = new List<(SpellId, float)>()
        {
            ( SpellId.DefenderSelf1,     0.25f ),
            ( SpellId.BloodDrinkerSelf1, 1.00f ),
            ( SpellId.SwiftKillerSelf1,  0.30f ),
            ( SpellId.HeartSeekerSelf1,  0.25f ),
        };

        public static List<SpellId> Roll(TreasureDeath treasureDeath)
        {
            var spells = new List<SpellId>();

            foreach (var spell in weaponMeleeSpells)
            {
                var rng = ThreadSafeRandom.NextInterval(treasureDeath.LootQualityMod);

                if (rng < spell.chance)
                    spells.Add(spell.spellId);
            }
            return spells;
        }
    }
}
