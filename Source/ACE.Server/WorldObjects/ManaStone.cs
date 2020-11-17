using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class ManaStone : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public ManaStone(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public ManaStone(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        private double? Efficiency
        {
            get => GetProperty(PropertyFloat.ItemEfficiency);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ItemEfficiency); else SetProperty(PropertyFloat.ItemEfficiency, value.Value); }
        }

        private double? DestroyChance
        {
            get => GetProperty(PropertyFloat.ManaStoneDestroyChance);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ManaStoneDestroyChance); else SetProperty(PropertyFloat.ManaStoneDestroyChance, value.Value); }
        }

        public void SetUiEffect(Player player, UiEffects effect)
        {
            UiEffects = effect;
            player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.UiEffects, (int)effect));
        }

        public override void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            WorldObject invTarget;

            var useResult = WeenieError.None;

            if (player != target)
            {
                invTarget = player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory | Player.SearchLocations.MyEquippedItems);
                if (invTarget == null)
                {
                    // Haven't looked to see if an error was sent for this case; however, this one fits
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouDoNotOwnThatItem));
                    return;
                }

                target = invTarget;
            }

            if (!ItemCurMana.HasValue)
            {
                if (target == player)
                    useResult = WeenieError.ActionCancelled;
                else if (target.ItemCurMana.HasValue && target.ItemCurMana.Value > 0 && target.ItemMaxMana.HasValue && target.ItemMaxMana.Value > 0)
                {
                    // absorb mana from the item
                    if (target.Retained)
                        useResult = WeenieError.ActionCancelled;
                    else
                    {
                        if (!player.TryConsumeFromInventoryWithNetworking(target, 1) && !player.TryDequipObjectWithNetworking(target.Guid, out _, Player.DequipObjectAction.ConsumeItem))
                        {
                            log.Error($"Failed to remove {target.Name} from player inventory.");
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.ActionCancelled));
                            return;
                        }

                        //The Mana Stone drains 5,253 points of mana from the Wand.
                        //The Wand is destroyed.

                        //The Mana Stone drains 4,482 points of mana from the Pantaloons.
                        //The Pantaloons is destroyed.

                        var manaDrained = (int)Math.Round(Efficiency.Value * target.ItemCurMana.Value);
                        ItemCurMana = manaDrained;
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The Mana Stone drains {ItemCurMana:N0} points of mana from the {target.Name}.\nThe {target.Name} is destroyed.", ChatMessageType.Broadcast));
                        SetUiEffect(player, ACE.Entity.Enum.UiEffects.Magical);
                    }
                }
                else
                {
                    useResult = WeenieError.ItemDoesntHaveEnoughMana;
                }
            }
            else if (ItemCurMana.Value > 0)
            {
                if (target == player)
                {
                    // dump mana into equipped items
                    var origItemsNeedingMana = player.EquippedObjects.Values.Where(k => k.ItemCurMana.HasValue && k.ItemMaxMana.HasValue && k.ItemCurMana < k.ItemMaxMana).ToList();
                    var itemsGivenMana = new Dictionary<WorldObject, int>();

                    while (ItemCurMana > 0)
                    {
                        var itemsNeedingMana = origItemsNeedingMana.Where(k => k.ItemCurMana < k.ItemMaxMana).ToList();
                        if (itemsNeedingMana.Count < 1)
                            break;

                        var ration = Math.Max(ItemCurMana.Value / itemsNeedingMana.Count, 1);

                        foreach (var item in itemsNeedingMana)
                        {
                            var manaNeededForTopoff = (int)(item.ItemMaxMana - item.ItemCurMana);
                            var adjustedRation = Math.Min(ration, manaNeededForTopoff);

                            ItemCurMana -= adjustedRation;

                            if (player.LumAugItemManaGain != 0)
                            {
                                adjustedRation = (int)Math.Round(adjustedRation * Creature.GetPositiveRatingMod(player.LumAugItemManaGain * 5));
                                if (adjustedRation > manaNeededForTopoff)
                                {
                                    var diff = adjustedRation - manaNeededForTopoff;
                                    adjustedRation = manaNeededForTopoff;
                                    ItemCurMana += diff;
                                }
                            }

                            item.ItemCurMana += adjustedRation;
                            if (!itemsGivenMana.ContainsKey(item))
                                itemsGivenMana[item] = adjustedRation;
                            else
                                itemsGivenMana[item] += adjustedRation;

                            if (ItemCurMana <= 0)
                                break;
                        }
                    }

                    if (itemsGivenMana.Count < 1)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat("You have no items equipped that need mana.", ChatMessageType.Broadcast));
                        useResult = WeenieError.ActionCancelled;
                    }
                    else
                    {
                        //The Mana Stone gives 4,496 points of mana to the following items: Fire Compound Crossbow, Qafiya, Celdon Sleeves, Amuli Leggings, Messenger's Collar, Heavy Bracelet, Scalemail Bracers, Olthoi Alduressa Gauntlets, Studded Leather Girth, Shoes, Chainmail Greaves, Loose Pants, Mechanical Scarab, Ring, Ring, Heavy Bracelet
                        //Your items are fully charged.

                        //The Mana Stone gives 1,921 points of mana to the following items: Haebrean Girth, Chiran Helm, Ring, Baggy Breeches, Scalemail Greaves, Alduressa Boots, Heavy Bracelet, Heavy Bracelet, Lorica Breastplate, Pocket Watch, Heavy Necklace
                        //You need 2,232 more mana to fully charge your items.

                        var additionalManaNeeded = origItemsNeedingMana.Sum(k => k.ItemMaxMana.Value - k.ItemCurMana.Value);
                        var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded:N0} more mana to fully charge your items." : "\nYour items are fully charged.";
                        var msg = $"The Mana Stone gives {itemsGivenMana.Values.Sum():N0} points of mana to the following items: {itemsGivenMana.Select(c => c.Key.Name).Aggregate((a, b) => a + ", " + b)}.{additionalManaText}";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                        if (!DoDestroyDiceRoll(player) && !UnlimitedUse)
                        {
                            ItemCurMana = null;
                            SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                        }

                        if (UnlimitedUse && ItemMaxMana.HasValue)
                            ItemCurMana = ItemMaxMana;
                    }
                }
                else if (target.ItemMaxMana.HasValue && target.ItemMaxMana.Value > 0)
                {
                    var targetItemCurMana = target.ItemCurMana ?? 0;

                    if (targetItemCurMana >= target.ItemMaxMana)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {target.Name} is already full of mana.", ChatMessageType.Broadcast));
                    }
                    else
                    {
                        // The Mana Stone gives 3,502 points of mana to the Focusing Stone.

                        // The Mana Stone gives 3,267 points of mana to the Protective Drudge Charm.

                        var targetManaNeeded = target.ItemMaxMana.Value - targetItemCurMana;
                        var manaToPour = Math.Min(targetManaNeeded, ItemCurMana.Value);

                        if (player.LumAugItemManaGain != 0)
                        {
                            manaToPour = (int)Math.Round(manaToPour * Creature.GetPositiveRatingMod(player.LumAugItemManaGain * 5));
                            manaToPour = Math.Min(targetManaNeeded, manaToPour);
                        }

                        target.ItemCurMana = targetItemCurMana + manaToPour;
                        var msg = $"The Mana Stone gives {manaToPour:N0} points of mana to the {target.Name}.";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                        if (!DoDestroyDiceRoll(player) && !UnlimitedUse)
                        {
                            ItemCurMana = null;
                            SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                        }
                    }
                }
                else
                {
                    useResult = WeenieError.ActionCancelled;
                }
            }

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, useResult));
        }

        private bool DoDestroyDiceRoll(Player player)
        {
            if (DestroyChance == 0)
                return false;

            var dice = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (dice < DestroyChance)
            {
                player.TryConsumeFromInventoryWithNetworking(this);
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The Mana Stone is destroyed.", ChatMessageType.Broadcast));
                    return true;
                }
            }

            return false;
        }
    }
}
