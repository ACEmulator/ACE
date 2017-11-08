﻿// WeenieType.Book

using ACE.Network.GameEvent.Events;
using ACE.Entity.Actions;
using System.Collections.Generic;
using ACE.Network;

namespace ACE.Entity
{
    public sealed class Book : WorldObject
    {
        public Book(AceObject aceO)
            : base(aceO)
        {
            Pages = (int)PropertiesBook.Count; // Set correct Page Count for appraisal based on data actually in database.
            MaxPages = MaxPages ?? 1; // If null, set MaxPages to 1.
        }

        // Called by the Landblock for books that are WorldObjects (some notes pinned to the ground, statues, pedestals and tips in training academy, etc
        public override void ActOnUse(ObjectGuid playerId)
        {
            Player player = CurrentLandblock.GetObject(playerId) as Player;
            if (player == null)
            {
                return;
            }

            // Make sure player is within the use radius of the item.
            if (!player.IsWithinUseRadiusOf(this))
                player.DoMoveTo(this);
            else
            {
                BookUseHandler(player.Session);
            }
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
