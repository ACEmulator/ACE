using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
            player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.UiEffects, (int)effect));
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
                        if (!player.TryConsumeFromInventoryWithNetworking(target))
                        {
                            log.Error($"Failed to remove {target.Name} from player inventory.");
                            return;
                        }
                        ItemCurMana = (int)Math.Round(Efficiency.Value * target.ItemCurMana.Value);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} drains {ItemCurMana.Value.ToString("N0")} points of mana from the {target.Name}.\nThe {target.Name} is destroyed.", ChatMessageType.Broadcast));
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
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have no items equipped that need mana.", ChatMessageType.Broadcast));
                        useResult = WeenieError.ActionCancelled;
                    }
                    else
                    {
                        var additionalManaNeeded = origItemsNeedingMana.Sum(k => k.ItemMaxMana.Value - k.ItemCurMana.Value);
                        var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded:N0} more mana to fully charge your items." : string.Empty;
                        var msg = $"The {Name} gives {itemsGivenMana.Values.Sum():N0} points of mana to the following items: {itemsGivenMana.Select(c => c.Key.Name).Aggregate((a, b) => a + ", " + b)}.{additionalManaText}";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                        if (!DoDestroyDiceRoll(player))
                        {
                            ItemCurMana = null;
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
                        var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded.ToString("N0")} more mana to fully charge your {target.Name}." : string.Empty;
                        var msg = $"The {Name} gives {manaToPour.ToString("N0")} points of mana to the {target.Name}.{additionalManaText}";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                        if (!DoDestroyDiceRoll(player))
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

        public bool DoDestroyDiceRoll(Player player)
        {
            if (DestroyChance == 0)
                return false;

            // TODO: special handling for "Eternal Mana Charge"
            var dice = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (dice < DestroyChance)
            {
                player.TryConsumeFromInventoryWithNetworking(this);
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} is destroyed.", ChatMessageType.Broadcast));
                    return true;
                }
            }

            return false;
        }
    }
}
