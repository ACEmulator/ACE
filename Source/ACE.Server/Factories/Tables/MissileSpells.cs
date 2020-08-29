using System.Collections.Generic;

using System.Diagnostics;

using log4net;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class MissileSpells
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<SpellId> spells = new List<SpellId>()
        {
            SpellId.StrengthSelf1,
            SpellId.EnduranceSelf1,
            SpellId.CoordinationSelf1,
            // quickness?

            SpellId.BloodDrinkerSelf1,
            SpellId.HeartSeekerSelf1,
            SpellId.DefenderSelf1,
            SpellId.SwiftKillerSelf1,

            SpellId.DirtyFightingMasterySelf1,
            SpellId.RecklessnessMasterySelf1,
            SpellId.SneakAttackMasterySelf1,
        };

        private static readonly int NumTiers = 8;

        // original api
        public static readonly SpellId[][] Table = new SpellId[spells.Count][];

        static MissileSpells()
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
                    log.Error($"MissileSpells - couldn't find {spell}");
                    continue;
                }

                if (spellLevels.Count != NumTiers)
                {
                    log.Error($"MissileSpells - expected {NumTiers} levels for {spell}, found {spellLevels.Count}");
                    continue;
                }

                for (var j = 0; j < NumTiers; j++)
                    Table[i][j] = spellLevels[j];
            }
        }
    }
}
