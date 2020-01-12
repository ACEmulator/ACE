
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Managers;

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
        public bool Broadcast;
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
