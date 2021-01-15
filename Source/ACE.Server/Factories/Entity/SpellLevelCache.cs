using System.Collections.Concurrent;

using ACE.Server.Entity;

namespace ACE.Server.Factories.Entity
{
    public static class SpellLevelCache
    {
        private static readonly ConcurrentDictionary<int, int> spellLevels = new ConcurrentDictionary<int, int>();

        public static int GetSpellLevel(int spellId)
        {
            if (!spellLevels.TryGetValue(spellId, out var spellLevel))
            {
                var spell = new Spell(spellId);
                spellLevel = spellLevels[spellId] = (int)spell.Formula.Level;
            }
            return spellLevel;
        }
    }
}
