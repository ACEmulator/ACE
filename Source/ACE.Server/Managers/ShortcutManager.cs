using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Manages the 1-9 hotkeys in bottom-right corner
    /// </summary>
    public class ShortcutManager
    {
        public Player Player { get; }

        /// <summary>
        /// Returns TRUE if player has any saved shortcuts
        /// </summary>
        public bool HasShortcuts => NumShortcuts > 0;

        /// <summary>
        /// Returns the total number of player saved shortcuts
        /// </summary>
        public int NumShortcuts => Player.Biota.BiotaPropertiesShortcutBarObject.Count;

        /// <summary>
        /// Constructs a new shortcut manager for a player
        /// </summary>
        public ShortcutManager(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Serializes the list of shortcuts to network stream
        /// </summary>
        public void SendShortcuts(BinaryWriter writer)
        {
            var shortcuts = new List<Shortcut>();

            foreach (var shortcut in Player.Biota.BiotaPropertiesShortcutBarObject)
                shortcuts.Add(new Shortcut(shortcut));

            writer.Write(shortcuts);
        }

        /// <summary>
        /// Adds or updates an item on the shortcut bar
        /// </summary>
        public void AddItem(Shortcut shortcut)
        {
            var newItem = BuildBiota(shortcut);
            var existItem = GetItem(shortcut.Index);

            if (existItem != null)
                UpdateItem(existItem, newItem);
            else
                Player.Biota.BiotaPropertiesShortcutBarObject.Add(newItem);
        }

        /// <summary>
        /// Updates an existing item on the shortcut bar
        /// </summary>
        private void UpdateItem(BiotaPropertiesShortcutBar existItem, BiotaPropertiesShortcutBar newItem)
        {
            RemoveItem(existItem);
            Player.Biota.BiotaPropertiesShortcutBarObject.Add(newItem);
        }

        /// <summary>
        /// Removes an existing item from the shortcut bar
        /// </summary>
        /// <param name="index">The slot # of the shortcut, 0-based</param>
        public void RemoveItem(uint index)
        {
            var existItem = GetItem(index);
            if (existItem != null)
                RemoveItem(existItem);
        }

        /// <summary>
        /// Removes an existing item from the shortcut bar - internal method
        /// </summary>
        private void RemoveItem(BiotaPropertiesShortcutBar existItem)
        {
            Player.RemoveShortcut(existItem.ShortcutBarIndex);
        }

        /// <summary>
        /// Returns the item on the shortcut bar at index
        /// </summary>
        /// <param name="index">The slot # of the shortcut, 0-based</param>
        private BiotaPropertiesShortcutBar GetItem(uint index)
        {
            return Player.Biota.BiotaPropertiesShortcutBarObject.FirstOrDefault(s => s.ShortcutBarIndex == index);
        }

        /// <summary>
        /// Constructs a new BiotaPropertiesShortcutbar from a Shortcut
        /// </summary>
        private BiotaPropertiesShortcutBar BuildBiota(Shortcut shortcut)
        {
            var item = new BiotaPropertiesShortcutBar();
            item.ObjectId = Player.Guid.Full;
            item.ShortcutBarIndex = shortcut.Index;
            item.ShortcutObjectId = shortcut.ObjectId;
            return item;
        }
    }
}
