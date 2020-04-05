using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventBookDeletePageResponse : GameEventMessage
    {
        public GameEventBookDeletePageResponse(Session session, uint bookGuid, int page, bool success)
            : base(GameEventType.BookDeletePageResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(bookGuid);
            Writer.Write(page);     // 0-based
            Writer.Write(Convert.ToUInt32(success));
        }
    }
}
