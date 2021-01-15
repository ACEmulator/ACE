using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Tables
{
    public static class WandSpells
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            SpellId.FocusSelf1,
            SpellId.WillpowerSelf1,

            SpellId.CreatureEnchantmentMasterySelf1,
            SpellId.ItemEnchantmentMasterySelf1,
            SpellId.LifeMagicMasterySelf1,
            SpellId.WarMagicMasterySelf1,
            SpellId.VoidMagicMasterySelf1,  // missing from original

            SpellId.DefenderSelf1,
            SpellId.HermeticLinkSelf1,
            SpellId.SpiritDrinkerSelf1,     // added to match WandCantrips

            SpellId.ArcaneEnlightenmentSelf1,
            SpellId.ManaMasterySelf1,

            SpellId.SneakAttackMasterySelf1,
        };

        private static readonly int NumTiers = 8;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];
        public static readonly List<SpellId> CreatureLifeTable = new List<SpellId>();

        static WandSpells()
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
                    log.Error($"WandSpells - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumTiers)
                {
                    log.Error($"WandSpells - expected {NumTiers} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumTiers; j++)
                    Table[i][j] = spellLevels[j];

                // build a version of this table w/out item spells
                switch (spell)
                {
                    case SpellId.DefenderSelf1:
                    case SpellId.HermeticLinkSelf1:
                    case SpellId.SpiritDrinkerSelf1:
                        break;

                    default:
                        CreatureLifeTable.Add(spell);
                        break;
                }
            }
        }

        // alt

        private static readonly List<(SpellId spellId, float chance)> wandSpells = new List<(SpellId, float)>()
        {
            ( SpellId.DefenderSelf1,      0.25f ),
            ( SpellId.HermeticLinkSelf1,  1.0f ),
            ( SpellId.SpiritDrinkerSelf1, 0.25f ),      // retail appears to have had a flat 25% chance for Spirit Drinker for all casters,
                                                        // regardless if they had a DamageType
        };

        public static List<SpellId> Roll(WorldObject wo, TreasureDeath treasureDeath)
        {
            var spells = new List<SpellId>();

            foreach (var spell in wandSpells)
            {
                // retail didn't have this logic, but...
                if (spell.spellId == SpellId.SpiritDrinkerSelf1 && wo.W_DamageType == DamageType.Undef)
                    continue;

                var rng = ThreadSafeRandom.NextInterval(treasureDeath.LootQualityMod);

                if (rng < spell.chance)
                    spells.Add(spell.spellId);
            }
            return spells;
        }
    }
}
