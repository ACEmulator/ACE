using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

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

        private static ChanceTable<SpellId> armorCantrips = new ChanceTable<SpellId>()
        {
            ( SpellId.CANTRIPSTRENGTH1,                    0.02f ),
            ( SpellId.CANTRIPENDURANCE1,                   0.02f ),
            ( SpellId.CANTRIPCOORDINATION1,                0.02f ),
            ( SpellId.CANTRIPQUICKNESS1,                   0.02f ),
            ( SpellId.CANTRIPFOCUS1,                       0.02f ),
            ( SpellId.CANTRIPWILLPOWER1,                   0.02f ),

            ( SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1,        0.02f ),
            ( SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1,        0.02f ),
            ( SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1,      0.02f ),
            ( SpellId.CANTRIPMISSILEWEAPONSAPTITUDE1,      0.02f ),
            ( SpellId.CANTRIPTWOHANDEDAPTITUDE1,           0.02f ),

            ( SpellId.CANTRIPIMPREGNABILITY1,              0.02f ),
            ( SpellId.CANTRIPINVULNERABILITY1,             0.02f ),
            ( SpellId.CANTRIPMAGICRESISTANCE1,             0.02f ),

            ( SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1, 0.02f ),
            ( SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,     0.02f ),
            ( SpellId.CANTRIPLIFEMAGICAPTITUDE1,           0.02f ),
            ( SpellId.CANTRIPWARMAGICAPTITUDE1,            0.02f ),
            ( SpellId.CantripVoidMagicAptitude1,           0.02f ),

            ( SpellId.CANTRIPIMPENETRABILITY1,             0.02f ),
            ( SpellId.CANTRIPACIDBANE1,                    0.02f ),
            ( SpellId.CANTRIPBLUDGEONINGBANE1,             0.02f ),
            ( SpellId.CANTRIPFLAMEBANE1,                   0.02f ),
            ( SpellId.CANTRIPFROSTBANE1,                   0.02f ),
            ( SpellId.CANTRIPPIERCINGBANE1,                0.02f ),
            ( SpellId.CANTRIPSLASHINGBANE1,                0.02f ),
            ( SpellId.CANTRIPSTORMBANE1,                   0.02f ),

            ( SpellId.CANTRIPARMOR1,                       0.02f ),
            ( SpellId.CANTRIPACIDWARD1,                    0.02f ),
            ( SpellId.CANTRIPBLUDGEONINGWARD1,             0.02f ),
            ( SpellId.CANTRIPFLAMEWARD1,                   0.02f ),
            ( SpellId.CANTRIPFROSTWARD1,                   0.02f ),
            ( SpellId.CANTRIPPIERCINGWARD1,                0.02f ),
            ( SpellId.CANTRIPSLASHINGWARD1,                0.02f ),
            ( SpellId.CANTRIPSTORMWARD1,                   0.02f ),

            ( SpellId.CANTRIPALCHEMICALPROWESS1,           0.01f ),
            ( SpellId.CANTRIPARCANEPROWESS1,               0.01f ),
            ( SpellId.CANTRIPARMOREXPERTISE1,              0.01f ),
            ( SpellId.CANTRIPCOOKINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPDECEPTIONPROWESS1,            0.01f ),
            ( SpellId.CANTRIPFEALTY1,                      0.01f ),
            ( SpellId.CANTRIPFLETCHINGPROWESS1,            0.01f ),
            ( SpellId.CANTRIPHEALINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPITEMEXPERTISE1,               0.01f ),
            ( SpellId.CANTRIPJUMPINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPLEADERSHIP1,                  0.01f ),
            ( SpellId.CANTRIPLOCKPICKPROWESS1,             0.01f ),
            ( SpellId.CANTRIPMAGICITEMEXPERTISE1,          0.01f ),
            ( SpellId.CANTRIPMANACONVERSIONPROWESS1,       0.01f ),
            ( SpellId.CANTRIPMONSTERATTUNEMENT1,           0.005f ),
            ( SpellId.CANTRIPPERSONATTUNEMENT1,            0.005f ),
            ( SpellId.CantripSalvaging1,                   0.01f ),
            ( SpellId.CANTRIPSPRINT1,                      0.01f ),
            ( SpellId.CANTRIPWEAPONEXPERTISE1,             0.01f ),

            ( SpellId.CantripDirtyFightingProwess1,        0.02f ),
            ( SpellId.CantripDualWieldAptitude1,           0.02f ),
            ( SpellId.CantripRecklessnessProwess1,         0.02f ),
            ( SpellId.CantripShieldAptitude1,              0.02f ),
            ( SpellId.CantripSneakAttackProwess1,          0.02f ),
            ( SpellId.CantripSummoningProwess1,            0.02f ),
        };

        public static SpellId Roll()
        {
            return armorCantrips.Roll();
        }
    }
}
