using ACE.Database.Models.Shard;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Wrapper class for linking Enchantments and Spells
    /// </summary>
    public class SpellEnchantment
    {
        public BiotaPropertiesEnchantmentRegistry Enchantment;
        public Spell Spell;

        public SpellEnchantment(BiotaPropertiesEnchantmentRegistry enchantment)
        {
            Enchantment = enchantment;
            Spell = new Spell(enchantment.SpellId);
        }
    }
}
