using System.Collections.Generic;

using ACE.Entity.Models;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Silently remove multiple enchantments from your character (no message in the chat window).
    /// </summary>
    public class GameEventMagicDispelMultipleEnchantments : GameEventMessage
    {
        public GameEventMagicDispelMultipleEnchantments(Session session, List<LayeredSpell> spells)
            : base(GameEventType.MagicDispelMultipleEnchantments, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spells);
        }

        public GameEventMagicDispelMultipleEnchantments(Session session, List<PropertiesEnchantmentRegistry> enchantments)
            : base(GameEventType.MagicDispelMultipleEnchantments, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(enchantments);
        }
    }
}
