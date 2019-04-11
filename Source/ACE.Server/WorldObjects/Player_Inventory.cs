using System;
using System.Collections.Generic;
using System.Linq;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Returns all inventory, side slot items, items in side containers, and all wielded items.
        /// </summary>
        public List<WorldObject> GetAllPossessions()
        {
            var results = new List<WorldObject>();

            results.AddRange(Inventory.Values);

            foreach (var item in Inventory.Values)
            {
                if (item is Container container)
                    results.AddRange(container.Inventory.Values);
            }

            results.AddRange(EquippedObjects.Values);

            return results;
        }


        public int GetEncumbranceCapacity()
        {
            var strength = Attributes[PropertyAttribute.Strength].Current;

            return (int)((150 * strength) + (AugmentationIncreasedCarryingCapacity * 30 * strength));
        }

        public bool HasEnoughBurdenToAddToInventory(WorldObject worldObject)
        {
            return (EncumbranceVal + worldObject.EncumbranceVal <= (GetEncumbranceCapacity() * 3));
        }


        /// <summary>
        /// If enough burden is available, this will try to add (via create) an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryCreateInInventoryWithNetworking(WorldObject item)
        {
            return TryCreateInInventoryWithNetworking(item, out _);
        }

        /// <summary>
        /// If enough burden is available, this will try to add (via create) an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryCreateInInventoryWithNetworking(WorldObject item, out Container container)
        {
            if (!TryAddToInventory(item, out container)) // We don't have enough burden available or no empty pack slot.
                return false;

            Session.Network.EnqueueSend(new GameMessageCreateObject(item));

            if (item is Container itemAsContainer)
                Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

            Session.Network.EnqueueSend(
                new GameEventItemServerSaysContainId(Session, item, container),
                new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        public bool TryConsumeFromInventoryWithNetworking(WorldObject item, int amount = int.MaxValue)
        {
            if (amount >= (item.StackSize ?? 1))
            {
                if (!TryRemoveFromInventory(item.Guid, out item))
                    return false;

                Session.Network.EnqueueSend(new GameMessageInventoryRemoveObject(item));

                item.Destroy();
            }
            else
            {
                var stack = FindObject(item.Guid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var stackFoundInContainer, out var stackRootOwner, out _);

                if (stack == null || stackRootOwner == null)
                    return false;

                AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));
            }

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        public bool TryConsumeFromInventoryWithNetworking(uint wcid, int amount = int.MaxValue)
        {
            var items = GetInventoryItemsOfWCID(wcid);

            var leftReq = amount;
            foreach (var item in items)
            {
                var removeNum = Math.Min(leftReq, item.StackSize ?? 1);
                if (!TryConsumeFromInventoryWithNetworking(item, removeNum))
                    return false;

                leftReq -= removeNum;
                if (leftReq <= 0)
                    break;
            }
            return true;
        }

        public enum RemoveFromInventoryAction
        {
            None,

            ToWieldedSlot,

            DropItem,
            GiveItem,
            TradeItem,
            SellItem,

            ToCorpseOnDeath
        }

        public bool TryRemoveFromInventoryWithNetworking(uint objectGuid, out WorldObject item, RemoveFromInventoryAction removeFromInventoryAction)
        {
            return TryRemoveFromInventoryWithNetworking(new ObjectGuid(objectGuid), out item, removeFromInventoryAction); // todo fix
        }

        public bool TryRemoveFromInventoryWithNetworking(ObjectGuid objectGuid, out WorldObject item, RemoveFromInventoryAction removeFromInventoryAction)
        {
            if (!TryRemoveFromInventory(objectGuid, out item))
                return false;

            if (removeFromInventoryAction != RemoveFromInventoryAction.SellItem && removeFromInventoryAction != RemoveFromInventoryAction.GiveItem)
                Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid));

            if (removeFromInventoryAction == RemoveFromInventoryAction.TradeItem || removeFromInventoryAction == RemoveFromInventoryAction.ToCorpseOnDeath)
                Session.Network.EnqueueSend(new GameMessageInventoryRemoveObject(item));

            if (removeFromInventoryAction != RemoveFromInventoryAction.ToWieldedSlot)
            {
                // The item has gone off-player, so we must do some additional work

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin || item.WeenieType == WeenieType.Container)
                    UpdateCoinValue();

                // We must update the database with the latest ContainerId and WielderId properties.
                // If we don't, the player can drop the item, log out, and log back in. If the landblock hasn't queued a database save in that time,
                // the player will end up loading with this object in their inventory even though the landblock is the true owner. This is because
                // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
                item.SaveBiotaToDatabase();
            }

            return true;
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryEquipObjectWithNetworking(WorldObject item, EquipMask wieldedLocation)
        {
            if (!TryEquipObjectWithBroadcasting(item, wieldedLocation))
                return false;

            Session.Network.EnqueueSend(
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid),
                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0),
                new GameEventWieldItem(Session, item.Guid.Full, wieldedLocation),
                new GameMessageSound(Guid, Sound.WieldObject));

            // do the appropriate combat stance shuffling, based on the item types
            // todo: instead of switching the weapon immediately, the weapon should be swpped in the middle of the animation chain

            if (CombatMode != CombatMode.NonCombat && CombatMode != CombatMode.Undef)
            {
                switch (wieldedLocation)
                {
                    case EquipMask.MissileWeapon:
                        SetCombatMode(CombatMode.Missile);
                        break;
                    case EquipMask.Held:
                        SetCombatMode(CombatMode.Magic);
                        break;
                    default:
                        SetCombatMode(CombatMode.Melee);
                        break;
                }
            }

            // does this item cast enchantments, and currently have mana?
            if (item.ItemCurMana > 1 || item.ItemCurMana == null) // TODO: Once Item Current Mana is fixed for loot generated items, '|| item.ItemCurMana == null' can be removed
            {
                // check activation requirements
                var result = item.CheckUseRequirements(this);
                if (!result.Success)
                {
                    if (result.Message != null)
                        Session.Network.EnqueueSend(result.Message);

                    return true;
                }

                foreach (var spell in item.Biota.BiotaPropertiesSpellBook)
                {
                    if (item.HasProcSpell((uint)spell.Spell))
                        continue;

                    var enchantmentStatus = CreateItemSpell(item, (uint)spell.Spell);
                    if (enchantmentStatus.Success)
                        item.IsAffecting = true;
                }

                // handle equipment sets
                if (item.HasItemSet)
                    EquipItemFromSet(item);

                if (item.IsAffecting ?? false)
                {
                    if (item.ItemCurMana.HasValue)
                        item.ItemCurMana--;     // ?
                }
            }
            return true;
        }

        public enum DequipObjectAction
        {
            None,

            DequipToPack,
            DequipToOffPlayerContainer,

            DropItem,
            GiveItem,
            TradeItem,
            SellItem,

            ToCorpseOnDeath,

            ConsumeItem
        }

        public bool TryDequipObjectWithNetworking(uint objectGuid, out WorldObject item, DequipObjectAction dequipObjectAction)
        {
            return TryDequipObjectWithNetworking(new ObjectGuid(objectGuid), out item, dequipObjectAction); // todo fix this
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest.<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObjectWithNetworking(ObjectGuid objectGuid, out WorldObject item, DequipObjectAction dequipObjectAction)
        {
            if (!TryDequipObjectWithBroadcasting(objectGuid, out item, out var wieldedLocation, (dequipObjectAction == DequipObjectAction.DropItem)))
                return false;

            Session.Network.EnqueueSend(
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid),
                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0),
                new GameMessagePickupEvent(item),
                new GameMessageSound(Guid, Sound.UnwieldObject));

            // If item has any spells, remove them from the registry on unequip
            if (item.Biota.BiotaPropertiesSpellBook != null)
            {
                foreach (var spell in item.Biota.BiotaPropertiesSpellBook)
                {
                    if (item.HasProcSpell((uint)spell.Spell))
                        continue;

                    RemoveItemSpell(item, (uint)spell.Spell, true);
                }
            }

            // handle equipment sets
            if (item.HasItemSet)
                DequipItemFromSet(item);

            if (dequipObjectAction == DequipObjectAction.ToCorpseOnDeath)
                Session.Network.EnqueueSend(new GameMessageDeleteObject(item));

            if (dequipObjectAction == DequipObjectAction.ConsumeItem)
            {
                Session.Network.EnqueueSend(new GameMessageDeleteObject(item));
                item.Destroy();
            }

            if (dequipObjectAction != DequipObjectAction.DequipToPack)
            {
                // The item has gone off-player, so we must do some additional work

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                // We must update the database with the latest ContainerId and WielderId properties.
                // If we don't, the player can drop the item, log out, and log back in. If the landblock hasn't queued a database save in that time,
                // the player will end up loading with this object in their inventory even though the landblock is the true owner. This is because
                // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
                item.SaveBiotaToDatabase();
            }

            if (dequipObjectAction != DequipObjectAction.ToCorpseOnDeath)
            {
                if (CombatMode == CombatMode.Missile && wieldedLocation == (int)EquipMask.MissileAmmo)
                {
                    SetCombatMode(CombatMode.NonCombat);
                    return true;
                }

                if (CombatMode == CombatMode.NonCombat || (wieldedLocation != (int)EquipMask.MeleeWeapon && wieldedLocation != (int)EquipMask.MissileWeapon && wieldedLocation != (int)EquipMask.Held && wieldedLocation != (int)EquipMask.Shield && !item.IsTwoHanded))
                    return true;

                SetCombatMode(CombatMode.Melee);
            }

            return true;
        }


        // =====================================
        // Helper Functions - Inventory Movement
        // =====================================

        [Flags]
        public enum SearchLocations
        {
            MyInventory         = 0x01,
            MyEquippedItems     = 0x02,
            Landblock           = 0x04,
            LastUsedContainer   = 0x08,
            WieldedByOther      = 0x10,
            LocationsICanMove   = MyInventory | MyEquippedItems | Landblock | LastUsedContainer,
            Everywhere          = 0xFF
        }

        public WorldObject FindObject(uint objectGuid, SearchLocations searchLocations)
        {
            return FindObject(new ObjectGuid(objectGuid), searchLocations, out Container foundInContainer, out Container rootOwner, out bool wasEquipped);
        }

        public WorldObject FindObject(uint objectGuid, SearchLocations searchLocations, out Container foundInContainer, out Container rootOwner, out bool wasEquipped)
        {
            return FindObject(new ObjectGuid(objectGuid), searchLocations, out foundInContainer, out rootOwner, out wasEquipped); // todo Fix this so it's not creating a new ObjectGuid
        }

        public WorldObject FindObject(ObjectGuid objectGuid, SearchLocations searchLocations, out Container foundInContainer, out Container rootOwner, out bool wasEquipped)
        {
            WorldObject result;

            foundInContainer = null;
            rootOwner = null;
            wasEquipped = false;

            if (objectGuid == Guid) // Object we're looking for is us
            {
                rootOwner = this;
                return this;
            }

            if (searchLocations.HasFlag(SearchLocations.MyInventory))
            {
                result = GetInventoryItem(objectGuid, out foundInContainer);

                if (result != null)
                {
                    rootOwner = this;
                    return result;
                }
            }

            if (searchLocations.HasFlag(SearchLocations.MyEquippedItems))
            {
                result = GetEquippedItem(objectGuid);

                if (result != null)
                {
                    rootOwner = this;
                    wasEquipped = true;
                    return result;
                }
            }

            if (searchLocations.HasFlag(SearchLocations.Landblock))
            {
                result = CurrentLandblock?.GetObject(objectGuid);

                if (result != null)
                    return result;
            }

            if (searchLocations.HasFlag(SearchLocations.LastUsedContainer))
            {
                if (CurrentLandblock?.GetObject(LastOpenedContainerId) is Container lastOpenedContainer)
                {
                    if (lastOpenedContainer is Vendor lastUsedVendor)
                    {
                        if (lastUsedVendor.AllItemsForSale.TryGetValue(objectGuid, out result))
                        {
                            rootOwner = lastUsedVendor;
                            return result;
                        }
                    }
                    if (lastOpenedContainer.IsOpen && lastOpenedContainer.Viewer == Guid.Full)
                    {
                        result = lastOpenedContainer.GetInventoryItem(objectGuid, out foundInContainer);

                        if (result != null)
                        {
                            rootOwner = lastOpenedContainer;
                            return result;
                        }
                    }
                }
            }

            if (searchLocations.HasFlag(SearchLocations.WieldedByOther))
            {
                result = CurrentLandblock?.GetWieldedObject(objectGuid);

                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// This would be used if you need to pickup something without a MoveTo action.
        /// It will broadcast the pickup motion and add a delay for the animation length
        /// </summary>
        private ActionChain StartPickupChain()
        {
            StopExistingMoveToChains();

            ActionChain pickupChain = new ActionChain();

            // start picking up item animation
            var motion = new Motion(CurrentMotionState.Stance, MotionPickup);
            EnqueueBroadcast(
                new GameMessageUpdatePosition(this),
                new GameMessageUpdateMotion(this, motion));

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionPickup, MotionCommand.Ready);
            pickupChain.AddDelaySeconds(pickupAnimationLength);

            return pickupChain;
        }

        /// <summary>
        /// This would be used if your pickup action first requires a MoveTo action
        /// It will add a chain to broadcast the pickup motion and then add a delay for the animation length
        /// </summary>
        private ActionChain AddPickupChainToMoveToChain(Container sourceContainer, Container destContainer)
        {
            var container = destContainer == this ? sourceContainer : destContainer;
            var pickupMotion = container != null ? container.MotionPickup : MotionCommand.Pickup;

            // start picking up item animation
            var motion = new Motion(CurrentMotionState.Stance, pickupMotion);
            EnqueueBroadcast(
                new GameMessageUpdatePosition(this),
                new GameMessageUpdateMotion(this, motion));

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, pickupMotion, MotionCommand.Ready);

            var pickupChain = new ActionChain();
            pickupChain.AddDelaySeconds(pickupAnimationLength);

            return pickupChain;
        }

        /// <summary>
        /// If you want to subtract from a stack, amount should be negative.
        /// </summary>
        private void AdjustStack(WorldObject stack, int amount, Container container, Container rootContainer)
        {
            stack.StackSize += amount;
            stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
            stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

            if (container != null)
            {
                // We add to these values because amount will be negative if we're subtracting from a stack, so we want to add a negative number.
                container.EncumbranceVal += (stack.StackUnitEncumbrance * amount);
                container.Value += (stack.StackUnitValue * amount);
            }

            if (rootContainer != null && rootContainer != container)
            {
                rootContainer.EncumbranceVal += (stack.StackUnitEncumbrance * amount);
                rootContainer.Value += (stack.StackUnitValue * amount);
            }
        }


        // =========================================
        // Game Action Handlers - Inventory Movement 
        // =========================================

        /// <summary>
        /// This method processes the Game Action (F7B1) Put Item In Container (0x0019)
        /// This is raised when we:
        /// - move an item around in our inventory.
        /// - dequip an item.
        /// - Pickup an item off of the landblock or a container on the landblock
        /// - Put an item into a container on the landblock
        /// - Move an item between containers on a landblock
        /// </summary>
        public void HandleActionPutItemInContainer(uint itemGuid, uint containerGuid, int placement = 0)
        {
            OnPutItemInContainer(itemGuid, containerGuid, placement);

            var item = FindObject(itemGuid, SearchLocations.LocationsICanMove, out _, out var itemRootOwner, out var itemWasEquipped);
            var container = FindObject(containerGuid, SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out _, out var containerRootOwner, out _) as Container;

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (!item.Guid.IsDynamic() || item is Creature || (item.BaseDescriptionFlags & ObjectDescriptionFlag.Stuck) != 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to move item 0x{2:X8}:{3}.", Guid.Full, Name, item.Guid.Full, item.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You can't move that!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (itemRootOwner != this && containerRootOwner == this && !HasEnoughBurdenToAddToInventory(item))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to carry that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (container == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target container not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (containerRootOwner != this) // Is our target on the landscape?
            {
                if (itemRootOwner == this && (item.Attuned ?? 0) >= 1)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.AttunedItem));
                    return;
                }

                if (containerRootOwner != null && !containerRootOwner.IsOpen)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.TheContainerIsClosed));
                    return;
                }
            }

            if (containerRootOwner is Corpse corpse)
            {
                if (corpse.IsMonster == false)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.Dead));
                    return;
                }
            }

            if ((itemRootOwner == this && containerRootOwner != this)  || (itemRootOwner != this && containerRootOwner == this)) // Movement is between the player and the world
            {
                Container itemAsContainer = item as Container;

                // Checking to see if item to pick is an container itself and IsOpen
                if (itemAsContainer != null && itemAsContainer.IsOpen)
                {
                    if (itemAsContainer.Viewer == Session.Player.Guid.Full)
                    {
                        // We're the one that has it open. Close it before picking it up
                        itemAsContainer.Close(this);
                    }
                    else
                    {
                        // We're not who has it open. Can't pick up something someone else is viewing!
                        Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.The_IsCurrentlyInUse, item.Name));
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                        return;
                    }
                }

                WorldObject moveToTarget;
                if (itemRootOwner == this)
                    moveToTarget = containerRootOwner ?? container; // Movement is from player
                else
                    moveToTarget = itemRootOwner ?? item; // Movement is too player

                CreateMoveToChain(moveToTarget, (success) =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to pick up the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    if (!success)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    // Was this item picked up by someone else?
                    if (itemRootOwner == null && item.CurrentLandblock == null)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    var pickupChain = AddPickupChainToMoveToChain(itemRootOwner, container);

                    pickupChain.AddAction(this, () =>
                    {
                        var returnStance = new Motion(CurrentMotionState.Stance);

                        // Was this item picked up by someone else?
                        if (itemRootOwner == null && item.CurrentLandblock == null)
                        {
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                            EnqueueBroadcastMotion(returnStance);
                            return;
                        }

                        var questSolve = false;

                        if (itemRootOwner != this && containerRootOwner == this && item.Quest != null) // We're picking up a quest item
                        {
                            if (!QuestManager.CanSolve(item.Quest))
                            {
                                QuestManager.HandleSolveError(item.Quest);
                                EnqueueBroadcastMotion(returnStance);
                                return;
                            }

                            questSolve = true;
                        }

                        if (DoHandleActionPutItemInContainer(item, itemRootOwner, itemWasEquipped, container, containerRootOwner, placement))
                        {
                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (item.WeenieType == WeenieType.Coin || item.WeenieType == WeenieType.Container)
                                UpdateCoinValue();

                            if (itemRootOwner == this)
                            {
                                item.EmoteManager.OnDrop(this);
                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                            }
                            else if (containerRootOwner == this)
                            {
                                if (itemAsContainer != null) // We're picking up a pack
                                {
                                    Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

                                    foreach (var packItem in itemAsContainer.Inventory)
                                        Session.Network.EnqueueSend(new GameMessageCreateObject(packItem.Value));
                                }

                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));

                                item.EmoteManager.OnPickup(this);
                                item.NotifyOfEvent(RegenerationType.PickUp);

                                if (questSolve)
                                    QuestManager.Update(item.Quest);
                            }
                        }

                        EnqueueBroadcastMotion(returnStance);
                    });

                    pickupChain.EnqueueChain();
                });
            }
            else // This is a self-contained movement
            {
                DoHandleActionPutItemInContainer(item, itemRootOwner, itemWasEquipped, container, containerRootOwner, placement);
            }
        }

        private bool DoHandleActionPutItemInContainer(WorldObject item, Container itemRootOwner, bool itemWasEquipped, Container container, Container containerRootOwner, int placement)
        {
            if (item.CurrentLandblock != null) // Movement is an item pickup off the landblock
            {
                item.CurrentLandblock.RemoveWorldObject(item.Guid, false, true);
                item.Location = null;
            }
            else if (itemWasEquipped) // Movement is an equipped item to a container on the landblock
            {
                var dequipObjectAction = containerRootOwner == this ? DequipObjectAction.DequipToPack : DequipObjectAction.DequipToOffPlayerContainer;

                if (!TryDequipObjectWithNetworking(item.Guid, out _, dequipObjectAction))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryDequipObjectWithNetworking failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                    return false;
                }
            }
            else // Movement is within the same pack or between packs in a container on the landblock
            {
                if (!itemRootOwner.TryRemoveFromInventory(item.Guid))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                }

                if (itemRootOwner == this && containerRootOwner != this)
                {
                    // We must update the database with the latest ContainerId and WielderId properties.
                    // If we don't, the player can drop the item, log out, and log back in. If the landblock hasn't queued a database save in that time,
                    // the player will end up loading with this object in their inventory even though the landblock is the true owner. This is because
                    // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
                    item.SaveBiotaToDatabase();
                }
            }

            if (!container.TryAddToInventory(item, placement, true))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "AddToInventory failed!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));

                // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.
                log.WarnFormat("Item 0x{0:X8}:{1} for player {2} lost from DoHandleActionPutItemInContainer failure.", item.Guid.Full, item.Name, Name);

                return false;
            }

            if (container != containerRootOwner && containerRootOwner != null)
            {
                containerRootOwner.EncumbranceVal += (item.EncumbranceVal ?? 0);
                containerRootOwner.Value += (item.Value ?? 0);
            }

            Session.Network.EnqueueSend(
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid),
                new GameEventItemServerSaysContainId(Session, item, container));

            return true;
        }

        /// <summary>
        /// This method processes the Game Action (F7B1) Drop Item (0x001B)
        /// This is raised when we:
        /// - drop an equipped item
        /// - drop an item from inventory
        /// </summary>
        public void HandleActionDropItem(uint itemGuid)
        {
            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out _, out _, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if ((item.Attuned ?? 0) >= 1)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.AttunedItem));
                return;
            }

            if (wasEquipped)
            {
                if (!TryDequipObjectWithNetworking(itemGuid, out item, DequipObjectAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to dequip item!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                    return;
                }
            }
            else
            {
                if (!TryRemoveFromInventoryWithNetworking(itemGuid, out item, RemoveFromInventoryAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to remove item from inventory!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                    return;
                }
            }

            var actionChain = StartPickupChain();

            actionChain.AddAction(this, () =>
            {
                if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to drop the item
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                    return;
                }

                item.Location = new Position(Location.InFrontOf(1.1f));
                item.Location.LandblockId = new LandblockId(item.Location.GetCell());

                item.Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.

                if (IsDirectVisible(item, item.Location) && CurrentLandblock.AddWorldObject(item))
                {
                    Session.Network.EnqueueSend(
                        new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid),
                        new GameEventItemServerSaysMoveItem(Session, item),
                        new GameMessageUpdatePosition(item));

                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                }
                else
                {
                    // not enough room to drop item
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.BadDrop));
                    TryAddToInventory(item);
                }

                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// This method processes the Game Action (F7B1) Get And Wield Item (0x001A)
        /// This is raised when we:
        /// - try to wield an item in from inventory
        /// - try to wield an item in a chest
        /// - try to wield an item on the landscape
        /// - try to transfer a wielded item to another wield location
        /// </summary>
        public void HandleActionGetAndWieldItem(uint itemGuid, EquipMask wieldedLocation)
        {
            // todo fix this, it seems IsAnimating is always true for a player
            // todo we need to know when a player is busy to avoid additional actions during that time
            /*if (IsAnimating)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.YoureTooBusy));
                return;
            }*/

            var item = FindObject(new ObjectGuid(itemGuid), SearchLocations.LocationsICanMove, out _, out var rootOwner, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (rootOwner != this && !HasEnoughBurdenToAddToInventory(item))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to carry that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (rootOwner != this) // Item is on the landscape, or in a landblock chest
            {
                CreateMoveToChain(rootOwner ?? item, (success) =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to pick up the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    if (!success)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    // Was this item picked up by someone else?
                    if (rootOwner == null && item.CurrentLandblock == null)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    var pickupChain = AddPickupChainToMoveToChain(rootOwner, this);

                    pickupChain.AddAction(this, () =>
                    {
                        var returnStance = new Motion(CurrentMotionState.Stance);

                        // Was this item picked up by someone else?
                        if (rootOwner == null && item.CurrentLandblock == null)
                        {
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                            EnqueueBroadcastMotion(returnStance);
                            return;
                        }

                        if (DoHandleActionGetAndWieldItem(item, rootOwner, wasEquipped, wieldedLocation))
                        {
                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));

                            item.EmoteManager.OnPickup(this);
                            item.NotifyOfEvent(RegenerationType.PickUp);
                        }

                        EnqueueBroadcastMotion(returnStance);
                    });

                    pickupChain.EnqueueChain();
                });
            }
            else
            {
                DoHandleActionGetAndWieldItem(item, rootOwner, wasEquipped, wieldedLocation);
            }
        }

        private bool DoHandleActionGetAndWieldItem(WorldObject item, Container itemRootOwner, bool wasEquipped, EquipMask wieldedLocation)
        {
            var wieldError = CheckWieldRequirement(item);

            if (wieldError != WeenieError.None)
            {
                // client doesnt show specific wieldError here, just '<item> can't be wielded'?
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full, wieldError));
                return false;
            }

            // Unwield wand/missile launcher if dual wielding
            if (wieldedLocation == EquipMask.Shield && !item.IsShield)
            {
                var mainWeapon = EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon || e.CurrentWieldedLocation == EquipMask.Held);
                if (mainWeapon != null)
                {
                    if (!TryDequipObjectWithNetworking(mainWeapon.Guid, out var dequippedItem, DequipObjectAction.DequipToPack))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to dequip existing weapon!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                        return false;
                    }

                    if (!TryCreateInInventoryWithNetworking(dequippedItem))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to add dequip back into inventory!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));

                        // todo: if this happens, we should just put back the dequipped item to where it was

                        return false;
                    }
                }
            }

            // TODO: this handles armor slots,
            // trinkets and weapons would need to be handled a bit differently

            // TODO: slots view is bugged here
            // for both slots view and non-slots view, the client is oddly sending 2 packets, similar to dual wielding weapon swapping
            // for non-slots view, the 2 packets it sends both have the full coverage slots in wieldedLocation
            // for slots view, it sends the correct packet first, with the full coverage, and then it sends a packet with coverage for just 1 slot
            // this bugs out CurrentWieldedLocation, as it won't be covering all of the slots... so for armor/clothing we set wieldedLocation to item.ValidLocations here
            if (item is Clothing)
                wieldedLocation = item.ValidLocations ?? 0;

            // verify item slot is valid
            // restricting this to two-handed for now, as without that clamp, it bugs out dual wielding and possibly other things
            if (item.WeaponSkill == Skill.TwoHandedCombat && (wieldedLocation & item.ValidLocations) == 0)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                return false;
            }

            if (!WieldedLocationIsAvailable(item, wieldedLocation))
            {
                // filtering to just armor here, or else trinkets and dual wielding breaks
                var existing = GetEquippedClothingArmor(item.ClothingPriority ?? 0).FirstOrDefault();

                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"You must remove your {existing.Name} to wear that"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                return false;
            }

            if (wasEquipped) // Movement is an equipped item to another equipped item slot
            {
                var prevLocation = item.CurrentWieldedLocation;

                item.CurrentWieldedLocation = wieldedLocation;
                Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, (int)wieldedLocation));

                Session.Network.EnqueueSend(new GameEventWieldItem(Session, item.Guid.Full, wieldedLocation));

                // handle swapping melee weapon between hands
                if (IsInChildLocation(item))
                {
                    ResetChild(item);
                    EnqueueBroadcast(new GameMessageParentEvent(this, item, (int?)item.ParentLocation ?? 0, (int?)item.Placement ?? 0));

                    // handle swapping dual-wielded weapons
                    if (IsDoubleSend)
                        HandleActionGetAndWieldItem(Prev_PutItemInContainer.ItemGuid, (EquipMask)prevLocation);
                    else
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject));
                }
                return true;
            }

            if (item.CurrentLandblock != null) // Movement is an item pickup off the landblock
            {
                item.CurrentLandblock.RemoveWorldObject(item.Guid, false, true);
                item.Location = null;
            }
            else // Movement is within the same pack or between packs in a container on the landblock
            {
                if (!itemRootOwner.TryRemoveFromInventory(item.Guid))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                }
            }

            if (!TryEquipObjectWithNetworking(item, wieldedLocation))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryEquipObjectWithNetworking failed!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));

                // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.
                log.WarnFormat("Item 0x{0:X8}:{1} for player {2} lost from DoHandleActionGetAndWieldItem failure.", item.Guid.Full, item.Name, Name);

                return false;
            }

            return true;
        }

        private WeenieError CheckWieldRequirement(WorldObject item)
        {
            if (!PropertyManager.GetBool("use_wield_requirements").Item)
                return WeenieError.None;

            var skillOrAttribute = item.WieldSkillType ?? 0;
            var difficulty = (uint)(item.WieldDifficulty ?? 0);

            switch (item.WieldRequirements)
            {
                case WieldRequirement.Skill:

                    // verify skill level - current / buffed
                    var skill = GetCreatureSkill(ConvertToMoASkill((Skill)skillOrAttribute));
                    if (skill.Current < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.RawSkill:

                    // verify skill level - base
                    skill = GetCreatureSkill(ConvertToMoASkill((Skill)skillOrAttribute));
                    if (skill.Base < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.Attrib:

                    // verify primary attribute - current / buffed
                    Attributes.TryGetValue((PropertyAttribute)skillOrAttribute, out var attribute);
                    if (attribute.Current < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.RawAttrib:

                    // verify primary attribute - base
                    Attributes.TryGetValue((PropertyAttribute)skillOrAttribute, out attribute);
                    if (attribute.Base < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.SecondaryAttrib:

                    // verify vital - current maxvalue
                    Vitals.TryGetValue((PropertyAttribute2nd)skillOrAttribute, out var vital);
                    if (vital.MaxValue < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.RawSecondaryAttrib:

                    // verify vital - base
                    Vitals.TryGetValue((PropertyAttribute2nd)skillOrAttribute, out vital);
                    if (vital.Base < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.Level:

                    // verify player level
                    if (Level < difficulty)
                        return WeenieError.LevelTooLow;
                    break;

                case WieldRequirement.Training:

                    // verify skill is trained / specialized
                    skill = GetCreatureSkill(ConvertToMoASkill((Skill)skillOrAttribute));
                    if ((int)skill.AdvancementClass < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.IntStat:      // unused in PY16

                    // verify PropertyInt minimum
                    var propInt = GetProperty((PropertyInt)skillOrAttribute);
                    if (propInt < difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.BoolStat:     // unused in PY16

                    // verify PropertyBool equal
                    var propBool = GetProperty((PropertyBool)skillOrAttribute);
                    if (propBool != Convert.ToBoolean(difficulty))
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.CreatureType:

                    // verify creature type
                    var creatureType = CreatureType ?? ACE.Entity.Enum.CreatureType.Invalid;
                    if ((int)creatureType != difficulty)
                        return WeenieError.SkillTooLow;
                    break;

                case WieldRequirement.HeritageType:

                    // verify heritage type
                    if ((int)HeritageGroup != difficulty)
                        return WeenieError.ArmorRequiresSpecificHeritage;
                    break;
            }

            return WeenieError.None;
        }


        // =========================================
        // Game Action Handlers - Inventory Stacking 
        // =========================================

        /// <summary>
        /// This method processes the Game Action (F7B1) Stackable Split To Container (0x0055)
        /// This is raised when we:
        /// - try to split a stack into the same container
        /// - try to split a stack off of the landblock into a container
        /// - try to split a stack into a different container that doesn't already have a stack that can support a merge
        /// </summary>
        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, int placementPosition, ushort amount)
        {
            if (amount == 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            var stack = FindObject(new ObjectGuid(stackId), SearchLocations.LocationsICanMove, out var stackFoundInContainer, out var stackRootOwner, out _);
            var container = FindObject(new ObjectGuid(containerId), SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out _, out var containerRootOwner, out _) as Container;

            if (stack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (stackRootOwner != this && containerRootOwner == this && !HasEnoughBurdenToAddToInventory(stack))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to carry that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (container == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target container not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (stack.StackSize == null || stack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split invalid item 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (stack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if ((stackRootOwner == this && containerRootOwner != this)  || (stackRootOwner != this && containerRootOwner == this)) // Movement is between the player and the world
            {
                WorldObject moveToObject;

                if (stackRootOwner == this)
                    moveToObject = containerRootOwner ?? container;
                else
                    moveToObject = stackRootOwner ?? stack;

                var stackOriginalContainer = stack.ContainerId;

                CreateMoveToChain(moveToObject, (success) =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to split the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId, WeenieError.ActionCancelled));
                        return;
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);

                    if (!success)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId, WeenieError.ActionCancelled));
                        return;
                    }

                    // We make sure the stack is still valid. It could have changed during our movement
                    if (stackOriginalContainer != stack.ContainerId || stack.StackSize < amount)
                    {
                        log.DebugFormat("Player 0x{0:X8}:{1} tried to split an item that's no longer valid 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId, WeenieError.ActionCancelled));
                        return;
                    }

                    var pickupChain = AddPickupChainToMoveToChain(stackRootOwner, containerRootOwner);

                    pickupChain.AddAction(this, () =>
                    {
                        // We make sure the stack is still valid. It could have changed during our pickup animation
                        if (stackOriginalContainer != stack.ContainerId || stack.StackSize < amount)
                        {
                            log.DebugFormat("Player 0x{0:X8}:{1} tried to split an item that's no longer valid 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split failed!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId, WeenieError.ActionCancelled));
                            EnqueueBroadcastMotion(returnStance);
                            return;
                        }

                        var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
                        newStack.SetStackSize(amount);

                        if (DoHandleActionStackableSplitToContainer(stack, stackFoundInContainer, stackRootOwner, container, containerRootOwner, newStack, placementPosition, amount))
                        {
                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (stack.WeenieType == WeenieType.Coin)
                                UpdateCoinValue();

                            if (stackRootOwner == this)
                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                            else if (containerRootOwner == this)
                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));
                        }

                        EnqueueBroadcastMotion(returnStance);
                    });

                    pickupChain.EnqueueChain();
                });
            }
            else // This is a self-contained movement
            {
                var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
                newStack.SetStackSize(amount);

                DoHandleActionStackableSplitToContainer(stack, stackFoundInContainer, stackRootOwner, container, containerRootOwner, newStack, placementPosition, amount);
            }
        }

        private bool DoHandleActionStackableSplitToContainer(WorldObject stack, Container stackFoundInContainer, Container stackRootOwner, Container container, Container containerRootOwner, WorldObject newStack, int placementPosition, int amount)
        {
            // Before we modify the original stack, we make sure we can add the new stack
            if (!container.TryAddToInventory(newStack, placementPosition, true))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryAddToInventory failed!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stack.Guid.Full));
                return false;
            }

            if (container != containerRootOwner && containerRootOwner != null)
            {
                containerRootOwner.EncumbranceVal += (stack.StackUnitEncumbrance * amount);
                containerRootOwner.Value += (stack.StackUnitValue * amount);
            }

            Session.Network.EnqueueSend(new GameMessageCreateObject(newStack));
            Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, newStack, container));

            AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
            if (stackRootOwner == null)
                EnqueueBroadcast(new GameMessageSetStackSize(stack));
            else
                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

            return true;
        }

        /// <summary>
        /// This method processes the Game Action (F7B1) Stackable Split To 3D (0x0056)
        /// This is raised when we:
        /// - try to split a stack onto the landblock
        /// </summary>
        public void HandleActionStackableSplitTo3D(uint stackId, int amount)
        {
            if (amount == 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            var stack = FindObject(new ObjectGuid(stackId), SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var stackFoundInContainer, out var stackRootOwner, out _);

            if (stack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (stack.StackSize == null || stack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split invalid item 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            if (stack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId));
                return;
            }

            var actionChain = StartPickupChain();

            actionChain.AddAction(this, () =>
            {
                if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to drop the item
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, stackId, WeenieError.ActionCancelled));
                    return;
                }

                AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
                newStack.SetStackSize(amount);

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                newStack.Location = new Position(Location.InFrontOf(1.1f));
                newStack.Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.

                if (CurrentLandblock.AddWorldObject(newStack))
                {
                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                }
                else
                {
                    // todo: if this happens, we should just put split amount back into the original stack
                    log.WarnFormat("Partial stack 0x{0:X8}:{1} for player {2} lost from HandleActionStackableSplitTo3D failure.", stack.Guid.Full, stack.Name, Name);
                }

                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// This method processes the Game Action (F7B1) Stackable Merge (0x0054)
        /// This is raised when we:
        /// - try to merge two stacks stack in the same container
        /// - try to merge two stacks stack in different container
        /// - try to merge a stack from the landblock into a container
        /// - try to split a stack into a different container that has a stack that can support a merge
        /// </summary>
        public void HandleActionStackableMerge(uint mergeFromGuid, uint mergeToGuid, int amount)
        {
            if (amount == 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            var sourceStack = FindObject(mergeFromGuid, SearchLocations.LocationsICanMove, out var sourceStackFoundInContainer, out var sourceStackRootOwner, out _);
            var targetStack = FindObject(mergeToGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.LastUsedContainer, out var targetStackFoundInContainer, out var targetStackRootOwner, out _);

            if (sourceStack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if (sourceStackRootOwner != this && targetStackRootOwner == this && !HasEnoughBurdenToAddToInventory(sourceStack))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to carry that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if (targetStack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if (sourceStack.WeenieClassId != targetStack.WeenieClassId)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to merge different items 0x{2:X8}:{3} and 0x{4:X8}:{5}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name, targetStack.Guid.Full, targetStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stacks not compatible!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid, WeenieError.YouCannotMergeDifferentStacks));
                return;
            }

            if (sourceStack.StackSize == null || sourceStack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to merge invalid source item 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if (targetStack.StackSize == null || targetStack.StackSize == 0 || targetStack.StackSize == targetStack.MaxStackSize)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to merge invalid target item 0x{2:X8}:{3}.", Guid.Full, Name, targetStack.Guid.Full, targetStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if (sourceStack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0}:{1} tried to merge item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid));
                return;
            }

            if ((sourceStackRootOwner == this && targetStackRootOwner != this)  || (sourceStackRootOwner != this && targetStackRootOwner == this)) // Movement is between the player and the world
            {
                WorldObject moveToObject;

                if (sourceStackRootOwner == this)
                    moveToObject = targetStackRootOwner ?? targetStack;
                else
                    moveToObject = sourceStackRootOwner ?? sourceStack;

                var sourceStackOriginalContainer = sourceStack.ContainerId;

                CreateMoveToChain(moveToObject, (success) =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to split the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);

                    if (!success)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    // We make sure the stack is still valid. It could have changed during our movement
                    if (sourceStackOriginalContainer != sourceStack.ContainerId || sourceStack.StackSize < amount)
                    {
                        log.DebugFormat("Player 0x{0}:{1} tried to merge an item that's no longer valid 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge Failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid, WeenieError.ActionCancelled));
                        return;
                    }

                    var pickupChain = AddPickupChainToMoveToChain(sourceStackRootOwner, targetStackRootOwner);

                    pickupChain.AddAction(this, () =>
                    {
                        // We make sure the stack is still valid. It could have changed during our pickup animation
                        if (sourceStackOriginalContainer != sourceStack.ContainerId || sourceStack.StackSize < amount)
                        {
                            log.DebugFormat("Player 0x{0}:{1} tried to merge an item that's no longer valid 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge Failed!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, mergeFromGuid, WeenieError.ActionCancelled));
                            EnqueueBroadcastMotion(returnStance);
                            return;
                        }

                        if (DoHandleActionStackableMerge(sourceStack, sourceStackFoundInContainer, sourceStackRootOwner, targetStack, targetStackFoundInContainer, targetStackRootOwner, amount))
                        {
                            // If the client used the R key to merge a partial stack from the landscape, it also tries to add the "ghosted" item of the picked up stack to the inventory as well.
                            if (sourceStackRootOwner != this && sourceStack.StackSize > 0)
                                Session.Network.EnqueueSend(new GameMessageCreateObject(sourceStack));

                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (sourceStack.WeenieType == WeenieType.Coin)
                                UpdateCoinValue();

                            if (sourceStackRootOwner == this)
                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                            else if (targetStackRootOwner == this)
                                EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));
                        }

                        EnqueueBroadcastMotion(returnStance);
                    });

                    pickupChain.EnqueueChain();
                });
            }
            else // This is a self-contained movement
            {
                DoHandleActionStackableMerge(sourceStack, sourceStackFoundInContainer, sourceStackRootOwner, targetStack, targetStackFoundInContainer, targetStackRootOwner, amount);
            }
        }

        private bool DoHandleActionStackableMerge(WorldObject sourceStack, Container sourceStackFoundInContainer, Container sourceStackRootOwner, WorldObject targetStack, Container targetStackFoundInContainer, Container targetStackRootOwner, int amount)
        {
            if (amount == sourceStack.StackSize && sourceStack.StackSize + targetStack.StackSize <= targetStack.MaxStackSize) // The merge will consume the entire source stack
            {
                if (sourceStack.CurrentLandblock != null) // Movement is an item pickup off the landblock
                {
                    sourceStack.CurrentLandblock.RemoveWorldObject(sourceStack.Guid, false, true);
                    sourceStack.Location = null;
                }
                else if (sourceStackRootOwner != null && !sourceStackRootOwner.TryRemoveFromInventory(sourceStack.Guid, out _))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, sourceStack.Guid.Full));
                    return false;
                }

                if (sourceStackRootOwner == this)
                    Session.Network.EnqueueSend(new GameMessageInventoryRemoveObject(sourceStack));
                else
                    Session.Network.EnqueueSend(new GameMessageDeleteObject(sourceStack));

                sourceStack.Destroy();

                AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
            }
            else // The merge will reduce the size of the source stack
            {
                AdjustStack(sourceStack, -amount, sourceStackFoundInContainer, sourceStackRootOwner);
                if (sourceStackRootOwner == null)
                    EnqueueBroadcast(new GameMessageSetStackSize(sourceStack));
                else
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(sourceStack));

                AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
            }

            return true;
        }


        // =============================================
        // Game Action Handlers - Inventory Give/Receive 
        // =============================================

        /// <summary>
        /// This method processes the Game Action (F7B1) Give Object Request (0x00CD)
        /// This is raised when we:
        /// - try to give an object to another player
        /// - try to give an object to an NPC
        /// </summary>
        public void HandleActionGiveObjectRequest(uint targetGuid, uint itemGuid, int amount)
        {
            var target = FindObject(targetGuid, SearchLocations.Landblock, out _, out _, out _) as Container;
            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var itemFoundInContainer, out var itemRootOwner, out var itemWasEquipped);

            if (target == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (amount == 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Give amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            if (item.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to Give item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, item.Guid.Full, item.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Give amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid));
                return;
            }

            CreateMoveToChain(target, (success) =>
            {
                if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to pick up the item
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                    return;
                }

                if (!success)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, itemGuid, WeenieError.ActionCancelled));
                    return;
                }

                if (target is Player targetAsPlayer)
                    GiveObjecttoPlayer(targetAsPlayer, item, itemFoundInContainer, itemRootOwner, itemWasEquipped, amount);
                else
                    GiveObjecttoNPC(target, item, itemFoundInContainer, itemRootOwner, itemWasEquipped, amount);
            });
        }

        private void GiveObjecttoPlayer(Player target, WorldObject item, Container itemFoundInContainer, Container itemRootOwner, bool itemWasEquipped, int amount)
        {
            if ((item.Attuned ?? 0) >= 1)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full, WeenieError.AttunedItem));
                return;
            }

            if ((target.Character.CharacterOptions1 & (int)CharacterOptions1.LetOtherPlayersGiveYouItems) != (int)CharacterOptions1.LetOtherPlayersGiveYouItems)
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsNotAcceptingGiftsRightNow, target.Name));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                return;
            }

            if (!RemoveItemForGive(item, itemFoundInContainer, itemWasEquipped, itemRootOwner, amount, out WorldObject itemToGive))
                return;

            if (!target.TryCreateInInventoryWithNetworking(itemToGive, out var targetContainer))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryCreateInInventoryWithNetworking failed!")); // Custom error message

                // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.
                log.WarnFormat("Item 0x{0:X8}:{1} for player {2} lost from GiveObjecttoPlayer failure.", item.Guid.Full, item.Name, Name);

                return;
            }

            if (item == itemToGive)
                Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, target));

            var stackSize = itemToGive.StackSize ?? 1;

            var stackMsg = stackSize > 1 ? $"{stackSize} " : "";
            var itemName = stackSize > 1 ? itemToGive.GetPluralName() : itemToGive.Name;

            Session.Network.EnqueueSend(new GameMessageSystemChat($"You give {target.Name} {stackMsg}{itemName}.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem));

            target.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} gives you {stackMsg}{itemName}.", ChatMessageType.Broadcast));
            target.Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem));

            // This is a hack because our Player_Tracking->RemoveTrackedEquippedObject() is doing GameMessageDeleteObject, not GameMessagePickupEvent
            // Without this, when you give an equipped item to a player, the player won't see it appear in their inventory
            // A bug still exists in the following scenario:
            // Player A equips weapon, gives weapon (while equipped) to player B.
            // Player B then gives weapon back to A. Player B is now bugged. The fix is to fix RemoveTrackedEquippedObject
            if (itemWasEquipped)
            {
                new ActionChain(target, () =>
                {
                    target.Session.Network.EnqueueSend(new GameMessageCreateObject(item));
                    target.Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, targetContainer));
                }).EnqueueChain();
            }
        }

        private void GiveObjecttoNPC(WorldObject target, WorldObject item, Container itemFoundInContainer, Container itemRootOwner, bool itemWasEquipped, int amount)
        {
            if (target == null || item == null) return;

            if (target.EmoteManager.IsBusy)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsTooBusyToAcceptGifts, target.Name));
                return;
            }

            if (!target.GetProperty(PropertyBool.AllowGive) ?? false)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsNotAcceptingGiftsRightNow, target.Name));
                return;
            }

            var acceptAll = (target.GetProperty(PropertyBool.AiAcceptEverything) ?? false) && (item.Attuned ?? 0) != (int)AttunedStatus.Sticky;

            if (target.HandleNPCReceiveItem(item, this, out var result) || acceptAll)
            {
                if (acceptAll || result.Category == (uint)EmoteCategory.Give)
                {
                    // for NPCs that accept items with EmoteCategory.Give,
                    // if stacked item, only give 1
                    if (RemoveItemForGive(item, itemFoundInContainer, itemWasEquipped, itemRootOwner, 1, out WorldObject itemToGive, true))
                    {
                        if (itemToGive == null)
                            Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, target));

                        Session.Network.EnqueueSend(new GameMessageSystemChat($"You give {target.Name} {item.Name}.", ChatMessageType.Broadcast));
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem));
                    }
                }
                else if (result.Category == (uint)EmoteCategory.Refuse)
                {
                    // Item rejected by npc
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"You allow {target.Name} to examine your {item.Name}.", ChatMessageType.Broadcast));
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full, WeenieError.TradeAiRefuseEmote));
                }
            }
            else
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, (WeenieErrorWithString)WeenieError.TradeAiDoesntWant, target.Name));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
            }
        }

        private bool RemoveItemForGive(WorldObject item, Container itemFoundInContainer, bool itemWasEquipped, Container itemRootOwner, int amount, out WorldObject itemToGive, bool destroy = false)
        {
            if (item.StackSize > 1 && amount < item.StackSize) // We're splitting a stack
            {
                AdjustStack(item, -amount, itemFoundInContainer, itemRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(item));

                var newStack = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);
                newStack.SetStackSize(amount);

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                itemToGive = newStack;

                return true;
            }

            // We're giving the whole object
            if (itemWasEquipped)
            {
                if (!TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.GiveItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryDequipObjectWithNetworking failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                    itemToGive = null;
                    return false;
                }
            }
            else
            {
                if (!TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.GiveItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventoryWithNetworking failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));
                    itemToGive = null;
                    return false;
                }
            }

            if (destroy)
            {
                item.Destroy();
                itemToGive = null;
            }
            else
                itemToGive = item;

            return true;
        }


        // ===========================
        // Game Action Handlers - Misc
        // ===========================

        /// <summary>
        /// This method processes the Game Action (F7B1) Set Inscription (0x00BF)
        /// This is raised when we:
        /// - try to inscribe an item
        /// </summary>
        public void HandleActionSetInscription(uint itemGuid, string inscriptionText)
        {
            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out _, out _, out _);

            if (item == null)
            {
                log.Error("Player_Inventory HandleActionSetInscription failed");
                return;
            }

            if (item.Inscribable == true && item.ScribeName != "prewritten")
            {
                if (item.ScribeName != null && item.ScribeName != Name)
                    ChatPacket.SendServerMessage(Session, "Only the original scribe may alter this without the use of an uninscription stone.", ChatMessageType.Broadcast);
                else
                {
                    if (inscriptionText != "")
                    {
                        item.Inscription = inscriptionText;
                        item.ScribeName = Name;
                        item.ScribeAccount = Session.Account;
                        Session.Network.EnqueueSend(new GameEventInscriptionResponse(Session, item.Guid.Full, item.Inscription, item.ScribeName, item.ScribeAccount));
                    }
                    else
                    {
                        item.Inscription = null;
                        item.ScribeName = null;
                        item.ScribeAccount = null;
                    }
                }
            }
            else
            {
                // Send some cool you cannot inscribe that item message. Not sure how that was handled live, I could not find a pcap of a failed inscription. Og II
                ChatPacket.SendServerMessage(Session, "Target item cannot be inscribed.", ChatMessageType.System);
            }
        }

        // This handles a peculiar sequence sent by the client in certain scenarios
        // The client will double-send 0x19 PutItemInContainer for the same object
        // (swapping dual wield weapons, swapping ammo types in combat)

        private PutItemInContainerEvent Prev_PutItemInContainer;
        private bool IsDoubleSend;

        private void OnPutItemInContainer(uint itemGuid, uint containerGuid, int placement)
        {
            var putItemInContainer = new PutItemInContainerEvent(itemGuid, containerGuid, placement);

            if (Prev_PutItemInContainer != null)
                IsDoubleSend = putItemInContainer.IsDoubleSend(Prev_PutItemInContainer);

            Prev_PutItemInContainer = putItemInContainer;
        }
    }
}
