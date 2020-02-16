using System;
using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
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
            InitializePropertyDictionaries();
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Book(Biota biota) : base(biota)
        {
            InitializePropertyDictionaries();
            SetEphemeralValues();
        }

        private void InitializePropertyDictionaries()
        {
            if (Biota.PropertiesBook == null)
                Biota.PropertiesBook = new PropertiesBook();
            if (Biota.PropertiesBookPageData == null)
                Biota.PropertiesBookPageData = new List<PropertiesBookPageData>();
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Book;

            SetProperty(PropertyInt.AppraisalPages, Biota.PropertiesBookPageData.Count);

            SetProperty(PropertyInt.AppraisalMaxPages, Biota.PropertiesBook.MaxNumPages);
        }

        public void SetProperties(string name, string shortDesc, string inscription, string scribeName, string scribeAccount)
        {
            if (!string.IsNullOrEmpty(name)) SetProperty(PropertyString.Name, name);
            if (!string.IsNullOrEmpty(shortDesc)) SetProperty(PropertyString.ShortDesc, shortDesc);
            if (!string.IsNullOrEmpty(inscription)) SetProperty(PropertyString.Inscription, inscription);
            if (!string.IsNullOrEmpty(scribeName)) SetProperty(PropertyString.ScribeName, scribeName);
            if (!string.IsNullOrEmpty(scribeAccount)) SetProperty(PropertyString.ScribeAccount, scribeAccount);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            var player = worldObject as Player;
            if (player == null) return;

            int maxChars = Biota.PropertiesBook.MaxNumCharsPerPage;
            int maxPages = Biota.PropertiesBook.MaxNumPages;

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

            var pages = Biota.PropertiesBookPageData.Clone(BiotaDatabaseLock);

            bool ignoreAuthor = IgnoreAuthor ?? false;

            string inscription;
            if (Inscription != null)
                inscription = Inscription;
            else
                inscription = "";

            var bookDataResponse = new GameEventBookDataResponse(player.Session, Guid.Full, maxChars, maxPages, pages, inscription, authorID, authorName, ignoreAuthor);
            player.Session.Network.EnqueueSend(bookDataResponse);
        }

        public PropertiesBookPageData AddPage(uint authorId, string authorName, string authorAccount, bool ignoreAuthor, string pageText, out int index)
        {
            if (Biota.PropertiesBookPageData.GetPageCount(BiotaDatabaseLock) >= Biota.PropertiesBook.MaxNumPages)
            {
                index = -1;
                return null;
            }

            var page = new PropertiesBookPageData
            {
                AuthorId = authorId,
                AuthorName = authorName,
                AuthorAccount = authorAccount,
                IgnoreAuthor = ignoreAuthor,
                PageText = pageText
            };

            Biota.PropertiesBookPageData.AddPage(page, out index, BiotaDatabaseLock);
            ChangesDetected = true;

            var newPageCount = Biota.PropertiesBookPageData.GetPageCount(BiotaDatabaseLock);
            SetProperty(PropertyInt.AppraisalPages, newPageCount);

            return page;
        }

        public bool ModifyPage(int index, string pageText, Player player)
        {
            var page = Biota.PropertiesBookPageData.GetPage(index, BiotaDatabaseLock);

            if (page == null || page.PageText.Equals(pageText))
                return false;

            if (page.IgnoreAuthor || (player.Guid.Full == page.AuthorId && player.Name == page.AuthorName && player.Account.AccountName == page.AuthorAccount) || player is Sentinel || player is Admin)
            {
                page.AuthorAccount = player.Account.AccountName;
                page.AuthorId = player.Guid.Full;
                page.AuthorName = player.Name;
                page.PageText = pageText;
                ChangesDetected = true;
            }
            else
                return false;

            return true;
        }

        public bool DeletePage(int index, Player player)
        {
            var page = Biota.PropertiesBookPageData.GetPage(index, BiotaDatabaseLock);

            if (page == null || (!page.IgnoreAuthor && player.Guid.Full != page.AuthorId && !(player is Sentinel) && !(player is Admin)))
                return false;

            Biota.PropertiesBookPageData.RemovePage(index, BiotaDatabaseLock);
            ChangesDetected = true;

            var newPageCount = Biota.PropertiesBookPageData.GetPageCount(BiotaDatabaseLock);
            SetProperty(PropertyInt.AppraisalPages, newPageCount);

            return true;
        }

        public PropertiesBookPageData GetPage(int index)
        {
            return Biota.PropertiesBookPageData.GetPage(index, BiotaDatabaseLock);
        }
    }
}
