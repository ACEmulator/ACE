namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventMagicDispelEnchantment : GameEventMessage
    {
        public GameEventMagicDispelEnchantment(Session session, ushort spellID, ushort layer)
            : base(GameEventType.MagicDispelEnchantment, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(spellID);
            Writer.Write(layer);
        }
    }
}
