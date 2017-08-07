using ACE.Network.GameEvent.Events;
using ACE.Entity.Actions;
using System.Collections.Generic;
using ACE.Network;

namespace ACE.Entity
{
    public sealed class Book : WorldObject
    {
        private AceObject ao;

        public Book(AceObject aceO)
            : base(aceO)
        {
            // var weenie = Database.DatabaseManager.World.GetAceObjectByWeenie(AceObject.WeenieClassId);
            ao = aceO;
        }

        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                uint aceObjectId = Guid.Full;
                uint pages = (uint)PropertiesBook.Count;
                string authorName = ao.BookAuthorName;
                string authorAccount = ao.BookAuthorAccount;
                uint authorID;
                if (ao.BookAuthorId == null)
                    authorID = 0xFFFFFFFF;
                else
                    authorID = (uint)ao.BookAuthorId;

                List<PageData> pageData = new List<PageData>();
                foreach (KeyValuePair<uint, AceObjectPropertiesBook> p in PropertiesBook)
                {
                    PageData myPage = new PageData();
                    myPage.AuthorID = p.Value.AuthorId;
                    myPage.AuthorName = p.Value.AuthorName;
                    myPage.AuthorAccount = p.Value.AuthorAccount;
                    pageData.Add(myPage);
                }

                bool ignoreAuthor;
                if (IgnoreAuthor == null)
                    ignoreAuthor = false;
                else
                    ignoreAuthor = (bool)IgnoreAuthor;

                var bookDataResponse = new GameEventBookDataResponse(player.Session, aceObjectId, pages, pageData, "", 0, "", ignoreAuthor);
                player.Session.Network.EnqueueSend(bookDataResponse);

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            });

            // Run on the player
            chain.EnqueueChain();
        }

        public override void OnUse(Session session) {
            uint aceObjectId = Guid.Full;
            uint pages = (uint)PropertiesBook.Count;
            string authorName = ao.BookAuthorName;
            string authorAccount = ao.BookAuthorAccount;
            uint authorID;
            if (ao.BookAuthorId == null)
                authorID = 0xFFFFFFFF;
            else
                authorID = (uint)ao.BookAuthorId;

            List<PageData> pageData = new List<PageData>();
            foreach (KeyValuePair<uint, AceObjectPropertiesBook> p in PropertiesBook)
            {
                PageData myPage = new PageData();
                myPage.AuthorID = p.Value.AuthorId;
                myPage.AuthorName = p.Value.AuthorName;
                myPage.AuthorAccount = p.Value.AuthorAccount;
                pageData.Add(myPage);
            }

            bool ignoreAuthor;
            if (IgnoreAuthor == null)
                ignoreAuthor = false;
            else
                ignoreAuthor = (bool)IgnoreAuthor;

            var bookDataResponse = new GameEventBookDataResponse(session, aceObjectId, pages, pageData, "", 0, "", ignoreAuthor);
            session.Network.EnqueueSend(bookDataResponse);

            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(sendUseDoneEvent);
        }
    }
}
