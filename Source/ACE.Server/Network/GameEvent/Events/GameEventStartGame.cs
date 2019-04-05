using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Starts a chess game
    /// </summary>
    public class GameEventStartGame: GameEventMessage
    {
        public GameEventStartGame(Session session, ObjectGuid boardGuid, ChessColor color)
            : base(GameEventType.StartGame, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write((int)color);     // which team that should go first
        }
    }
}
