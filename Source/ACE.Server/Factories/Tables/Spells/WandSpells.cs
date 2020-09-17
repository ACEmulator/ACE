using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;

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
            }
        }
    }
}
