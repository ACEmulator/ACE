using System.Collections.Generic;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Book;

            SetProperty(PropertyInt.AppraisalPages, Biota.BiotaPropertiesBookPageData.Count);

            if (Biota.BiotaPropertiesBook != null)
                SetProperty(PropertyInt.AppraisalMaxPages, Biota.BiotaPropertiesBook.MaxNumPages);
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

            var bookDataResponse = new GameEventBookDataResponse(player.Session, Guid.Full, maxChars, maxPages, pageData, inscription, authorID, authorName, ignoreAuthor);
            player.Session.Network.EnqueueSend(bookDataResponse);
        }

        public BiotaPropertiesBookPageData AddPage(uint authorId, string authorName, string authorAccount, bool ignoreAuthor, string pageText)
        {
            var pages = Biota.GetBookAllPages(Guid.Full, BiotaDatabaseLock);

            if (pages == null || pages.Count == AppraisalMaxPages)
                return null;

            var page = new BiotaPropertiesBookPageData()
            {
                ObjectId = Biota.Id,
                PageId = (uint)pages.Count,
                AuthorId = authorId,
                AuthorName = authorName,
                AuthorAccount = authorAccount,
                IgnoreAuthor = ignoreAuthor,
                PageText = pageText
            };

            Biota.AddBookPage(page, BiotaDatabaseLock, out var alreadyExists);

            if (alreadyExists) return null;

            SetProperty(PropertyInt.AppraisalPages, pages.Count + 1);
            ChangesDetected = true;

            return page;
        }

        public bool ModifyPage(uint pageId, string pageText, Player player)
        {
            var page = Biota.GetBookPageData(Guid.Full, pageId, BiotaDatabaseLock);

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

        public bool DeletePage(uint pageId, Player player)
        {
            var pages = Biota.GetBookAllPages(Guid.Full, BiotaDatabaseLock);

            var page = Biota.GetBookPageData(Guid.Full, pageId, BiotaDatabaseLock);

            if (page == null || (!page.IgnoreAuthor && player.Guid.Full != page.AuthorId && !(player is Sentinel) && !(player is Admin)))
                return false;

            var success = Biota.DeleteBookPage(pageId, out var entity, BiotaDatabaseLock);

            if (!success)
                return false;

            if (pageId < pages.Count - 1)
            {
                // handle deleting page from middle of book
                for (var i = pageId + 1; i < pages.Count; i++)
                {
                    var page2 = Biota.GetBookPageData(Guid.Full, i, BiotaDatabaseLock);
                    page2.PageId--;
                }
            }
            SetProperty(PropertyInt.AppraisalPages, pages.Count - 1);
            ChangesDetected = true;

            return true;
        }

        public BiotaPropertiesBookPageData GetPage(uint pageId)
        {
            var page = Biota.GetBookPageData(Guid.Full, pageId, BiotaDatabaseLock);

            return page;
        }
    }
}
