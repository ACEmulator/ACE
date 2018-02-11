namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventYourDeath : GameEventMessage
    {
        public GameEventYourDeath(Session session, string deathMessage)
            : base(GameEventType.VictimNotification, GameMessageGroup.UIQueue, session)
        {
            // Spell out your death in text:
            Writer.WriteString16L(deathMessage);
        }
    }
}
