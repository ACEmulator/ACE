using ACE.Entity;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Chess opponent turn
    /// </summary>
    public class GameEventOpponentTurn : GameEventMessage
    {
        public GameEventOpponentTurn(Session session, ObjectGuid boardGuid, ChessMoveData moveData)
            : base(GameEventType.OpponentTurn, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write((int)moveData.Color);
            Writer.Write(moveData);
        }
    }
}
