
using ACE.Entity;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
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
