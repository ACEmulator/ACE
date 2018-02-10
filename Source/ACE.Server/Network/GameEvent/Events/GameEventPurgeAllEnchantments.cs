namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventPurgeAllEnchantments : GameEventMessage
    {
        public GameEventPurgeAllEnchantments(Session session)
            : base(GameEventType.PurgeAllEnchantments, GameMessageGroup.UIQueue, session)
        {
            // do nothing here
        }
    }
}
