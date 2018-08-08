using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Add Title to Title Registry
        /// </summary>
        /// <param name="titleId">Id of Title to Add</param>
        /// <param name="setAsDisplayTitle">If this is true, make this the player's current title</param>
        public void AddTitle(uint titleId, bool setAsDisplayTitle = false)
        {
            if (!Enum.IsDefined(typeof(CharacterTitle), titleId))
                return;

            var titlebook = new List<uint>();

            foreach (var title in Character.CharacterPropertiesTitleBook)
                titlebook.Add(title.TitleId);

            NumCharacterTitles = titlebook.Count();

            bool sendMsg = false;
            bool notifyNewTitle = false;

            if (!titlebook.Contains(titleId))
            {
                Character.CharacterPropertiesTitleBook.Add(new Database.Models.Shard.CharacterPropertiesTitleBook { CharacterId = Guid.Full, TitleId = titleId });
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

            if (sendMsg && FirstEnterWorldDone.Value)
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

        public void HandleActionSetTitle(uint title)
        {
            AddTitle(title, true);
        }

        public void SetTitle(CharacterTitle title)
        {
            AddTitle(title, true);
        }
    }
}
