
using ACE.Entity;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void ReadBookPage(uint bookGuid, uint pageNum)
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
                    var pd = new PageData();

                    pd.AuthorAccount = page.AuthorAccount;
                    pd.AuthorID = page.AuthorId;
                    pd.AuthorName = page.AuthorName;
                    pd.IgnoreAuthor = page.IgnoreAuthor;
                    pd.PageIdx = page.PageId;
                    pd.PageText = page.PageText;

                    var pdr = new GameEventBookPageDataResponse(Session, book.Guid.Full, pd);

                    Session.Network.EnqueueSend(pdr);
                }
            }
        }

        public void HandleActionBookAddPage(uint bookGuid)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var page = book.AddPage(Guid.Full, Name, Session.Account, book.IgnoreAuthor ?? false, "");

            if (page != null)
                Session.Network.EnqueueSend(new GameEventBookAddPageResponse(Session, bookGuid, page.PageId, true));
        }

        public void HandleActionBookModifyPage(uint bookGuid, uint pageId, string pageText)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var success = book.ModifyPage(pageId, pageText, this);

            Session.Network.EnqueueSend(new GameEventBookModifyPageResponse(Session, bookGuid, pageId, true));
        }

        public void HandleActionBookDeletePage(uint bookGuid, uint pageId)
        {
            // find inventory book
            var book = FindObject(new ObjectGuid(bookGuid), SearchLocations.MyInventory | SearchLocations.LastUsedHook, out var container, out var rootOwner, out var wasEquipped) as Book;
            if (book == null) return;

            var success = book.DeletePage(pageId, this);

            Session.Network.EnqueueSend(new GameEventBookDeletePageResponse(Session, bookGuid, pageId, success));
        }
    }
}
