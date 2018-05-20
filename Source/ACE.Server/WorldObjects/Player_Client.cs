using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        // =====================================
        // Character Options
        // =====================================

        public bool GetCharacterOption(CharacterOption option)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                return GetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()));

            return GetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()));
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                SetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()), value);
            else
                SetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()), value);
        }

        public bool GetCharacterOptions1(CharacterOptions1 option)
        {
            return (GetProperty(PropertyInt.CharacterOptions1) & (uint)option) != 0;
        }

        public void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            var options = GetProperty(PropertyInt.CharacterOptions1) ?? 0;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetProperty(PropertyInt.CharacterOptions1, options);
        }

        public void SetCharacterOptions1(int value)
        {
            SetProperty(PropertyInt.CharacterOptions1, value);
        }

        public bool GetCharacterOptions2(CharacterOptions2 option)
        {
            return (GetProperty(PropertyInt.CharacterOptions2) & (uint)option) != 0;
        }

        public void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            var options = GetProperty(PropertyInt.CharacterOptions2) ?? 0;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetProperty(PropertyInt.CharacterOptions2, options);
        }

        public void SetCharacterOptions2(int value)
        {
            SetProperty(PropertyInt.CharacterOptions2, value);
        }


        // =====================================
        // 1-9 Shortcuts
        // =====================================

        public List<Shortcut> GetShortcuts()
        {
            var shortcuts = new List<Shortcut>();

            foreach (var shortcut in Biota.BiotaPropertiesShortcutBar)
                shortcuts.Add(new Shortcut(shortcut));

            return shortcuts;
        }

        /// <summary>
        /// Handles the adding of items to 1-9 shortcut bar in lower-right corner.<para />
        /// Note that there are two rows. The top row is 1-9, the bottom row has no hotkeys.
        /// </summary>
        public void HandleActionAddShortcut(Shortcut shortcut)
        {
            // When a shortcut is added on top of an existing item, the client automatically sends the RemoveShortcut command for that existing item first, then will add the new item, and re-add the existing item to the appropriate place.

            var entity = new BiotaPropertiesShortcutBar { ObjectId = Biota.Id, ShortcutBarIndex = shortcut.Index, ShortcutObjectId = shortcut.ObjectId, Object = Biota };

            Biota.BiotaPropertiesShortcutBar.Add(entity);
            ChangesDetected = true;
        }

        /// <summary>
        /// Handles the removing of items from 1-9 shortcut bar in lower-right corner
        /// </summary>
        public void HandleActionRemoveShortcut(uint index)
        {
            var entity = Biota.BiotaPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index);

            if (entity != null)
            {
                Biota.BiotaPropertiesShortcutBar.Remove(entity);
                entity.Object = null;

                if (ExistsInDatabase && entity.ObjectId != 0)
                    DatabaseManager.Shard.RemoveEntity(entity, null);
            }
        }
    }
}
