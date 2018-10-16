namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Remove an enchantment from your character.
    /// </summary>
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
