using ACE.Entity.Enum;

namespace ACE.Server.Factories.Entity
{
    public class SpellChance
    {
        public SpellId Spell;
        public float Chance;

        public SpellChance(SpellId spell, float chance)
        {
            Spell = spell;
            Chance = chance;
        }
    }
}
