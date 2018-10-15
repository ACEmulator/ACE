namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Silently remove an enchantment from your character (no message in the chat window).
    /// </summary>
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
