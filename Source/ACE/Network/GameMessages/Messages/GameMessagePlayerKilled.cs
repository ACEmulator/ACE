using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    /// <summary>
    /// 019E: Player Killed
    /// </summary>
    public class GameMessagePlayerKilled : GameMessage
    {
        public GameMessagePlayerKilled(string deathMessage, ObjectGuid victimId, ObjectGuid killerId)
            : base(GameMessageOpcode.PlayerKilled, GameMessageGroup.Group09)
        {
            Writer.WriteString16L(deathMessage);
            Writer.WriteGuid(victimId);
            Writer.WriteGuid(killerId);
        }
    }
}
