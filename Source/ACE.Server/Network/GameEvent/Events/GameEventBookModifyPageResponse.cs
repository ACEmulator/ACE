using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventBookModifyPageResponse : GameEventMessage
    {
        public GameEventBookModifyPageResponse(Session session, uint bookGuid, uint page, bool success)
            : base(GameEventType.BookModifyPageResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(bookGuid);
            Writer.Write(page);     // 0-based
            Writer.Write(Convert.ToUInt32(success));
        }
    }
}
