using System.Collections.Generic;

using ACE.Entity.Models;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventBookDataResponse : GameEventMessage
    {
        public GameEventBookDataResponse(Session session, uint bookID, int maxChars, int maxPages, List<PropertiesBookPageData> pageData, string inscription, uint authorId, string authorName, bool ignoreAuthor)
            : base(GameEventType.BookDataResponse, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(bookID);

            // PCAPs show these page numbers were always the same regardless of if the pages were filled or blank.
            Writer.Write(maxPages); // maxNumPages          
            Writer.Write(maxPages); // numPages

            Writer.Write(maxChars); // maxNumCharsPerPage

            Writer.Write(pageData.Count);
            for (int i = 0; i < pageData.Count; i++)
            {
                Writer.Write(pageData[i].AuthorId);
                Writer.WriteString16L(pageData[i].AuthorName);
                // Check if player is admin and hide AuthorAccount if not. Potential security hole if we are sending out account usernames.
                if (session.Player.IsAdmin || session.Player.IsSentinel || session.Player.IsEnvoy || session.Player.IsArch || session.Player.IsPsr)
                    Writer.WriteString16L(pageData[i].AuthorAccount);
                else
                    Writer.WriteString16L("beer good");

                // With this flag set, it tells the client to always read the next two items. 
                // Might result in more data than retail in some instances, but easier to manage and control for us.
                Writer.Write(0xFFFF0002); 
                
                if (pageData[i].PageText != null) // This will always be null for this event.
                {
                    Writer.Write(1); // Text Included
                    if (ignoreAuthor)
                        Writer.Write(1); // Ignore Author
                    else
                        Writer.Write(0); // Ignore Author
                    Writer.WriteString16L(pageData[i].PageText);
                }
                else
                {
                    Writer.Write(0); // Text Included
                    if (ignoreAuthor)
                        Writer.Write(1); // Ignore Author
                    else
                        Writer.Write(0); // Ignore Author
                }
            }

            Writer.WriteString16L(inscription);
            Writer.Write(authorId);
            Writer.WriteString16L(authorName);
        }
    }
}
