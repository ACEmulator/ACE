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
            player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.UiEffects, (int)effect));
            UiEffects = effect;
        }

        public void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            var useResult = WeenieError.None;
            if (!ItemCurMana.HasValue)
            {
                if (target == player)
                    useResult = WeenieError.ActionCancelled;
                else if (target.ItemCurMana.HasValue && target.ItemCurMana.Value > 0)
                {
                    // suck up the mana
                    if (target.Retained ?? false)
                        useResult = WeenieError.ActionCancelled;
                    else
                    {
                        var sourceMana = target.ItemCurMana.Value;
                        if (!player.TryRemoveFromInventoryWithNetworking(target)) throw new Exception($"Failed to remove {target.Name} from player inventory.");
                        ItemCurMana = (int)(Efficiency.Value * target.ItemCurMana.Value);
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
                        var puddles = new Dictionary<WorldObject, int>();
                        var manaAvailable = ItemCurMana.Value;
                        var continuePouring = true;


                        while (manaAvailable > 0 && continuePouring)
                        {
                            var itemsNeedingMana = player.EquippedObjects.Where(k => k.Value.ItemCurMana.HasValue && k.Value.ItemMaxMana.HasValue && k.Value.ItemCurMana.Value < k.Value.ItemMaxMana.Value).ToList();
                            if (itemsNeedingMana.Count < 1) continuePouring = false;
                            itemsNeedingMana.ForEach(k =>
                            {
                                var manaNeededForTopoff = (int)(k.Value.ItemMaxMana - k.Value.ItemCurMana);
                                var manaToPour = Math.Min(manaAvailable, 50);
                                manaToPour = Math.Min(manaToPour, manaNeededForTopoff);
                                if (manaToPour > 0)
                                {
                                    k.Value.ItemCurMana += manaToPour;
                                    manaAvailable -= manaToPour;
                                    if (puddles.Any(c => c.Key.Guid == k.Key))
                                        puddles[k.Value] += manaToPour;
                                    else
                                        puddles[k.Value] = manaToPour;
                                }
                            });
                        }
                        if (puddles.Count < 1)
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have no items equipped that need mana.", ChatMessageType.Broadcast));
                            useResult = WeenieError.ActionCancelled;
                        }
                        else
                        {
                            var itemsNeedingMana = player.EquippedObjects.Where(k => k.Value.ItemCurMana.HasValue && k.Value.ItemMaxMana.HasValue && k.Value.ItemCurMana.Value < k.Value.ItemMaxMana.Value).ToList();
                            var additionalManaNeeded = itemsNeedingMana.Sum(k => k.Value.ItemMaxMana.Value - k.Value.ItemCurMana.Value);
                            var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded} more mana to fully charge your items." : string.Empty;
                            var msg = $"The {Name} gives {puddles.Sum(k => k.Value).ToString("n0")} points of mana to the following items: {puddles.Select(c => c.Key.Name).Aggregate((a, b) => a + ", " + b)}{additionalManaText}";
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                            SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                            ItemCurMana = null;
                            Destroy(player, true);
                        }
                    }
                }
                else if (target.ItemMaxMana.HasValue)
                {
                    if (target.ItemCurMana.Value >= target.ItemMaxMana.Value)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {target.Name} is already full of mana.", ChatMessageType.Broadcast));
                    }
                    else
                    {
                        var targetManaNeeded = target.ItemCurMana.HasValue ? (target.ItemMaxMana.Value - target.ItemCurMana.Value) : target.ItemMaxMana.Value;
                        var manaToPour = Math.Min(targetManaNeeded, ItemCurMana.Value);
                        target.ItemCurMana += manaToPour;
                        var additionalManaNeeded = targetManaNeeded - manaToPour;
                        var additionalManaText = (additionalManaNeeded > 0) ? $"\nYou need {additionalManaNeeded} more mana to fully charge your {target.Name}." : string.Empty;
                        var msg = $"The {Name} gives {manaToPour} points of mana to the {target.Name}.{additionalManaText}";
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                        Destroy(player, true);
                        SetUiEffect(player, ACE.Entity.Enum.UiEffects.Undef);
                    }
                }
            }

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, useResult));
        }

        public bool Destroy(Player player, bool rollTheDice)
        {
            // TODO: special handling for "Eternal Mana Charge"
            // TODO: differentiate between rechargable stones and store bought
            bool destroy = false;
            if (!rollTheDice) destroy = true;
            var dice = Physics.Common.Random.RollDice(0.0f, 1.0f);
            if (dice < DestroyChance) destroy = true;
            if (destroy)
            {
                player.TryRemoveFromInventoryWithNetworking(this);
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {Name} is destroyed.", ChatMessageType.Broadcast));
                return true;
            }
            else return false;
        }
    }
}
