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


        /// <summary>
        /// Returns all wielded items.
        /// </summary>
        public List<WorldObject> GetAllWieldedItems()
        {
            var results = new List<WorldObject>();

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
        public bool TryCreateInInventoryWithNetworking(WorldObject worldObject, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            return TryCreateInInventoryWithNetworking(worldObject, out _, placementPosition, limitToMainPackOnly);
        }

        /// <summary>
        /// If enough burden is available, this will try to add (via create) an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryCreateInInventoryWithNetworking(WorldObject worldObject, out Container container, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            if (!TryAddToInventory(worldObject, out container, placementPosition, limitToMainPackOnly)) // We don't have enough burden available or no empty pack slot.
                return false;

            Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));

            if (worldObject is Container lootAsContainer)
                Session.Network.EnqueueSend(new GameEventViewContents(Session, lootAsContainer));

            Session.Network.EnqueueSend(
                new GameEventItemServerSaysContainId(Session, worldObject, container),
                new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (worldObject.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        /// <summary>
        /// This method is used to remove X number of items from a stack.<para />
        /// If amount to remove is greater or equal to the current stacksize, the stack will be destroyed..
        /// </summary>
        public bool TryRemoveItemFromInventoryWithNetworkingWithDestroy(WorldObject worldObject, ushort amount)
        {
            if (amount >= (worldObject.StackSize ?? 1))
            {
                if (TryRemoveFromInventoryWithNetworking(worldObject))
                {
                    worldObject.Destroy();
                    return true;
                }

                return false;
            }

            worldObject.StackSize -= amount;

            Session.Network.EnqueueSend(new GameMessageSetStackSize(worldObject));

            EncumbranceVal = (EncumbranceVal - (worldObject.StackUnitEncumbrance * amount));
            Value = (Value - (worldObject.StackUnitValue * amount));

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            if (worldObject.WeenieType == WeenieType.Coin)
                UpdateCoinValue();

            return true;
        }

        /// <summary>
        /// If isFromMergeEvent is false, update messages will be sent for EncumbranceVal and if WeenieType is Coin, CoinValue will be updated and update messages will be sent for CoinValue.
        /// </summary>
        /// <returns></returns>
        public bool TryRemoveFromInventoryWithNetworking(WorldObject worldObject, bool isFromMergeEvent = false)
        {
            if (TryRemoveFromInventory(worldObject.Guid))
            {
                Session.Network.EnqueueSend(new GameEventInventoryRemoveObject(Session, worldObject));

                if (!isFromMergeEvent)
                {
                    Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                    if (worldObject.WeenieType == WeenieType.Coin)
                        UpdateCoinValue();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an item from either the player's inventory,
        /// or equipped items, and sends network messages
        /// </summary>
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

            return TryRemoveFromInventoryWithNetworking(item);
        }


        /// <summary>
        /// Returns an item from the landblock, or the lastUsedContainer
        /// </summary>
        private WorldObject GetPickupItem(ObjectGuid itemGuid, out bool itemWasRestingOnLandblock)
        {
            // Grab a reference to the item before its removed from the CurrentLandblock
            var item = CurrentLandblock?.GetObject(itemGuid);
            itemWasRestingOnLandblock = false;

            if (item != null)
            {
                itemWasRestingOnLandblock = true;
                return item;
            }

            var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

            if (lastUsedContainer != null)
                lastUsedContainer.Inventory.TryGetValue(itemGuid, out item);

            if (item == null)
                log.Error($"{Name}.GetPickupItem({itemGuid}) - couldn't find item");

            return item;
        }

        // =====================================
        // Helper Functions - Inventory Movement
        // =====================================
        // Used by HandleActionPutItemInContainer, HandleActionDropItem

        /// <summary>
        /// This method is used to pick items off the world - out of 3D space and into our inventory or to a wielded slot.
        /// It checks the use case needed, sends the appropriate response messages.
        /// In addition, it will move to objects that are out of range in the attemp to pick them up.
        /// It will call update apperiance if needed and you have wielded an item from the ground. Og II
        /// </summary>
        private void PickupItemWithNetworking(Container container, ObjectGuid itemGuid, int placementPosition, PropertyInstanceId iidPropertyId)
        {
            var item = GetPickupItem(itemGuid, out bool itemWasRestingOnLandblock);
            if (item == null) return;

            var targetLocation = FindItemLocation(itemGuid);
            if (targetLocation == null) return;

            // rotate / move towards object
            // TODO: only do this if not within use distance
            ActionChain pickUpItemChain = new ActionChain();
            pickUpItemChain.AddChain(CreateMoveToChain(targetLocation, out var thisMoveToChainNumber));

            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            // rotate towards object
            // TODO: should rotating be added directly to moveto chain?

            /*pickUpItemChain.AddAction(this, () => Rotate(targetLocation));
            var angle = GetAngle(targetLocation);
            var rotateTime = GetRotateDelay(angle);
            pickUpItemChain.AddDelaySeconds(rotateTime);*/

            pickUpItemChain.AddAction(this, () =>
            {
                /*if (thisMoveToChainNumberCopy != moveToChainCounter)
                {
                    // todo we need to break the pickUpItemChain to stop further elements from executing
                    // todo alternatively, we create a MoveToManager (see the physics implementation) to manage this.
                    // todo I figured having the ability to someChain.BreakChain() might come in handy in the future. - Mag
                    return;
                }*/

                // start picking up item animation
                var motion = new Motion(CurrentMotionState.Stance, MotionCommand.Pickup);
                EnqueueBroadcast(new GameMessageUpdatePosition(this), new GameMessageUpdateMotion(this, motion));
            });

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(CurrentMotionState.Stance, MotionCommand.Pickup, MotionCommand.Ready);
            pickUpItemChain.AddDelaySeconds(pickupAnimationLength);

            // pick up item
            pickUpItemChain.AddAction(this, () =>
            {
                // handle quest items
                var questSolve = false;

                if (item.Quest != null)
                {
                    if (!QuestManager.CanSolve(item.Quest))
                    {
                        QuestManager.HandleSolveError(item.Quest);
                        return;
                    }
                    else
                        questSolve = true;
                }

                if (itemWasRestingOnLandblock)
                {
                    if (CurrentLandblock != null && CurrentLandblock.RemoveWorldObjectFromPickup(itemGuid))
                        item.NotifyOfEvent(RegenerationType.PickUp);
                }
                else
                {
                    var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;
                    if (lastUsedContainer.TryRemoveFromInventory(itemGuid, out item))
                    {
                        item.NotifyOfEvent(RegenerationType.PickUp);
                    }
                    else
                    {
                        // Item is in the container which we should have open
                        log.Error($"{Name}.PickUpItemWithNetworking({itemGuid}): picking up items from world containers side pack WIP");
                        return;
                    }

                    var containerInventory = lastUsedContainer.Inventory;

                    var lastUsedHook = lastUsedContainer as Hook;
                    if (lastUsedHook != null)
                        lastUsedHook.OnRemoveItem();
                }

                // If the item still has a location, CurrentLandblock failed to remove it
                if (item.Location != null)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.NoObject));
                    log.Error("Player_Inventory PickupItemWithNetworking item.Location != null");
                    return;
                }

                // If the item has a ContainerId, it was probably picked up by someone else before us
                if (itemWasRestingOnLandblock && item.ContainerId != null && item.ContainerId != 0)
                {
                    log.Error("Player_Inventory PickupItemWithNetworking item.ContainerId != 0");
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.ObjectGone));
                    return;
                }

                item.SetPropertiesForContainer();

                // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...

                if (iidPropertyId == PropertyInstanceId.Container)
                {
                    if (!container.TryAddToInventory(item, placementPosition, true))
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.NoObject));
                        log.Error("Player_Inventory PickupItemWithNetworking TryAddToInventory failed");
                        return;
                    }

                    // If we've put the item to a side pack, we must increment our main EncumbranceValue and Value
                    if (container != this && container.ContainerId == Guid.Full)
                    {
                        EncumbranceVal += item.EncumbranceVal;
                        Value += item.Value;
                    }

                    if (item is Container itemAsContainer)
                    {
                        Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

                        foreach (var packItem in itemAsContainer.Inventory)
                            Session.Network.EnqueueSend(new GameMessageCreateObject(packItem.Value));
                    }

                    Session.Network.EnqueueSend(
                        new GameMessageSound(Guid, Sound.PickUpItem, 1.0f),
                        new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid),
                        new GameEventItemServerSaysContainId(Session, item, container));
                }
                else if (iidPropertyId == PropertyInstanceId.Wielder)
                {
                    // wield requirements check
                    var canWield = CheckWieldRequirement(item);
                    if (canWield != WeenieError.None)
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, canWield));
                        return;
                    }
                    if (!TryEquipObject(item, placementPosition))
                    {
                        Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.NoObject));
                        return;
                    }
                }

                EnqueueBroadcast(new GameMessagePickupEvent(item));

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                if (iidPropertyId == PropertyInstanceId.Wielder)
                    TryWieldItem(item, placementPosition);

                if (questSolve)
                    QuestManager.Update(item.Quest);
            });

            // return to previous stance
            pickUpItemChain.AddAction(this, () =>
            {
                var motion = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(motion);
            });

            // Set chain to run
            pickUpItemChain.EnqueueChain();
        }

        /// <summary>
        /// This method is called in response to a put item in container message. It is used when the item going into a container was wielded.
        /// It sets the appropriate properties, sends out response messages  and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        private bool UnwieldItemWithNetworking(Container container, WorldObject item, int placement = 0)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            // If item has any spells, remove them from the registry on unequip
            if (item.Biota.BiotaPropertiesSpellBook != null)
            {
                for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                    DispelItemSpell(item.Guid, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell);
            }

            if (!TryDequipObject(item.Guid))
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

            if ((oldLocation & EquipMask.Selectable) != 0)
            {
                // We are coming from a hand shield slot.
                Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
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

        /// <summary>
        /// Method is called in response to put item in container message. This use case is we are just reorganizing our items.
        /// It is either a in pack slot to slot move, or we could be going from one pack (container) to another.
        /// This method is called from an action chain.  Og II
        /// </summary>
        /// <param name="item">the item we are moving</param>
        /// <param name="container">what container are we going in</param>
        /// <param name="placementPosition">what is my slot position within that container</param>
        private void MoveItemWithNetworking(WorldObject item, Container container, int placementPosition)
        {
            if (!TryRemoveFromInventory(item.Guid))
            {
                log.Error("Player_Inventory MoveItemWithNetworking TryRemoveFromInventory failed");
                return;
            }

            if (!container.TryAddToInventory(item, placementPosition))
            {
                log.Error("Player_Inventory MoveItemWithNetworking TryAddToInventory failed");
                return;
            }

            // If we've moved the item to a side pack, we must increment our main EncumbranceValue and Value
            if (container != this && container.ContainerId == Guid.Full)
            {
                EncumbranceVal += item.EncumbranceVal;
                Value += item.Value;
            }

            Session.Network.EnqueueSend(
                new GameEventItemServerSaysContainId(Session, item, container),
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid));
        }

        private bool IsAttuned(ObjectGuid itemGuid, out bool isAttuned)
        {
            WorldObject item = GetInventoryItem(itemGuid);
            if (item == null)
            {
                isAttuned = false;
                return false;
            }

            int attunedProperty = (item.GetProperty(PropertyInt.Attuned) ?? 0);
            if (attunedProperty == 1)
            {
                isAttuned = true;
                return true;
            }

            isAttuned = false;
            return true;
        }

        // =========================================
        // Game Action Handlers - Inventory Movement 
        // =========================================
        // These are raised by client actions

        /// <summary>
        /// This is raised when we move an item around in our inventory.
        /// It is also raised when we dequip an item.
        /// </summary>
        public void HandleActionPutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, int placement = 0)
        {
            bool containerOwnedByPlayer = true;

            Container container;
            
            if (containerGuid == Guid)
                container = this; // Destination is main pack
            else
                container = (Container)GetInventoryItem(containerGuid); // Destination is side pack

            if (container == null) // Destination is a container in the world, not in our possession
            {
                containerOwnedByPlayer = false;
                container = CurrentLandblock?.GetObject(containerGuid) as Container;

                if (container == null) // Container is a container within a container in the world....
                {
                    var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

                    if (lastUsedContainer != null && lastUsedContainer.Inventory.TryGetValue(containerGuid, out var value))
                        container = value as Container;
                }
            }

            if (container == null)
            {
                log.Error("Player_Inventory HandleActionPutItemInContainer container not found");
                return;
            }

            var item = GetInventoryItem(itemGuid) ?? GetWieldedItem(itemGuid);
                        
            if (item != null)
            {
                //Console.WriteLine($"HandleActionPutItemInContainer({item.Name})");

                IsAttuned(itemGuid, out bool isAttuned);
                if (isAttuned == true && containerOwnedByPlayer == false)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.AttunedItem));
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, this));
                    return;
                }
            }

            var corpse = container as Corpse;
            if (corpse != null)
            {
                if (corpse.IsMonster == false)
                {
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.Dead));
                    return;
                }
            }

            // Is this something I already have? If not, it has to be a pickup - do the pickup and out.
            if (item == null)
            {
                var itemToPickup = CurrentLandblock?.GetObject(itemGuid);

                if (itemToPickup != null)
                {
                    //Checking to see if item to pick is an container itself
                    if (itemToPickup.WeenieType == WeenieType.Container)
                    {
                        //Check to see if the container is open
                        if (itemToPickup.IsOpen)
                        {
                            var containerToPickup = CurrentLandblock?.GetObject(itemGuid) as Container;

                            if (containerToPickup.Viewer == Session.Player.Guid.Full)
                            {
                                //We're the one that has it open. Close it before picking it up
                                containerToPickup.Close(Session.Player);
                            }
                            else
                            {
                                //We're not who has it open. Can't pick up something someone else is viewing!

                                //TODO: These messages are what I remember of retail. I was unable to confirm or deny with PCAPs
                                //TODO: This will likley need revisited at some point to align with retail
                                Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.The_IsCurrentlyInUse, itemToPickup.Name));
                                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.Stuck));
                                return;
                            }
                        }
                    }
                }

                // This is a pickup into our main pack.
                PickupItemWithNetworking(container, itemGuid, placement, PropertyInstanceId.Container);
                return;
            }

            // Ok, I know my container and I know I must have the item so let's get it.

            // Was I equiped? If so, lets take care of that and unequip
            if (item.WielderId != null)
            {
                UnwieldItemWithNetworking(container, item, placement);
                item.IsAffecting = false;
                return;
            }

            // if were are still here, this needs to do a pack pack or main pack move.
            MoveItemWithNetworking(item, container, placement);

            container.OnAddItem();
        }

        /// <summary>
        /// This is raised when we drop an item. It can be a wielded item, or an item in our inventory.
        /// </summary>
        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            IsAttuned(itemGuid, out bool isAttuned);
            if (isAttuned == true)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You cannot drop that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, WeenieError.None));
                return;
            }

            // check packs of item.
            WorldObject item;

            if (!TryRemoveFromInventory(itemGuid, out item))
            {
                // check to see if this item is wielded
                if (TryDequipObject(itemGuid, out item))
                {
                    Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));

                    //Session.Network.EnqueueSend(
                    //    new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                    //    new GameMessageObjDescEvent(this),
                    //    new GameMessageUpdateInstanceId(item.Sequences, new ObjectGuid(0), item.Guid, PropertyInstanceId.Wielder));

                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                        new GameMessageObjDescEvent(this),
                        new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, new ObjectGuid(0)));
                }
            }

            if (item == null)
            {
                log.Error("Player_Inventory HandleActionDropItem item is null");
                return;
            }

            item.SetPropertiesForWorld(this, 1.1f);

            // We must update the database with the latest ContainerId and WielderId properties.
            // If we don't, the player can drop the item, log out, and log back in. If the landblock hasn't queued a database save in that time,
            // the player will end up loading with this object in their inventory even though the landblock is the true owner. This is because
            // when we load player inventory, the database still has the record that shows this player as the ContainerId for the item.
            item.SaveBiotaToDatabase();

            //var motion = new Motion(MotionStance.NonCombat);
            var motion = new Motion(this, MotionCommand.Pickup);
            Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)));

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
                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem, (float)1.0));

                Session.Network.EnqueueSend(
                    new GameEventItemServerSaysMoveItem(Session, item),
                    new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                    new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                if (item.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                CurrentLandblock?.AddWorldObject(item);

                //Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                EnqueueBroadcast(new GameMessageUpdatePosition(item));
            });

            dropChain.EnqueueChain();
        }

        /// <summary>
        /// create spells by an equipped item
        /// </summary>
        /// <param name="item">the equipped item doing the spell creation</param>
        /// <param name="suppressSpellChatText">prevent spell text from being sent to the player's chat windows</param>
        /// <param name="ignoreRequirements">disregard item activation requirements</param>
        /// <returns>if any spells were created or not</returns>
        public bool CreateEquippedItemSpells(WorldObject item, bool suppressSpellChatText = false, bool ignoreRequirements = false)
        {
            bool spellCreated = false;
            if (item.Biota.BiotaPropertiesSpellBook != null)
            {
                // TODO: Once Item Current Mana is fixed for loot generated items, '|| item.ItemCurMana == null' can be removed
                if (item.ItemCurMana > 1 || item.ItemCurMana == null)
                {
                    for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                    {
                        if (CreateItemSpell(item.Guid, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell, suppressSpellChatText, ignoreRequirements))
                            spellCreated = true;
                    }
                    item.IsAffecting = spellCreated;
                    if (item.IsAffecting ?? false)
                    {
                        if (item.ItemCurMana.HasValue)
                            item.ItemCurMana--;
                    }
                }
            }
            return spellCreated;
        }

        /// <summary>
        /// Called when network message is received for 'GetAndWieldItem'
        /// </summary>
        public void HandleActionGetAndWieldItem(uint itemId, int wieldLocation)
        {
            var itemGuid = new ObjectGuid(itemId);

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
            var wieldedItem = GetWieldedItem(itemGuid);
            if (wieldedItem != null)
            {
                var result = TryWieldItem(wieldedItem, wieldLocation);
                return;
            }

            // We don't have possession of the item so we must pick it up.
            // should this be wielding the item afterwards?
            PickupItemWithNetworking(this, itemGuid, wieldLocation, PropertyInstanceId.Wielder);
        }

        public bool TryWieldItem(WorldObject item, int wieldLocation, bool preCheck = false)
        {
            //Console.WriteLine($"TryWieldItem({item.Name}, {(EquipMask)wieldLocation})");

            var wieldError = CheckWieldRequirement(item);

            if (wieldError != WeenieError.None)
            {
                var containerId = (uint)item.ContainerId;
                var container = GetInventoryItem(new ObjectGuid(containerId));
                if (container == null) container = this;

                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, errorType: wieldError));
                Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, container));
                return false;
            }

            // unwield wand / missile launcher if dual wielding
            if ((EquipMask)wieldLocation == EquipMask.Shield && !item.IsShield)
            {
                var mainWeapon = EquippedObjects.Values.FirstOrDefault(e => e.CurrentWieldedLocation == EquipMask.MissileWeapon || e.CurrentWieldedLocation == EquipMask.Held);
                if (mainWeapon != null)
                {
                    if (!UnwieldItemWithNetworking(this, mainWeapon))
                        return false;
                }
            }

            TryRemoveFromInventory(item.Guid, out var containerItem);

            if (!TryEquipObject(item, wieldLocation))
            {
                log.Error("Player_Inventory HandleActionGetAndWieldItem TryEquipObject failed");
                return false;
            }

            CreateEquippedItemSpells(item);

            // TODO: I think we need to recalc our SetupModel here. see CalculateObjDesc()
            var msgWieldItem = new GameEventWieldItem(Session, item.Guid.Full, wieldLocation);
            var sound = new GameMessageSound(Guid, Sound.WieldObject, 1.0f);

            if ((EquipMask)wieldLocation != EquipMask.MissileAmmo)
            {
                var updateContainer = new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0));
                var updateWielder = new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid);
                var updateWieldLoc = new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldLocation);

                if (((EquipMask)wieldLocation & EquipMask.Selectable) == 0)
                {
                    EnqueueBroadcast(msgWieldItem, sound, updateContainer, updateWielder, updateWieldLoc, new GameMessageObjDescEvent(this));
                    return true;
                }

                SetChild(item, wieldLocation, out var placementId, out var childLocation);

                // TODO: wait for HandleQueueStance() here?
                EnqueueBroadcast(new GameMessageParentEvent(this, item, childLocation, placementId), msgWieldItem, sound, updateContainer, updateWielder, updateWieldLoc);

                if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                    return true;

                switch ((EquipMask)wieldLocation)
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
            else
            {
                Session.Network.EnqueueSend(msgWieldItem, sound);

                // new ammo becomes visible
                // FIXME: can't get this to work without breaking client
                // existing functionality also broken while swapping multiple arrows in missile combat mode
                /*if (CombatMode == CombatMode.Missile)
                {
                    EnqueueBroadcast(new GameMessageParentEvent(this, item, (int)ACE.Entity.Enum.ParentLocation.RightHand, (int)ACE.Entity.Enum.Placement.RightHandCombat));
                }*/
            }

            return true;
        }

        public bool UseWieldRequirement = true;

        public WeenieError CheckWieldRequirement(WorldObject item)
        {
            if (!UseWieldRequirement) return WeenieError.None;

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

        public Skill ConvertToMoASkill(Skill skill)
        {
            var player = this as Player;
            if (player != null)
            {
                if (SkillExtensions.RetiredMelee.Contains(skill))
                    return player.GetHighestMeleeSkill();
                if (SkillExtensions.RetiredMissile.Contains(skill))
                    return Skill.MissileWeapons;
            }
            return skill;
        }

        /// <summary>
        /// Dictionary for salvage bags/material types
        /// TODO: This list needs to go somewhere else
        /// </summary>

        static Dictionary<int, int> dict = new Dictionary<int, int>()
                                                            {
                                                                {1, 20983},
                                                                {2, 21067},
                                                                {3, 0},//cloth
                                                                {4, 20987},
                                                                {5, 20992},
                                                                {6, 21076},
                                                                {7, 20994},
                                                                {8, 20995},
                                                                {9, 0},//gem
                                                                {10, 21034},
                                                                {11, 21035},
                                                                {12, 21036},
                                                                {13, 21037},
                                                                {14, 21038},
                                                                {15, 21039},
                                                                {16, 21040},
                                                                {17, 21041},
                                                                {18, 21043},
                                                                {19, 21044},
                                                                {20, 21046},
                                                                {21, 21048},
                                                                {22, 21049},
                                                                {23, 21050},
                                                                {24, 21051},
                                                                {25, 21053},
                                                                {26, 21054},
                                                                {27, 21056},
                                                                {28, 21057},
                                                                {29, 21058},
                                                                {30, 21060},
                                                                {31, 21062},
                                                                {32, 21064},
                                                                {33, 21065},
                                                                {34, 21066},
                                                                {35, 21069},
                                                                {36, 21070},
                                                                {37, 21071},
                                                                {38, 21072},
                                                                {39, 21074},
                                                                {40, 21078},
                                                                {41, 21079},
                                                                {42, 21081},
                                                                {43, 21082},
                                                                {44, 21083},
                                                                {45, 21084},
                                                                {46, 21085},
                                                                {47, 21086},
                                                                {48, 21087},
                                                                {49, 21088},
                                                                {50, 21089},
                                                                {51, 21055},
                                                                {52, 21059},
                                                                {53, 20981},
                                                                {54, 21052},
                                                                {55, 20991},
                                                                {56, 0}, //metal
                                                                {57, 21042},
                                                                {58, 20982},
                                                                {59, 21045},
                                                                {60, 20984},
                                                                {61, 20986},
                                                                {62, 21068},
                                                                {63, 21077},
                                                                {64, 20993},
                                                                {65, 0}, //stone
                                                                {66, 20980},
                                                                {67, 20985},
                                                                {68, 21061},
                                                                {69, 21063},
                                                                {70, 21073},
                                                                {71, 21075},
                                                                {72, 0}, //wood
                                                                {73, 21047},
                                                                {74, 20988},
                                                                {75, 20989},
                                                                {76, 20990},
                                                                {77, 21080}
                                                            };

        /// <summary>
        /// This code handles salvaginng materials with the Ust.
        /// </summary>
        /// ///FORMULA FOR VALUE
        ///(skill) / 387 * (1.0 + (augmentations)
        ////WHERE
        /// skill = Salvaging Skill
        ////augmentations = 0.0 for no augmentations, 0.25 for one, 0.5 for two, 
        ///0.75 for three, 1.0 for four

        public void HandleTinkeringTool(List<ObjectGuid> list)
        {
            
            //CreatureSkill skill = GetCreatureSkill(Skill.Salvaging);
            double salvageSkill = (double)Math.Max((uint)GetCreatureSkill(Skill.Salvaging).Current, Math.Max((uint)GetCreatureSkill(Skill.ArmorTinkering).Current, Math.Max((uint)GetCreatureSkill(Skill.MagicItemTinkering).Current, Math.Max((uint)GetCreatureSkill(Skill.WeaponTinkering).Current, (uint)GetCreatureSkill(Skill.ItemTinkering).Current))));
            double workAverage = 0;
            int materialType = 0;
            int amount = 0;
            int numItems = 0;
            int value = 0;
            if (GetCharacterOption(CharacterOption.SalvageMultipleMaterialsAtOnce))
            {
                int counter = 0;
                int objectCounter = 0;
                WorldObject[] salvageBags = new WorldObject[list.Count()];
                WorldObject[] items = new WorldObject[list.Count()];
                int[] materials = new int[list.Count()];
                foreach (ObjectGuid guid in list)
                {
                    bool inMaterialsList = false;
                    WorldObject item = GetInventoryItem(guid) as WorldObject;
                    items[objectCounter] = item;
                    objectCounter++;
                    int materialsPlace = 0;
                    foreach (int a in materials)
                    {
                        if (item.GetProperty(PropertyInt.MaterialType) == a)
                        {
                            inMaterialsList = true;
                            break;
                        }
                        materialsPlace++;
                    }
                    if (!inMaterialsList)
                    {
                        numItems = 0;
                        value = 0;
                        amount = 0;
                        materialType = 0;
                        materials[counter] = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                        int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                        if (numItemsinThisItem > 1)
                        {
                            value += item.GetProperty(PropertyInt.Value) ?? 0;
                        }
                        else
                        {
                            value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                        }
                        if (value > 75000)
                        {
                            value = 75000;
                        }
                        numItems += numItemsinThisItem;
                        workAverage += (double)workmanship;
                        double multiplier = (salvageSkill / 225.0);
                        double multiplier2 = .6 > multiplier ? .6 : multiplier;
                        amount += (int)Math.Ceiling(workmanship * multiplier2);
                        TryRemoveItemFromInventoryWithNetworkingWithDestroy(item, 1);
                        salvageBags[counter] = WorldObjectFactory.CreateNewWorldObject((uint)dict[materials[counter]]);
                        salvageBags[counter].SetProperty(PropertyInt.Structure, amount);
                        salvageBags[counter].SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                        salvageBags[counter].SetProperty(PropertyInt.ItemWorkmanship, amount);
                        salvageBags[counter].SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                        salvageBags[counter].SetProperty(PropertyInt.Value, value);
                        counter++;
                    }
                    else
                    {
                        materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                        int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                        int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                        if (numItemsinThisItem > 1)
                        {
                            value += item.GetProperty(PropertyInt.Value) ?? 0;
                        }
                        else
                        {
                            value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                        }
                        if (value > 75000)
                        {
                            value = 75000;
                        }
                        numItems += numItemsinThisItem;
                        workAverage += (double)workmanship;
                        double multiplier = (salvageSkill / 225.0);
                        double multiplier2 = .6 > multiplier ? .6 : multiplier;
                        amount += (int)Math.Ceiling(workmanship * multiplier2);
                        TryRemoveItemFromInventoryWithNetworkingWithDestroy(item, 1);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.Structure, amount);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                        salvageBags[materialsPlace].SetProperty(PropertyInt.ItemWorkmanship, amount);
                        salvageBags[materialsPlace].SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                        salvageBags[materialsPlace].SetProperty(PropertyInt.Value, value);
                    }

                }
                foreach (WorldObject wo2 in salvageBags)
                {
                    if (wo2 != null)
                    {
                        Console.WriteLine("The name of this bag is " + wo2.Name);
                        TryCreateInInventoryWithNetworking(wo2);
                    }
                }
            }
            else
            {

                foreach (ObjectGuid guid in list)
                {
                    WorldObject item = GetInventoryItem(guid) as WorldObject;
                    materialType = item.GetProperty(PropertyInt.MaterialType) ?? 0;
                    int workmanship = item.GetProperty(PropertyInt.ItemWorkmanship) ?? 0;
                    int numItemsinThisItem = item.GetProperty(PropertyInt.NumItemsInMaterial) ?? 1;
                    if (numItemsinThisItem > 1)
                    {
                        value += item.GetProperty(PropertyInt.Value) ?? 0;
                    }
                    else
                    {
                        value += (int)((item.GetProperty(PropertyInt.Value) ?? 0) * (salvageSkill / 387.0));
                    }
                    if (value > 75000)
                    {
                        value = 75000;
                    }
                    numItems += numItemsinThisItem;
                    workAverage += (double)workmanship;
                    double multiplier = (salvageSkill / 225.0);
                    double multiplier2 = .6 > multiplier ? .6 : multiplier;
                    amount += (int)Math.Ceiling(workmanship * multiplier2);
                    TryRemoveItemFromInventoryWithNetworkingWithDestroy(item, 1);
                }

                WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)dict[materialType]);
                wo.SetProperty(PropertyInt.Structure, amount);
                wo.SetProperty(PropertyInt.NumItemsInMaterial, numItems);
                wo.SetProperty(PropertyInt.ItemWorkmanship, amount);
                wo.SetProperty(PropertyString.Name, "Salvage (" + amount + ")");
                wo.SetProperty(PropertyInt.Value, value);
                TryCreateInInventoryWithNetworking(wo);
            }
        }

        /// <summary>
        /// Called when player attempts to give an object to someone else,
        /// ie. to another player, or NPC
        public void HandleActionGiveObjectRequest(ObjectGuid targetID, ObjectGuid itemGuid, uint amount)
        {
            var target = CurrentLandblock?.GetObject(targetID);
            var item = GetInventoryItem(itemGuid) ?? GetWieldedItem(itemGuid);
            if (target == null || item == null) return;

            // giver rotates to receiver
            var rotateDelay = Rotate(target);

            var giveChain = new ActionChain();
            giveChain.AddChain(CreateMoveToChain(targetID, out var thisMoveToChainNumber));

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

            if ((item.GetProperty(PropertyInt.Attuned) ?? 0) == 1)
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
                            if (TryRemoveFromInventory(item.Guid, false))
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
        /// This code handles receiving objects from other players, ie. attempting to place the item in the target's inventory
        /// </summary>
        private bool HandlePlayerReceiveItem(WorldObject item, Player player)
        {
            if ((this as Player) == null)
                return false;

            Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} gives you {item.Name}.", ChatMessageType.Broadcast));
            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.ReceiveItem, 1));

            TryCreateInInventoryWithNetworking(item);

            return true;
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

        // ===========================
        // Game Action Handlers - Misc
        // ===========================
        // These are raised by client actions

        /// <summary>
        /// This method handles inscription.   If you remove the inscription, it will remove the data from the object and
        /// remove it from the shard database - all inscriptions are stored in ace_object_properties_string Og II
        /// </summary>
        /// <param name="itemGuid">This is the object that we are trying to inscribe</param>
        /// <param name="inscriptionText">This is our inscription</param>
        public void HandleActionSetInscription(ObjectGuid itemGuid, string inscriptionText)
        {
            var item = GetInventoryItem(itemGuid) ?? GetWieldedItem(itemGuid);

            if (item == null)
            {
                log.Error("Player_Inventory HandleActionSetInscription failed");
                return;
            }

            if (item.Inscribable == true && item.ScribeName != "prewritten")
            {
                if (item.ScribeName != null && item.ScribeName != Name)
                {
                    ChatPacket.SendServerMessage(Session, "Only the original scribe may alter this without the use of an uninscription stone.", ChatMessageType.Broadcast);
                }
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


        // =====================================
        // Helper Functions - Inventory Stacking
        // =====================================
        // Used by HandleActionStackableSplitToContainer

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
                EnqueueBroadcast( new GameMessageSetStackSize(toWo));
            else
                EnqueueBroadcast(new GameEventItemServerSaysContainId(Session, toWo, this), new GameMessageSetStackSize(toWo));
        }


        // =========================================
        // Game Action Handlers - Inventory Stacking 
        // =========================================
        // These are raised by client actions

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting item. Og II
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are spliting</param>
        /// <param name="containerId">The guid of the container</param>
        /// <param name="placementPosition">Place is the slot in the container we are spliting into. Range 0-MaxCapacity</param>
        /// <param name="amount">The amount of the stack we are spliting from that we are moving to a new stack.</param>
        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, int placementPosition, ushort amount)
        {
            Container container;
            if (containerId == Guid.Full)
                container = this;
            else
                container = GetInventoryItem(new ObjectGuid(containerId)) as Container;

            if (container == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer container not found");
                return;
            }

            var stack = container.GetInventoryItem(new ObjectGuid(stackId));

            if (stack == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not found");
                return;
            }

            if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not large enough for amount");
                return;
            }

            // Ok we are in business
            stack.StackSize -= amount;
            stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
            stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

            var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
            newStack.StackSize = amount;
            newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
            newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

            if (!container.TryAddToInventory(newStack, placementPosition, true))
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer TryAddToInventory failed");
                return;
            }

            /* todo this needs to be fixed. There are many scenarios here, to/from main pack, to/from side pack, to/from landscape container.
            if (container.Guid.Full == stack.ContainerId)
            {
                // We subtract the new stack from our main values because TryAddToInventory will end up readding them.
                EncumbranceVal -= newStack.EncumbranceVal;
                Value -= newStack.Value;
                // todo CoinValue .. would only apply if we're going to/from landscape container
            }*/

            // todo i'm not sure if this is right? Should it be a landblock broadcast if we're splitting items on our own person?
            // todo Probably only landblock if the container exists on the landscape, but even then... i don't think so
            EnqueueBroadcast(new GameEventItemServerSaysContainId(Session, newStack, container),
                new GameMessageSetStackSize(stack),
                new GameMessageCreateObject(newStack));
        }

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting item.
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are spliting</param>
        /// <param name="amount">The amount of the stack we are spliting from that we are moving to a new stack.</param>
        public void HandleActionStackableSplitTo3D(uint stackId, uint amount)
        {
            var stack = GetInventoryItem(new ObjectGuid(stackId), out var container);

            if (stack == null)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not found");
                return;
            }

            if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
            {
                log.Error("Player_Inventory HandleActionStackableSplitToContainer stack not large enough for amount");
                return;
            }

            // Ok we are in business
            stack.StackSize -= (ushort)amount;
            stack.EncumbranceVal = (stack.StackUnitEncumbrance ?? 0) * (stack.StackSize ?? 1);
            stack.Value = (stack.StackUnitValue ?? 0) * (stack.StackSize ?? 1);

            var newStack = WorldObjectFactory.CreateNewWorldObject(stack.WeenieClassId);
            newStack.StackSize = (ushort)amount;
            newStack.EncumbranceVal = (newStack.StackUnitEncumbrance ?? 0) * (newStack.StackSize ?? 1);
            newStack.Value = (newStack.StackUnitValue ?? 0) * (newStack.StackSize ?? 1);

            newStack.SetPropertiesForWorld(this, 1.1f);

            container.EncumbranceVal -= newStack.EncumbranceVal;
            container.Value -= newStack.Value;

            if (container != this)
            {
                EncumbranceVal -= newStack.EncumbranceVal;
                Value -= newStack.Value;
            }

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
                if (container != this)
                    Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(container, PropertyInt.EncumbranceVal, container.EncumbranceVal ?? 0));
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                var returnStance = new Motion(CurrentMotionState.Stance);
                EnqueueBroadcastMotion(returnStance);

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.DropItem, 1.0f));

                Session.Network.EnqueueSend(new GameMessageSetStackSize(stack));

                if (newStack.WeenieType == WeenieType.Coin)
                    UpdateCoinValue();

                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                    newStack.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                newStack.Sequences.GetNextSequence(SequenceType.ObjectVector);

                CurrentLandblock?.AddWorldObject(newStack);

                    //Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                    EnqueueBroadcast(new GameMessageUpdatePosition(newStack));
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
                    TryRemoveFromInventoryWithNetworking(fromItem, true);
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
    }
}
