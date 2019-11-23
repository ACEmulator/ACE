using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The different categories death items are sorted into
    /// This is used to determine the adjusted value
    /// </summary>
    public enum DeathItemCategory
    {
        None,
        MeleeWeapon,
        MissileWeapon,
        MagicCaster,
        Armor,
        Clothing,
        Jewelry,
        Food,
        Gem,
        SpellComponent,
        ManaStone,
        CraftingIngredient,
        ParchmentBook,
        Key,
        TradeNote,
        Misc
    };

    /// <summary>
    /// An inventory sorted by which items drop first during death
    /// </summary>
    public class DeathItems
    {
        public List<DeathItem> Inventory;

        public Dictionary<DeathItemCategory, List<DeathItem>> InventoryGroups;  // for debugging

        /// <summary>
        /// Constructs a list of death items
        /// </summary>
        /// <param name="inventory">The list of possible inventory items that can be dropped on death</param>
        public DeathItems(List<WorldObject> inventory)
        {
            Inventory = new List<DeathItem>();

            foreach (var item in inventory)
                Inventory.Add(new DeathItem(item));

            BuildGroups();

            // Here's the system, taken straight from the developer spec, that Asheron's Call uses to determine the way items are lost: 

            // First, we sort the inventory by order of value.
            // Second, we go back through the inventory starting at the most expensive item and look at each item's category.
            // If we've seen the item category before, we divide the value of the item in half.
            // At this point, we add a random 0 - 10 % variance to each item's value.

            // Reference: http://acpedia.org/wiki/Recovering_from_Death

            // exclude unknown types
            Inventory = Inventory.Where(i => i.Category != DeathItemCategory.None).ToList();

            // sort by original value and category
            // todo: Switch this to use ThenBy to avoid having to iterate over the list twice. If we use ThenBy, the left/right sides probably need to be swapped.
            // todo: If you do switch this, it needs to be tested thoroughly.
            Inventory = Inventory.OrderByDescending(i => i.AdjustedValue).OrderBy(i => i.Category).ToList();

            // halve the values of every item, except the most valued item in each category
            HalfValues(Inventory);

            // adjusted value by randomized variance
            foreach (var item in Inventory)
                item.AdjustedValue = GetValueWithVariance(item.AdjustedValue);

            // re-sort by final adjusted value
            Inventory = Inventory.OrderByDescending(i => i.AdjustedValue).ToList();
        }

        /// <summary>
        /// Builds the list of death items grouped by category
        /// </summary>
        public void BuildGroups()
        {
            InventoryGroups = new Dictionary<DeathItemCategory, List<DeathItem>>();

            foreach (var item in Inventory)
            {
                if (!InventoryGroups.ContainsKey(item.Category))
                    InventoryGroups.Add(item.Category, new List<DeathItem>());

                InventoryGroups[item.Category].Add(item);
            }
        }

        /// <summary>
        /// Randomize the original item values by a variance %
        /// </summary>
        public static float MaxVariance = 0.10f;

        /// <summary>
        /// Returns the input value adjusted between -variance and +variance
        /// </summary>
        public static int GetValueWithVariance(int value)
        {
            // At this point, we add a random 0 - 10 % variance to each item's value.
            // should this be +/- 10%, or even +/- 5%?

            // http://www.postcount.net/forum/showthread.php?79784-death-item-formula
            // according to this post, it could be +/- 10%

            var variance = ThreadSafeRandom.Next(-MaxVariance, MaxVariance);

            return (int)(value + value * variance);
        }

        /// <summary>
        /// Halves the values of every item, except the most valued item in each category
        /// </summary>
        public static void HalfValues(List<DeathItem> inventory)
        {
            var prevCategory = DeathItemCategory.None;

            // assumes list has been pre-sorted by category and item value
            foreach (var item in inventory)
            {
                if (item.Category != prevCategory)
                    prevCategory = item.Category;
                else
                    item.AdjustedValue /= 2;
            }
        }

        /// <summary>
        /// An inventory item that can be possibly dropped on death
        /// </summary>
        public class DeathItem
        {
            public WorldObject WorldObject;
            public DeathItemCategory Category;
            public string Name;

            /// <summary>
            /// The original value is randomized slightly,
            /// and the non-most expensive items in each category are halved
            /// </summary>
            public int AdjustedValue;

            public DeathItem(WorldObject wo)
            {
                WorldObject = wo;
                Name = wo.Name;
                Category = GetCategory(wo);
                AdjustedValue = wo.Value ?? 0;  // stack size?
                if ((wo.StackSize ?? 1) > 1)
                    AdjustedValue /= wo.StackSize.Value;
            }

            public static DeathItemCategory GetCategory(WorldObject wo)
            {
                switch (wo.ItemType)
                {
                    case ItemType.MeleeWeapon:
                        return DeathItemCategory.MeleeWeapon;
                    case ItemType.MissileWeapon:
                        return DeathItemCategory.MissileWeapon;
                    case ItemType.Caster:
                    case ItemType.MagicWieldable:
                        return DeathItemCategory.MagicCaster;
                    case ItemType.Armor:
                        return DeathItemCategory.Armor;
                    case ItemType.Clothing:
                        return DeathItemCategory.Clothing;
                    case ItemType.Jewelry:
                        return DeathItemCategory.Jewelry;
                    case ItemType.Food:
                        return DeathItemCategory.Food;
                    case ItemType.Gem:
                        return DeathItemCategory.Gem;
                    case ItemType.SpellComponents:
                        return DeathItemCategory.SpellComponent;
                    case ItemType.ManaStone:
                        return DeathItemCategory.ManaStone;
                    case ItemType.CraftAlchemyBase:
                    case ItemType.CraftAlchemyIntermediate:
                    case ItemType.CraftCookingBase:
                    case ItemType.CraftFletchingBase:
                    case ItemType.CraftFletchingIntermediate:
                        return DeathItemCategory.CraftingIngredient;
                    case ItemType.Writable:
                        return DeathItemCategory.ParchmentBook;
                    case ItemType.Key:
                        return DeathItemCategory.Key;
                    case ItemType.PromissoryNote:
                        return DeathItemCategory.TradeNote; // should not drop?
                    case ItemType.Misc:
                        return DeathItemCategory.Misc;
                    case ItemType.Container:
                        return DeathItemCategory.None;  // containers don't drop?
                    default:
                        //Console.WriteLine("Unknown death item type: " + wo.ItemType);
                        return DeathItemCategory.None;
                }
            }
        }
    }
}
