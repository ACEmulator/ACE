using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// A list of players who have granted corpse looting permissions
        /// with /permit
        /// </summary>
        public Dictionary<ObjectGuid, DateTime> LootPermission;

        /// <summary>
        /// Called when a player dies, in conjunction with Die()
        /// </summary>
        /// <param name="lastDamager">The last damager that landed the death blow</param>
        /// <param name="damageType">The damage type for the death message</param>
        public override DeathMessage OnDeath(DamageHistoryInfo lastDamager, DamageType damageType, bool criticalHit = false)
        {
            var topDamager = DamageHistory.GetTopDamager(false);

            HandlePKDeathBroadcast(lastDamager, topDamager);

            var deathMessage = base.OnDeath(lastDamager, damageType, criticalHit);

            var lastDamagerObj = lastDamager?.TryGetAttacker();

            if (lastDamagerObj != null)
                lastDamagerObj.EmoteManager.OnKill(this);

            var playerMsg = "";
            if (lastDamager != null)
                playerMsg = string.Format(deathMessage.Victim, Name, lastDamager.Name);
            else
                playerMsg = deathMessage.Victim;

            var msgYourDeath = new GameEventVictimNotification(Session, playerMsg);
            Session.Network.EnqueueSend(msgYourDeath);

            // broadcast to nearby players
            var nearbyMsg = "";
            if (lastDamager != null)
                nearbyMsg = string.Format(deathMessage.Broadcast, Name, lastDamager.Name);
            else
                nearbyMsg = deathMessage.Broadcast;

            var broadcastMsg = new GameMessagePlayerKilled(nearbyMsg, Guid, lastDamager?.Guid ?? ObjectGuid.Invalid);

            log.Debug("[CORPSE] " + nearbyMsg);

            var excludePlayers = new List<Player>();

            var nearbyPlayers = EnqueueBroadcast(excludePlayers, true, broadcastMsg);

            excludePlayers.AddRange(nearbyPlayers);

            if (Fellowship != null)
                Fellowship.OnDeath(this);

            // if the player's lifestone is in a different landblock, also broadcast their demise to that landblock
            if (PropertyManager.GetBool("lifestone_broadcast_death").Item && Sanctuary != null && Location.Landblock != Sanctuary.Landblock)
            {
                // ActionBroadcastKill might not work if other players around lifestone aren't aware of this player yet...
                // this existing broadcast method is also based on the current visible objects to the player,
                // and the player hasn't entered portal space or teleported back to the lifestone yet, so this doesn't work
                //ActionBroadcastKill(nearbyMsg, Guid, lastDamager.Guid);

                // instead, we get all of the players in the lifestone landblock + adjacent landblocks,
                // and possibly limit that to some radius around the landblock?
                var lifestoneBlock = LandblockManager.GetLandblock(new LandblockId(Sanctuary.Landblock << 16 | 0xFFFF), true);

                // We enqueue the work onto the target landblock to ensure thread-safety. It's highly likely the lifestoneBlock is far away, and part of a different landblock group (and thus different thread).
                lifestoneBlock.EnqueueAction(new ActionEventDelegate(() => lifestoneBlock.EnqueueBroadcast(excludePlayers, true, Sanctuary, LocalBroadcastRangeSq, broadcastMsg)));
            }

            return deathMessage;
        }

        public void HandlePKDeathBroadcast(DamageHistoryInfo lastDamager, DamageHistoryInfo topDamager)
        {
            if (topDamager == null || !topDamager.IsPlayer)
                return;

            var pkPlayer = topDamager.TryGetAttacker() as Player;
            if (pkPlayer == null)
                return;

            if (IsPKDeath(topDamager))
            {
                pkPlayer.PkTimestamp = Time.GetUnixTime();
                pkPlayer.PlayerKillsPk++;

                var globalPKDe = $"{lastDamager.Name} has defeated {Name}!";

                if ((Location.Cell & 0xFFFF) < 0x100)
                    globalPKDe += $" The kill occured at {Location.GetMapCoordStr()}";

                globalPKDe += "\n[PKDe]";

                PlayerManager.BroadcastToAll(new GameMessageSystemChat(globalPKDe, ChatMessageType.Broadcast));
            }
            else if (IsPKLiteDeath(topDamager))
                pkPlayer.PlayerKillsPkl++;
        }

        /// <summary>
        /// Inflicts vitae
        /// </summary>
        public void InflictVitaePenalty(int amount = 5)
        {
            DeathLevel = Level; // for calculating vitae XP
            VitaeCpPool = 0;    // reset vitae XP earned

            var msgDeathLevel = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.DeathLevel, DeathLevel ?? 0);
            var msgVitaeCpPool = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.VitaeCpPool, VitaeCpPool.Value);

            Session.Network.EnqueueSend(msgDeathLevel, msgVitaeCpPool);

            var vitae = EnchantmentManager.UpdateVitae();

            var spellID = (uint)SpellId.Vitae;
            var spell = new Spell(spellID);
            var vitaeEnchantment = new Enchantment(this, Guid.Full, spellID, 0, (EnchantmentMask)spell.StatModType, vitae);
            Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(Session, vitaeEnchantment));
        }


        public bool IsInDeathProcess;

        /// <summary>
        /// Broadcasts the player death animation, updates vitae, and sends network messages for player death
        /// Queues the action to call TeleportOnDeath and enter portal space soon
        /// </summary>
        protected override void Die(DamageHistoryInfo lastDamager, DamageHistoryInfo topDamager)
        {
            IsInDeathProcess = true;

            if (topDamager?.Guid == Guid && IsPKType)
            {
                var topDamagerOther = DamageHistory.GetTopDamager(false);

                if (topDamagerOther != null && topDamagerOther.IsPlayer)
                    topDamager = topDamagerOther;
            }

            UpdateVital(Health, 0);
            NumDeaths++;
            suicideInProgress = false;

            // todo: since we are going to be using 'time since Player last died to an OlthoiPlayer'
            // as a factor in slag generation, this will eventually be moved to after the slag generation

            //if (topDamager != null && topDamager.IsOlthoiPlayer)
                //OlthoiLootTimestamp = (int)Time.GetUnixTime();

            if (CombatMode == CombatMode.Magic && MagicState.IsCasting)
                FailCast(false);

            // TODO: instead of setting IsBusy here,
            // eventually all of the places that check for states such as IsBusy || Teleporting
            // might want to use a common function, and IsDead should return a separate error
            IsBusy = true;

            // killer = top damager for looting rights
            if (topDamager != null)
                KillerId = topDamager.Guid.Full;

            // broadcast death animation
            var deathAnim = new Motion(MotionStance.NonCombat, MotionCommand.Dead);
            EnqueueBroadcastMotion(deathAnim);

            // create network messages for player death
            var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, 0);

            // TODO: death sounds? seems to play automatically in client
            // var msgDeathSound = new GameMessageSound(Guid, Sound.Death1, 1.0f);
            var msgNumDeaths = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.NumDeaths, NumDeaths);

            // send network messages for player death
            Session.Network.EnqueueSend(msgHealthUpdate, msgNumDeaths);

            if (lastDamager?.Guid == Guid) // suicide
            {
                var msgSelfInflictedDeath = new GameEventWeenieError(Session, WeenieError.YouKilledYourself);
                Session.Network.EnqueueSend(msgSelfInflictedDeath);
            }

            var hadVitae = HasVitae;

            // update vitae
            // players who died in a PKLite fight do not accrue vitae
            if (!IsPKLiteDeath(topDamager))
                InflictVitaePenalty();

            if (IsPKDeath(topDamager) || AugmentationSpellsRemainPastDeath == 0)
            {
                var msgPurgeEnchantments = new GameEventMagicPurgeEnchantments(Session);
                EnchantmentManager.RemoveAllEnchantments();
                Session.Network.EnqueueSend(msgPurgeEnchantments);
            }
            else
            {
                var msgPurgeBadEnchantments = new GameEventMagicPurgeBadEnchantments(Session);
                EnchantmentManager.RemoveAllBadEnchantments();
                Session.Network.EnqueueSend(msgPurgeBadEnchantments, new GameMessageSystemChat("Your augmentation prevents the tides of death from ripping away your current enchantments!", ChatMessageType.Broadcast));
            }

            // wait for the death animation to finish
            var dieChain = new ActionChain();
            var animLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead);
            dieChain.AddDelaySeconds(animLength + 1.0f);

            dieChain.AddAction(this, () =>
            {
                CreateCorpse(topDamager, hadVitae);

                ThreadSafeTeleportOnDeath(); // enter portal space

                if (IsPKDeath(topDamager) || IsPKLiteDeath(topDamager))
                    SetMinimumTimeSincePK();

                IsBusy = false;
            });

            dieChain.EnqueueChain();
        }

        /// <summary>
        /// Called when the player enters portal space after dying
        /// </summary>
        public void ThreadSafeTeleportOnDeath()
        {
            // teleport to sanctuary or best location
            var newPosition = Sanctuary ?? Instantiation ?? Location;

            WorldManager.ThreadSafeTeleport(this, newPosition, new ActionEventDelegate(() =>
            {
                // Stand back up
                SetCombatMode(CombatMode.NonCombat);

                SetLifestoneProtection();

                var teleportChain = new ActionChain();
                if (!IsLoggingOut) // If we're in the process of logging out, we skip the delay
                    teleportChain.AddDelaySeconds(3.0f);
                teleportChain.AddAction(this, () =>
                {
                    // currently happens while in portal space
                    var newHealth = (uint)Math.Round(Health.MaxValue * 0.75f);
                    var newStamina = (uint)Math.Round(Stamina.MaxValue * 0.75f);
                    var newMana = (uint)Math.Round(Mana.MaxValue * 0.75f);

                    var msgHealthUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Health, newHealth);
                    var msgStaminaUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Stamina, newStamina);
                    var msgManaUpdate = new GameMessagePrivateUpdateAttribute2ndLevel(this, Vital.Mana, newMana);

                    UpdateVital(Health, newHealth);
                    UpdateVital(Stamina, newStamina);
                    UpdateVital(Mana, newMana);

                    Session.Network.EnqueueSend(msgHealthUpdate, msgStaminaUpdate, msgManaUpdate);

                    // reset damage history for this player
                    DamageHistory.Reset();

                    OnHealthUpdate();

                    IsInDeathProcess = false;

                    if (IsLoggingOut)
                        LogOut_Final(true);
                });

                teleportChain.EnqueueChain();
            }));
        }

        public bool suicideInProgress;

        /// <summary>
        /// Called when player uses the /die command
        /// </summary>
        public void HandleActionDie()
        {
            if (IsDead || Teleporting)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            if (suicideInProgress)
                return;

            suicideInProgress = true;

            if (PropertyManager.GetBool("suicide_instant_death").Item)
                Die(new DamageHistoryInfo(this), DamageHistory.TopDamager);
            else
                HandleSuicide(NumDeaths);
        }

        private static List<string> SuicideMessages = new List<string>()
        {
            "I feel faint...",
            "My sight is growing dim...",
            "My life is flashing before my eyes...",
            "I see a light...",
            "Oh cruel, cruel world!"
        };

        private void HandleSuicide(int numDeaths, int step = 0)
        {
            if (!suicideInProgress || numDeaths != NumDeaths)
                return;

            if (step < SuicideMessages.Count)
            {
                EnqueueBroadcast(new GameMessageHearSpeech(SuicideMessages[step], GetNameWithSuffix(), Guid.Full, ChatMessageType.Speech), LocalBroadcastRange);

                var suicideChain = new ActionChain();
                suicideChain.AddDelaySeconds(3.0f);
                suicideChain.AddAction(this, () => HandleSuicide(numDeaths, step + 1));
                suicideChain.EnqueueChain();
            }
            else
                Die(new DamageHistoryInfo(this), DamageHistory.TopDamager);
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

            // You can protect important items like your armor and weapons by carrying death items. These are items that are
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

            // if player dies in a PKLite battle,
            // they don't drop any items, and revert back to NPK status

            // if player dies on a No Drop landblock,
            // they don't drop any items

            if (corpse.IsOnNoDropLandblock || IsPKLiteDeath(corpse.KillerId))
                return new List<WorldObject>();

            var numItemsDropped = GetNumItemsDropped(corpse);

            var numCoinsDropped = GetNumCoinsDropped();

            var level = Level ?? 1;
            var canDropWielded = level >= 35;

            // get all items in inventory
            var inventory = GetAllPossessions();

            // exclude pyreals from randomized death item calculation
            inventory = inventory.Where(i => i.WeenieClassId != coinStackWcid).ToList();

            // exclude wielded items if < level 35
            if (!canDropWielded)
                inventory = inventory.Where(i => i.CurrentWieldedLocation == null).ToList();

            // exclude bonded items
            inventory = inventory.Where(i => (i.GetProperty(PropertyInt.Bonded) ?? 0) == 0).ToList();

            // handle items with BondedStatus.Destroy
            var destroyedItems = HandleDestroyBonded();

            // construct the list of death items
            var sorted = new DeathItems(inventory);

            var dropItems = new List<WorldObject>();

            if (numCoinsDropped > 0)
            {
                // add pyreals to dropped items
                var pyreals = SpendCurrency(coinStackWcid, (uint)numCoinsDropped);
                dropItems.AddRange(pyreals);
                //Console.WriteLine($"Dropping {numCoinsDropped} pyreals");
            }

            // Remove the items from inventory
            for (var i = 0; i < numItemsDropped && i < sorted.Inventory.Count; i++)
            {
                var deathItem = sorted.Inventory[i];

                // split stack if needed
                if ((deathItem.WorldObject.StackSize ?? 1) > 1)
                {
                    var stack = FindObject(deathItem.WorldObject.Guid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var foundInContainer, out var rootContainer, out _);

                    if (stack != null)
                    {
                        AdjustStack(stack, -1, foundInContainer, rootContainer);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                        var dropItem = WorldObjectFactory.CreateNewWorldObject(deathItem.WorldObject.WeenieClassId);
                        dropItem.SetStackSize(1);

                        //Console.WriteLine("Dropping " + deathItem.WorldObject.Name + " (stack)");
                        dropItems.Add(dropItem);
                    }
                    else
                    {
                        log.WarnFormat("Couldn't find death item stack 0x{0:X8}:{1} for player {2}", deathItem.WorldObject.Guid.Full, deathItem.WorldObject.Name, Name);
                    }
                }
                else
                {
                    if (TryRemoveFromInventoryWithNetworking(deathItem.WorldObject.Guid, out _, RemoveFromInventoryAction.ToCorpseOnDeath) || TryDequipObjectWithNetworking(deathItem.WorldObject.Guid, out _, DequipObjectAction.ToCorpseOnDeath))
                    {
                        //Console.WriteLine("Dropping " + deathItem.WorldObject.Name);
                        dropItems.Add(deathItem.WorldObject);
                    }
                    else
                    {
                        log.WarnFormat("Couldn't find death item 0x{0:X8}:{1} for player {2}", deathItem.WorldObject.Guid.Full, deathItem.WorldObject.Name, Name);
                    }
                }
            }

            // handle items with BondedStatus.Slippery: always drop on death
            var slipperyItems = GetSlipperyItems();

            foreach (var item in slipperyItems)
            {
                if (TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.ToCorpseOnDeath) || TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.ToCorpseOnDeath))
                    dropItems.Add(item);
            }

            var destroyCoins = PropertyManager.GetBool("corpse_destroy_pyreals").Item;

            // add items to corpse
            foreach (var dropItem in dropItems)
            {
                // coins already removed from SpendCurrency
                if (destroyCoins && dropItem.WeenieType == WeenieType.Coin)
                {
                    dropItem.Destroy();
                    continue;
                }

                if (!corpse.TryAddToInventory(dropItem))
                {
                    log.Warn($"Player_Death: couldn't add item to {Name}'s corpse: {dropItem.Name}");

                    if (!TryAddToInventory(dropItem))
                        log.Warn($"Player_Death: couldn't re-add item to {Name}'s inventory: {dropItem.Name}");
                }
            }

            // notify player of destroyed items?
            dropItems.AddRange(destroyedItems);

            // send network messages
            var dropList = DropMessage(dropItems, numCoinsDropped);
            if (!string.IsNullOrWhiteSpace(dropList))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat(dropList, ChatMessageType.Broadcast));

                DeathItemLog(dropItems, corpse);
            }

            return dropItems;
        }

        public void DeathItemLog(List<WorldObject> dropItems, Corpse corpse)
        {
            if (dropItems.Count == 0)
                return;

            var msg = $"[CORPSE] {Name} dropped items on corpse (0x{corpse.Guid}): ";

            foreach (var dropItem in dropItems)
                msg += $"{(dropItem.StackSize.HasValue && dropItem.StackSize > 1 ? dropItem.StackSize.Value.ToString("N0") + " " + dropItem.GetPluralName() : dropItem.Name)} (0x{dropItem.Guid}){(dropItem.WeenieClassId == 273 && PropertyManager.GetBool("corpse_destroy_pyreals").Item ? $" which {(dropItem.StackSize.HasValue && dropItem.StackSize > 1 ? "were" : "was")} destroyed" : "")}, ";

            msg = msg.Substring(0, msg.Length - 2);

            log.Debug(msg);
        }

        /// <summary>
        /// The maximum # of items a player can drop
        /// </summary>
        public const int MaxItemsDropped = 14;

        /// <summary>
        /// Rolls for the # of items to drop for a player death
        /// </summary>
        /// <returns></returns>
        public int GetNumItemsDropped(Corpse corpse)
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
                return ThreadSafeRandom.Next(0, 1);

            // level 21+
            var numItemsDropped = (level / 20) + ThreadSafeRandom.Next(0, 2);

            numItemsDropped = Math.Min(numItemsDropped, MaxItemsDropped);   // is this really a max cap?

            // The number of items you drop can be reduced with the Clutch of the Miser augmentation. If you get the
            // augmentation three times you will no longer drop any items (except half of your Pyreals and all Rares except if you're a PK).
            // If you drop no items, you will not leave a corpse.

            if (!IsPKDeath(corpse.KillerId) && AugmentationLessDeathItemLoss > 0)
            {
                numItemsDropped = Math.Max(0, numItemsDropped - AugmentationLessDeathItemLoss * 5);
            }

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
        public string DropMessage(List<WorldObject> dropItems, int numCoinsDropped)
        {
            var msg = "";
            var coinMsg = true;

            for (var i = 0; i < dropItems.Count; i++)
            {
                var dropItem = dropItems[i];

                var isCoin = dropItem.Name.Equals("Pyreal");

                if (isCoin && !coinMsg)
                    continue;

                if (i == 0)
                    msg += "You've lost ";
                else
                {
                    msg += ", ";

                    if (i == dropItems.Count - 1)
                        msg += "and ";
                }

                var stackSize = dropItem.StackSize ?? 1;
                if (isCoin)
                {
                    stackSize = numCoinsDropped;
                    coinMsg = false;
                }
                else
                    msg += "your ";

                if (stackSize == 1)
                    msg += dropItem.Name;
                else
                    msg += stackSize.ToString("N0") + " " + dropItem.GetPluralName();
            }
            if (msg.Length > 0)
                msg += "!";

            return msg;
        }

        public static TimeSpan PermitTime = TimeSpan.FromHours(1);

        public void HandleActionAddPlayerPermission(string playerName)
        {
            // is this player online?
            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} is not online.", ChatMessageType.Broadcast));
                return;
            }

            // check for self-permit
            if (Name.Equals(player.Name))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You already have permission to loot your corpse.", ChatMessageType.Broadcast));
                return;
            }

            // verify other player has /consent on
            if (!player.GetCharacterOption(CharacterOption.AcceptCorpseLootingPermissions))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not accepting corpse looting permissions from other players.", ChatMessageType.Broadcast));
                return;
            }

            // do they already have permission?
            if (player.HasLootPermission(Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} already has permission to loot your corpse.", ChatMessageType.Broadcast));
                return;
            }

            player.LootPermission.Add(Guid, DateTime.UtcNow + PermitTime);

            // send messages to both players
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has given you permission to loot one of his or her corpses. This permission will last one hour.", ChatMessageType.Broadcast));

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have given permission to {player.Name} to loot one of your corpses. This permission will last one hour.", ChatMessageType.Broadcast));
        }

        public void HandleActionRemovePlayerPermission(string playerName)
        {
            // is this player online?
            var player = PlayerManager.GetOnlinePlayer(playerName);

            // check for self-revoke
            if (Name.Equals(player.Name))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You always have permission to loot your corpse.", ChatMessageType.Broadcast));
                return;
            }

            // do they already have permission?
            if (player == null || !player.HasLootPermission(Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} doesn't have permission to loot your corpse.", ChatMessageType.Broadcast));
                return;
            }

            // remove looting permissions
            player.LootPermission.Remove(Guid);

            // send messages to both players
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} has revoked permission to loot one of his or her corpses.", ChatMessageType.Broadcast));

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s permission to loot your corpse has been revoked.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Cleans out any expired permissions
        /// </summary>
        public void PrunePermissions()
        {
            LootPermission = LootPermission.Where(p => p.Value >= DateTime.UtcNow).ToDictionary(p => p.Key, p => p.Value);
        }

        public bool HasLootPermission(ObjectGuid guid)
        {
            PrunePermissions();

            return LootPermission.ContainsKey(guid);
        }

        public void HandleActionDisplayPlayerConsentList()
        {
            PrunePermissions();

            if (LootPermission.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You do not have permission to loot anyone's corpse.", ChatMessageType.Broadcast));
                return;
            }

            var playerNames = new List<string>();

            foreach (var playerGuid in LootPermission.Keys)
            {
                // is the granter required to stay online?
                var player = PlayerManager.FindByGuid(playerGuid);

                if (player == null)
                {
                    Console.WriteLine($"{Name}.HandleActionDisplayPlayerConsentList(): couldn't find player guid {playerGuid}");
                    continue;
                }
                playerNames.Add(player.Name);
            }
            Session.Network.EnqueueSend(new GameMessageSystemChat("You have permissions to loot a corpse from these players:\n" + string.Join("\n", playerNames), ChatMessageType.Broadcast));
        }

        public void HandleActionClearPlayerConsentList()
        {
            PrunePermissions();

            if (LootPermission.Count == 0)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You do not have permission to loot anyone's corpse.", ChatMessageType.Broadcast));
                return;
            }

            LootPermission.Clear();

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have cleared your consent list. Players will have to permit you again to allow you access to their corpse.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// A player can remove corpse looting permissions that were granted to them.
        /// </summary>
        /// <param name="playerName">The granter name</param>
        public void HandleActionRemoveFromPlayerConsentList(string playerName)
        {
            var player = PlayerManager.FindByName(playerName);

            if (player == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} is not online.", ChatMessageType.Broadcast));
                return;
            }

            // check for self-revoke
            if (Name.Equals(player.Name))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You always have permission to loot your corpse.", ChatMessageType.Broadcast));
                return;
            }

            // do we have permissions?
            if (!HasLootPermission(player.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"You don't have permission to loot {player.Name}'s corpse.", ChatMessageType.Broadcast));
                return;
            }

            // remove looting permissions
            LootPermission.Remove(player.Guid);

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You have removed your permissions to loot {player.Name}'s corpse.", ChatMessageType.Broadcast));
        }

        public bool UnderLifestoneProtection
        {
            get => GetProperty(PropertyBool.UnderLifestoneProtection) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.UnderLifestoneProtection); else SetProperty(PropertyBool.UnderLifestoneProtection, value); }
        }

        public double? LifestoneProtectionTimestamp
        {
            get => GetProperty(PropertyFloat.LifestoneProtectionTimestamp) ?? null;
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.LifestoneProtectionTimestamp); else SetProperty(PropertyFloat.LifestoneProtectionTimestamp, value.Value); }
        }

        public void SetLifestoneProtection()
        {
            UnderLifestoneProtection = true;
            LifestoneProtectionTimestamp = 0;
        }

        public void HandleLifestoneProtection()
        {
            Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.LifestoneMagicProtectsYou));
            EnqueueBroadcast(new GameMessageScript(Guid, PlayScript.ShieldUpBlue));
        }

        public static TimeSpan LifestoneProtectionTime = TimeSpan.FromMinutes(1);

        public void LifestoneProtectionTick()
        {
            if (!UnderLifestoneProtection)
                return;

            LifestoneProtectionTimestamp += CachedHeartbeatInterval;

            if (LifestoneProtectionTimestamp < LifestoneProtectionTime.TotalSeconds)
                return;

            UnderLifestoneProtection = false;
            LifestoneProtectionTimestamp = null;

            Session.Network.EnqueueSend(new GameMessageSystemChat("You're no longer protected by the Lifestone's magic!", ChatMessageType.Magic));
        }

        public void LifestoneProtectionDispel()
        {
            UnderLifestoneProtection = false;
            LifestoneProtectionTimestamp = null;

            Session.Network.EnqueueSend(new GameMessageSystemChat("Your actions have dispelled the Lifestone's magic!", ChatMessageType.Magic));
        }

        public double? MinimumTimeSincePk
        {
            get => GetProperty(PropertyFloat.MinimumTimeSincePk);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.MinimumTimeSincePk); else SetProperty(PropertyFloat.MinimumTimeSincePk, value.Value); }
        }

        public void SetMinimumTimeSincePK()
        {
            if (IsOlthoiPlayer)
                return;

            if (PlayerKillerStatus == PlayerKillerStatus.NPK && MinimumTimeSincePk == null)
                return;

            var prevStatus = PlayerKillerStatus;

            MinimumTimeSincePk = 0;
            PlayerKillerStatus = PlayerKillerStatus.NPK;

            if (prevStatus == PlayerKillerStatus.PK)
            {
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus));
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreTemporarilyNoLongerPK));
            }
            else if (prevStatus == PlayerKillerStatus.PKLite)
            {
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus));
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNonPKAgain));
            }
        }

        public void PK_DeathTick()
        {
            if (MinimumTimeSincePk == null || (PropertyManager.GetBool("pk_server_safe_training_academy").Item && RecallsDisabled))
                return;

            if (PkLevel == PKLevel.NPK && !PropertyManager.GetBool("pk_server").Item && !PropertyManager.GetBool("pkl_server").Item)
            {
                MinimumTimeSincePk = null;
                return;
            }

            MinimumTimeSincePk += CachedHeartbeatInterval;

            if (MinimumTimeSincePk < PropertyManager.GetDouble("pk_respite_timer").Item)
                return;

            MinimumTimeSincePk = null;

            var werror = WeenieError.None;
            var pkLevel = PkLevel;

            if (PropertyManager.GetBool("pk_server").Item)
                pkLevel = PKLevel.PK;
            else if (PropertyManager.GetBool("pkl_server").Item)
                pkLevel = PKLevel.PKLite;

            switch (pkLevel)
            {
                case PKLevel.NPK:
                    return;

                case PKLevel.PK:
                    PlayerKillerStatus = PlayerKillerStatus.PK;
                    werror = WeenieError.YouArePKAgain;
                    break;

                case PKLevel.PKLite:
                    PlayerKillerStatus = PlayerKillerStatus.PKLite;
                    werror = WeenieError.YouAreNowPKLite;
                    break;
            }

            EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus));
            Session.Network.EnqueueSend(new GameEventWeenieError(Session, werror));
        }

        public List<WorldObject> GetSlipperyItems()
        {
            var allPossessions = GetAllPossessions();

            return allPossessions.Where(i => i.Bonded == BondedStatus.Slippery).ToList();
        }

        public List<WorldObject> HandleDestroyBonded()
        {
            var destroyedItems = new List<WorldObject>();

            var allPossessions = GetAllPossessions();
            foreach (var destroyItem in allPossessions.Where(i => i.Bonded == BondedStatus.Destroy).ToList())
            {
                TryConsumeFromInventoryWithNetworking(destroyItem, (destroyItem.StackSize ?? 1));
                destroyedItems.Add(destroyItem);
            }
            return destroyedItems;
        }

        private static Database.Models.World.TreasureDeath OlthoiDeathTreasureType => Database.DatabaseManager.World.GetCachedDeathTreasure(2222) ?? new()
        {
            TreasureType = 2222,
            Tier = 8,
            LootQualityMod = 0,
            UnknownChances = 19,
            ItemChance = 100,
            ItemMinAmount = 1,
            ItemMaxAmount = 2,
            ItemTreasureTypeSelectionChances = 8,
            MagicItemChance = 100,
            MagicItemMinAmount = 2,
            MagicItemMaxAmount = 3,
            MagicItemTreasureTypeSelectionChances = 8,
            MundaneItemChance = 100,
            MundaneItemMinAmount = 0,
            MundaneItemMaxAmount = 1,
            MundaneItemTypeSelectionChances = 7
        };

        /// <summary>
        /// Determines the amount of slag to drop on a Player corpse when killed by an OlthoiPlayer or the loot to drop when an OlthoiPlayer is killed by a Player Killer
        /// </summary>
        public List<WorldObject> CalculateDeathItems_Olthoi(Corpse corpse, bool hadVitae, bool killerIsOlthoiPlayer, bool killerIsPkPlayer)
        {
            if (killerIsOlthoiPlayer)
            {
                var slag = LootGenerationFactory.RollSlag(this, hadVitae);

                if (slag == null)
                    return new();

                if (!corpse.TryAddToInventory(slag))
                    log.Warn($"CalculateDeathItems_Olthoi: couldn't add item to {Name}'s corpse: {slag.Name}");

                return new() { slag };
            }
            else if (killerIsPkPlayer)
            {
                if (hadVitae)
                    return new();

                var items = LootGenerationFactory.CreateRandomLootObjects(OlthoiDeathTreasureType);

                var gland = LootGenerationFactory.RollGland(this, hadVitae);

                if (gland != null)
                {
                    items.Add(gland);
                }

                foreach (WorldObject wo in items)
                {
                    if (!corpse.TryAddToInventory(wo))
                        log.Warn($"CalculateDeathItems_Olthoi: couldn't add item to {Name}'s corpse: {wo.Name}");
                }

                return items;
            }
            else
            {
                return new();
            }
        }
    }
}
