namespace ACE.Network.GameEvent.Events
{
    public class GameEventYourDeath : GameEventMessage
    {
        public GameEventYourDeath(Session session)
            : base(GameEventType.YourDeath, GameMessageGroup.Group09, session)
        {
            // Do nothing yet.
        }
    }
}