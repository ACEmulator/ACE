using ACE.Entity;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventBookPageDataResponse : GameEventMessage
    {
        public GameEventBookPageDataResponse(Session session, uint bookID, PageData pageData)
            : base(GameEventType.BookPageDataResponse, GameMessageGroup.Group09, session)
        {
            Writer.Write(bookID);
            Writer.Write(pageData.PageIdx);
            Writer.Write(pageData.AuthorID);
            Writer.WriteString16L(pageData.AuthorName);
            Writer.WriteString16L(pageData.AuthorAccount);
            Writer.Write(0xFFFF0002); // flags
            Writer.Write(1); // textIncluded
            Writer.Write(1); // ignoreAuthor
            Writer.WriteString16L(pageData.PageText);
        }
    }
}