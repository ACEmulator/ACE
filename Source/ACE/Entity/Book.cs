// WeenieType.Book
using System.Collections.Generic;
using System.Threading.Tasks;

using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Entity.Actions;

namespace ACE.Entity
{
    public sealed class Book : WorldObject
    {
        public Book()
        {
        }

        protected override async Task Init(AceObject aceO)
        {
            await base.Init(aceO);
            Pages = (int)PropertiesBook.Count; // Set correct Page Count for appraisal based on data actually in database.
            MaxPages = MaxPages ?? 1; // If null, set MaxPages to 1.
        }

        // Called by the Landblock for books that are WorldObjects (some notes pinned to the ground, statues, pedestals and tips in training academy, etc
        public override async Task ActOnUse(ObjectGuid playerId)
        {
            Player player = await CurrentLandblock.GetObject(playerId) as Player;
            if (player == null)
            {
                return;
            }

            // Make sure player is within the use radius of the item.
            if (!player.IsWithinUseRadiusOf(this))
                await player.MoveTo(this);
            else
            {
                BookUseHandler(player.Session);
            }
        }

        // Called when the items is in a player's inventory
        // Diable warning, matching OnUse Task interface
        #pragma warning disable 1998
        public override async Task OnUse(Session session) {
            BookUseHandler(session);
        }
        #pragma warning restore 1998

        /// <summary>
        /// One function to handle both Player.OnUse and Landblock.HandleACtionOnUse functions
        /// </summary>
        /// <param name="session"></param>
        private void BookUseHandler(Session session)
        {
            int maxChars = MaxCharactersPerPage ?? 1000;
            int maxPages = MaxPages ?? 1;

            string authorName;
            if (ScribeName != null)
                authorName = ScribeName;
            else
                authorName = "";

            string authorAccount;
            if (ScribeAccount != null)
                authorAccount = ScribeAccount;
            else
                authorAccount = "";

            uint authorID = Scribe ?? 0xFFFFFFFF;

            List<PageData> pageData = new List<PageData>();
            foreach (KeyValuePair<uint, AceObjectPropertiesBook> p in PropertiesBook)
            {
                PageData newPage = new PageData();
                newPage.AuthorID = p.Value.AuthorId;
                newPage.AuthorName = p.Value.AuthorName;
                newPage.AuthorAccount = p.Value.AuthorAccount;
                pageData.Add(newPage);
            }

            bool ignoreAuthor = IgnoreAuthor ?? false;

            string inscription;
            if (Inscription != null)
                inscription = Inscription;
            else
                inscription = "";

            var bookDataResponse = new GameEventBookDataResponse(session, Guid.Full, maxChars, maxPages, pageData, inscription, authorID, authorName, ignoreAuthor);
            session.Network.EnqueueSend(bookDataResponse);

            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(sendUseDoneEvent);
        }
    }
}
