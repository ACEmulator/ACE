using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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
using ACE.Server.Network.Sequence;

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
        public bool TryCreateInInventoryWithNetworking(WorldObject item, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            return TryCreateInInventoryWithNetworking(item, out _, placementPosition, limitToMainPackOnly);
        }

        /// <summary>
        /// If enough burden is available, this will try to add (via create) an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryCreateInInventoryWithNetworking(WorldObject item, out Container container, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            if (!TryAddToInventory(item, out container, placementPosition, limitToMainPackOnly)) // We don't have enough burden available or no empty pack slot.
                return false;

            Session.Network.EnqueueSend(new GameMessageCreateObject(item));

            if (item is Container lootAsContainer)
                Session.Network.EnqueueSend(new GameEventViewContents(Session, lootAsContainer));

            Session.Network.EnqueueSend(
                new GameEventItemServerSaysContainId(Session, item, container),
                new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (item.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        public enum RemoveFromInventoryAction
        {
            None,
            WieldedToPack,
            DropItem
        }

        public bool TryRemoveFromInventoryWithNetworking(ObjectGuid objectGuid, out WorldObject item, RemoveFromInventoryAction removeFromInventoryAction)
        {
            if (!TryRemoveFromInventory(objectGuid, out item))
                return false;

            if (removeFromInventoryAction == RemoveFromInventoryAction.DropItem)
            {
                item.ContainerId = null;

                Session.Network.EnqueueSend(
                    new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)));

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


        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryEquipObjectWithNetworking(WorldObject item, int wieldedLocation)
        {
            if (!TryEquipObjectWithBroadcasting(item, wieldedLocation))
                return false;

            // todo add the network messages here

            return true;
        }

        /// <summary>
        /// This will remove the Wielder and CurrentWieldedLocation properties on the item and will remove it from the EquippedObjects dictionary.<para />
        /// It does not add it to inventory as you could be unwielding to the ground or a chest. Og II<para />
        /// It will also decrease the EncumbranceVal and Value.
        /// </summary>
        public bool TryDequipObjectWithNetworking(ObjectGuid objectGuid, out WorldObject item, RemoveFromInventoryAction removeFromInventoryAction)
        {
            if (!TryDequipObjectWithBroadcasting(objectGuid, out item, (removeFromInventoryAction == RemoveFromInventoryAction.DropItem)))
                return false;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (removeFromInventoryAction == RemoveFromInventoryAction.DropItem)
            {
                EnqueueBroadcast(
                    new GameMessageObjDescEvent(this),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid),
                    new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0));

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
            var motion = new Motion(CurrentMotionState.Stance, MotionCommand.Pickup);
            EnqueueBroadcast(
                new GameMessageUpdatePosition(this),
                new GameMessageUpdateMotion(this, motion));

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionCommand.Pickup, MotionCommand.Ready);
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
                var motion = new Motion(CurrentMotionState.Stance, MotionCommand.Pickup);
                EnqueueBroadcast(
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(this, motion));
            });

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionCommand.Pickup, MotionCommand.Ready);
            moveToChain.AddDelaySeconds(pickupAnimationLength);
        }


        // =========================================
        // Game Action Handlers - Inventory Movement 
        // =========================================

        /// <summary>
        /// This is raised when we:
        /// - move an item around in our inventory.
        /// - dequip an item.
        /// - Pickup an item off of the landblock or a container on the landblock
        /// </summary>
        public void HandleActionPutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, int placement = 0)
        {
            /* PCapped Examples
             *
             * Moving an item ? to same or different container?
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * new GameEventItemServerSaysContainId(Session, item, container)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid)
             *
             * Picking up an item
             * [Update Position if applicable]
             * MotionCommand.Pickup
             * new GameMessageSound(Guid, Sound.PickUpItem)
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * new GameEventItemServerSaysContainId(Session, item, container)
             * return movement
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid)
             *
             * Picking up a container
             * [Update Position if applicable]
             * MotionCommand.Pickup
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * new GameEventViewContents(player.Session, container)
             * new GameEventItemServerSaysContainId(Session, item, container)

             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid)
             *
             * Dequipping an item
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * new GameEventItemServerSaysContainId(Session, item, container)
             * new GameMessageSound(Guid, Sound.UnwieldObject)
             * EnqueueBroadcast(new GameMessagePickupEvent(worldObject))
             * new GameMessagePublicUpdateInstanceID(worldObject, PropertyInstanceId.Wielder, ObjectGuid.Invalid)
             * new GameMessagePublicUpdatePropertyInt(worldObject, PropertyInt.CurrentWieldedLocation, 0)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid)
             * EnqueueBroadcast(new GameMessagePickupEvent(worldObject))
             */

            var item = FindObject(itemGuid, SearchLocations.Everywhere, out var itemFoundInContainer, out var itemRootOwner, out var itemWasEquipped);
            var container = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out var containerFoundInContainer, out var containerRootOwner, out _);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Source item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                return;
            }

            if (container == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Target container not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
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
                // Checking to see if item to pick is an container itself and IsOpen
                if (item.WeenieType == WeenieType.Container && item.IsOpen && item is Container itemAsContainer)
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
                    // todo

                    var returnStance = new Motion(CurrentMotionState.Stance);
                    EnqueueBroadcastMotion(returnStance);

                    // This is a pickup or drop
                    //PickupItemWithNetworking(container, itemGuid, placement, PropertyInstanceId.Container);

                    // todo
                });

                actionChain.EnqueueChain();
            }
            else // This is a self-contained movement
            {
                if (itemWasEquipped) // Movement is an equipped item to a local pack
                {
                    /*// Was I equiped? If so, lets take care of that and unequip
                    if (item.WielderId != null)
                    {
                        UnwieldItemWithNetworking(container, item, placement);
                        item.IsAffecting = false;
                        return;
                    }*/
                }
                else if (itemFoundInContainer == container) // Move  is within the same pack
                {
                    // if were are still here, this needs to do a pack pack or main pack move.
                    // MoveItemWithNetworking(item, container, placement);
                }
                else // Move is between packs
                {
                    // if were are still here, this needs to do a pack pack or main pack move.
                    // MoveItemWithNetworking(item, container, placement);
                }
            }
        }

        /// <summary>
        /// This is raised when we:
        /// - drop an item from inventory
        /// - drop an equipped item
        /// </summary>
        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            /* PCapped Examples
             *
             * Dropping an equipped item
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * new GameMessageObjDescEvent(this)
             * new GameMessageSound(Guid, Sound.UnwieldObject)
             * MotionCommand.Pickup
             * new GameMessagePublicUpdateInstanceID(worldObject, PropertyInstanceId.Wielder, ObjectGuid.Invalid)
             * new GameMessagePublicUpdatePropertyInt(worldObject, PropertyInt.CurrentWieldedLocation, 0)
             * - wait for animation -
             * new GameEventItemServerSaysMoveItem(Session, item)
             * return movement
             * new GameMessageSound(Guid, Sound.DropItem)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid)
             * new GameMessageUpdatePosition(item)
             *
             * Dropping an item from inventory
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * MotionCommand.Pickup
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid)
             * - wait for animation -
             * new GameEventItemServerSaysMoveItem(Session, item)
             * return movement
             * new GameMessageSound(Guid, Sound.DropItem)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, ObjectGuid.Invalid)
             * new GameMessageUpdatePosition(item)
             */

            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out _, out _, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                return;
            }

            if ((item.Attuned ?? 0) == 1)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                return;
            }

            if (!wasEquipped)
            {
                if (!TryRemoveFromInventoryWithNetworking(itemGuid, out item, RemoveFromInventoryAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to remove item from inventory!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                    return;
                }
            }
            else
            {
                if (!TryDequipObjectWithNetworking(itemGuid, out item, RemoveFromInventoryAction.DropItem))
                {
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Failed to dequip item!")); // Custom error message
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                    return;
                }
            }

            var actionChain = StartPickupChain();

            actionChain.AddAction(this, () =>
            {
                // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                item.SetPropertiesForWorld(this, 1.1f);

                CurrentLandblock?.AddWorldObject(item);

                Session.Network.EnqueueSend(new GameEventItemServerSaysMoveItem(Session, item));

                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem));

                // todo: These messages should really be sent by the tracking code.
                // todo: The equipped items should already be tracked/known by other players.
                // todo: When we add an equipped item to the landblock, and then call NotifyPlayers, we should simply send these two messages
                // todo: If the item was not equipped, a CreateObject message would be sent to others, and this player would receive the Container and Position updates
                EnqueueBroadcast(
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                    new GameMessageUpdatePosition(item));
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// This is raised when we:
        /// - try to wield an item in from inventory
        /// - try to wield an item in a chest
        /// - try to wield an item on the landscape
        /// - try to transfer a wielded item to another wield location
        /// </summary>
        public void HandleActionGetAndWieldItem(uint itemGuid, int wieldLocation)
        {
            /* PCapped Examples
             *
             * Wielding an armor item from inventory
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * cast enchantments: CURRENT_ATTACKER_IID + Spells
             * new GameEventWieldItem(Session, item.Guid.Full, wieldLocation)
             * EnqueueBroadcast(new GameMessageSound(Guid, Sound.WieldObject))
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid.Invalid)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid)
             * objdesc
             * new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldLocation)
             *
             * Wielding a weapon from inventory
             * new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0)
             * cast enchantments: CURRENT_ATTACKER_IID + Spells
             * new GameEventWieldItem(Session, item.Guid.Full, wieldLocation)
             * EnqueueBroadcast(new GameMessageSound(Guid, Sound.WieldObject))
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid.Invalid)
             * new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid)
             * new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldLocation)
             * physics parent event
             */

            var item = FindObject(new ObjectGuid(itemGuid), SearchLocations.MyInventory | SearchLocations.Landblock | SearchLocations.LastUsedContainer, out var foundInContainer, out var rootOwner, out var wasEquipped);

            if (item == null)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Item not found!")); // Custom error message
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                return;
            }

            if (rootOwner == null || rootOwner != this) // Item is on the landscape, or in a landblock chest
            {
                var moveToChain = CreateMoveToChain(item, out var thisMoveToChainNumber);

                AddPickupChainToMoveToChain(moveToChain, thisMoveToChainNumber);

                moveToChain.AddAction(this, () =>
                {
                    if (thisMoveToChainNumber != moveToChainCounter)
                    {
                        Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Action GetAndWieldItem cancelled!")); // Custom error message
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                        return;
                    }

                    // todo
                });

                moveToChain.EnqueueChain();
            }
            else if (wasEquipped) // We're moving a wielded item to another wield location
            {
                // TryRemoveFromInventoryWithNetworking
                // TryDequipObjectWithNetworking
                // TryEquipObjectWithNetworking
            }
            else // Item is in our inventory
            {
                // TryRemoveFromInventoryWithNetworking
                // TryEquipObjectWithNetworking
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

        private bool UseWieldRequirement = true;

        public Skill ConvertToMoASkill(Skill skill)
        {
            if (this is Player player)
            {
                if (SkillExtensions.RetiredMelee.Contains(skill))
                    return player.GetHighestMeleeSkill();
                if (SkillExtensions.RetiredMissile.Contains(skill))
                    return Skill.MissileWeapons;
            }

            return skill;
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
                        if (item.CurrentWieldedLocation != null)
                            UnwieldItemWithNetworking(this, item, 0);       // refactor, duplicate code from above

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
            if (item.CurrentWieldedLocation != null)
                UnwieldItemWithNetworking(this, item, 0);       // refactor, duplicate code from above

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


        // =========================================
        // Game Action Handlers - Inventory Stacking 
        // =========================================

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting item. Og II
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are splitting</param>
        /// <param name="containerId">The guid of the container</param>
        /// <param name="placementPosition">Place is the slot in the container we are splitting into. Range 0-MaxCapacity</param>
        /// <param name="amount">The amount of the stack we are splitting from that we are moving to a new stack.</param>
        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, int placementPosition, ushort amount)
        {
            Container sourceContainer;
            bool sourceContainerIsOnLandblock = false;

            Container targetContainer;
            bool targetContainerIsOnLandblock = false;

            // Init our source vars
            var stack = GetInventoryItem(new ObjectGuid(stackId), out sourceContainer);

            if (stack == null)
            {
                stack = CurrentLandblock?.GetObject(new ObjectGuid(stackId));

                if (stack == null)
                {
                    sourceContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

                    if (sourceContainer != null)
                    {
                        stack = sourceContainer.GetInventoryItem(new ObjectGuid(stackId), out sourceContainer);

                        if (stack != null)
                            sourceContainerIsOnLandblock = true;
                    }
                }
            }

            if (stack == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not found");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.YouDoNotOwnThatItem));
                return;
            }

            if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not large enough for amount");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.BadParam));
                return;
            }

            // Init our target vars
            if (containerId == Guid.Full)
                targetContainer = this;
            else
                targetContainer = GetInventoryItem(new ObjectGuid(containerId)) as Container;

            if (targetContainer == null)
            {
                targetContainer = CurrentLandblock?.GetObject(containerId) as Container;

                if (targetContainer == null)
                {
                    var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

                    if (lastUsedContainer != null)
                        targetContainer = lastUsedContainer.GetInventoryItem(new ObjectGuid(containerId)) as Container;
                }

                targetContainerIsOnLandblock = (targetContainer != null);
            }

            if (targetContainer == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer container not found");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.YouDoNotOwnThatItem));
                return;
            }

            // Ok we are in business

            if (sourceContainer == null || (sourceContainerIsOnLandblock && !targetContainerIsOnLandblock))
            {
                // Pickup from 3D
                // TODO
                return;
            }

            if (!sourceContainerIsOnLandblock && targetContainerIsOnLandblock)
            {
                // Drop to 3D
                // TODO
                return;
            }

            // TODO we need animation of we're going to/from 3D

            var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
            newStack.StackSize = amount;
            newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
            newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

            // Before we modify the original stack, we make sure we can add the new stack
            if (!targetContainer.TryAddToInventory(newStack, placementPosition, true))
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer TryAddToInventory failed");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.BadParam));
                return;
            }

            stack.StackSize -= amount;
            stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
            stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

            Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

            Session.Network.EnqueueSend(new GameMessageCreateObject(newStack));
            Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, newStack, targetContainer));

            // Adjust EncumbranceVal and Value

            sourceContainer.EncumbranceVal -= newStack.EncumbranceVal;
            sourceContainer.Value -= newStack.Value;
            
            if (sourceContainer == this && sourceContainer != targetContainer && !targetContainerIsOnLandblock)
            {
                // Add back the encunbrance and value back to the player since we moved it from the player to a side pack
                EncumbranceVal += newStack.EncumbranceVal;
                Value += newStack.Value;
            }

            if ((sourceContainerIsOnLandblock || targetContainerIsOnLandblock) && sourceContainerIsOnLandblock != targetContainerIsOnLandblock)
            {
                // Between the player and an external pack

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();
            }
        }

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting item.
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are splitting</param>
        /// <param name="amount">The amount of the stack we are splitting from that we are moving to a new stack.</param>
        public void HandleActionStackableSplitTo3D(uint stackId, uint amount)
        {
            StopExistingMoveToChains();

            var stack = GetInventoryItem(new ObjectGuid(stackId), out var container);

            if (stack == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not found");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.YouDoNotOwnThatItem));
                return;
            }

            if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not large enough for amount");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.BadParam));
                return;
            }

            // Ok we are in business

            var motion = new Motion(this, MotionCommand.Pickup);

            // Set drop motion
            EnqueueBroadcastMotion(motion);

            // Now wait for Drop Motion to finish -- use ActionChain
            var dropChain = new ActionChain();

            // Wait for drop animation
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionCommand.Pickup, MotionCommand.Ready);
            dropChain.AddDelaySeconds(pickupAnimationLength);

            // Play drop sound
            // Put item on landblock
            dropChain.AddAction(this, () =>
            {
                if (CurrentLandblock == null)
                    return; // Maybe we were teleported as we were motioning to drop the item

                stack.StackSize -= (ushort)amount;
                stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
                stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

                var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
                newStack.StackSize = (ushort)amount;
                newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
                newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                // Adjust EncumbranceVal and Value

                container.EncumbranceVal -= newStack.EncumbranceVal;
                container.Value -= newStack.Value;

                if (container != this)
                {
                    EncumbranceVal -= newStack.EncumbranceVal;
                    Value -= newStack.Value;
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem, 1.0f));

                // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                newStack.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                newStack.Sequences.GetNextSequence(SequenceType.ObjectVector);

                newStack.SetPropertiesForWorld(this, 1.1f);

                CurrentLandblock.AddWorldObject(newStack);
            });

            dropChain.EnqueueChain();
        }

        /// <summary>
        /// This method processes the Stackable Merge Game Action (F7B1) Stackable Merge (0x0054)
        /// </summary>
        /// <param name="mergeFromGuid">Guid of the item are we merging from</param>
        /// <param name="mergeToGuid">Guid of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromGuid into the toGuid</param>
        public void HandleActionStackableMerge(ObjectGuid mergeFromGuid, ObjectGuid mergeToGuid, int amount)
        {
            // is this something I already have? If not, it has to be a pickup - do the pickup and out.
            if (!HasInventoryItem(mergeFromGuid))
            {
                // This is a pickup into our main pack.
                HandleActionPutItemInContainer(mergeFromGuid, Guid);
                return;
            }

            var fromItem = GetInventoryItem(mergeFromGuid);
            var toItem = GetInventoryItem(mergeToGuid);

            if (fromItem == null || toItem == null)
                return;

            // Check to see if we are trying to merge into a full stack. If so, nothing to do here.
            // Check this and see if I need to call UpdateToStack to clear the action with an amount of 0 Og II
            if (toItem.MaxStackSize == toItem.StackSize)
                return;

            var missileAmmo = toItem.ItemType == ItemType.MissileWeapon;

            if (toItem.MaxStackSize >= (ushort)((toItem.StackSize ?? 0) + amount))
            {
                // The toItem has enoguh capacity to take the full amount
                UpdateToStack(fromItem, toItem, amount, missileAmmo);

                // Ok did we merge it all? If so, let's remove the item.
                if (fromItem.StackSize == amount)
                    TryRemoveFromInventoryWithNetworking(fromItem.Guid, out _, true);
                else
                    UpdateFromStack(fromItem, amount);
            }
            else
            {
                // The toItem does not have enough capacity to take the full amount. Just add what we can and adjust both.
                Debug.Assert(toItem.MaxStackSize != null, "toWo.MaxStackSize != null");

                var amtToFill = (toItem.MaxStackSize ?? 0) - (toItem.StackSize ?? 0);

                UpdateToStack(fromItem, toItem, amtToFill, missileAmmo);
                UpdateFromStack(toItem, amtToFill);
            }
        }

        /// <summary>
        /// This method handles the second part of the merge if we have not merged ALL of the fromWo into the toWo - split out for code reuse.
        /// It calculates the updated values for stack size, value and burden, creates the needed client messages and sends them.
        /// This must be called from within an action chain. Og II
        /// </summary>
        /// <param name="fromWo">World object of the item are we merging from</param>
        /// <param name="amount">How many are we merging fromWo into the toWo</param>
        private void UpdateFromStack(WorldObject fromWo, int amount)
        {
            Debug.Assert(fromWo.Value != null, "fromWo.Value != null");
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            Debug.Assert(fromWo.EncumbranceVal != null, "fromWo.EncumbranceVal != null");

            // ok, there are some left, we need up update the stack size, value and burden of the fromWo
            int newFromValue = (int)(fromWo.Value + ((fromWo.Value / fromWo.StackSize) * -amount));
            uint newFromBurden = (uint)(fromWo.EncumbranceVal + ((fromWo.EncumbranceVal / fromWo.StackSize) * -amount));

            int oldFromStackSize = (int)fromWo.StackSize;
            fromWo.StackSize -= (ushort)amount;
            fromWo.Value = newFromValue;
            fromWo.EncumbranceVal = (int)newFromBurden;

            // Build the needed messages to the client.
            EnqueueBroadcast(new GameMessageSetStackSize(fromWo));
        }

        /// <summary>
        /// This method handles the first part of the merge - split out for code reuse.
        /// It calculates the updated values for stack size, value and burden, creates the needed client messages and sends them.
        /// This must be called from within an action chain. Og II
        /// </summary>
        /// <param name="fromWo">World object of the item are we merging from</param>
        /// <param name="toWo">World object of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromWo into the toWo</param>
        private void UpdateToStack(WorldObject fromWo, WorldObject toWo, int amount, bool missileAmmo = false)
        {
            Debug.Assert(toWo.Value != null, "toWo.Value != null");
            Debug.Assert(fromWo.Value != null, "fromWo.Value != null");
            Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            Debug.Assert(toWo.EncumbranceVal != null, "toWo.EncumbranceVal != null");
            Debug.Assert(fromWo.EncumbranceVal != null, "fromWo.EncumbranceVal != null");

            int newValue = (int)(toWo.Value + ((fromWo.Value / fromWo.StackSize) * amount));
            uint newBurden = (uint)(toWo.EncumbranceVal + ((fromWo.EncumbranceVal / fromWo.StackSize) * amount));

            int oldStackSize = (int)toWo.StackSize;
            toWo.StackSize += (ushort)amount;
            toWo.Value = newValue;
            toWo.EncumbranceVal = (int)newBurden;

            // Build the needed messages to the client.
            if (missileAmmo)
                EnqueueBroadcast(new GameMessageSetStackSize(toWo));
            else
                EnqueueBroadcast(new GameEventItemServerSaysContainId(Session, toWo, this), new GameMessageSetStackSize(toWo));
        }


        // ===========================
        // Game Action Handlers - Misc
        // ===========================

        /// <summary>
        /// This method handles inscription.   If you remove the inscription, it will remove the data from the object and
        /// remove it from the shard database - all inscriptions are stored in ace_object_properties_string Og II
        /// </summary>
        /// <param name="itemGuid">This is the object that we are trying to inscribe</param>
        /// <param name="inscriptionText">This is our inscription</param>
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

            if (!TryDequipObjectWithNetworking(item.Guid, out _, RemoveFromInventoryAction.WieldedToPack))
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
