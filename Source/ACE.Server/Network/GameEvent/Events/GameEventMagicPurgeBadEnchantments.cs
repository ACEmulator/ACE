namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Silently remove all negative enchantments from a player.
    /// </summary>
    public class GameEventMagicPurgeBadEnchantments : GameEventMessage
    {
        public GameEventMagicPurgeBadEnchantments(Session session)
            : base(GameEventType.MagicPurgeBadEnchantments, GameMessageGroup.UIQueue, session)
        {
            // nothing else to send here
        }
    }
}
