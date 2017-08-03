using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventBookDataResponse : GameEventMessage
    {
        public GameEventBookDataResponse(Session session, uint bookID, uint maxPages, List<PageData> pageData, string inscription, uint authorId, string authorName)
            : base(GameEventType.BookDataResponse, GameMessageGroup.Group09, session)
        {
            if (maxPages < pageData.Count)
            {
                // TODO - Some sort of error handling here. These numbers should match.
                maxPages = (uint)pageData.Count;
            }

            Writer.Write(bookID);
            Writer.Write(maxPages); // maxNumPages
            Writer.Write(pageData.Count); // numPages
            Writer.Write(1000); // maxNumCharsPerPage

            Writer.Write(pageData.Count);
            for (int i = 0; i < pageData.Count; i++)
            {
                Writer.Write(pageData[i].AuthorID);
                Writer.WriteString16L(pageData[i].AuthorName);
                Writer.WriteString16L(pageData[i].AuthorAccount);
                Writer.Write(0xFFFF0002);
                
                if (pageData[i].PageText != null) // This will always be null for this event.
                {
                    Writer.Write(1); // Text Included
                    Writer.Write(0); // Ignore Author
                    Writer.WriteString16L(pageData[i].PageText);
                }
                else
                {
                    Writer.Write(0); // Text Included
                    Writer.Write(1); // Ignore Author
                }
            }

            Writer.WriteString16L(inscription);
            Writer.Write(authorId);
            Writer.WriteString16L(authorName);
        }
    }
}
