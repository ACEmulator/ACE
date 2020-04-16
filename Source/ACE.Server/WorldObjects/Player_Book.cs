
using ACE.Entity;
using ACE.Entity.Models;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void ReadBook(uint bookGuid)
        {
            // This appears to have been sent in response 0x00AA (not pcapped)
            // When a book is blank, client automatically adds a "blank" page after opening, gets a GameEventBookAddPageResponse success and then sends 0x00AA, expecting this response or the page/book is not writable or usable, unless it is closed and opened again

            // TODO: Do we want to throttle this request, like appraisals?

            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            // found book
            if (book != null)
            {
                int maxChars = book.Biota.PropertiesBook.MaxNumCharsPerPage;
                int maxPages = book.Biota.PropertiesBook.MaxNumPages;

                string authorName;
                if (book.ScribeName != null)
                    authorName = book.ScribeName;
                else
                    authorName = "";

                //string authorAccount;
                //if (book.ScribeAccount != null)
                //    authorAccount = ScribeAccount;
                //else
                //    authorAccount = "";

                //uint authorID = ScribeIID ?? 0xFFFFFFFF;
                uint authorID = (book.ScribeIID.HasValue) ? (uint)book.ScribeIID : 0xFFFFFFFF;

                var pages = book.Biota.PropertiesBookPageData.Clone(book.BiotaDatabaseLock);

                bool ignoreAuthor = book.IgnoreAuthor ?? false;

                string inscription;
                if (book.Inscription != null)
                    inscription = book.Inscription;
                else
                    inscription = "";

                var bookDataResponse = new GameEventBookDataResponse(Session, book.Guid.Full, maxChars, maxPages, pages, inscription, authorID, authorName, ignoreAuthor);
                Session.Network.EnqueueSend(bookDataResponse);
            }
        }

        public void ReadBookPage(uint bookGuid, int pageNum)
        {
            // This is completely unused. 

            // TODO: Do we want to throttle this request, like appraisals?

            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            // found book
            if (book != null)
            {
                var page = book.GetPage(pageNum);

                if (page != null)
                {
                    var pdr = new GameEventBookPageDataResponse(Session, book.Guid.Full, pageNum, page);

                    Session.Network.EnqueueSend(pdr);
                }
            }
        }

        public void HandleActionBookAddPage(uint bookGuid)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var page = book.AddPage(Guid.Full, Name, Session.Account, book.IgnoreAuthor ?? false, "", out var index);

            if (page != null)
                Session.Network.EnqueueSend(new GameEventBookAddPageResponse(Session, bookGuid, index, true));
        }

        public void HandleActionBookModifyPage(uint bookGuid, int pageId, string pageText)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var success = book.ModifyPage(pageId, pageText, this);

            Session.Network.EnqueueSend(new GameEventBookModifyPageResponse(Session, bookGuid, pageId, true));
        }

        public void HandleActionBookDeletePage(uint bookGuid, int pageId)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var success = book.DeletePage(pageId, this);

            Session.Network.EnqueueSend(new GameEventBookDeletePageResponse(Session, bookGuid, pageId, success));
        }
    }
}
