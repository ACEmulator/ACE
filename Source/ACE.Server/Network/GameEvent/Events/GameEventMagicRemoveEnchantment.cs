namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicRemoveEnchantment : GameEventMessage
    {
        public GameEventMagicRemoveEnchantment(Session session, uint spellID, uint layer)
            : base(GameEventType.MagicRemoveEnchantment, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellID);
            Writer.Write(layer);
        }
    }
}
