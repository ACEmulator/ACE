using System;
using System.Collections.Generic;
using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public static class SpellSet
    {
        // helper collection for spell sorting
        public static readonly HashSet<int> SetSpells = new HashSet<int>();

        static SpellSet()
        {
            foreach (var spellSet in DatManager.PortalDat.SpellTable.SpellSet.Values)
            {
                foreach (var tier in spellSet.SpellSetTiers.Values)
                {
                    foreach (var spell in tier.Spells)
                    {
                        // cutoff for enchantment manager bug fix sorting
                        if (spell >= (uint)SpellId.SetCoordination1)
                            SetSpells.Add((int)spell);
                    }
                }
            }
            /*Console.WriteLine($"Added {SetSpells.Count} set spells:");

            foreach (var setSpell in SetSpells.OrderBy(i => i))
                Console.WriteLine($"{setSpell} - {(SpellId)setSpell}");*/
        }
    }
}
