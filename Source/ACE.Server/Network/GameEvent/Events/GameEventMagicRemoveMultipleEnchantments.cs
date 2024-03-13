using System.Collections.Generic;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Removes multiple enchantments from your character.
    /// </summary>
    public class GameEventMagicRemoveMultipleEnchantments : GameEventMessage
    {
        public GameEventMagicRemoveMultipleEnchantments(Session session, List<LayeredSpell> spells)
            : base(GameEventType.MagicRemoveMultipleEnchantments, GameMessageGroup.UIQueue, session, 56) // 56 is the max seen in retail pcaps
        {
            Writer.Write(spells);
        }
    }
}
