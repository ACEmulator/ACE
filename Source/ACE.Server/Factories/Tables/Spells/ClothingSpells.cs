using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class ClothingSpells
    {
        private static readonly List<(SpellId spellId, float chance)> clothingSpells = new List<(SpellId, float)>()
        {
            ( SpellId.PiercingBane1,    0.075f ),
            ( SpellId.FlameBane1,       0.075f ),
            ( SpellId.FrostBane1,       0.075f ),
            ( SpellId.Impenetrability1, 0.500f ),
            ( SpellId.AcidBane1,        0.075f ),
            ( SpellId.BladeBane1,       0.075f ),
            ( SpellId.LightningBane1,   0.075f ),
            ( SpellId.BludgeonBane1,    0.075f ),
        };

        public static List<SpellId> Roll(TreasureDeath treasureDeath)
        {
            var spells = new List<SpellId>();

            foreach (var spell in clothingSpells)
            {
                var rng = ThreadSafeRandom.NextInterval(treasureDeath.LootQualityMod);

                if (rng < spell.chance)
                    spells.Add(spell.spellId);
            }
            return spells;
        }
    }
}
