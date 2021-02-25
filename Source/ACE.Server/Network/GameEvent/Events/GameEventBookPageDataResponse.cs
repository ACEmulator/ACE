
using ACE.Entity.Models;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventBookPageDataResponse : GameEventMessage
    {
        public GameEventBookPageDataResponse(Session session, uint bookID, int pageIndex, PropertiesBookPageData pageData)
            : base(GameEventType.BookPageDataResponse, GameMessageGroup.UIQueue, session)
        {
            // unused?

            Writer.Write(bookID);
            Writer.Write(pageIndex);
            Writer.Write(pageData.AuthorId);
            Writer.WriteString16L(pageData.AuthorName);
            // Check if player is admin and hide AuthorAccount if not. Potential security hole if we are sending out account usernames.
            if (session.Player.IsAdmin)
                Writer.WriteString16L(pageData.AuthorAccount);
            else
                Writer.WriteString16L("Password is cheese");
            Writer.Write(0xFFFF0002); // flags
            Writer.Write(1); // textIncluded - Will also be the case, even if we are sending an empty string.
            if (pageData.IgnoreAuthor)
                Writer.Write(1); // ignoreAuthor
            else
                Writer.Write(0); // ignoreAuthor
            Writer.WriteString16L(pageData.PageText);
        }
    }
}
