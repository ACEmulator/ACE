using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class ArmorCantrips
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            // creature cantrips

            SpellId.CANTRIPSTRENGTH1,
            SpellId.CANTRIPENDURANCE1,
            SpellId.CANTRIPCOORDINATION1,
            SpellId.CANTRIPQUICKNESS1,
            SpellId.CANTRIPFOCUS1,
            SpellId.CANTRIPWILLPOWER1,

            SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1,
            SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1,
            SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1,
            SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1,
            SpellId.CANTRIPTWOHANDEDAPTITUDE1,

            SpellId.CANTRIPINVULNERABILITY1,
            SpellId.CANTRIPIMPREGNABILITY1,
            SpellId.CANTRIPMAGICRESISTANCE1,

            SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPLIFEMAGICAPTITUDE1,
            SpellId.CANTRIPWARMAGICAPTITUDE1,
            SpellId.CantripVoidMagicAptitude1,
            SpellId.CantripSummoningProwess1,

            SpellId.CANTRIPARCANEPROWESS1,
            SpellId.CANTRIPDECEPTIONPROWESS1,
            SpellId.CANTRIPHEALINGPROWESS1,
            SpellId.CANTRIPJUMPINGPROWESS1,         // missing from original cantrips, but was in spells
                                                    // should a separate lower armor cantrips table be added for this?
            SpellId.CANTRIPMANACONVERSIONPROWESS1,
            SpellId.CANTRIPLOCKPICKPROWESS1,
            SpellId.CANTRIPSPRINT1,                 // missing from original cantrips, but was in spells
                                                    // should a separate lower armor cantrips table be added for this?

            SpellId.CantripDirtyFightingProwess1,
            SpellId.CantripDualWieldAptitude1,
            SpellId.CantripRecklessnessProwess1,    // was in original twice
            SpellId.CantripSneakAttackProwess1,

            SpellId.CantripShieldAptitude1,

            SpellId.CANTRIPALCHEMICALPROWESS1,
            SpellId.CANTRIPCOOKINGPROWESS1,
            SpellId.CANTRIPFLETCHINGPROWESS1,

            SpellId.CANTRIPLEADERSHIP1,
            SpellId.CANTRIPFEALTY1,

            SpellId.CantripSalvaging1,
            SpellId.CANTRIPARMOREXPERTISE1,
            SpellId.CANTRIPITEMEXPERTISE1,
            SpellId.CANTRIPMAGICITEMEXPERTISE1,
            SpellId.CANTRIPWEAPONEXPERTISE1,

            SpellId.CANTRIPMONSTERATTUNEMENT1,
            SpellId.CANTRIPPERSONATTUNEMENT1,

            // life cantrips

            SpellId.CANTRIPARMOR1,      // was in original twice
            SpellId.CANTRIPACIDWARD1,
            SpellId.CANTRIPBLUDGEONINGWARD1,
            SpellId.CANTRIPFROSTWARD1,
            SpellId.CANTRIPSTORMWARD1,
            SpellId.CANTRIPFLAMEWARD1,
            SpellId.CANTRIPSLASHINGWARD1,
            SpellId.CANTRIPPIERCINGWARD1,

            // item cantrips

            SpellId.CANTRIPIMPENETRABILITY1,
            SpellId.CANTRIPSLASHINGBANE1,
            SpellId.CANTRIPACIDBANE1,
            SpellId.CANTRIPBLUDGEONINGBANE1,
            SpellId.CANTRIPFROSTBANE1,
            SpellId.CANTRIPSTORMBANE1,
            SpellId.CANTRIPFLAMEBANE1,
            SpellId.CANTRIPPIERCINGBANE1,
        };

        private static readonly int NumLevels = 4;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];

        static ArmorCantrips()
        {
            // takes ~0.3ms
            BuildSpells();
        }

        private static void BuildSpells()
        {
            for (var i = 0; i < spells.Count; i++)
                Table[i] = new SpellId[NumLevels];

            for (var i = 0; i < spells.Count; i++)
            {
                var spell = spells[i];

                var spellLevels = SpellLevelProgression.GetSpellLevels(spell);

                if (spellLevels == null)
                {
                    log.Error($"ArmorCantrips - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumLevels)
                {
                    log.Error($"ArmorCantrips - expected {NumLevels} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumLevels; j++)
                    Table[i][j] = spellLevels[j];
            }
        }
    }
}
