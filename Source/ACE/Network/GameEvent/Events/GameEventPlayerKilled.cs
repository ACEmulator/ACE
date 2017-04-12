using ACE.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPlayerKilled : GameEventMessage
    {
        public GameEventPlayerKilled(Session session, string deathText)
            : base(GameEventType.PlayerKilled, GameMessageGroup.Group0A, session)
        {
            // Do nothing yet.
            Writer.WriteString16L(deathText);
        }
    }
}