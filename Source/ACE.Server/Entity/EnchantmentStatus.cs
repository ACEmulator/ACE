using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Tracks the status of applying an enchantment
    /// </summary>
    public class EnchantmentStatus
    {
        public Spell Spell;
        public StackType StackType;
        public GameMessageSystemChat Message;
        public bool Success;

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
