using System.Collections.Generic;

using log4net;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class MissileCantrips
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            SpellId.CANTRIPSTRENGTH1,
            SpellId.CANTRIPENDURANCE1,
            SpellId.CANTRIPCOORDINATION1,
            SpellId.CANTRIPQUICKNESS1,      // added, according to spellSelectionGroup6

            SpellId.CANTRIPBLOODTHIRST1,
            SpellId.CANTRIPHEARTTHIRST1,
            SpellId.CANTRIPDEFENDER1,
            SpellId.CANTRIPSWIFTHUNTER1,

            SpellId.CantripDirtyFightingProwess1,
            SpellId.CantripRecklessnessProwess1,
            SpellId.CantripSneakAttackProwess1,
        };

        private static readonly int NumLevels = 4;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];

        static MissileCantrips()
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
                    log.Error($"MissileCantrips - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumLevels)
                {
                    log.Error($"MissileCantrips - expected {NumLevels} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumLevels; j++)
                    Table[i][j] = spellLevels[j];
            }
        }
    }
}
