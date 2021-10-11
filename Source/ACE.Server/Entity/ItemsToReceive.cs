using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Database;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Helper class to verify player has enough free inventory slots / container slots / burden to receive some items
    /// </summary>
    public class ItemsToReceive
    {
        /// <summary>
        /// The player to receive the items
        /// </summary>
        private Player player { get; set; }

        public int RequiredInventorySlots { get; set; }
        public int RequiredContainerSlots { get; set; }
        public int RequiredBurden { get; set; }

        /// <summary>
        /// The total amount of items needing to be created.
        /// </summary>
        public int RequiredSlots => RequiredContainerSlots + RequiredInventorySlots;

        private int playerFreeInventorySlots { get; set; }
        private int playerFreeContainerSlots { get; set; }
        private int playerAvailableBurden { get; set; }

        public bool PlayerOutOfInventorySlots => RequiredInventorySlots > playerFreeInventorySlots;
        public bool PlayerOutOfContainerSlots => RequiredContainerSlots > playerFreeContainerSlots;
        public bool PlayerExceedsAvailableBurden => RequiredBurden > playerAvailableBurden;

        public bool PlayerExceedsLimits => PlayerOutOfInventorySlots || PlayerOutOfContainerSlots || PlayerExceedsAvailableBurden;

        public ItemsToReceive(Player player)
        {
            this.player = player;

            playerFreeInventorySlots = player.GetFreeInventorySlots();
            playerFreeContainerSlots = player.GetFreeContainerSlots();
            playerAvailableBurden = player.GetAvailableBurden();
        }

        public bool Add(uint weenieClassId, int amount)
        {
            return Process(weenieClassId, amount);
        }

        public bool Remove(uint weenieClassId, int amount)
        {
            return Process(weenieClassId, amount, true);
        }

        private bool Process(uint weenieClassId, int amount, bool negate = false)
        {
            var requiredSlots = GetItemSlotAndBurdenRequirements(weenieClassId, amount, out var requiredEncumbrance, out var itemRequiresBackpackSlot);

            if (negate)
            {
                requiredSlots *= -1;
                requiredEncumbrance *= -1;
            }

            RequiredBurden += requiredEncumbrance;

            if (itemRequiresBackpackSlot)
                RequiredContainerSlots += requiredSlots;
            else
                RequiredInventorySlots += requiredSlots;

            return !PlayerExceedsLimits;
        }

        /// <summary>
        /// Returns the number of slots required for the items
        /// </summary>
        private int GetItemSlotAndBurdenRequirements(uint weenieClassId, int amount, out int requiredEncumbrance, out bool requiresBackpackSlot)
        {
            requiredEncumbrance = 0;
            requiresBackpackSlot = false;

            var item = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (item == null || item.IsVendorService())
                return 0;

            requiresBackpackSlot = item.RequiresBackpackSlotOrIsContainer();

            if (!item.IsStackable())
            {
                requiredEncumbrance = amount * item.GetProperty(PropertyInt.EncumbranceVal) ?? 0;
                return amount;
            }

            var itemStackUnitEncumbrance = item.GetStackUnitEncumbrance();

            var itemMaxStackSize = item.GetMaxStackSize();

            requiredEncumbrance = amount * itemStackUnitEncumbrance;

            var itemStacks = amount / itemMaxStackSize;

            if (amount % itemMaxStackSize > 0)
                itemStacks++;

            return itemStacks;
        }
    }
}
