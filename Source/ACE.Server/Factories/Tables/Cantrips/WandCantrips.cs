using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class WandCantrips
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            SpellId.CANTRIPFOCUS1,
            SpellId.CANTRIPWILLPOWER1,

            SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,
            SpellId.CANTRIPLIFEMAGICAPTITUDE1,
            SpellId.CANTRIPWARMAGICAPTITUDE1,
            SpellId.CantripVoidMagicAptitude1,      // missing from original

            SpellId.CANTRIPARCANEPROWESS1,
            SpellId.CANTRIPMANACONVERSIONPROWESS1,

            SpellId.CantripSneakAttackProwess1,

            SpellId.CANTRIPDEFENDER1,
            SpellId.CantripHermeticLink1,
            SpellId.CantripSpiritThirst1,
        };

        private static readonly int NumLevels = 4;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];

        static WandCantrips()
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
                    log.Error($"WandCantrips - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumLevels)
                {
                    log.Error($"WandCantrips - expected {NumLevels} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumLevels; j++)
                    Table[i][j] = spellLevels[j];
            }
        }

        private static readonly ChanceTable<SpellId> casterCantrips = new ChanceTable<SpellId>()
        {
            ( SpellId.CANTRIPALCHEMICALPROWESS1,           0.01f ),
            ( SpellId.CANTRIPARCANEPROWESS1,               0.05f ),
            ( SpellId.CANTRIPARMOREXPERTISE1,              0.01f ),
            ( SpellId.CANTRIPCOOKINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPCREATUREENCHANTMENTAPTITUDE1, 0.05f ),
            ( SpellId.CANTRIPDECEPTIONPROWESS1,            0.01f ),
            ( SpellId.CANTRIPFEALTY1,                      0.01f ),
            ( SpellId.CANTRIPFLETCHINGPROWESS1,            0.01f ),
            ( SpellId.CANTRIPHEALINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPIMPREGNABILITY1,              0.03f ),
            ( SpellId.CANTRIPINVULNERABILITY1,             0.03f ),
            ( SpellId.CANTRIPITEMENCHANTMENTAPTITUDE1,     0.05f ),
            ( SpellId.CANTRIPITEMEXPERTISE1,               0.01f ),
            ( SpellId.CANTRIPJUMPINGPROWESS1,              0.01f ),
            ( SpellId.CANTRIPLEADERSHIP1,                  0.01f ),
            ( SpellId.CANTRIPLIFEMAGICAPTITUDE1,           0.05f ),
            ( SpellId.CANTRIPLOCKPICKPROWESS1,             0.01f ),
            ( SpellId.CANTRIPMAGICITEMEXPERTISE1,          0.01f ),
            ( SpellId.CANTRIPMAGICRESISTANCE1,             0.03f ),
            ( SpellId.CANTRIPMANACONVERSIONPROWESS1,       0.05f ),
            ( SpellId.CANTRIPMONSTERATTUNEMENT1,           0.01f ),
            ( SpellId.CANTRIPPERSONATTUNEMENT1,            0.01f ),
            ( SpellId.CANTRIPSPRINT1,                      0.01f ),
            ( SpellId.CANTRIPWARMAGICAPTITUDE1,            0.05f ),
            ( SpellId.CANTRIPWEAPONEXPERTISE1,             0.01f ),
            ( SpellId.CANTRIPACIDWARD1,                    0.01f ),
            ( SpellId.CANTRIPBLUDGEONINGWARD1,             0.01f ),
            ( SpellId.CANTRIPFLAMEWARD1,                   0.01f ),
            ( SpellId.CANTRIPFROSTWARD1,                   0.01f ),
            ( SpellId.CANTRIPPIERCINGWARD1,                0.01f ),
            ( SpellId.CANTRIPSLASHINGWARD1,                0.01f ),
            ( SpellId.CANTRIPSTORMWARD1,                   0.01f ),
            ( SpellId.CANTRIPDEFENDER1,                    0.05f ),
            ( SpellId.CantripHermeticLink1,                0.05f ),
            ( SpellId.CANTRIPCOORDINATION1,                0.01f ),
            ( SpellId.CANTRIPENDURANCE1,                   0.06f ),
            ( SpellId.CANTRIPFOCUS1,                       0.06f ),
            ( SpellId.CANTRIPQUICKNESS1,                   0.01f ),
            ( SpellId.CANTRIPSTRENGTH1,                    0.06f ),
            ( SpellId.CANTRIPWILLPOWER1,                   0.06f ),
            ( SpellId.CANTRIPARMOR1,                       0.02f ),
        };

        public static SpellId Roll()
        {
            return casterCantrips.Roll();
        }
    }
}
