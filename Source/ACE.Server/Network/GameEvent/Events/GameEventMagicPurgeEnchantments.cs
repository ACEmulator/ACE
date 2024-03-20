namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Silently remove all enchantments from your character, e.g. when you die (no message in the chat window).
    /// </summary>
    public class GameEventMagicPurgeEnchantments : GameEventMessage
    {
        public GameEventMagicPurgeEnchantments(Session session)
            : base(GameEventType.MagicPurgeEnchantments, GameMessageGroup.UIQueue, session, 4)
        {
            // nothing else to send here
        }
    }
}
