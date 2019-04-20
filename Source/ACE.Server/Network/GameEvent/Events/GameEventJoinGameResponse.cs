using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventJoinGameResponse : GameEventMessage
    {
        public GameEventJoinGameResponse(Session session, ObjectGuid boardGuid, ChessColor color)
            : base(GameEventType.JoinGameResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(boardGuid.Full);
            Writer.Write((int)color);     // -1 indicates failure, otherwise which team you are for this game
        }
    }
}
