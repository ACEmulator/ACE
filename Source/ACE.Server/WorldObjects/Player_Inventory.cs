using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
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
            var encumbranceAgumentations = 0; // todo

            var strength = Attributes[PropertyAttribute.Strength].Current;

            return (int)((150 * strength) + (encumbranceAgumentations * 30 * strength));
        }

        public bool HasEnoughBurdenToAddToInventory(WorldObject worldObject)
        {
            return (Burden + worldObject.Burden <= GetEncumbranceCapacity());
        }

        /// <summary>
        /// If enough burden is available, this will try to add (via create) an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryCreateInInventoryWithNetworking(WorldObject worldObject, out Container container, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            if (!TryAddToInventory(worldObject, out container, placementPosition, limitToMainPackOnly)) // We don't have enough burden available or no empty pack slot.
                return false;

            TrackObject(worldObject);

            if (worldObject is Container lootAsContainer)
                Session.Network.EnqueueSend(new GameEventViewContents(Session, lootAsContainer));

            Session.Network.EnqueueSend(
                new GameMessagePutObjectInContainer(Session, container.Guid, worldObject, worldObject.PlacementPosition ?? 0),
                new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.EncumbranceVal, Burden));

            DatabaseManager.Shard.AddBiota(worldObject.Biota, null);

            return true;
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
        public WorldObject AddNewItemToInventory(uint weenieClassId)
        {
            var wo = Factories.WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            wo.ContainerId = (int)Guid.Full;
            wo.PlacementPosition = 0;
            AddToInventory(wo);
            TrackObject(wo);
            return wo;
        }

        public void PutItemInContainer(ObjectGuid itemGuid, ObjectGuid containerGuid, int placement = 0)
        {
            Container container;

            if (containerGuid.IsPlayer())
                container = this;
            else
            {
                // Ok I am going into player pack - not the main pack.

                // TODO pick up here - I have a generic object for a container, need to find out why.
                container = (Container)GetInventoryItem(containerGuid);
            }

            // is this something I already have? If not, it has to be a pickup - do the pickup and out.
            if (!HasInventoryItem(itemGuid) && !HasWieldedItem(itemGuid))
            {
                // This is a pickup into our main pack.
                HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Container);
                return;
            }

            // Ok, I know my container and I know I must have the item so let's get it.
            WorldObject item = GetInventoryItem(itemGuid);

            // check wilded.
            if (item == null)
                item = GetWieldedItem(itemGuid);

            // Was I equiped?   If so, lets take care of that and unequip
            if (item.WielderId != null)
            {
                HandleUnwieldItem(container, item, placement);
                return;
            }

            // if were are still here, this needs to do a pack pack or main pack move.
            HandleMove(ref item, container, placement);
        }

        /// <summary>
        /// Context: only call when in the player action loop
        /// </summary>
        public void DestroyInventoryItem(WorldObject wo)
        {
            TryRemoveFromInventory(wo.Guid, out WorldObject _);
            Session.Network.EnqueueSend(new GameMessageRemoveObject(wo));
            ////Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, (uint)Burden));
        }


        /// <summary>
        /// This method removes an item from Inventory and adds it to wielded items.
        /// It also clears all properties used when an object is contained and sets the needed properties to be wielded Og II
        /// </summary>
        /// <param name="item">The world object we are wielding</param>
        /// <param name="wielder">Who is wielding the item</param>
        /// <param name="currentWieldedLocation">What wield location are we going into</param>
        private void AddToWieldedObjects(ref WorldObject item, WorldObject wielder, EquipMask currentWieldedLocation)
        {
            // Unset container fields
            item.PlacementPosition = null;
            item.ContainerId = null;
            // Set fields needed to be wielded.
            item.WielderId = (int)wielder.Guid.Full;
            item.CurrentWieldedLocation = currentWieldedLocation;

            if (wielder is Creature creature)
            {
                if (!creature.EquippedObjects.ContainsKey(item.Guid))
                {
                    creature.EquippedObjects.Add(item.Guid, item);

                    //Burden += item.Burden;
                }
            }
        }


        /// <summary>
        /// This method sets properties needed for items that will be child items.
        /// Items here are only items equipped in the hands.  This deals with the orientation
        /// and positioning for visual appearance of the child items held by the parent. Og II
        /// </summary>
        /// <param name="container">Who is the parent of this child item</param>
        /// <param name="item">The child item - we link them together</param>
        /// <param name="placement">Where is this on the parent - where is it equipped</param>
        /// <param name="placementId">out parameter - this deals with the orientation of the child item as it relates to parent model</param>
        /// <param name="childLocation">out parameter - this is another part of the orientation data for correct visual display</param>
        public void SetChild(Container container, WorldObject item, int placement, out int placementId, out int childLocation)
        {
            placementId = 0;
            childLocation = 0;
            // TODO:   I think there is a state missing - it is one of the edge cases.   I need to revist this.   Og II
            switch ((EquipMask)placement)
            {
                case EquipMask.MissileWeapon:
                    {
                        ////if (item.DefaultCombatStyle == MotionStance.BowAttack ||
                        ////    item.DefaultCombatStyle == MotionStance.CrossBowAttack ||
                        ////    item.DefaultCombatStyle == MotionStance.AtlatlCombat)
                        if (item.DefaultCombatStyle == CombatStyle.Atlatl ||
                            item.DefaultCombatStyle == CombatStyle.Bow ||
                            item.DefaultCombatStyle == CombatStyle.Crossbow)
                        {
                            childLocation = 2;
                            placementId = 3;
                        }
                        else
                        {
                            childLocation = 1;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Shield:
                    {
                        if (item.ItemType == ItemType.Armor)
                        {
                            childLocation = 3;
                            placementId = 6;
                        }
                        else
                        {
                            childLocation = 8;
                            placementId = 1;
                        }
                        break;
                    }
                case EquipMask.Held:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
                default:
                    {
                        childLocation = 1;
                        placementId = 1;
                        break;
                    }
            }
            if (item.CurrentWieldedLocation != null)
                container.Children.Add(new HeldItem(item.Guid.Full, childLocation, (EquipMask)item.CurrentWieldedLocation));
            item.ParentLocation = (ParentLocation)childLocation;
            item.Location = Location;
            item.Placement = (Placement)placementId;
        }








        /// <summary>
        /// Add New WorldObject to Inventory
        /// </summary>
        public void HandleAddNewWorldObjectToInventory(WorldObject wo)
        {
            // Get Next Avalibale Pack Location.
            // uint packid = GetCreatureInventoryFreePack();

            // default player until I get above code to work!
            uint packid = Guid.Full;

            if (packid != 0)
            {
                wo.ContainerId = (int)packid;
                AddToInventory(wo);
                Session.Network.EnqueueSend(new GameMessageCreateObject(wo));
                if (wo is Container container)
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, container));
            }
        }

        /// <summary>
        /// Call this to add any new World Objects to inventory
        /// </summary>
        public void HandleAddNewWorldObjectsToInventory(List<WorldObject> wolist)
        {
            foreach (WorldObject wo in wolist)
            {
                HandleAddNewWorldObjectToInventory(wo);
            }
        }

        /// <summary>
        /// This method is used to pick items off the world - out of 3D space and into our inventory or to a wielded slot.
        /// It checks the use case needed, sends the appropriate response messages.   In addition, it will move to objects
        /// that are out of range in the attemp to pick them up.   It will call update apperiance if needed and you have
        /// wielded an item from the ground. Og II
        /// </summary>
        /// <param name="container"></param>
        /// <param name="itemGuid"></param>
        /// <param name="placement"></param>
        /// <param name="iidPropertyId"></param>
        private void HandlePickupItem(Container container, ObjectGuid itemGuid, int placement, PropertyInstanceId iidPropertyId)
        {
            // Logical operations:
            // !! FIXME: How to handle repeat on condition?
            // while (!objectInRange)
            //   try Move to object
            // !! FIXME: How to handle conditional
            // Try acquire from landblock
            // if acquire successful:
            //   add to container
            ActionChain pickUpItemChain = new ActionChain();

            // Move to the object
            pickUpItemChain.AddChain(CreateMoveToChain(itemGuid, PickUpDistance));

            // Pick up the object
            // Start pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                var motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                    new GameMessageUpdatePosition(this),
                    new GameMessageUpdateMotion(Guid,
                        Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                        Sequences, motion));
            });
            // Wait for animation to progress
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
            var pickupAnimationLength = motionTable.GetAnimationLength(MotionCommand.Pickup);
            pickUpItemChain.AddDelaySeconds(pickupAnimationLength);

            // Ask landblock to transfer item
            // pickUpItemChain.AddAction(CurrentLandblock, () => CurrentLandblock.TransferItem(itemGuid, containerGuid));
            if (container.Guid.IsPlayer())
                CurrentLandblock.QueueItemTransfer(pickUpItemChain, itemGuid, container.Guid);
            else
                CurrentLandblock.ScheduleItemTransferInContainer(pickUpItemChain, itemGuid, (Container)GetInventoryItem(container.Guid));

            // Finish pickup animation
            pickUpItemChain.AddAction(this, () =>
            {
                // If success, the item is in our inventory:
                WorldObject item = GetInventoryItem(itemGuid);

                if (item.ContainerId != Guid.Full)
                {
                    //Burden += item.Burden ?? 0;

                    if (item.WeenieType == WeenieType.Coin)
                    {
                        UpdateCurrencyClientCalculations(WeenieType.Coin);
                    }
                }

                if (item is Container itemAsContainer)
                {
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, itemAsContainer));

                    foreach (var packItem in itemAsContainer.Inventory)
                    {
                        Session.Network.EnqueueSend(new GameMessageCreateObject(packItem.Value));
                        UpdateCurrencyClientCalculations(WeenieType.Coin);
                    }
                }

                // Update all our stuff if we succeeded
                if (item != null)
                {
                    item.SetPropertiesForContainer(placement);
                    // FIXME(ddevec): I'm not 100% sure which of these need to be broadcasts, and which are local sends...
                    var motion = new UniversalMotion(MotionStance.Standing);
                    if (iidPropertyId == PropertyInstanceId.Container)
                    {
                        Session.Network.EnqueueSend(
                            ////new GameMessagePrivateUpdatePropertyInt(Session.Player.Sequences, PropertyInt.EncumbranceVal, UpdateBurden()),
                            new GameMessageSound(Guid, Sound.PickUpItem, 1.0f),
                            new GameMessageUpdateInstanceId(itemGuid, container.Guid, iidPropertyId),
                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement));
                    }
                    else
                    {
                        AddToWieldedObjects(ref item, container, (EquipMask)placement);
                        Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                    new GameMessageObjDescEvent(this),
                                                    new GameMessageUpdateInstanceId(container.Guid, itemGuid, PropertyInstanceId.Wielder),
                                                    new GameEventWieldItem(Session, itemGuid.Full, placement));
                    }

                    CurrentLandblock.EnqueueBroadcast(
                        Location,
                        Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(
                            Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences,
                            motion),
                        new GameMessagePickupEvent(item));

                    if (iidPropertyId == PropertyInstanceId.Wielder)
                        CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessageObjDescEvent(this));

                    // TODO: Og II - check this later to see if it is still required.
                    Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                }
                // If we didn't succeed, just stand up and be ashamed of ourself
                else
                {
                    var motion = new UniversalMotion(MotionStance.Standing);

                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                        new GameMessageUpdateMotion(Guid,
                            Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                            Sequences, motion));
                    // CurrentLandblock.EnqueueBroadcast(self shame);
                }
            });
            // Set chain to run
            pickUpItemChain.EnqueueChain();
        }

        /// <summary>
        /// This code handle objects between players and other world objects
        /// </summary>
        public void HandleGiveObjectRequest(uint targetID, uint objectID, uint amount)
        {
            ////ObjectGuid target = new ObjectGuid(targetID);
            ////ObjectGuid item = new ObjectGuid(objectID);
            ////WorldObject targetObject = CurrentLandblock.GetObject(target) as WorldObject;
            ////WorldObject itemObject = GetInventoryItem(item);
            ////////WorldObject itemObject = CurrentLandblock.GetObject(item) as WorldObject;
            ////Session.Network.EnqueueSend(new GameMessagePutObjectInContainer(Session, (ObjectGuid)targetObject.Guid, itemObject, 0));
            ////SendUseDoneEvent();
        }

        /// <summary>
        /// Method is called in response to put item in container message.   This use case is we are just
        /// reorganizing our items.   It is either a in pack slot to slot move, or we could be going from one
        /// pack (container) to another. This method is called from an action chain.  Og II
        /// </summary>
        /// <param name="item">the item we are moving</param>
        /// <param name="container">what container are we going in</param>
        /// <param name="placement">what is my slot position within that container</param>
        private void HandleMove(ref WorldObject item, Container container, int placement)
        {
            TryRemoveFromInventory(item.Guid, out WorldObject _);

            item.ContainerId = (int)container.Guid.Full;
            item.PlacementPosition = placement;

            container.AddToInventory(item, placement);

            if (item.ContainerId != Guid.Full)
            {
                //Burden += item.Burden ?? 0;
                if (item.WeenieType == WeenieType.Coin)
                    UpdateCurrencyClientCalculations(WeenieType.Coin);
            }
            Session.Network.EnqueueSend(
                new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container,
                    container.Guid));

            Session.SaveSession();
        }

        /// <summary>
        /// This method is used to split a stack of any item that is stackable - arrows, tapers, pyreal etc.
        /// It creates the new object and sets the burden of the new item, adjusts the count and burden of the splitting
        /// item. Og II
        /// </summary>
        /// <param name="stackId">This is the guild of the item we are spliting</param>
        /// <param name="containerId">The guid of the container</param>
        /// <param name="place">Place is the slot in the container we are spliting into.   Range 0-MaxCapacity</param>
        /// <param name="amount">The amount of the stack we are spliting from that we are moving to a new stack.</param>
        /// <returns></returns>

        public void HandleActionStackableSplitToContainer(uint stackId, uint containerId, int place, ushort amount)
        {
            // TODO: add the complementary method to combine items Og II
            ActionChain splitItemsChain = new ActionChain();
            splitItemsChain.AddAction(this, () =>
            {
                Container container;
                if (containerId == Guid.Full)
                {
                    container = this;
                }
                else
                {
                    container = (Container)GetInventoryItem(new ObjectGuid(containerId));
                }

                if (container == null)
                {
                    log.InfoFormat("Asked to split stack {0} in container {1} - the container was not found",
                        stackId,
                        containerId);
                    return;
                }
                WorldObject stack = container.GetInventoryItem(new ObjectGuid(stackId));
                if (stack == null)
                {
                    log.InfoFormat("Asked to split stack {0} in container {1} - the stack item was not found",
                        stackId,
                        containerId);
                    return;
                }
                if (stack.Value == null || stack.StackSize < amount || stack.StackSize == 0)
                {
                    log.InfoFormat(
                        "Asked to split stack {0} in container {1} - with amount of {2} but there is not enough to split",
                        stackId, containerId, amount);
                    return;
                }

                // Ok we are in business
                throw new System.NotImplementedException();/*
                WorldObject newStack = WorldObjectFactory.CreateWorldObject(stack.NewAceObjectFromCopy()); // Fix suggested by Mogwai and Og II
                container.AddToInventory(newStack);

                ushort oldStackSize = (ushort)stack.StackSize;
                stack.StackSize = (ushort)(oldStackSize - amount);

                newStack.StackSize = amount;

                GameMessagePutObjectInContainer msgPutObjectInContainer =
                    new GameMessagePutObjectInContainer(Session, container.Guid, newStack, place);
                GameMessageSetStackSize msgAdjustOldStackSize = new GameMessageSetStackSize(stack.Sequences,
                    stack.Guid, (int)stack.StackSize, (int)stack.Value);
                GameMessageCreateObject msgNewStack = new GameMessageCreateObject(newStack);

                CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange,
                    msgPutObjectInContainer, msgAdjustOldStackSize, msgNewStack);

                if (stack.WeenieType == WeenieType.Coin)
                    UpdateCurrencyClientCalculations(WeenieType.Coin);*/
            });
            splitItemsChain.EnqueueChain();
        }

        /// <summary>
        /// This method is used to remove X number of items from a stack, including
        /// If amount to remove is greater or equal to the current stacksize, item will be removed.
        /// </summary>
        /// <param name="stackId">Guid.Full of the stacked item</param>
        /// <param name="containerId">Guid.Full of the container that contains the item</param>
        /// <param name="amount">Amount taken out of the stack</param>
        public void HandleActionRemoveItemFromInventory(uint stackId, uint containerId, ushort amount)
        {
            // FIXME: This method has been morphed into doing a few things we either need to rename it or
            // something.   This may or may not remove item from inventory.
            ActionChain removeItemsChain = new ActionChain();
            removeItemsChain.AddAction(this, () =>
            {
                Container container;
                if (containerId == Guid.Full)
                    container = this;
                else
                    container = (Container)GetInventoryItem(new ObjectGuid(containerId));

                if (container == null)
                {
                    log.InfoFormat("Asked to remove an item {0} in container {1} - the container was not found",
                        stackId,
                        containerId);
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
                    DestroyInventoryItem(item);
                    // Clean up the shard database.
                    // todo fix for EF
                    throw new NotImplementedException();
                    //DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);
                }
                //else
                    //Burden = (ushort)(Burden - (item.StackUnitEncumbrance * amount));
            });
            removeItemsChain.EnqueueChain();
        }

        public void HandleActionDropItem(ObjectGuid itemGuid)
        {
            var dropChain = new ActionChain();

            // Goody Goody -- lets build  drop chain
            // First start drop animation
            dropChain.AddAction(this, () =>
            {
                // check packs of item.
                WorldObject item;

                if (!TryRemoveFromInventory(itemGuid, out item))
                {
                    // check to see if this item is wielded
                    if (TryDequipObject(itemGuid, out item))
                    {
                        Session.Network.EnqueueSend(
                            new GameMessageSound(Guid, Sound.WieldObject, 1.0f),
                            new GameMessageObjDescEvent(this),
                            new GameMessageUpdateInstanceId(Guid, new ObjectGuid(0), PropertyInstanceId.Wielder));
                    }
                }

                item.SetPropertiesForWorld(this);

                UniversalMotion motion = new UniversalMotion(MotionStance.Standing);
                motion.MovementData.ForwardCommand = (uint)MotionCommand.Pickup;
                Session.Network.EnqueueSend(new GameMessageUpdateInstanceId(itemGuid, new ObjectGuid(0), PropertyInstanceId.Container));

                // Set drop motion
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);

                // Now wait for Drop Motion to finish -- use ActionChain
                ActionChain chain = new ActionChain();

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
                    Session.Network.EnqueueSend(
                        new GameMessageSound(Guid, Sound.DropItem, (float)1.0),
                        new GameMessagePutObjectIn3d(Session, this, itemGuid),
                        new GameMessageUpdateInstanceId(itemGuid, new ObjectGuid(0), PropertyInstanceId.Container));

                    // This is the sequence magic - adds back into 3d space seem to be treated like teleport.
                    Debug.Assert(item != null, "item != null");
                    item.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
                    item.Sequences.GetNextSequence(SequenceType.ObjectVector);

                    CurrentLandblock.AddWorldObject(item);

                    Session.Network.EnqueueSend(new GameMessageUpdateObject(item));
                });

                chain.EnqueueChain();
                // Removed SaveSession - this was causing items that were dropped to not be removed from inventory.
                // If this causes a problem with vendor, we need to fix vendor.  Og II
            });

            dropChain.EnqueueChain();
        }


        public void HandleActionWieldItem(Container container, uint itemId, int placement)
        {
            ActionChain wieldChain = new ActionChain();
            wieldChain.AddAction(this, () =>
            {
                ObjectGuid itemGuid = new ObjectGuid(itemId);
                WorldObject item = GetInventoryItem(itemGuid);
                WorldObject packItem;

                if (item != null)
                {
                    ObjectGuid containerGuid = ObjectGuid.Invalid;
                    var containers = Inventory.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
                    foreach (var subpack in containers)
                    {
                        if (((Container)subpack.Value).Inventory.TryGetValue(itemGuid, out packItem))
                        {
                            containerGuid = subpack.Value.Guid;
                            break;
                        }
                    }

                    Container pack;
                    if (item != null && containerGuid != ObjectGuid.Invalid)
                    {
                        pack = (Container)GetInventoryItem(containerGuid);

                        TryRemoveFromInventory(itemGuid, out WorldObject _);
                    }
                    else
                    {
                        if (item != null)
                            TryRemoveFromInventory(itemGuid, out WorldObject _);
                    }

                    AddToWieldedObjects(ref item, container, (EquipMask)placement);

                    if ((EquipMask)placement == EquipMask.MissileAmmo)
                    {
                        Session.Network.EnqueueSend(new GameEventWieldItem(Session, itemGuid.Full, placement),
                            new GameMessageSound(Guid, Sound.WieldObject, (float)1.0));
                    }
                    else
                    {
                        if (((EquipMask)placement & EquipMask.Selectable) != 0)
                        {
                            SetChild(container, item, placement, out var placementId, out var childLocation);


                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                            new GameMessageParentEvent(Session.Player, item, childLocation, placementId),
                                                            new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                            new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement));

                            if (CombatMode == CombatMode.NonCombat || CombatMode == CombatMode.Undef)
                                return;

                            switch ((EquipMask)placement)
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

                            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                                        new GameEventWieldItem(Session, itemGuid.Full, placement),
                                                        new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                                                        new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, new ObjectGuid(0)),
                                                        new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, container.Guid),
                                                        new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, placement),
                                                        new GameMessageObjDescEvent(container));
                        }
                    }
                }
                else
                {
                    HandlePickupItem(container, itemGuid, placement, PropertyInstanceId.Wielder);
                }
            });
            wieldChain.EnqueueChain();
        }

        /// <summary>
        /// This method is called in response to a put item in container message.  It is used when the item going
        /// into a container was wielded.   It sets the appropriate properties, sends out response messages
        /// and handles switching stances - for example if you have a bow wielded and are in bow combat stance,
        /// when you unwield the bow, this also sends the messages needed to go into unarmed combat mode. Og II
        /// </summary>
        private void HandleUnwieldItem(Container container, WorldObject item, int placement)
        {
            EquipMask? oldLocation = item.CurrentWieldedLocation;

            item.ContainerId = (int)container.Guid.Full;
            item.SetPropertiesForContainer(placement);

            TryDequipObject(item.Guid, out WorldObject _);

            if ((oldLocation & EquipMask.Selectable) != 0)
            {
                // We are coming from a hand shield slot.
                Children.Remove(Children.Find(s => s.Guid == item.Guid.Full));
            }

            // Set the container stuff
            item.ContainerId = (int)container.Guid.Full;
            item.PlacementPosition = placement;

            ActionChain inContainerChain = new ActionChain();
            inContainerChain.AddAction(this, () =>
            {
                if (container.Guid != Guid)
                {
                    container.AddToInventory(item, placement);
                    //Burden += item.Burden;
                }
                else
                    AddToInventory(item, placement);
            });
            inContainerChain.EnqueueChain();

            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange,
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                                            new GameMessagePublicUpdatePropertyInt(Session.Player.Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, 0),
                                            new GameMessageUpdateInstanceId(Session.Player.Sequences, item.Guid, PropertyInstanceId.Container, container.Guid),
                                            new GameMessagePickupEvent(item),
                                            new GameMessageSound(Guid, Sound.UnwieldObject, (float)1.0),
                                            new GameMessagePutObjectInContainer(Session, container.Guid, item, placement),
                                            new GameMessageObjDescEvent(this));

            if ((oldLocation != EquipMask.MissileWeapon && oldLocation != EquipMask.Held && oldLocation != EquipMask.MeleeWeapon) || ((CombatMode & CombatMode.CombatCombat) == 0))
                return;
            HandleSwitchToPeaceMode(CombatMode);
            HandleSwitchToMeleeCombatMode(CombatMode);
        }


        public void HandleActionUse(ObjectGuid usedItemId)
        {
            new ActionChain(this, () =>
            {
                WorldObject iwo = GetInventoryItem(usedItemId);
                if (iwo != null)
                {
                    iwo.OnUse(Session);
                }
                else
                {
                    if (CurrentLandblock != null)
                    {
                        // Just forward our action to the appropriate user...
                        WorldObject wo = CurrentLandblock.GetObject(usedItemId);
                        if (wo != null)
                        {
                            wo.ActOnUse(Guid);
                        }
                    }
                }
            }).EnqueueChain();
        }

        public void HandleActionUseOnTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            ActionChain chain = new ActionChain(this, () =>
            {
                WorldObject invSource = GetInventoryItem(sourceObjectId);
                WorldObject invTarget = GetInventoryItem(targetObjectId);

                if (invTarget != null)
                {
                    // inventory on inventory, we can do this now
                    RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
                }
                else if (invSource.WeenieType == WeenieType.Key)
                {
                    WorldObject theTarget = CurrentLandblock.GetObject(targetObjectId);
                    Key key = invSource as Key;
                    key.HandleActionUseOnTarget(this, theTarget);
                }
                else if (targetObjectId == Guid)
                {
                    // using something on ourselves
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
                }
                else
                {
                    WorldObject theTarget = CurrentLandblock.GetObject(targetObjectId);
                    RecipeManager.UseObjectOnTarget(this, invSource, theTarget);
                }
            });
            chain.EnqueueChain();
        }


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
                WorldObject iwo = GetInventoryItem(itemGuid);
                if (iwo == null)
                {
                    return;
                }

                //if (iwo.Inscribable && iwo.ScribeName != "prewritten")
                //{
                //    if (iwo.ScribeName != null && iwo.ScribeName != this.Name)
                //    {
                //        ChatPacket.SendServerMessage(Session,
                //            "Only the original scribe may alter this without the use of an uninscription stone.",
                //            ChatMessageType.Broadcast);
                //    }
                //    else
                //    {
                //        if (inscriptionText != "")
                //        {
                //            iwo.Inscription = inscriptionText;
                //            iwo.ScribeName = this.Name;
                //            iwo.ScribeAccount = Session.Account;
                //            Session.Network.EnqueueSend(new GameEventInscriptionResponse(Session, iwo.Guid.Full,
                //                iwo.Inscription, iwo.ScribeName, iwo.ScribeAccount));
                //        }
                //        else
                //        {
                //            iwo.Inscription = null;
                //            iwo.ScribeName = null;
                //            iwo.ScribeAccount = null;
                //        }
                //    }
                //}
                //else
                //{
                //    // Send some cool you cannot inscribe that item message.   Not sure how that was handled live,
                //    // I could not find a pcap of a failed inscription. Og II
                //    ChatPacket.SendServerMessage(Session, "Target item cannot be inscribed.", ChatMessageType.System);
                //}
            }).EnqueueChain();
        }
    }
}
