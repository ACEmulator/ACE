using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.WorldObjects
{
    public class ManaStone : WorldObject
    {
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
            player.CurrentLandblock.EnqueueBroadcast(player.Location, new GameMessagePublicUpdatePropertyInt(this, PropertyInt.UiEffects, (int)effect));
            UiEffects = effect;
        }

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            var useResult = WeenieError.None;
            if (!ItemCurMana.HasValue)
            {
                if (target == player)
                    useResult = WeenieError.ActionCancelled;
                else if (target.ItemCurMana.HasValue && target.ItemCurMana.Value > 0 && target.ItemMaxMana.HasValue && target.ItemMaxMana.Value > 0)
                {
                    // absorb mana from the item
                    if (target.Retained ?? false)
                        useResult = WeenieError.ActionCancelled;
                    else
                    {
                        var sourceMana = target.ItemCurMana.Value;
                        if (!player.TryRemoveItemWithNetworking(target)) throw new Exception($"Failed to remove {target.Name} from player inventory.");
                        ItemCurMana = (int)Math.Round(Efficiency.Value * target.ItemCurMana.Value);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} drains {ItemCurMana} points of mana from the {target.Name}.\nThe {target.Name} is destroyed.", ChatMessageType.Broadcast));
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
                    if (player.EquippedObjectsLoaded)
                    {
                        var manaAvailable = ItemCurMana.Value;
                        var origItemsNeedingMana = player.EquippedObjects.Where(k => k.Value.ItemCurMana.HasValue && k.Value.ItemMaxMana.HasValue && k.Value.ItemCurMana.Value < k.Value.ItemMaxMana.Value).ToList();
                        origItemsNeedingMana.ForEach(m => m.Value.ManaGiven = 0);
                        while (manaAvailable > 0)
                        {
                            var itemsNeedingMana = origItemsNeedingMana.Where(k => k.Value.ItemCurMana.Value + k.Value.ManaGiven < k.Value.ItemMaxMana.Value).ToList();
                            if (itemsNeedingMana.Count < 1) break;
                            else
                            {
                                var ration = manaAvailable / itemsNeedingMana.Count;
                                itemsNeedingMana.ForEach(k =>
                                {
                                    var manaNeededForTopoff = (int)(k.Value.ItemMaxMana - k.Value.ItemCurMana - k.Value.ManaGiven);
                                    var adjustedRation = Math.Min(ration, manaNeededForTopoff);
                                    k.Value.ManaGiven += adjustedRation;
                                    manaAvailable -= adjustedRation;
                                });
                            }
                        }
                        var itemsGivenMana = origItemsNeedingMana.Where(k => k.Value.ManaGiven > 0).ToList();
                        if (itemsGivenMana.Count < 1)
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have no items equipped that need mana.", ChatMessageType.Broadcast));
                            useResult = WeenieError.ActionCancelled;
                        }
                        else
                        {
                            var itemsNeedingMana = origItemsNeedingMana.Where(k => k.Value.ItemCurMana.Value + k.Value.ManaGiven < k.Value.ItemMaxMana.Value).ToList();
                            var additionalManaNeeded = itemsNeedingMana.Sum(k => k.Value.ItemMaxMana.Value - k.Value.ItemCurMana.Value - k.Value.ManaGiven);
                            var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded} more mana to fully charge your items." : string.Empty;
                            var msg = $"The {Name} gives {itemsGivenMana.Sum(k => k.Value.ManaGiven).ToString("n0")} points of mana to the following items: {itemsGivenMana.Select(c => c.Value.Name).Aggregate((a, b) => a + ", " + b)}{additionalManaText}";
                            itemsGivenMana.ForEach(k => k.Value.ItemCurMana += k.Value.ManaGiven);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                            ItemCurMana = null;
                            if (!Destroy(player))
                                SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                        }
                    }
                }
                else if (target.ItemMaxMana.HasValue && target.ItemMaxMana.Value > 0)
                {
                    if (target.ItemCurMana.Value >= target.ItemMaxMana.Value)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {target.Name} is already full of mana.", ChatMessageType.Broadcast));
                    }
                    else
                    {
                        // dump mana into the item
                        var targetManaNeeded = target.ItemCurMana.HasValue ? (target.ItemMaxMana.Value - target.ItemCurMana.Value) : target.ItemMaxMana.Value;
                        var manaToPour = Math.Min(targetManaNeeded, ItemCurMana.Value);
                        target.ItemCurMana += manaToPour;
                        var additionalManaNeeded = targetManaNeeded - manaToPour;
                        var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded} more mana to fully charge your {target.Name}." : string.Empty;
                        var msg = $"The {Name} gives {manaToPour} points of mana to the {target.Name}.{additionalManaText}";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                        if (!Destroy(player))
                            SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                    }
                }
                else
                {
                    useResult = WeenieError.ActionCancelled;
                }
            }

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, useResult));
        }

        public bool Destroy(Player player)
        {
            if (DestroyChance == 0) return false;
            // TODO: special handling for "Eternal Mana Charge"
            var dice = Physics.Common.Random.RollDice(0.0f, 1.0f);
            if (dice < DestroyChance)
            {
                player.TryRemoveFromInventoryWithNetworking(this);
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} is destroyed.", ChatMessageType.Broadcast));
                return true;
            }
            else return false;
        }
    }
}
