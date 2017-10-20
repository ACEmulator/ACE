using ACE.DatLoader.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventMagicUpdateEnchantment : GameEventMessage
    {
        public GameEventMagicUpdateEnchantment(Session session, SpellBase spellBase)
            : base(GameEventType.MagicUpdateEnchantment, GameMessageGroup.Group09, session)
        {
            var x = (spellBase.Category << 16) >= 1;
            var y = spellBase.Category & 0xFFFF;

            Writer.Align();
        }
    }
}
