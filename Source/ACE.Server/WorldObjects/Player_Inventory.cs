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
            return int.MaxValue; // fix
            /*var encumbranceAgumentations = 0; // todo

            var strength = Attributes[PropertyAttribute.Strength].Current;

            return (int)((150 * strength) + (encumbranceAgumentations * 30 * strength));*/
        }

        public bool HasEnoughBurdenToAddToInventory(WorldObject worldObject)
        {
            return (EncumbranceVal + worldObject.EncumbranceVal <= GetEncumbranceCapacity());
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

        public bool TryConsumeFromInventoryWithNetworking(WorldObject item, int amount)
        {
            if (amount >= (item.StackSize ?? 1))
            {
                if (!TryRemoveFromInventory(item.Guid, out item))
                    return false;

                Session.Network.EnqueueSend(new GameEventInventoryRemoveObject(Session, item));

                item.Destroy();
            }
            else
            {
                var stack = FindObject(item.Guid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var stackFoundInContainer, out var stackRootOwner, out _);

                if (stack == null || stackFoundInContainer == null || stackRootOwner == null)
                    return false;

                AdjustStack(stack, amount, stackFoundInContainer, stackRootOwner);
            }

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        public enum RemoveFromInventoryAction
        {
            None,

            ToWieldedSlot,

            DropItem,
            PlaceItemInLandblockContainer,
        }

        public bool TryRemoveFromInventoryWithNetworking(ObjectGuid objectGuid, out WorldObject item, RemoveFromInventoryAction removeFromInventoryAction)
        {
            if (!TryRemoveFromInventory(objectGuid, out item))
                return false;

            Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)));

            if (removeFromInventoryAction != RemoveFromInventoryAction.ToWieldedSlot)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin)
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
        public bool TryEquipObjectWithNetworking(WorldObject item, int wieldedLocation)
        {
            if (!TryEquipObjectWithBroadcasting(item, wieldedLocation))
                return false;

            Session.Network.EnqueueSend(new GameEventWieldItem(Session, item.Guid.Full, wieldedLocation));

            if ((EquipMask)wieldedLocation != EquipMask.MissileAmmo)
            {
                //var msgWieldItem = new GameEventWieldItem(Session, item.Guid.Full, wieldedLocation);

                Session.Network.EnqueueSend(
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid),
                    new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldedLocation));

                EnqueueBroadcast(new GameMessageParentEvent(this, item, (int?)item.ParentLocation ?? 0, (int?)item.Placement ?? 0));

                EnqueueBroadcast(new GameMessageObjDescEvent(this));

                /*if (((EquipMask)wieldedLocation & EquipMask.Selectable) == 0)
                {
                    EnqueueBroadcast(msgWieldItem, updateContainer, updateWielder, updateWieldLoc, new GameMessageObjDescEvent(this));
                    return true;
                }

                SetChild(item, wieldedLocation, out var placementId, out var childLocation);

                // TODO: wait for HandleQueueStance() here?
                EnqueueBroadcast(
                    new GameMessageParentEvent(this, item, childLocation, placementId),
                    msgWieldItem, updateContainer, updateWielder, updateWieldLoc);

                if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                    return true;

                switch ((EquipMask)wieldedLocation)
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
                }*/
            }

            return true;
        }

        public enum DequipObjectAction
        {
            None,

            DequipToPack,
            DequipToOffPlayerContainer,

            DropItem,
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest.<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObjectWithNetworking(ObjectGuid objectGuid, out WorldObject item, DequipObjectAction dequipObjectAction)
        {
            if (!TryDequipObjectWithBroadcasting(objectGuid, out item, (dequipObjectAction == DequipObjectAction.DropItem)))
                return false;

            // If item has any spells, remove them from the registry on unequip
            if (item.Biota.BiotaPropertiesSpellBook != null)
            {
                for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                    DispelItemSpell(item.Guid, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell);
            }

            Session.Network.EnqueueSend(new GameMessagePickupEvent(item));

            if (dequipObjectAction == DequipObjectAction.DropItem)
            {
                EnqueueBroadcast(
                    new GameMessageObjDescEvent(this),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid),
                    new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0));

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                // We must update the database with the latest ContainerId and WielderId properties.
                // If we don't, the player can drop the item, log out, and log back in. If the landblock hasn't queued a database save in that time,
                // the player will end up loading with this object in their inventory even though the landblock is the true owner. This is because
                // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
                item.SaveBiotaToDatabase();
            }
            else
            {
                Session.Network.EnqueueSend(
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid),
                    new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0));
            }

            return true;
        }


        // =====================================
        // Helper Functions - Inventory Movement
        // =====================================

        [Flags]
        private enum SearchLocations
        {
            None                = 0x00,
            MyInventory         = 0x01,
            MyEquippedItems     = 0x02,
            Landblock           = 0x04,
            LastUsedContainer   = 0x08,
            Everywhere          = 0xFF
        }

        private WorldObject FindObject(ObjectGuid objectGuid, SearchLocations searchLocations, out Container foundInContainer, out Container rootOwner, out bool wasEquipped)
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
                if (CurrentLandblock?.GetObject(lastUsedContainerId) is Container lastUsedContainer)
                {
                    result = lastUsedContainer.GetInventoryItem(objectGuid, out foundInContainer);

                    if (result != null)
                    {
                        rootOwner = lastUsedContainer;
                        return result;
                    }
                }
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
        private void AddPickupChainToMoveToChain(ActionChain moveToChain, int existingMoveToChainNumber)
        {
            moveToChain.AddAction(this, () =>
            {
                if (existingMoveToChainNumber != moveToChainCounter)
                    return;

                // start picking up item animation
                var motion = new Motion(CurrentMotionState.Stance, MotionPickup);
                EnqueueBroadcast(
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(this, motion));
            });

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionPickup, MotionCommand.Ready);
            moveToChain.AddDelaySeconds(pickupAnimationLength);
        }

        private void AdjustStack(WorldObject stack, int amount, Container container, Container rootContainer)
        {
            stack.StackSize += amount;
            stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
            stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

            if (container != null)
            {
                container.EncumbranceVal -= (stack.StackUnitEncumbrance * amount);
                container.Value -= (stack.StackUnitValue * amount);

                if (rootContainer != container)
                {
                    rootContainer.EncumbranceVal -= (stack.StackUnitEncumbrance * amount);
                    rootContainer.Value -= (stack.StackUnitValue * amount);
                }
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
        public void HandleActionPutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, int placement = 0)
        {
            var item = FindObject(itemGuid, SearchLocations.Everywhere, out var itemFoundInContainer, out var itemRootOwner, out var itemWasEquipped);
            var container = FindObject(containerGuid, SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out _, out var containerRootOwner, out _) as Container;

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (!item.Guid.IsDynamic() || item is Creature)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to move item 0x{2:X8}:{3}.", Guid.Full, Name, item.Guid.Full, item.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You can't move that!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (container == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target container not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (containerRootOwner != this) // Is our target on the landscape?
            {
                if (itemRootOwner == this && (item.Attuned ?? 0) == 1)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                    return;
                }

                if (containerRootOwner != null && !containerRootOwner.IsOpen)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.TheContainerIsClosed));
                    return;
                }
            }

            if (containerRootOwner is Corpse corpse)
            {
                if (corpse.IsMonster == false)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.Dead));
                    return;
                }
            }

            if (itemRootOwner != this || containerRootOwner != this) // Either the source or destination is not possessed by the player
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
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.Stuck));
                        return;
                    }
                }

                var actionChain = StartPickupChain();

                actionChain.AddAction(this, () =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to pick up the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                        return;
                    }

                    var questSolve = false;

                    if (itemRootOwner != this && containerRootOwner == this && item.Quest != null) // We're picking up a quest item
                    {
                        if (!QuestManager.CanSolve(item.Quest))
                        {
                            QuestManager.HandleSolveError(item.Quest);
                            return;
                        }

                        questSolve = true;
                    }

                    if (item.CurrentLandblock != null) // Movement is an item pickup off the landblock
                    {
                        item.CurrentLandblock.RemoveWorldObject(item.Guid, false, true);
                        item.Location = null;
                    }
                    else if (itemWasEquipped) // Movement is an equipped item to a container on the landblock
                    {
                        if (!TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.DequipToOffPlayerContainer))
                        {
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryDequipObjectWithNetworking failed!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                            return;
                        }
                    }
                    else if (itemRootOwner == this) // Movement is an inventory item to a container on the landblock
                    {
                        if (!TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.PlaceItemInLandblockContainer))
                        {
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to remove item from inventory!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                            return;
                        }
                    }
                    else // Movement is within the same pack or between packs in a container on the landblock
                    {
                        if (!itemFoundInContainer.TryRemoveFromInventory(item.Guid))
                        {
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                        }
                    }

                    if (container.TryAddToInventory(item, placement, true))
                    {
                        Session.Network.EnqueueSend(
                            new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid),
                            new GameEventItemServerSaysContainId(Session, item, container));

                        if (itemRootOwner != this && containerRootOwner == this) // We've picked up an item
                        { 
                            if (itemAsContainer != null) // We're picking up a pack
                            {
                                Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

                                foreach (var packItem in itemAsContainer.Inventory)
                                    Session.Network.EnqueueSend(new GameMessageCreateObject(packItem.Value));
                            }

                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (item.WeenieType == WeenieType.Coin)
                                UpdateCoinValue();

                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));

                            item.NotifyOfEvent(RegenerationType.PickUp);

                            if (questSolve)
                                QuestManager.Update(item.Quest);
                        }
                        else if (itemRootOwner == this && containerRootOwner != this) // We've dropped up an item
                        {
                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (item.WeenieType == WeenieType.Coin)
                                UpdateCoinValue();

                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                        }
                    }
                    else
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "AddToInventory failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));

                        // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);
                    EnqueueBroadcastMotion(returnStance);
                });

                actionChain.EnqueueChain();
            }
            else // This is a self-contained movement
            {
                if (itemWasEquipped) // Movement is an equipped item to a local pack
                {
                    if (!TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.DequipToPack))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryDequipObjectWithNetworking failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                        return;
                    }
                }
                else // Movement is within the same pack or between packs
                {
                    if (!itemFoundInContainer.TryRemoveFromInventory(item.Guid))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                        return;
                    }
                }

                if (!container.TryAddToInventory(item, placement))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryAddToInventory failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));

                    // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.

                    return;
                }

                // If we've moved the item from the main pack to a side pack, we must increment our main EncumbranceValue and Value
                if (itemFoundInContainer == this && container != this)
                {
                    EncumbranceVal += item.EncumbranceVal;
                    Value += item.Value;
                }

                Session.Network.EnqueueSend(
                    new GameEventItemServerSaysContainId(Session, item, container),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid));
            }
        }

        /// <summary>
        /// This method processes the Game Action (F7B1) Drop Item (0x001B)
        /// This is raised when we:
        /// - drop an equipped item
        /// - drop an item from inventory
        /// </summary>
        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out _, out _, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if ((item.Attuned ?? 0) == 1)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                return;
            }

            if (wasEquipped)
            {
                if (!TryDequipObjectWithNetworking(itemGuid, out item, DequipObjectAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to dequip item!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                    return;
                }
            }
            else
            {
                if (!TryRemoveFromInventoryWithNetworking(itemGuid, out item, RemoveFromInventoryAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to remove item from inventory!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                    return;
                }
            }

            var actionChain = StartPickupChain();

            actionChain.AddAction(this, () =>
            {
                if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to drop the item
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                    return;
                }

                item.Location = new Position(Location.InFrontOf(1.1f));
                item.Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.

                CurrentLandblock.AddWorldObject(item);

                Session.Network.EnqueueSend(
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                    new GameEventItemServerSaysMoveItem(Session, item),
                    new GameMessageUpdatePosition(item));

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));

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
        public void HandleActionGetAndWieldItem(uint itemGuid, int wieldLocation)
        {
            var item = FindObject(new ObjectGuid(itemGuid), SearchLocations.Everywhere, out var foundInContainer, out var rootOwner, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************
            // todo ***********************************************************************************************************

            WeenieError wieldError;

            if (rootOwner != this) // Item is on the landscape, or in a landblock chest
            {
                var moveToChain = CreateMoveToChain(item, out var thisMoveToChainNumber);

                AddPickupChainToMoveToChain(moveToChain, thisMoveToChainNumber);

                moveToChain.AddAction(this, () =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to pick up the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                        return;
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);

                    if (thisMoveToChainNumber != moveToChainCounter)
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Action GetAndWieldItem cancelled!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));

                        EnqueueBroadcastMotion(returnStance);

                        return;
                    }

                    wieldError = CheckWieldRequirement(item);

                    if (wieldError != WeenieError.None)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, wieldError));

                        EnqueueBroadcastMotion(returnStance);

                        return;
                    }

                    // todo dequip item if needed

                    if (item.CurrentLandblock != null) // Item is resting on the landblock
                    {
                        item.CurrentLandblock.RemoveWorldObject(item.Guid, false, true);
                        item.Location = null;
                    }
                    else // Item is in a landblock container
                    {
                        if (!foundInContainer.TryRemoveFromInventory(item.Guid))
                        {
                            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                            return;
                        }
                    }

                    // todo
                    /*Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));

                    item.NotifyOfEvent(RegenerationType.PickUp);*/

                    EnqueueBroadcastMotion(returnStance);
                });

                moveToChain.EnqueueChain();

                return;
            }

            wieldError = CheckWieldRequirement(item);

            if (wieldError != WeenieError.None)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, wieldError));
                return;
            }

            // todo dequip item if needed

            if (wasEquipped) // We're moving a wielded item to another wield location
            {
                // TryRemoveFromInventoryWithNetworking
                // TryDequipObjectWithNetworking
                // TryEquipObjectWithNetworking
            }
            else // Item is in our inventory
            {
                if (!TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.ToWieldedSlot))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventoryWithNetworking failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));

                    return;
                }
                // TryRemoveFromInventoryWithNetworking
                // TryEquipObjectWithNetworking
            }

            if (!TryEquipObjectWithNetworking(item, wieldLocation))
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryEquipObjectWithNetworking failed!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));

                // todo: So the item isn't lost, we should try to put the item in the players inventory, or if that's full, on the landblock.

                return;
            }

            // WERROR WITH STRING ACTIVATION SKILL TOO LOW

            /*var itemGuid = new ObjectGuid(itemId);

            // handle inventory item -> weapon/shield slot
            var item = GetInventoryItem(itemGuid);
            if (item != null)
            {
                //Console.WriteLine($"HandleActionGetAndWieldItem({item.Name})");

                var result = TryWieldItem(item, wieldLocation);
                return;
            }

            // handle 1 wielded slot -> the other wielded slot
            // (weapon swap)
            var wieldedItem = GetEquippedItem(itemGuid);
            if (wieldedItem != null)
            {
                var result = TryWieldItem(wieldedItem, wieldLocation);
                return;
            }

            // We don't have possession of the item so we must pick it up.
            // should this be wielding the item afterwards?
            PickupItemWithNetworking(this, itemGuid, wieldLocation, PropertyInstanceId.Wielder);*/
        }

        private WeenieError CheckWieldRequirement(WorldObject item)
        {
            var itemWieldReq = (WieldRequirement)(item.GetProperty(PropertyInt.WieldRequirements) ?? 0);

            switch (itemWieldReq)
            {
                case WieldRequirement.RawSkill:
                    // Check WieldDifficulty property against player's Skill level, defined by item's WieldSkilltype property
                    var itemSkillReq = ConvertToMoASkill((Skill)(item.GetProperty(PropertyInt.WieldSkilltype) ?? 0));

                    if (itemSkillReq != Skill.None)
                    {
                        var playerSkill = GetCreatureSkill(itemSkillReq).Current;

                        var skillDifficulty = (uint)(item.GetProperty(PropertyInt.WieldDifficulty) ?? 0);

                        if (playerSkill < skillDifficulty)
                            return WeenieError.SkillTooLow;
                    }
                    break;

                case WieldRequirement.Level:
                    // Check WieldDifficulty property against player's level
                    if (Level < (uint)(item.GetProperty(PropertyInt.WieldDifficulty) ?? 0))
                        return WeenieError.LevelTooLow;
                    break;

                case WieldRequirement.Attrib:
                    // Check WieldDifficulty property against player's Attribute, defined by item's WieldSkilltype property
                    var itemAttributeReq = (PropertyAttribute)(item.GetProperty(PropertyInt.WieldSkilltype) ?? 0);

                    if (itemAttributeReq != PropertyAttribute.Undef)
                    {
                        var playerAttribute = Attributes[itemAttributeReq].Current;

                        if (playerAttribute < (uint)(item.GetProperty(PropertyInt.WieldDifficulty) ?? 0))
                            return WeenieError.SkillTooLow;
                    }
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
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            var stack = FindObject(new ObjectGuid(stackId), SearchLocations.Everywhere, out var stackFoundInContainer, out var stackRootOwner, out _);
            var container = FindObject(new ObjectGuid(containerId), SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out _, out var containerRootOwner, out _) as Container;

            if (stack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (container == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target container not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (stack.StackSize == null || stack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split invalid item 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (stack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
            newStack.StackSize = amount;
            newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
            newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

            if (stackRootOwner != this || containerRootOwner != this) // Either the source or destination is not possessed by the player
            {
                var actionChain = StartPickupChain();

                actionChain.AddAction(this, () =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to slit the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                        return;
                    }

                    // Before we modify the original stack, we make sure we can add the new stack
                    if (!container.TryAddToInventory(newStack, placementPosition, true))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryAddToInventory failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                        return;
                    }

                    if (container != containerRootOwner && containerRootOwner != null)
                    {
                        containerRootOwner.EncumbranceVal += (stack.StackUnitEncumbrance * amount);
                        containerRootOwner.Value += (stack.StackUnitValue * amount);
                    }

                    Session.Network.EnqueueSend(new GameMessageCreateObject(newStack));
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, newStack, container));

                    AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                    if (stackRootOwner == this || containerRootOwner == this)
                    {
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                        if (stack.WeenieType == WeenieType.Coin)
                            UpdateCoinValue();

                        if (stackRootOwner == this)
                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                        else if (containerRootOwner == this)
                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);
                    EnqueueBroadcastMotion(returnStance);
                });

                actionChain.EnqueueChain();
            }
            else // This is a self-contained movement
            {
                // Before we modify the original stack, we make sure we can add the new stack
                if (!container.TryAddToInventory(newStack, placementPosition, true))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryAddToInventory failed!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                    return;
                }

                if (container != containerRootOwner)
                {
                    containerRootOwner.EncumbranceVal += (stack.StackUnitEncumbrance * amount);
                    containerRootOwner.Value += (stack.StackUnitValue * amount);
                }

                Session.Network.EnqueueSend(new GameMessageCreateObject(newStack));
                Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, newStack, container));

                AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));
            }
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
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            var stack = FindObject(new ObjectGuid(stackId), SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out var stackFoundInContainer, out var stackRootOwner, out _);

            if (stack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (stack.StackSize == null || stack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split invalid item 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (stack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to split item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, stack.Guid.Full, stack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Split amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            var actionChain = StartPickupChain();

            actionChain.AddAction(this, () =>
            {
                if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to drop the item
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                    return;
                }

                AdjustStack(stack, -amount, stackFoundInContainer, stackRootOwner);
                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
                newStack.StackSize = (ushort)amount;
                newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
                newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                newStack.Location = new Position(Location.InFrontOf(1.1f));
                newStack.Placement = ACE.Entity.Enum.Placement.Resting; // This is needed to make items lay flat on the ground.

                CurrentLandblock.AddWorldObject(newStack);

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));

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
        /// - try to merge a stack fron the landblock into a container
        /// - try to split a stack into a different container that has a stack that can support a merge
        /// </summary>
        public void HandleActionStackableMerge(ObjectGuid mergeFromGuid, ObjectGuid mergeToGuid, int amount)
        {
            if (amount == 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            var sourceStack = FindObject(mergeFromGuid, SearchLocations.Everywhere, out var sourceStackFoundInContainer, out var sourceStackRootOwner, out _);
            var targetStack = FindObject(mergeToGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.LastUsedContainer, out var targetStackFoundInContainer, out var targetStackRootOwner, out _);

            if (sourceStack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (targetStack == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target stack not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (sourceStack.StackSize == null || sourceStack.StackSize == 0)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to merge invalid source item 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Stack not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (targetStack.StackSize == null || targetStack.StackSize == 0 || targetStack.StackSize == targetStack.MaxStackSize)
            {
                log.WarnFormat("Player 0x{0:X8}:{1} tried to merge invalid target item 0x{2:X8}:{3}.", Guid.Full, Name, targetStack.Guid.Full, targetStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (sourceStack.StackSize < amount)
            {
                log.WarnFormat("Player 0x{0}:{1} tried to merge item with invalid amount 0x{2:X8}:{3}.", Guid.Full, Name, sourceStack.Guid.Full, sourceStack.Name);
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Merge amount not valid!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                return;
            }

            if (sourceStackRootOwner != this || targetStackRootOwner != this) // Either the source or destination is not possessed by the player
            {
                var actionChain = StartPickupChain();

                actionChain.AddAction(this, () =>
                {
                    if (CurrentLandblock == null) // Maybe we were teleported as we were motioning to slit the item
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ActionCancelled));
                        return;
                    }

                    if (amount == sourceStack.StackSize && sourceStack.StackSize + targetStack.StackSize <= targetStack.MaxStackSize) // The merge will consume the entire source stack
                    {
                        if (sourceStackRootOwner == this)
                        {
                            if (!TryConsumeFromInventoryWithNetworking(sourceStack, amount))
                            {
                                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryConsumeFromInventoryWithNetworking failed!")); // Custom error message
                                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                                return;
                            }
                        }
                        else
                        {
                            if (!sourceStackRootOwner.TryRemoveFromInventory(sourceStack.Guid, out _))
                            {
                                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryRemoveFromInventory failed!")); // Custom error message
                                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                                return;

                            }

                            Session.Network.EnqueueSend(new GameMessageDeleteObject(sourceStack));

                            sourceStack.Destroy();
                        }

                        AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
                    }
                    else // The merge will reduce the size of the source stack
                    {
                        AdjustStack(sourceStack, -amount, sourceStackFoundInContainer, sourceStackRootOwner);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(sourceStack));

                        AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
                    }

                    if (sourceStackRootOwner == this || targetStackRootOwner == this)
                    {
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                        if (sourceStack.WeenieType == WeenieType.Coin)
                            UpdateCoinValue();

                        if (sourceStackRootOwner == this)
                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));
                        else if (targetStackRootOwner == this)
                            EnqueueBroadcast(new GameMessageSound(Guid, Sound.PickUpItem));
                    }

                    var returnStance = new Motion(CurrentMotionState.Stance);
                    EnqueueBroadcastMotion(returnStance);
                });

                actionChain.EnqueueChain();
            }
            else // This is a self-contained movement
            {
                if (amount == sourceStack.StackSize && sourceStack.StackSize + targetStack.StackSize <= targetStack.MaxStackSize) // The merge will consume the entire source stack
                {
                    if (!TryConsumeFromInventoryWithNetworking(sourceStack, amount))
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "TryConsumeFromInventoryWithNetworking failed!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session));
                        return;
                    }

                    AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
                }
                else // The merge will reduce the size of the source stack
                {
                    AdjustStack(sourceStack, -amount, sourceStackFoundInContainer, sourceStackRootOwner);
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(sourceStack));

                    AdjustStack(targetStack, amount, targetStackFoundInContainer, targetStackRootOwner);
                    Session.Network.EnqueueSend(new GameMessageSetStackSize(targetStack));
                }
            }
        }


        // =============================================
        // Game Action Handlers - Inventory Give/Receive 
        // =============================================

        /// <summary>
        /// Called when player attempts to give an object to someone else,
        /// ie. to another player, or NPC
        /// </summary>
        public void HandleActionGiveObjectRequest(ObjectGuid targetID, ObjectGuid itemGuid, uint amount)
        {
            var target = CurrentLandblock?.GetObject(targetID);
            var item = GetInventoryItem(itemGuid) ?? GetEquippedItem(itemGuid);
            if (target == null || item == null) return;

            // giver rotates to receiver
            var rotateDelay = Rotate(target);

            var giveChain = new ActionChain();
            giveChain.AddChain(CreateMoveToChain(target, out var thisMoveToChainNumber));

            if (target is Player)
                giveChain.AddAction(this, () => GiveObjecttoPlayer(target as Player, item, (ushort)amount));
            else
            {
                var receiveChain = new ActionChain();
                giveChain.AddAction(this, () =>
                {
                    GiveObjecttoNPC(target, item, amount, giveChain, receiveChain);
                    giveChain.AddChain(receiveChain);
                });
            }
            giveChain.EnqueueChain();
        }

        /// <summary>
        /// This code handle objects between players and other players
        /// </summary>
        private void GiveObjecttoPlayer(Player target, WorldObject item, ushort amount)
        {
            Player player = this;

            if ((item.Attuned ?? 0) == 1)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem)); // Second message appears in PCAPs

                return;
            }

            if ((Character.CharacterOptions1 & (int)CharacterOptions1.LetOtherPlayersGiveYouItems) == (int)CharacterOptions1.LetOtherPlayersGiveYouItems)
            {
                if (target != player)
                {
                    // todo This should be refactored
                    // The order should be something like:
                    // See if target can accept the item
                    // Remove item from giver
                    // Save item to db
                    // Give item to receiver
                    if (target.HandlePlayerReceiveItem(item, player))
                    {
                        // TODO FIX
                        //if (item.CurrentWieldedLocation != null)
                        //    UnwieldItemWithNetworking(this, item, 0);       // refactor, duplicate code from above

                        if (amount >= (item.StackSize ?? 1))
                        {
                            if (TryRemoveFromInventory(item.Guid)) // todo this had withclear
                            {
                                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                                if (item.WeenieType == WeenieType.Coin)
                                    UpdateCoinValue();
                            }
                        }
                        else
                        {
                            item.StackSize -= amount;

                            Session.Network.EnqueueSend(new GameMessageSetStackSize(item));

                            EncumbranceVal = (EncumbranceVal - (item.StackUnitEncumbrance * amount));
                            Value = (Value - (item.StackUnitValue * amount));

                            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                            if (item.WeenieType == WeenieType.Coin)
                                UpdateCoinValue();
                        }

                        // We must update the database with the latest ContainerId and WielderId properties.
                        // If we don't, the player can give the item, log out, and log back in. If the receiver hasn't queued a database save in that time,
                        // the player will end up loading with this object in their inventory even though the receiver is the true owner. This is because
                        // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
                        item.SaveBiotaToDatabase();

                        Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, target));
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"You give {target.Name} {item.Name}.", ChatMessageType.Broadcast));
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem, 1));
                    }
                }
            }
            else
            {
                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsNotAcceptingGiftsRightNow, target.Name));
                Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
            }
        }

        /// <summary>
        /// This code handles objects between players and other world objects
        /// </summary>
        private void GiveObjecttoNPC(WorldObject target, WorldObject item, uint amount, ActionChain giveChain, ActionChain receiveChain)
        {
            if (target == null || item == null) return;

            if (target.EmoteManager.IsBusy)
            {
                giveChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsTooBusyToAcceptGifts, target.Name));
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
                });
            }
            else if (target.GetProperty(PropertyBool.AiAcceptEverything) ?? false)
            {
                // NPC accepts any item
                giveChain.AddAction(this, () => ItemAccepted(item, amount, target));
            }
            else if (!target.GetProperty(PropertyBool.AllowGive) ?? false)
            {
                giveChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString._IsNotAcceptingGiftsRightNow, target.Name));
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
                });
            }
            else if (((item.GetProperty(PropertyInt.Attuned) ?? 0) == 1) && ((target as Player) != null))
            {
                giveChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem)); // Second message appears in PCAPs
                });
            }
            else
            {
                var result = target.Biota.BiotaPropertiesEmote.Where(emote => emote.WeenieClassId == item.WeenieClassId);
                WorldObject player = this;
                if (target.HandleNPCReceiveItem(item, player, receiveChain))
                {
                    if (result.ElementAt(0).Category == (uint)EmoteCategory.Give)
                    {
                        // Item accepted by collector/NPC
                        giveChain.AddAction(this, () => ItemAccepted(item, amount, target));
                    }
                    else if (result.ElementAt(0).Category == (uint)EmoteCategory.Refuse)
                    {
                        // Item rejected by npc
                        giveChain.AddAction(this, () =>
                        {
                            Session.Network.EnqueueSend(new GameMessageSystemChat($"You allow {target.Name} to examine your {item.Name}.", ChatMessageType.Broadcast));
                            Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.TradeAiRefuseEmote));
                        });
                    }
                }
                else
                {
                    giveChain.AddAction(this, () =>
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.TradeAiDoesntWant));
                        Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, (WeenieErrorWithString)WeenieError.TradeAiDoesntWant, target.Name));
                    });
                }
            }
        }

        /// <summary>
        /// Giver methods used upon successful acceptance of item by NPC<para />
        /// The item will be destroyed after processing.
        /// </summary>
        private void ItemAccepted(WorldObject item, uint amount, WorldObject target)
        {
            // TODO FIX
            //if (item.CurrentWieldedLocation != null)
            //    UnwieldItemWithNetworking(this, item, 0);       // refactor, duplicate code from above

            TryRemoveItemFromInventoryWithNetworkingWithDestroy(item, (ushort)amount);

            Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, target));
            Session.Network.EnqueueSend(new GameMessageSystemChat($"You give {target.Name} {item.Name}.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem, 1));

            Session.Network.EnqueueSend(new GameEventInventoryRemoveObject(Session, item));

            item.Destroy();
        }

        /// <summary>
        /// This code handles receiving objects from other players, ie. attempting to place the item in the target's inventory
        /// </summary>
        private bool HandlePlayerReceiveItem(WorldObject item, Player player)
        {
            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} gives you {item.Name}.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem));

            TryCreateInInventoryWithNetworking(item);

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
        public void HandleActionSetInscription(ObjectGuid itemGuid, string inscriptionText)
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













        /// <summary>
        /// If isFromMergeEvent is false, update messages will be sent for EncumbranceVal and if WeenieType is Coin, CoinValue will be updated and update messages will be sent for CoinValue.
        /// </summary>
        [Obsolete]
        public bool TryRemoveFromInventoryWithNetworking(ObjectGuid objectGuid, out WorldObject item, bool isFromMergeEvent = false)
        {
            if (!TryRemoveFromInventory(objectGuid, out item))
                return false;

            Session.Network.EnqueueSend(new GameEventInventoryRemoveObject(Session, item));

            if (!isFromMergeEvent)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();
            }

            return true;
        }

        /// <summary>
        /// This method is used to remove X number of items from a stack.<para />
        /// If amount to remove is greater or equal to the current stacksize, the stack will be destroyed..
        /// </summary>
        [Obsolete]
        public bool TryRemoveItemFromInventoryWithNetworkingWithDestroy(WorldObject item, ushort amount)
        {
            if (amount >= (item.StackSize ?? 1))
            {
                if (TryRemoveFromInventoryWithNetworking(item.Guid, out _))
                {
                    item.Destroy();
                    return true;
                }

                return false;
            }

            item.StackSize -= amount;

            Session.Network.EnqueueSend(new GameMessageSetStackSize(item));

            EncumbranceVal = (EncumbranceVal - (item.StackUnitEncumbrance * amount));
            Value = (Value - (item.StackUnitValue * amount));

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        // TODO this function doesn't really fit the model. It needs to be re-evaluated 2018-12-16 Mag-nus
        /// <summary>
        /// Removes an item from either the player's inventory, or equipped items, and sends network messages
        /// </summary>
        [Obsolete]
        public bool TryRemoveItemWithNetworking(WorldObject item)
        {
            if (item.CurrentWieldedLocation != null)
            {
                if (!UnwieldItemWithNetworking(this, item))
                {
                    log.Warn($"Player_Inventory.TryRemoveItemWithNetworking: couldn't unwield item from {Name} ({item.Name})");
                    return false;
                }
            }

            return TryRemoveFromInventoryWithNetworking(item.Guid, out _);
        }

        /// <summary>
        /// This method is called in response to a put item in container message. It is used when the item going into a container was wielded.
        /// It sets the appropriate properties, sends out response messages  and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        [Obsolete]
        private bool UnwieldItemWithNetworking(Container container, WorldObject item, int placement = 0)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            // If item has any spells, remove them from the registry on unequip
            if (item.Biota.BiotaPropertiesSpellBook != null)
            {
                for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                    DispelItemSpell(item.Guid, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell);
            }

            if (!TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.DequipToPack))
            {
                log.Error("Player_Inventory UnwieldItemWithNetworking TryDequipObject failed");
                return false;
            }

            item.SetPropertiesForContainer();

            if (!container.TryAddToInventory(item, placement))
            {
                log.Error("Player_Inventory UnwieldItemWithNetworking TryAddToInventory failed");
                return false;
            }

            // If we've unwielded the item to a side pack, we must increment our main EncumbranceValue and Value
            if (container != this && container.ContainerId == Guid.Full)
            {
                EncumbranceVal += item.EncumbranceVal;
                Value += item.Value;
            }
            // todo I think we need to recalc our SetupModel here. see CalculateObjDesc()

            EnqueueBroadcast(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0),
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid),
                new GameMessagePickupEvent(item),
                new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0),
                new GameEventItemServerSaysContainId(Session, item, container),
                new GameMessageObjDescEvent(this));

            if (CombatMode == CombatMode.NonCombat || (oldLocation != EquipMask.MeleeWeapon && oldLocation != EquipMask.MissileWeapon && oldLocation != EquipMask.Held && oldLocation != EquipMask.Shield))
                return true;

            SetCombatMode(CombatMode.Melee);
            return true;
        }
    }
}
