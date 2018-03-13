using System.Collections.Generic;
using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        //public string Title
        //{
        //    get { return GetProperty(PropertyString.Title); }
        //    set { SetProperty(PropertyString.Title, value); }
        //}

        //public int? CharacterTitleId
        //{
        //    get { return GetProperty(PropertyInt.CharacterTitleId); }
        //    set { SetProperty(PropertyInt.CharacterTitleId, value); }
        //}


        //public int? NumCharacterTitles
        //{
        //    get { return GetProperty(PropertyInt.NumCharacterTitles); }
        //    set { SetProperty(PropertyInt.NumCharacterTitles, value); }
        //}

        public int? CharacterTitleId
        {
            get => GetProperty(PropertyInt.CharacterTitleId);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.CharacterTitleId); else SetProperty(PropertyInt.CharacterTitleId, value.Value); }
        }

        //public int NumCharacterTitles => Biota.BiotaPropertiesTitleBook.Count();

        public int? NumCharacterTitles
        {
            get => GetProperty(PropertyInt.NumCharacterTitles);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.NumCharacterTitles); else SetProperty(PropertyInt.NumCharacterTitles, value.Value); }
        }

        /// <summary>
        /// Add Title to Title Registry
        /// </summary>
        /// <param name="titleId">Id of Title to Add</param>
        /// <param name="setAsDisplayTitle">If this is true, make this the player's current title</param>
        public void AddTitle(uint titleId, bool setAsDisplayTitle = false)
        {
            var titlebook = new List<uint>();

            foreach (var title in Biota.BiotaPropertiesTitleBook)
                titlebook.Add(title.TitleId);

            NumCharacterTitles = titlebook.Count();

            bool sendMsg = false;
            bool notifyNewTitle = false;

            if (!titlebook.Contains(titleId))
            {
                Biota.BiotaPropertiesTitleBook.Add(new Database.Models.Shard.BiotaPropertiesTitleBook { ObjectId = Guid.Full, TitleId = titleId });
                titlebook.Add(titleId);
                NumCharacterTitles++;
                sendMsg = true;
                notifyNewTitle = true;
            }

            if (setAsDisplayTitle && CharacterTitleId != titleId)
            {
                CharacterTitleId = (int)titleId;
                sendMsg = true;
            }

            if (sendMsg && FirstEnterWorldDone)
            {
                var message = new GameEventUpdateTitle(Session, titleId, setAsDisplayTitle);
                Session.Network.EnqueueSend(message);
                if (notifyNewTitle)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You have been granted a new title."));
            }
        }

        public void AddTitle(CharacterTitle title, bool setAsDisplayTitle = false)
        {
            AddTitle((uint)title, setAsDisplayTitle);
        }

        public void SetTitle(uint title)
        {
            AddTitle(title, true);
        }

        public void SetTitle(CharacterTitle title)
        {
            AddTitle(title, true);
        }
    }
}
