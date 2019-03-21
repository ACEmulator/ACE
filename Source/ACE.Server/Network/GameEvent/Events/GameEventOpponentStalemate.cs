using System;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    /// <summary>
    /// Chess opponent stalemate
    /// </summary>
    public class GameEventOpponentStalemate : GameEventMessage
    {
        public GameEventOpponentStalemate(Session session, ObjectGuid boardGuid, ChessColor color, bool stalemate)
            : base(GameEventType.OpponentStalemate, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write((int)color);
            Writer.Write(Convert.ToInt32(stalemate));  // 1 = offering stalemate, 0 = retracting stalemate
        }
    }
}
