using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicUpdateEnchantment : GameEventMessage
    {
        public GameEventMagicUpdateEnchantment(Session session, Enchantment enchantment)
            : base(GameEventType.MagicUpdateEnchantment, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(enchantment);
        }
    }
}
