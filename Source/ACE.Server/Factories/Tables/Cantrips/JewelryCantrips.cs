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

        private static readonly ChanceTable<SpellId> jewelryCantrips = new ChanceTable<SpellId>()
        {
            ( SpellId.CANTRIPALCHEMICALPROWESS1,      0.01f ),
            ( SpellId.CANTRIPARCANEPROWESS1,          0.01f ),
            ( SpellId.CANTRIPARMOREXPERTISE1,         0.01f ),
            ( SpellId.CANTRIPLIGHTWEAPONSAPTITUDE1,   0.01f ),
            ( SpellId.CANTRIPCOOKINGPROWESS1,         0.01f ),
            ( SpellId.CANTRIPFINESSEWEAPONSAPTITUDE1, 0.01f ),
            ( SpellId.CANTRIPDECEPTIONPROWESS1,       0.01f ),
            ( SpellId.CANTRIPFEALTY1,                 0.01f ),
            ( SpellId.CANTRIPFLETCHINGPROWESS1,       0.01f ),
            ( SpellId.CANTRIPHEALINGPROWESS1,         0.01f ),
            ( SpellId.CANTRIPIMPREGNABILITY1,         0.01f ),
            ( SpellId.CANTRIPINVULNERABILITY1,        0.01f ),
            ( SpellId.CANTRIPITEMEXPERTISE1,          0.01f ),
            ( SpellId.CANTRIPJUMPINGPROWESS1,         0.01f ),
            ( SpellId.CANTRIPLEADERSHIP1,             0.01f ),
            ( SpellId.CANTRIPLOCKPICKPROWESS1,        0.01f ),
            ( SpellId.CANTRIPMACEAPTITUDE1,           0.01f ),
            ( SpellId.CANTRIPMAGICITEMEXPERTISE1,     0.01f ),
            ( SpellId.CANTRIPMAGICRESISTANCE1,        0.01f ),
            ( SpellId.CANTRIPMONSTERATTUNEMENT1,      0.01f ),
            ( SpellId.CANTRIPPERSONATTUNEMENT1,       0.01f ),
            ( SpellId.CANTRIPMACEAPTITUDE1,           0.01f ),
            ( SpellId.CANTRIPSPRINT1,                 0.01f ),
            ( SpellId.CANTRIPSTAFFAPTITUDE1,          0.01f ),
            ( SpellId.CANTRIPHEAVYWEAPONSAPTITUDE1,   0.01f ),
            ( SpellId.CANTRIPTHROWNAPTITUDE1,         0.01f ),
            ( SpellId.CANTRIPUNARMEDAPTITUDE1,        0.01f ),
            ( SpellId.CANTRIPWEAPONEXPERTISE1,        0.01f ),
            ( SpellId.CANTRIPACIDWARD1,               0.01f ),
            ( SpellId.CANTRIPBLUDGEONINGWARD1,        0.01f ),
            ( SpellId.CANTRIPFLAMEWARD1,              0.01f ),
            ( SpellId.CANTRIPFROSTWARD1,              0.01f ),
            ( SpellId.CANTRIPPIERCINGWARD1,           0.01f ),
            ( SpellId.CANTRIPSLASHINGWARD1,           0.01f ),
            ( SpellId.CANTRIPSTORMWARD1,              0.01f ),
            ( SpellId.CANTRIPIMPENETRABILITY1,        0.05f ),
            ( SpellId.CANTRIPSTORMBANE1,              0.05f ),
            ( SpellId.CANTRIPACIDBANE1,               0.05f ),
            ( SpellId.CANTRIPBLUDGEONINGBANE1,        0.05f ),
            ( SpellId.CANTRIPFLAMEBANE1,              0.05f ),
            ( SpellId.CANTRIPFROSTBANE1,              0.05f ),
            ( SpellId.CANTRIPPIERCINGBANE1,           0.05f ),
            ( SpellId.CANTRIPSLASHINGBANE1,           0.05f ),
            ( SpellId.CANTRIPCOORDINATION1,           0.04f ),
            ( SpellId.CANTRIPENDURANCE1,              0.04f ),
            ( SpellId.CANTRIPFOCUS1,                  0.03f ),
            ( SpellId.CANTRIPQUICKNESS1,              0.04f ),
            ( SpellId.CANTRIPSTRENGTH1,               0.04f ),
            ( SpellId.CANTRIPWILLPOWER1,              0.03f ),
            ( SpellId.CANTRIPARMOR1,                  0.03f ),
        };

        public static SpellId Roll()
        {
            return jewelryCantrips.Roll();
        }
    }
}
