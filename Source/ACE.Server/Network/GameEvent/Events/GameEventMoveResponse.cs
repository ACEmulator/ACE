using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Chess piece move response
    /// </summary>
    public class GameEventMoveResponse : GameEventMessage
    {
        public GameEventMoveResponse(Session session, ObjectGuid boardGuid, ChessMoveResult result)
            : base(GameEventType.MoveResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write((int)result);
        }
    }
}
