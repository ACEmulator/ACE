
using ACE.Entity.Models;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Wrapper class for linking Enchantments and Spells
    /// </summary>
    public class SpellEnchantment
    {
        public PropertiesEnchantmentRegistry Enchantment;
        public Spell Spell;

        public SpellEnchantment(PropertiesEnchantmentRegistry enchantment)
        {
            Enchantment = enchantment;
            Spell = new Spell(enchantment.SpellId);
        }
    }
}
