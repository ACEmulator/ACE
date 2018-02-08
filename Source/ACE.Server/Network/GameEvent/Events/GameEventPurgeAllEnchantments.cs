namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventPurgeAllEnchantments : GameEventMessage
    {
        public GameEventPurgeAllEnchantments(Session session)
            : base(GameEventType.PurgeAllEnchantments, GameMessageGroup.Group09, session)
        {
            // do nothing here
        }
    }
}
