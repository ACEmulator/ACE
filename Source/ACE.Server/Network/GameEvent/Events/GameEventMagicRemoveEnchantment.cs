namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicRemoveEnchantment : GameEventMessage
    {
        public GameEventMagicRemoveEnchantment(Session session, ushort spellID, ushort layer)
            : base(GameEventType.MagicRemoveEnchantment, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellID);
            Writer.Write(layer);
        }
    }
}
