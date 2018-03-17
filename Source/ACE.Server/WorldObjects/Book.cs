using System;
using System.Collections.Generic;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public sealed class Book : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Book(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            //SetProperty(PropertyInt.EncumbranceVal, 0);
            //SetProperty(PropertyInt.Value, 0);

            //SetProperty(PropertyBool.IgnoreAuthor, false);
            //SetProperty(PropertyInt.AppraisalPages, 0);
            //SetProperty(PropertyInt.AppraisalMaxPages, 1);

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Book(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Book;

            SetProperty(PropertyInt.AppraisalPages, Biota.BiotaPropertiesBookPageData.Count);
            SetProperty(PropertyInt.AppraisalMaxPages, Biota.BiotaPropertiesBook.MaxNumPages);
        }

        public void SetProperties(string name, string shortDesc, string inscription, string scribeName, string scribeAccount)
        {
            if (!String.IsNullOrEmpty(name)) SetProperty(PropertyString.Name, name);
            if (!String.IsNullOrEmpty(shortDesc)) SetProperty(PropertyString.ShortDesc, shortDesc);
            if (!String.IsNullOrEmpty(inscription)) SetProperty(PropertyString.Inscription, inscription);
            if (!String.IsNullOrEmpty(scribeName)) SetProperty(PropertyString.ScribeName, scribeName);
            if (!String.IsNullOrEmpty(scribeAccount)) SetProperty(PropertyString.ScribeAccount, scribeAccount);
        }

        public void AddPage(uint authorId, string authorName, string authorAccount, bool ignoreAuthor, string pageText)
        {
            var page = new BiotaPropertiesBookPageData()
            {
                ObjectId = Biota.Id,
                PageId = (uint)Biota.BiotaPropertiesBookPageData.Count,
                AuthorId = authorId,
                AuthorName = authorName,
                IgnoreAuthor = ignoreAuthor,
                PageText = pageText
            };

            Biota.BiotaPropertiesBookPageData.Add(page);
        }

        // Called by the Landblock for books that are WorldObjects (some notes pinned to the ground, statues, pedestals and tips in training academy, etc
        public override void ActOnUse(ObjectGuid playerId)
        {
            var player = CurrentLandblock.GetObject(playerId) as Player;

            if (player == null)
                return;

            // Make sure player is within the use radius of the item.
            if (!player.IsWithinUseRadiusOf(this))
                player.DoMoveTo(this);
            else
                BookUseHandler(player.Session);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.
        /// </summary>
        public override void DoActionUseItem(Session session)
        {
            BookUseHandler(session);
        }

        /// <summary>
        /// One function to handle both Player.OnUse and Landblock.HandleACtionOnUse functions
        /// </summary>
        /// <param name="session"></param>
        private void BookUseHandler(Session session)
        {
            int maxChars = Biota.BiotaPropertiesBook.MaxNumCharsPerPage;
            int maxPages = Biota.BiotaPropertiesBook.MaxNumPages;

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

            //uint authorID = ScribeIID ?? 0xFFFFFFFF;
            uint authorID = (ScribeIID.HasValue) ? (uint)ScribeIID : 0xFFFFFFFF;

            List<PageData> pageData = new List<PageData>();
            foreach (var p in Biota.BiotaPropertiesBookPageData)
            {
                PageData newPage = new PageData();
                newPage.AuthorID = p.AuthorId;
                newPage.AuthorName = p.AuthorName;
                newPage.AuthorAccount = p.AuthorAccount;
                newPage.PageText = p.PageText;
                newPage.IgnoreAuthor = p.IgnoreAuthor;
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
