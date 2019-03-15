using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        /// <summary>
        /// For item leveling,
        /// ~ the amount of item xp per level
        /// </summary>
        public long? ItemBaseXp
        {
            get => GetProperty(PropertyInt64.ItemBaseXp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.ItemBaseXp); else SetProperty(PropertyInt64.ItemBaseXp, value.Value); }
        }

        public int? ItemMaxLevel
        {
            get => GetProperty(PropertyInt.ItemMaxLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemMaxLevel); else SetProperty(PropertyInt.ItemMaxLevel, value.Value); }
        }

        /// <summary>
        /// The total amount of XP earned for this item
        /// </summary>
        public long? ItemTotalXp
        {
            get => GetProperty(PropertyInt64.ItemTotalXp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt64.ItemTotalXp); else SetProperty(PropertyInt64.ItemTotalXp, value.Value); }
        }

        /// <summary>
        /// The item xp formula type
        /// determines the amount of XP for each item level
        /// </summary>
        public ItemXpStyle? ItemXpStyle
        {
            get => (ItemXpStyle?)GetProperty(PropertyInt.ItemXpStyle);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ItemXpStyle); else SetProperty(PropertyInt.ItemXpStyle, (int)value.Value); }
        }

        /// <summary>
        /// Returns TRUE if this item is part of a set
        /// </summary>
        /// <returns></returns>
        public bool HasItemSet()
        {
            return false;
        }

        /// <summary>
        /// Returns TRUE if this item can be leveled
        /// </summary>
        public bool HasItemLevel()
        {
            // seems like ItemMaxLevel would be good enough here,
            // but using the client formula from ItemExamineUI::Appraisal_ShowItemLevelInfo
            return ItemBaseXp != null && ItemBaseXp > 0 &&
                   ItemMaxLevel != null && ItemMaxLevel > 0 &&
                   ItemXpStyle != null && ItemXpStyle > 0;
        }

        /// <summary>
        /// Grants XP to an item that can be leveled up
        /// </summary>
        /// <param name="amount">The amount of item XP to add</param>
        public long EarnItemXP(long amount)
        {
            if (!HasItemLevel())
            {
                Console.WriteLine($"{Name}.GrantItemXp({amount}): no item level");
                return 0;
            }

            var itemTotalXp = ItemTotalXp ?? 0;
            var prevAmount = itemTotalXp;
            itemTotalXp += amount;

            var maxLevelXp = (long)ExperienceSystem.ItemLevelToTotalXP(ItemMaxLevel.Value, (ulong)ItemBaseXp.Value, ItemMaxLevel.Value, ItemXpStyle.Value);

            if (itemTotalXp > maxLevelXp)
                itemTotalXp = maxLevelXp;

            if (itemTotalXp == prevAmount)
                return 0;

            ItemTotalXp = itemTotalXp;

            // return amount added
            return itemTotalXp - prevAmount;
        }
    }
}
