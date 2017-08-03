using ACE.Network.GameEvent.Events;
using ACE.Entity.Actions;
using System.Collections.Generic;

namespace ACE.Entity
{
    public sealed class Book : WorldObject
    {
        // private byte portalSocietyId;

        public Book(AceObject aceO)
            : base(aceO)
        {
            var weenie = Database.DatabaseManager.World.GetAceObjectByWeenie(AceObject.WeenieClassId);
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

                // TODO - GET THESE FROM THE DATABASE
                uint aceObjectId = 2012229680;
                uint pages = 1;
                string authorName = "Training Master";
                string authorAccount = "beer good";
                uint authorID = 0xFFFFFFFF;

                List<PageData> pageData = new List<PageData>();
                PageData myPage = new PageData();
                myPage.AuthorID = authorID;
                myPage.AuthorName = authorName;
                myPage.AuthorAccount = authorAccount;
                pageData.Add(myPage);

                var BookDataResponse = new GameEventBookDataResponse(player.Session, aceObjectId, pages, pageData, "", 0, "");
                player.Session.Network.EnqueueSend(BookDataResponse);

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            });

            // Run on the player
            chain.EnqueueChain();
        }
    }
}
