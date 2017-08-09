using ACE.Network.GameEvent.Events;
using ACE.Entity.Actions;
using System.Collections.Generic;
using ACE.Network;
using ACE.DatLoader.FileTypes;

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

        // Called by the Landblock for books that are WorldObjects (some notes pinned to the ground, statues, pedestals and tips in training academy, etc
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

                // Make sure player is within the use radius of the item.
                SetupModel csetup = SetupModel.ReadFromDat(SetupTableId.Value);
                float radiusSquared = (UseRadius.Value + csetup.Radius) * (UseRadius.Value + csetup.Radius);
                float playerDistanceTo = player.Location.SquaredDistanceTo(Location);
                if (playerDistanceTo >= radiusSquared)
                {
                    ActionChain moveToBookChain = new ActionChain();

                    moveToBookChain.AddChain(player.CreateMoveToChain(Guid, 0.2f));
                    moveToBookChain.AddDelaySeconds(0.50); // Not sure what this is for, but it is copied from Door.cs

                    moveToBookChain.AddAction(this, () => HandleActionOnUse(playerId));

                    moveToBookChain.EnqueueChain();
                }
                else
                {
                    BookUseHandler(player.Session);
                }
            });

            // Run on the player
            chain.EnqueueChain();
        }

        // Called when the items is in a player's inventory
        public override void OnUse(Session session) {
            BookUseHandler(session);
        }

        /// <summary>
        /// One function to handle both Player.OnUse and Landblock.HandleACtionOnUse functions
        /// </summary>
        /// <param name="session"></param>
        private void BookUseHandler(Session session)
        {
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
