using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Common;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// ObjectId of the currently selected Target (only players and creatures)
        /// </summary>
        private WorldObjectInfo selectedTarget { get; set; }

        /// <summary>
        /// This dictionary is used to keep track of the last use of any item that implemented shared cooldown.
        /// It is session specific.   I think (could be wrong) cooldowns reset if you logged out and back in.
        /// This is a different mechanic than quest repeat timers and rare item use timers.
        /// example - contacts have a shared cooldown key value 100 so each time a player uses an item that has
        /// a shared cooldown we just add to the dictionary 100, datetime.now()   The check becomes trivial at that
        /// point if on a subsequent use, now() minus the last use value from the dictionary
        /// is greater than or equal to the cooldown, we can do the use - if not you must wait message.   Og II
        /// </summary>
        public Dictionary<int, DateTime> LastUseTracker { get; set; }

        /// <summary>
        /// The link to this player's Object Maintenance
        /// This is the system from client physics that tracks known objects, visible objects,
        /// and objects that have been occluded for less than 25s that are pending destruction
        /// </summary>
        public ObjectMaint ObjMaint => PhysicsObj.ObjMaint;

        /// <summary>
        /// Returns the list of WorldObjects this Player this player currently knows about
        /// </summary>
        public List<WorldObject> GetKnownObjects()
        {
            return ObjMaint.GetKnownObjectsValuesWhere(wo => wo != null).Select(o => o.WeenieObj.WorldObject).ToList();
        }

        /// <summary>
        /// Sends a network message to player for CreateObject, if applicable
        /// </summary>
        public void TrackObject(WorldObject worldObject, bool delay = false)
        {
            //Console.WriteLine($"TrackObject({worldObject.Name}, {delay})");

            if (worldObject == null || worldObject.Guid == Guid)
                return;

            // If Visibility is true, do not send object to client, object is meant for server side only, unless Adminvision is true.
            if (worldObject.Visibility && !Adminvision)
                return;

            Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject, Adminvision, Adminvision));

            //Console.WriteLine($"Player {Name} - TrackObject({worldObject.Name})");

            // add creature equipped objects / wielded items
            if (worldObject is Creature creature)
            {
                foreach (var wieldedItem in creature.EquippedObjects.Values)
                    if (IsInChildLocation(wieldedItem))
                        TrackEquippedObject(creature, wieldedItem);

                if (creature.IsMoving)
                    creature.BroadcastMoveTo(this);
            }
        }

        public bool AddTrackedObject(WorldObject worldObject)
        {
            // does this work for equipped objects?
            if (ObjMaint.KnownObjectsContainsValue(worldObject.PhysicsObj))
            {
                //Console.WriteLine($"Player {Name} - AddTrackedObject({worldObject.Name}) skipped, already tracked");
                return false;
            }

            ObjMaint.AddKnownObject(worldObject.PhysicsObj);
            ObjMaint.AddVisibleObject(worldObject.PhysicsObj);

            TrackObject(worldObject);
            return true;
        }

        public void RemoveTrackedObject(WorldObject wo, bool fromPickup)
        {
            //log.Info($"{Name}.RemoveTrackedObject({wo.Name} ({wo.Guid}), {fromPickup})");

            if (fromPickup)
            {
                Session.Network.EnqueueSend(new GameMessagePickupEvent(wo));

                if (wo.WielderId != null && (wo.ParentLocation ?? 0) != 0)
                    Session.Network.EnqueueSend(new GameMessageParentEvent(wo.Wielder, wo));
            }
            else
                Session.Network.EnqueueSend(new GameMessageDeleteObject(wo));

            if (wo is Creature creature)
            {
                foreach (var wieldedItem in creature.EquippedObjects.Values)
                    RemoveTrackedEquippedObject(creature, wieldedItem);
            }
        }


        public void TrackEquippedObject(Creature wielder, WorldObject wieldedItem)
        {
            //Console.WriteLine($"Player {Name} - TrackEquippedObject({wieldedItem.Name}) on Wielder {wielder.Name}");

            // We make sure the item is actually wielded and selectable
            if (((wieldedItem.CurrentWieldedLocation ?? 0) & EquipMask.SelectablePlusAmmo) == 0)
                return;

            // The wielder already knows about this object
            if (wielder == this)
                return;

            Session.Network.EnqueueSend(new GameMessageCreateObject(wieldedItem));
        }

        public void RemoveTrackedEquippedObject(Creature formerWielder, WorldObject worldObject)
        {
            //Console.WriteLine($"Player {Name} - RemoveTrackedEquippedObject({worldObject.Name}) on Former Wielder {formerWielder.Name}");

            // We don't need to remove objects that couldn't have been tracked in the first place
            if (((worldObject.ValidLocations ?? 0) & EquipMask.SelectablePlusAmmo) == 0)
                return;

            // The former wielder already knows about this object was removed
            if (formerWielder == this)
                return;

            // intended for cloaked objects, as DO's should not be sent for them
            // but this breaks regular players, as the state of worldObject has already changed, and is never in a ChildLocation
            //if (!IsInChildLocation(worldObject))
                //return;

            // todo: Until we can fix the tracking system better, sending the PickupEvent like retail causes weapon dissapearing bugs on relog
            //Session.Network.EnqueueSend(new GameMessagePickupEvent(worldObject));

            Session.Network.EnqueueSend(new GameMessageDeleteObject(worldObject));
        }

        public void DeCloak()
        {
            if (CloakStatus == CloakStatus.Off)
                return;

            var actionChain = new ActionChain();

            actionChain.AddAction(this, () =>
            {
                EnqueueBroadcast(false, new GameMessageDeleteObject(this));
            });
            actionChain.AddAction(this, () =>
            {
                NoDraw = true;
                EnqueueBroadcastPhysicsState();
                Visibility = false;
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(this, () =>
            {
                EnqueueBroadcast(false, new GameMessageCreateObject(this));
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(this, () =>
            {
                Cloaked = false;
                Ethereal = false;
                NoDraw = false;
                EnqueueBroadcastPhysicsState();
            });

            actionChain.EnqueueChain();
        }

        public void HandleCloak()
        {
            if (CloakStatus == CloakStatus.On)
                return;

            var actionChain = new ActionChain();

            actionChain.AddAction(this, () =>
            {
                Cloaked = true;
                Ethereal = true;
                NoDraw = true;
                EnqueueBroadcastPhysicsState();
            });
            actionChain.AddAction(this, () =>
            {
                EnqueueBroadcast(false, new GameMessageDeleteObject(this));
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(this, () =>
            {
                Visibility = true;
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(this, () =>
            {
                EnqueueBroadcast(false, new GameMessageCreateObject(this, true, true));
            });

            actionChain.EnqueueChain();
        }
    }
}
