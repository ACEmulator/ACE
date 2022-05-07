
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Tracks the status of applying an enchantment
    /// </summary>
    public class EnchantmentStatus
    {
        public Spell Spell { get; set; }
        public StackType StackType { get; set; }
        public GameMessageSystemChat Message { get; set; }
        public bool Success { get; set; }

        public EnchantmentStatus(Spell spell)
        {
            Spell = spell;
        }

        public EnchantmentStatus(uint spellID)
        {
            Spell = new Spell(spellID);
        }

        public EnchantmentStatus(bool success)
        {
            Success = success;
        }
    }
}
