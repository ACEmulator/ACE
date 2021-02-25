using ACE.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePlayerKilled : GameMessage
    {
        public GameMessagePlayerKilled(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
            : base(GameMessageOpcode.PlayerKilled, GameMessageGroup.UIQueue)
        {
            // player broadcasts when they die, including to self
            Writer.WriteString16L(deathMessage);
            Writer.WriteGuid(victimId);
            Writer.WriteGuid(killerId);
        }
    }
}
