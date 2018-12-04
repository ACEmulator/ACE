using System.Collections.Generic;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Updates multiple enchantments for your character.
    /// </summary>
    public class GameEventMagicUpdateMultipleEnchantments : GameEventMessage
    {
        public GameEventMagicUpdateMultipleEnchantments(Session session, List<Enchantment> enchantments)
            : base(GameEventType.MagicUpdateMultipleEnchantments, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(enchantments);
        }
    }
}
