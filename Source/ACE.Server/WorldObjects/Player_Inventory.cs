using System;
using System.Collections.Generic;
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
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private static readonly float PickUpDistance = .75f;


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
                new GameMessagePutObjectInContainer(Session, container.Guid, worldObject, worldObject.PlacementPosition ?? 0),
                new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

            return true;
        }

        public bool TryDestroyFromInventoryWithNetworking(WorldObject worldObject, bool sendUpdateEncumbranceValMessage = true)
        {
            if (TryRemoveFromInventory(worldObject.Guid))
            {
                Session.Network.EnqueueSend(new GameMessageRemoveObject(worldObject));

                if (sendUpdateEncumbranceValMessage)
                    Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                worldObject.RemoveBiotaFromDatabase();

                return true;
            }

            return false;
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
            ActionChain pickUpItemChain = new ActionChain();

            // Move to the object
            pickUpItemChain.AddChain(CreateMoveToChain(itemGuid, PickUpDistance, out var thisMoveToChainNumber));

            // Pick up the object
            // Start pickup animation
            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            pickUpItemChain.AddAction(this, () =>
            {
                /*if (thisMoveToChainNumberCopy != moveToChainCounter)
                {
                    // todo we need to break the pickUpItemChain to stop further elements from executing
                    // todo alternatively, we create a MoveToManager (see the physics implementation) to manage this.
                    // todo I figured having the ability to someChain.BreakChain() might come in handy in the future. - Mag
                    return;
                }*/

                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(Guid, Sequences.GetCurrentSequence(SequenceType.ObjectInstance), Sequences, motion));
            });

            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(MotionCommand.Pickup);
            pickUpItemChain.AddDelaySeconds(pickupAnimationLength);

            // Grab a reference to the item before its removed from the CurrentLandblock
            var item = CurrentLandblock.GetObject(itemGuid);
            var itemWasRestingOnLandblock = false;

            if (item != null)
            {
                itemWasRestingOnLandblock = true;

                // Queue up an action that wait for the landblock to remove the item. The action that gets queued, when fired, will be run on the landblocks ActionChain, not this players.
                CurrentLandblock.QueueItemRemove(pickUpItemChain, itemGuid);
            }
            else
            {
                var lastUsedContainer = CurrentLandblock.GetObject(lastUsedContainerId) as Container;

                if (lastUsedContainer != null)
                {
                    if (!lastUsedContainer.TryRemoveFromInventory(itemGuid, out item))
                    {
                        // Item is in the container which we should have open
                        log.Error("Player_Inventory PickupItemWithNetworking picking up items from world containers side pack WIP");
                        return;
                    }
                }
            }

            if (item == null)
            {
                log.Error("Player_Inventory PickupItemWithNetworking item == null");
                return;
            }

            // Finish pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                // If the item still has a location, CurrentLandblock failed to remove it
                if (item.Location != null)
                {
                    log.Error("Player_Inventory PickupItemWithNetworking item.Location != null");
                    return;
                }

                // If the item has a ContainerId, it was probably picked up by someone else before us
                if (itemWasRestingOnLandblock && item.ContainerId != null && item.ContainerId != 0)
                {
                    log.Error("Player_Inventory PickupItemWithNetworking item.ContainerId != 0");
                    return;
                }

                item.SetPropertiesForContainer();

                // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...

                if (iidPropertyId == PropertyInstanceId.Container)
                {
                    if (!container.TryAddToInventory(item, placementPosition, true))
                    {
                        log.Error("Player_Inventory PickupItemWithNetworking TryAddToInventory failed");
                        return;
                    }

                    // If we've put the item to a side pack, we must increment our main EncumbranceValue and Value
                    if (container != this && container.ContainerId == Guid.Full)
                    {
                        EncumbranceVal += item.EncumbranceVal;
                        Value += item.Value;
                        // todo CoinValue
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
                        new GameMessageCreateObject(item),
                        new GameMessagePutObjectInContainer(Session, container.Guid, item, placementPosition));
                }
                else if (iidPropertyId == PropertyInstanceId.Wielder)
                {
                    if (!TryEquipObject(item, placementPosition))
                    {
                        log.Error("Player_Inventory PickupItemWithNetworking TryEquipObject failed");
                        return;
                    }

                    if (((EquipMask)placementPosition & EquipMask.Selectable) != 0)
                        SetChild(item, placementPosition, out _, out _);

                    // todo I think we need to recalc our SetupModel here. see CalculateObjDesc()

                    Session.Network.EnqueueSend(
                        new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                        //new GameMessageObjDescEvent(this),
                        new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, container.Guid),
                        new GameEventWieldItem(Session, itemGuid.Full, placementPosition),
                        new GameMessageCreateObject(item),
                        new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, (int)item.CurrentWieldedLocation));
                }

                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                var motion = new UniversalMotion(MotionStance.Standing);

                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                    new GameMessageUpdateMotion(Guid, Sequences.GetCurrentSequence(SequenceType.ObjectInstance), Sequences, motion),
                    new GameMessagePickupEvent(item));

                if (iidPropertyId == PropertyInstanceId.Wielder)
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageObjDescEvent(this));

                // TODO: Og II - check this later to see if it is still required.
                //Session.Network.EnqueueSend(new GameMessageUpdateObject(item));

                // Was Item controlled by a generator?
                // TODO: Should this be happening this way? Should the landblock notify the object of pickup or the generator...
                /*if (item.GeneratorId > 0)
                {
                    WorldObject generator = GetObject(new ObjectGuid((uint)item.GeneratorId));

                    item.GeneratorId = null;

                    generator.NotifyGeneratorOfPickup(item.Guid.Full);
                }*/

                item.SaveBiotaToDatabase();
            });
            // Set chain to run
            pickUpItemChain.EnqueueChain();
        }

        /// <summary>
        /// This method is called in response to a put item in container message. It is used when the item going into a container was wielded.
        /// It sets the appropriate properties, sends out response messages  and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        private void UnwieldItemWithNetworking(Container container, WorldObject item, int placement)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            if (!TryDequipObject(item.Guid))
            {
                log.Error("Player_Inventory UnwieldItemWithNetworking TryDequipObject failed");
                return;
            }

            item.SetPropertiesForContainer();

            if (!container.TryAddToInventory(item, placement))
            {
                log.Error("Player_Inventory UnwieldItemWithNetworking TryAddToInventory failed");
                return;
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

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0),
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid),
                new GameMessagePickupEvent(item),
                new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0),
                new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                new GameMessageObjDescEvent(this));

            if ((oldLocation != EquipMask.MissileWeapon && oldLocation != EquipMask.Held && oldLocation != EquipMask.MeleeWeapon) || ((CombatMode & CombatMode.CombatCombat) == 0))
                return;

            HandleSwitchToPeaceMode(CombatMode);
            HandleSwitchToMeleeCombatMode(CombatMode);
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

            // If we're putting the item into a container not on our person, we should save the changes to the db
            if (container != this && container.ContainerId != Guid.Full)
                item.SaveBiotaToDatabase();

            Session.Network.EnqueueSend(
                new GameMessagePutObjectInContainer(Session, container.Guid, item, placementPosition),
                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, container.Guid));
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
            new ActionChain(this, () =>
            {
                Container container;

                if (containerGuid == Guid)
                    container = this; // Destination is main pack
                else
                    container = (Container)GetInventoryItem(containerGuid); // Destination is side pack

                if (container == null) // Destination is a container in the world, not in our possession
                {
                    container = CurrentLandblock.GetObject(containerGuid) as Container;

                    if (container == null) // Container is a container within a container in the world....
                    {
                        var lastUsedContainer = CurrentLandblock.GetObject(lastUsedContainerId) as Container;

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

                // Is this something I already have? If not, it has to be a pickup - do the pickup and out.
                if (item == null)
                {
                    // This is a pickup into our main pack.
                    PickupItemWithNetworking(container, itemGuid, placement, PropertyInstanceId.Container);
                    return;
                }

                // Ok, I know my container and I know I must have the item so let's get it.

                // Was I equiped? If so, lets take care of that and unequip
                if (item.WielderId != null)
                {
                    UnwieldItemWithNetworking(container, item, placement);
                    return;
                }

                // if were are still here, this needs to do a pack pack or main pack move.
                MoveItemWithNetworking(item, container, placement);
            }).EnqueueChain();
        }

        /// <summary>
        /// This is raised when we drop an item. It can be a wielded item, or an item in our inventory.
        /// </summary>
        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            // Goody Goody -- lets build  drop chain
            // First start drop animation
            new ActionChain(this, () =>
            {
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

                        CurrentLandblock.EnqueueBroadcast(Location,
                            new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                            new GameMessageObjDescEvent(this),
                            new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, new ObjectGuid(0)));
                    }
                }

                if (item == null)
                {
                    log.Error("Player_Inventory HandleActionDropItem item is null");
                    return;
                }

                item.SetPropertiesForWorld(this);

                // It's important that we save an item after it's been removed from inventory.
                // We want to avoid the scenario where the server crashes and a player has too many items.
                item.SaveBiotaToDatabase();

                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)));

                // Set drop motion
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);

                // Now wait for Drop Motion to finish -- use ActionChain
                var chain = new ActionChain();

                // Wait for drop animation
                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
                var pickupAnimationLength = motionTable.GetAnimationLength(MotionCommand.Pickup);
                chain.AddDelaySeconds(pickupAnimationLength);

                // Play drop sound
                // Put item on landblock
                chain.AddAction(this, () =>
                {
                    motion = new UniversalMotion(MotionStance.Standing);
                    CurrentLandblock.EnqueueBroadcastMotion(this, motion);
                    CurrentLandblock.EnqueueBroadcast(Location, new GameMessageSound(Guid, Sound.DropItem, (float)1.0));
                    Session.Network.EnqueueSend(
                        new GameMessagePutObjectIn3d(Session, this, itemGuid),
                        new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                        new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                    item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                    item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                    CurrentLandblock.AddWorldObject(item);

                    //Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                    CurrentLandblock.EnqueueBroadcast(Location, new GameMessageUpdatePosition(item));
                });

                chain.EnqueueChain();
            }).EnqueueChain();
        }

        public void HandleActionGetAndWieldItem(uint itemId, int wieldLocation)
        {
            new ActionChain(this, () =>
            {
                var itemGuid = new ObjectGuid(itemId);

                if (TryRemoveFromInventory(itemGuid, out WorldObject item))
                {
                    if (!TryEquipObject(item, wieldLocation))
                    {
                        log.Error("Player_Inventory HandleActionGetAndWieldItem TryEquipObject failed");
                        return;
                    }

                    if ((EquipMask)wieldLocation == EquipMask.MissileAmmo)
                    {
                        Session.Network.EnqueueSend(
                            new GameEventWieldItem(Session, itemGuid.Full, wieldLocation),
                            new GameMessageSound(Guid, Sound.WieldObject, 1.0f));
                    }
                    else
                    {
                        if (((EquipMask)wieldLocation & EquipMask.Selectable) != 0)
                        {
                            SetChild(item, wieldLocation, out var placementId, out var childLocation);

                            // todo I think we need to recalc our SetupModel here. see CalculateObjDesc()

                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                new GameMessageParentEvent(Session.Player, item, childLocation, placementId),
                                new GameEventWieldItem(Session, itemGuid.Full, wieldLocation),
                                new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid),
                                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldLocation));

                            if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                                return;

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
                            // todo I think we need to recalc our SetupModel here. see CalculateObjDesc()

                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                new GameEventWieldItem(Session, itemGuid.Full, wieldLocation),
                                new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, new ObjectGuid(0)),
                                new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, Guid),
                                new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, wieldLocation),
                                new GameMessageObjDescEvent(this));
                        }
                    }
                }
                else
                {
                    // We don't have possession of the item so we must pick it up.
                    PickupItemWithNetworking(this, itemGuid, wieldLocation, PropertyInstanceId.Wielder);
                }
            }).EnqueueChain();
        }

        /// <summary>
        /// This code handle objects between players and other world objects
        /// </summary>
        public void HandleActionGiveObjectRequest(uint targetID, uint objectID, uint amount)
        {
            log.Error("Player_Inventory HandleActionGiveObjectRequest not implemented");
            ////ObjectGuid target = new ObjectGuid(targetID);
            ////ObjectGuid item = new ObjectGuid(objectID);
            ////WorldObject targetObject = CurrentLandblock.GetObject(target) as WorldObject;
            ////WorldObject itemObject = GetInventoryItem(item);
            ////////WorldObject itemObject = CurrentLandblock.GetObject(item) as WorldObject;
            ////Session.Network.EnqueueSend(new GameMessagePutObjectInContainer(Session, (ObjectGuid)targetObject.Guid, itemObject, 0));
            ////SendUseDoneEvent();
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
            new ActionChain(this, () =>
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
            }).EnqueueChain();
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
            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange,
                new GameMessagePrivateUpdatePropertyInt(fromWo, PropertyInt.Value, newFromValue),
                new GameMessageSetStackSize(fromWo.Sequences, fromWo.Guid, fromWo.StackSize ?? 0, oldFromStackSize));
        }

        /// <summary>
        /// This method handles the first part of the merge - split out for code reuse.
        /// It calculates the updated values for stack size, value and burden, creates the needed client messages and sends them.
        /// This must be called from within an action chain. Og II
        /// </summary>
        /// <param name="fromWo">World object of the item are we merging from</param>
        /// <param name="toWo">World object of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromWo into the toWo</param>
        private void UpdateToStack(WorldObject fromWo, WorldObject toWo, int amount)
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
            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange,
                new GameMessagePrivateUpdatePropertyInt(toWo, PropertyInt.Value, newValue),
                new GameMessagePutObjectInContainer(Session, Guid, toWo, toWo.PlacementPosition ?? 0),
                new GameMessageSetStackSize(toWo.Sequences, toWo.Guid, toWo.StackSize ?? 0, oldStackSize));
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
            new ActionChain(this, () =>
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

                /* todo this needs to be fixed. There are many scenarios here, to/from main pack, to/from side pack, to/from landscape container??? maybe, test that.
                if (container.Guid.Full == stack.ContainerId)
                {
                    // We subtract the new stack from our main values because TryAddToInventory will end up readding them.
                    EncumbranceVal -= newStack.EncumbranceVal;
                    Value -= newStack.Value;
                    // todo CoinValue
                }*/

                CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange,
                    new GameMessagePutObjectInContainer(Session, container.Guid, newStack, placementPosition),
                    new GameMessageSetStackSize(stack.Sequences, stack.Guid, stack.StackSize ?? 0, stack.Value ?? 0),
                    new GameMessageCreateObject(newStack));
            }).EnqueueChain();
        }

        /// <summary>
        /// This method processes the Stackable Merge Game Action (F7B1) Stackable Merge (0x0054)
        /// </summary>
        /// <param name="mergeFromGuid">Guid of the item are we merging from</param>
        /// <param name="mergeToGuid">Guid of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromGuid into the toGuid</param>
        public void HandleActionStackableMerge(ObjectGuid mergeFromGuid, ObjectGuid mergeToGuid, int amount)
        {
            new ActionChain(this, () =>
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

                if (toItem.MaxStackSize >= (ushort)((toItem.StackSize ?? 0) + amount))
                {
                    // The toItem has enoguh capacity to take the full amount
                    UpdateToStack(fromItem, toItem, amount);

                    // Ok did we merge it all? If so, let's destroy the from item.
                    if (fromItem.StackSize == amount)
                        TryDestroyFromInventoryWithNetworking(fromItem, false);
                    else
                        UpdateFromStack(fromItem, amount);
                }
                else
                {
                    // The toItem does not have enoguh capacity to take the full amount. Just add what we can and adjust both.
                    Debug.Assert(toItem.MaxStackSize != null, "toWo.MaxStackSize != null");

                    var amtToFill = (toItem.MaxStackSize ?? 0) - (toItem.StackSize ?? 0);

                    UpdateToStack(fromItem, toItem, amtToFill);
                    UpdateFromStack(toItem, amtToFill);
                }
            }).EnqueueChain();
        }







        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        /// <summary>
        /// Adds a new object to the 's inventory of the specified weenie class.  intended use case: giving items to players
        /// while they are plaplayerying.  this calls all the necessary helper functions to have the item be tracked and sent to the client.
        /// </summary>
        /// <returns>the object created</returns>
        [Obsolete("This needs to be refactored into the new system")]
        public WorldObject AddNewItemToInventory(uint weenieClassId)
        {
            var wo = Factories.WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            wo.ContainerId = Guid.Full;
            wo.PlacementPosition = 0;
            AddToInventory(wo);
            TrackObject(wo);
            return wo;
        }

        /// <summary>
        /// This method is used to remove X number of items from a stack, including
        /// If amount to remove is greater or equal to the current stacksize, item will be removed.
        /// </summary>
        /// <param name="stackId">Guid.Full of the stacked item</param>
        /// <param name="containerId">Guid.Full of the container that contains the item</param>
        /// <param name="amount">Amount taken out of the stack</param>
        [Obsolete("This needs to be refactored into the new system")]
        public void RemoveItemFromInventory(uint stackId, uint containerId, ushort amount)
        {
            // FIXME: This method has been morphed into doing a few things we either need to rename it or something. This may or may not remove item from inventory.
            new ActionChain(this, () =>
            {
                Container container;
                if (containerId == Guid.Full)
                    container = this;
                else
                    container = (Container)GetInventoryItem(new ObjectGuid(containerId));

                if (container == null)
                {
                    log.InfoFormat("Asked to remove an item {0} in container {1} - the container was not found", stackId, containerId);
                    return;
                }

                WorldObject item = container.GetInventoryItem(new ObjectGuid(stackId));
                if (item == null)
                {
                    log.InfoFormat("Asked to remove an item {0} in container {1} - the item was not found",
                        stackId,
                        containerId);
                    return;
                }

                if (amount >= item.StackSize)
                    amount = (ushort)item.StackSize;

                ushort oldStackSize = (ushort)item.StackSize;
                ushort newStackSize = (ushort)(oldStackSize - amount);

                if (newStackSize < 1)
                    newStackSize = 0;

                item.StackSize = newStackSize;

                Session.Network.EnqueueSend(new GameMessageSetStackSize(item.Sequences, item.Guid, (int)item.StackSize, (int)item.Value));

                if (newStackSize < 1)
                {
                    // Remove item from inventory
                    TryDestroyFromInventoryWithNetworking(item);
                }
                else
                    EncumbranceVal = (EncumbranceVal - (item.StackUnitEncumbrance * amount));
            }).EnqueueChain();
        }

        /// <summary>
        /// Add New WorldObject to Inventory
        /// </summary>
        [Obsolete("This needs to be refactored into the new system")]
        private void AddNewWorldObjectToInventory(WorldObject wo)
        {
            // Get Next Avalibale Pack Location.
            // uint packid = GetCreatureInventoryFreePack();

            // default player until I get above code to work!
            uint packid = Guid.Full;

            if (packid != 0)
            {
                wo.ContainerId = packid;
                AddToInventory(wo);
                Session.Network.EnqueueSend(new GameMessageCreateObject(wo));
                if (wo is Container container)
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, container));
            }
        }
    }
}
