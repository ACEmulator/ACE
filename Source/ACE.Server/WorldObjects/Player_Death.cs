using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Player Death/Kill, use this to kill a session's player
        /// </summary>
        /// <remarks>
        ///     TODO:
        ///         1. Find the best vitae formula and add vitae
        ///         2. Generate the correct death message, or have it passed in as a parameter.
        ///         3. Find the correct player death noise based on the player model and play on death.
        ///         4. Determine if we need to Send Queued Action for Lifestone Materialize, after Action Location.
        ///         5. Find the health after death formula and Set the correct health
        /// </remarks>
        private void OnKill(Session killerSession)
        {
            ObjectGuid killerId = killerSession.Player.Guid;

            IsAlive = false;
            Health.Current = 0; // Set the health to zero
            NumDeaths++; // Increase the NumDeaths counter
            DeathLevel = Level; // For calculating vitae XP
            VitaeCpPool = 0; // Set vitae XP

            // TODO: Generate a death message based on the damage type to pass in to each death message:
            string currentDeathMessage = $"died to {killerSession.Player.Name}.";

            // Send Vicitim Notification, or "Your Death" event to the client:
            // create and send the client death event, GameEventYourDeath
            var msgYourDeath = new GameEventYourDeath(Session, $"You have {currentDeathMessage}");
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.NumDeaths, NumDeaths ?? 0);
            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.DeathLevel, DeathLevel ?? 0);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value);
            var msgPurgeEnchantments = new GameEventPurgeAllEnchantments(Session);
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);

            // handle vitae
            var vitae = EnchantmentManager.UpdateVitae();

            var spellID = (uint)Network.Enum.Spell.Vitae;
            var spellBase = DatManager.PortalDat.SpellTable.Spells[spellID];
            var spell = DatabaseManager.World.GetCachedSpell(spellID);
            var vitaeEnchantment = new Enchantment(this, spellID, (double)spell.Duration, 0, spell.StatModType, vitae);
            var msgVitaeEnchantment = new GameEventMagicUpdateEnchantment(Session, vitaeEnchantment);

            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, 0);

            // Send first death message group
            Session.Network.EnqueueSend(msgHealthUpdate, msgYourDeath, msgNumDeaths, msgDeathLevel, msgVitaeCpPool, msgPurgeEnchantments, msgVitaeEnchantment);

            // Broadcast the 019E: Player Killed GameMessage
            ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerId);
        }

        protected override void DoOnKill(Session killerSession)
        {
            // First do on-kill
            OnKill(killerSession);

            // Then get onKill from our parent
            ActionChain killChain = OnKillInternal(killerSession);

            // Send the teleport out after we animate death
            killChain.AddAction(this, () =>
            {
                // teleport to sanctuary or best location
                var newPosition = Sanctuary ?? LastPortal ?? Location;

                // Enqueue a teleport action, followed by Stand-up
                // Queue the teleport to lifestone
                ActionChain teleportChain = GetTeleportChain(newPosition);

                teleportChain.AddAction(this, () =>
                {
                    var newHealth = (uint)Math.Round(Health.MaxValue * 0.75f);
                    var newStamina = (uint)Math.Round(Stamina.MaxValue * 0.75f);
                    var newMana = (uint)Math.Round(Mana.MaxValue * 0.75f);

                    var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, newHealth);
                    var msgStaminaUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Stamina, newStamina);
                    var msgManaUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Mana, newMana);

                    UpdateVital(Health, newHealth);
                    UpdateVital(Stamina, newStamina);
                    UpdateVital(Mana, newMana);

                    killerSession.Network.EnqueueSend(msgHealthUpdate, msgStaminaUpdate, msgManaUpdate);

                    // Stand back up
                    DoMotion(new UniversalMotion(MotionStance.Standing));

                    // add a Corpse at the current location via the ActionQueue to honor the motion and teleport delays
                    // QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
                    // AddToActionQueue(addCorpse);
                    // If the player is outside of the landblock we just died in, then reboadcast the death for
                    // the players at the lifestone.
                    if (Positions.ContainsKey(PositionType.LastOutsideDeath) && Positions[PositionType.LastOutsideDeath].Cell != newPosition.Cell)
                    {
                        string currentDeathMessage = $"died to {killerSession.Player.Name}.";
                        ActionBroadcastKill($"{Name} has {currentDeathMessage}", Guid, killerSession.Player.Guid);
                    }
                });
                teleportChain.EnqueueChain();
            });
            killChain.EnqueueChain();
        }

        public void HandleActionDie()
        {
            new ActionChain(this, () =>
            {
                DoOnKill(Session);
            }).EnqueueChain();
        }

        public override void Smite(ObjectGuid smiter)
        {
            HandleActionDie();
        }

        public void CreateCorpse()
        {

        }

        public List<WorldObject> CalculateDeathItems(Corpse corpse)
        {
            // https://web.archive.org/web/20140712134108/http://support.turbine.com/link/portal/24001/24001/Article/464/How-do-death-items-work-in-Asheron-s-Call-Could-you-explain-how-the-game-decides-what-you-drop-when-you-die-in-Asheron-s-Call

            // Original formula:

            // - When you are level 5 or under, you don't drop anything when you die.
            // - From level 6 to level 10, you lose half your coins (not trade notes) and nothing else.
            // - From level 11 to level 20, you lose half your coins and possibly one non-wielded item (that is, something that you were neither wearing nor holding in your hands).
            // - From level 21 to level 35, you lose half your coins and some number of non-wielded items.
            // - After level 35, you lose half your coins and some number of items. At this point, you can drop items that you were wearing or holding.

            // Now, in those last two cases, I said 'some number'. Some number here is equal to your level divided by 10, rounded down, plus a random number between 0 and 2.
            // So from level 21 to 29 you can lose between 2 and 4 items; from level 30 to 39 you can lose 3-5 items; from 40-49 you can lose 4-6 items, and so forth.
            // By level 126 you can be losing up to 14 items.

            // (one caveat here: if you were killed in a PK battle, you always lose items as if you were over level 35, although the exact number you lose
            // is still determined by your real level / 10. In other words, PK deaths do not get the special protection from item loss that NPK deaths get under level 35.)

            // So that's how many items you lose on death -- but how do we determine which items are lost? This is where the categories come into it.

            // Each item in the game has a particular item type associated with it. The actual types (categories) are listed below.
            // When we are deciding what items you drop on death, we make a list of all the items you are carrying, sorted by value (high value first).
            // But we may adjust their values in two ways. If the item is not the most valuable item in that category, then we cut its value in half.
            // And whether or not it is the most valuable item in its category, we randomize its value a little bit to mix things up.
            // (To answer your first question explicitly, the cutting-the-value-in-half is not cumulative -- the second- and third- and fourth-most valuable items
            // of one category all have their values halved, not halved and then quartered and then eighthed).

            // Now note that we still keep these things in a list sorted by adjusted value -- so its possible to have your two expensive weapons
            // listed first and second, if the value of the second weapon cut in half is still higher than the value of the third item. What I am trying to get at here is
            // that we do not segregate the list based on item type; we only use item type to determine how we adjust the value of the item.

            // Finally, we go down the list and mark the first # things as dropped, where # is the number of items we have calculated that you are going to drop this death.
            // For instance, if you are level 48, you will drop the first 4-6 items on that list.

            // The categories of items are:

            // - Melee weapons
            // - Missile weapons
            // - Magic casters (like orbs & wands)
            // - Armor
            // - Clothing
            // - Jewelry
            // - Food
            // - Gems
            // - Components
            // - Mana stones
            // - Crafting ingredients
            // - Parchments & books
            // - Keys
            // - Tradenotes
            // - Miscellaneous

            // A few categories are left out such as lifestones -- because you aren't likely to be carrying an item of that type.

            // http://asheron.wikia.com/wiki/Death_Penalty
            // The first item to drop will be the highest value item regardless of type.
            // All items of the same type will now be counted at half their face value. This process repeats (next highest value item,
            // if an item of the same type has already dropped, the face value is halved.)

            // The number of items you drop can be reduced with the Clutch of the Miser augmentation. If you get the
            // augmentation three times you will no longer drop any items (except half of your Pyreals and all Rares except if you're a PK).
            // If you drop no items, you will not leave a corpse.

            // Some players use gems, particularly the Archmage Portal Gems as death items, so it's important to note how
            // stackable items work with the death system. When you die, a stack is considered to be one item for the purposes
            // of death items. However if that stack is selected, only one item off the stack will drop. For example, let's say you
            // can currently cover all of your items with 5 Portal Gems. After the event, if you do not stack these gems, there
            // will be no change. However, if you combine the 5 Gems into a single stack, you would drop 1 gem and 4 other items on death.

            // When you die there is a notification in your chat dialog stating what items you dropped, and your corpse location
            // (if on the landscape). If you do not leave a corpse the notification will state so.

            // Accounts with the Throne of Destiny expansion received a new formula for death items to prevent losing a
            // pack per death at high levels. The formula was changed to divide by 20 levels instead of 10, thereby making
            // the maximum dropped items the same.
            // Tier 1 and 2 Rares will always drop, and do not count toward your death item count, so store them in a safe place.
            // PKLite - no items or pyreals are lost on PKL death.

            // Death items

            // You can protect important items like your armor and weapons by carrying death items. THese are items that are
            // ideally light weight as well as high in value. You can tinker loot items with bags of Salvaged Gold to raise their
            // face value. Although tinkering special items can remove some of the beneift, as you feel obligated to recover
            // them. Items that can be purchased can just be left if the corpse would be too difficult to recover.

            // Popular items to use as Death Items:

            // - Massive Mana Charges (most common death item)
            // - Pristine Mana Shards
            // - Crowns (tinkered with gold)
            // - Robes sold by the Mastermages
            // - Nanner Island Portal Gems (somewhat difficult to obtain)

            // Bonded items:

            // Items that have the Special Property Bonded will never drop on death. Some notable bonded items include:
            // - Academy Weapons (Starter weapons)
            // - Augmentation Gems (Asheron's Benediction and Blackmoor's Favor)
            // - Pathwarden Armor (Starter armor)
            // - Trade Notes

            // http://acpedia.org/wiki/Recovering_from_Death

            // Recovering from Death

            // Fortunately, you won't be in immediate danger of being killed again, because you're invulnerable to attack for a full minute if you don't attack another
            // creature of cast any spells. Also, your secondary attributes, even if all had been reduced to 0 when you died, will be at 75 percent of their new
            // maximum score, and any poisons or harmful enchantments afflicting you just before you died will be gone, giving you a fighting chance of making it
            // back to town or meeting up with your allies.
            // You can also find the location of your last corpse outdoors by typing @corpse.

            // Corpse timer = Math.Max(1 hour, 5 mins * level)
            // TODO: @permit and @consent commands
            // You can have up to 20 people in your consent list at once. However, when you log off, all permissions to loot corpses will be removed.

            // First, we sort the inventory by order of value.
            // Second, we go back through the inventory starting at the most expensive item and look at each item's category.
            // If we've seen the item category before, we divide the value of the item in half.
            // At this point, we add a random 0-10% variance to each item's value.

            // http://acpedia.org/wiki/Death_Item

            // A Reign of Stone (April 2001) - Corpse permission commands added.
            // The Changing of the Ways (May 2001) - Players are now given the coordinates of their characters' corpses when they die.
            // Hidden Vein (May 2002) - Many changes made on the way item loss on death works. See http://acpedia.org/wiki/Hidden_Vein and http://acpedia.org/wiki/Announcements_-_2002/05_-_Hidden_Vein#Letter_to_the_Players for more details
            // Throne of Destiny expansion (July 2005) - The formula used for working out how many items a character drops was updated. For details on the old formula and how it changed see http://acpedia.org/wiki/Announcements_-_2005/07_-_Throne_of_Destiny_(expansion)#FAQ_-_AC:TD_Level_Cap_Update

            var numItemsDropped = GetNumItemsDropped();

            var numCoinsDropped = GetNumCoinsDropped();

            var level = Level ?? 1;
            var canDropWielded = level >= 35;

            // get all items in inventory
            var inventory = GetAllPossessions();

            // exclude pyreals from randomized death item calculation
            inventory = inventory.Where(i => !i.Name.Equals("Pyreal")).ToList();

            // exclude wielded items if < level 35
            if (!canDropWielded)
                inventory = inventory.Where(i => i.CurrentWieldedLocation != null).ToList();

            // exclude bonded items
            inventory = inventory.Where(i => (i.GetProperty(PropertyInt.Bonded) ?? 0) == 0).ToList();

            // construct the list of death items
            var sorted = new DeathItems(inventory);

            var dropItems = new List<WorldObject>();

            if (numCoinsDropped > 0)
            {
                // add pyreals to dropped items
                var pyreals = SpendCurrency((uint)numCoinsDropped, WeenieType.Coin);
                dropItems.AddRange(pyreals);
                //Console.WriteLine($"Dropping {numCoinsDropped} pyreals");
            }

            for (var i = 0; i < numItemsDropped && i < sorted.Inventory.Count; i++)
            {
                var deathItem = sorted.Inventory[i];

                // split stack if needed
                var stackSize = deathItem.WorldObject.StackSize ?? 1;
                var stackMsg = stackSize > 1 ? " (stack)" : "";

                var dropItem = deathItem.WorldObject;
                if (stackSize > 1)
                {
                    deathItem.WorldObject.StackSize--;
                    Session.Network.EnqueueSend(new GameMessageUpdateObject(deathItem.WorldObject));

                    dropItem = WorldObjectFactory.CreateNewWorldObject(deathItem.WorldObject.WeenieClassId);
                    TryAddToInventory(dropItem);
                }

                //Console.WriteLine("Dropping " + deathItem.WorldObject.Name + stackMsg);
                dropItems.Add(dropItem);
            }

            // add items to corpse
            foreach (var dropItem in dropItems)
            {
                // coins already removed from SpendCurrency
                if (dropItem.WeenieType == WeenieType.Coin)
                    continue;

                if (!TryRemoveItemWithNetworking(dropItem))
                {
                    Console.WriteLine($"Player_Death: couldn't remove item from {Name}'s inventory: {dropItem.Name}");
                    continue;
                }
                if (!corpse.TryAddToInventory(dropItem))
                {
                    Console.WriteLine($"Player_Death: couldn't add item to {Name}'s corpse: {dropItem.Name}");

                    if (!TryAddToInventory(dropItem))
                        Console.WriteLine($"Player_Death: couldn't re-add item to {Name}'s inventory: {dropItem.Name}");
                }
            }

            // send network messages
            var dropList = DropMessage(dropItems);
            Session.Network.EnqueueSend(new GameMessageSystemChat(dropList, ChatMessageType.WorldBroadcast));

            return dropItems;
        }

        /// <summary>
        /// The maximum # of items a player can drop
        /// </summary>
        public static readonly int MaxItemsDropped = 14;

        /// <summary>
        /// Rolls for the # of items to drop for a player death
        /// </summary>
        /// <returns></returns>
        public int GetNumItemsDropped()
        {
            // Original formula:

            // - When you are level 5 or under, you don't drop anything when you die.
            // - From level 6 to level 10, you lose half your coins (not trade notes) and nothing else.
            // - From level 11 to level 20, you lose half your coins and possibly one non-wielded item (that is, something that you were neither wearing nor holding in your hands).
            // - From level 21 to level 35, you lose half your coins and some number of non-wielded items.
            // - After level 35, you lose half your coins and some number of items. At this point, you can drop items that you were wearing or holding.

            // Now, in those last two cases, I said 'some number'. Some number here is equal to your level divided by 10 (*20 after patch), rounded down, plus a random number between 0 and 2.
            // So from level 21 to 29 you can lose between 2 and 4 items; from level 30 to 39 you can lose 3-5 items; from 40-49 you can lose 4-6 items, and so forth.
            // By level 126 you can be losing up to 14 items.

            // (one caveat here: if you were killed in a PK battle, you always lose items as if you were over level 35, although the exact number you lose
            // is still determined by your real level / 10. In other words, PK deaths do not get the special protection from item loss that NPK deaths get under level 35.)

            // So that's how many items you lose on death -- but how do we determine which items are lost? This is where the categories come into it.

            // take augments into consideration?

            var level = Level ?? 1;

            if (level <= 10)
                return 0;

            if (level >= 11 && level <= 20)
                return Physics.Common.Random.RollDice(0, 1);

            // level 21+
            var numItemsDropped = (level / 20) + Physics.Common.Random.RollDice(0, 2);

            numItemsDropped = Math.Min(numItemsDropped, MaxItemsDropped);   // is this really a max cap?

            // TODO: PK deaths

            // The number of items you drop can be reduced with the Clutch of the Miser augmentation. If you get the
            // augmentation three times you will no longer drop any items(except half of your Pyreals and all Rares except if you're a PK).
            // If you drop no items, you will not leave a corpse.

            return numItemsDropped;
        }

        public int GetNumCoinsDropped()
        {
            // if level > 5, lose half coins
            // (trade notes excluded)
            var level = Level ?? 1;
            var coins = CoinValue ?? 0;

            var numCoinsDropped = level > 5 ? coins / 2 : 0;

            return numCoinsDropped;
        }

        /// <summary>
        /// Builds the network text message for list of items dropped
        /// </summary>
        public string DropMessage(List<WorldObject> dropItems)
        {
            var msg = "";

            for (var i = 0; i < dropItems.Count; i++)
            {
                var dropItem = dropItems[i];

                if (i == 0)
                    msg += "You've lost ";
                else
                {
                    msg += ", ";

                    if (i == dropItems.Count - 1)
                        msg += "and ";
                }

                if (!dropItem.Name.Equals("Pyreal"))
                    msg += "your ";

                var stackSize = dropItem.StackSize ?? 1;

                if (stackSize == 1)
                    msg += dropItem.Name;
                else
                    msg += stackSize.ToString("N0") + " " + dropItem.GetPluralName();
            }
            if (msg.Length > 0)
                msg += "!";

            return msg;
        }
    }
}
