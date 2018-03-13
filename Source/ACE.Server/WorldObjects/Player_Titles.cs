using System.Collections.Generic;
using System.Linq;

using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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

        public int NumCharacterTitles => Biota.BiotaPropertiesTitleBook.Count();

        /// <summary>
        /// Add Title to Title Registry
        /// </summary>
        /// <param name="titleId">Id of Title to Add</param>
        /// <param name="setAsDisplayTitle">If this is true, make this the player's current title</param>
        public void AddTitle(uint titleId, bool setAsDisplayTitle = false)
        {
            var titlebook = new List<uint>();

            //foreach (var title in Biota.BiotaPropertiesTitleBook)
            //    titlebook.Add(title.TitleId);

            //var message = new GameMessageSystemChat($"{amount} experience granted.", ChatMessageType.Advancement);
            //Session.Network.EnqueueSend(message);
        }

        public void AddTitle(CharacterTitle title, bool setAsDisplayTitle = false)
        {
            AddTitle((uint)title, setAsDisplayTitle);
        }
    }
}
