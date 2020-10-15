using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class JewelryCantrips
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
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
            SpellId.CANTRIPLOCKPICKPROWESS1,
            SpellId.CANTRIPJUMPINGPROWESS1,
            SpellId.CANTRIPMANACONVERSIONPROWESS1,
            SpellId.CANTRIPSPRINT1,

            SpellId.CantripDualWieldAptitude1,
            SpellId.CantripDirtyFightingProwess1,
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

            // life

            SpellId.CANTRIPARMOR1,      // was in original twice
            SpellId.CANTRIPACIDWARD1,
            SpellId.CANTRIPBLUDGEONINGWARD1,
            SpellId.CANTRIPFROSTWARD1,
            SpellId.CANTRIPSTORMWARD1,
            SpellId.CANTRIPFLAMEWARD1,
            SpellId.CANTRIPSLASHINGWARD1,
            SpellId.CANTRIPPIERCINGWARD1,
        };

        private static readonly int NumLevels = 4;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];

        static JewelryCantrips()
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
                    log.Error($"JewelryCantrips - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumLevels)
                {
                    log.Error($"JewelryCantrips - expected {NumLevels} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumLevels; j++)
                    Table[i][j] = spellLevels[j];
            }
        }

        private static ChanceTable<SpellId> jewelryCantrips = new ChanceTable<SpellId>()
        {
            ( SpellId.CANTRIPARMOR1,                       0.03f ),
            ( SpellId.CANTRIPACIDWARD1,                    0.03f ),
            ( SpellId.CANTRIPBLUDGEONINGWARD1,             0.03f ),
            ( SpellId.CANTRIPFLAMEWARD1,                   0.03f ),
            ( SpellId.CANTRIPFROSTWARD1,                   0.03f ),
            ( SpellId.CANTRIPPIERCINGWARD1,                0.03f ),
            ( SpellId.CANTRIPSLASHINGWARD1,                0.03f ),
            ( SpellId.CANTRIPSTORMWARD1,                   0.03f ),

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

            ( SpellId.CANTRIPALCHEMICALPROWESS1,           0.02f ),
            ( SpellId.CANTRIPARCANEPROWESS1,               0.02f ),
            ( SpellId.CANTRIPARMOREXPERTISE1,              0.02f ),
            ( SpellId.CANTRIPCOOKINGPROWESS1,              0.02f ),
            ( SpellId.CANTRIPDECEPTIONPROWESS1,            0.02f ),
            ( SpellId.CANTRIPFEALTY1,                      0.02f ),
            ( SpellId.CANTRIPFLETCHINGPROWESS1,            0.02f ),
            ( SpellId.CANTRIPHEALINGPROWESS1,              0.02f ),
            ( SpellId.CANTRIPITEMEXPERTISE1,               0.02f ),
            ( SpellId.CANTRIPJUMPINGPROWESS1,              0.02f ),
            ( SpellId.CANTRIPLEADERSHIP1,                  0.02f ),
            ( SpellId.CANTRIPLOCKPICKPROWESS1,             0.02f ),
            ( SpellId.CANTRIPMAGICITEMEXPERTISE1,          0.02f ),
            ( SpellId.CANTRIPMANACONVERSIONPROWESS1,       0.02f ),
            ( SpellId.CANTRIPMONSTERATTUNEMENT1,           0.02f ),
            ( SpellId.CANTRIPPERSONATTUNEMENT1,            0.02f ),
            ( SpellId.CANTRIPSPRINT1,                      0.02f ),
            ( SpellId.CANTRIPWEAPONEXPERTISE1,             0.02f ),

            ( SpellId.CantripDirtyFightingProwess1,        0.02f ),
            ( SpellId.CantripDualWieldAptitude1,           0.02f ),
            ( SpellId.CantripRecklessnessProwess1,         0.02f ),
            ( SpellId.CantripSalvaging1,                   0.02f ),
            ( SpellId.CantripShieldAptitude1,              0.02f ),
            ( SpellId.CantripSneakAttackProwess1,          0.02f ),
            ( SpellId.CantripSummoningProwess1,            0.02f ),
        };

        public static SpellId Roll()
        {
            return jewelryCantrips.Roll();
        }
    }
}
