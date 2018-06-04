using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// This will be false when in portal space
        /// </summary>
        public bool InWorld { get; set; }

        /// <summary>
        /// Different than InWorld which is false when in portal space
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// ObjectId of the currently selected Target (only players and creatures)
        /// </summary>
        private ObjectGuid selectedTarget = ObjectGuid.Invalid;

        /// <summary>
        /// Temp tracked Objects of vendors / trade / containers.. needed for id / maybe more.
        /// </summary>
        private readonly Dictionary<ObjectGuid, WorldObject> interactiveWorldObjects = new Dictionary<ObjectGuid, WorldObject>();

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
        /// FIXME(ddevec): This is the only object that need be locked in the player under the new model.
        ///   It must be locked because of how we handle object updates -- We can clean this up in the future
        /// </summary>
        private readonly Dictionary<ObjectGuid, double> clientObjectList = new Dictionary<ObjectGuid, double>();


        /// <summary>
        /// Tracks Interacive world object you are have interacted with recently.  this should be
        /// called from the context of an action chain being executed by the landblock loop.
        /// </summary>
        public void TrackInteractiveObjects(List<WorldObject> worldObjects)
        {
            // todo: figure out a way to expire objects.. objects clearly not in range of interaction /etc
            foreach (WorldObject wo in worldObjects)
            {
                if (interactiveWorldObjects.ContainsKey(wo.Guid))
                    interactiveWorldObjects[wo.Guid] = wo;
                else
                    interactiveWorldObjects.Add(wo.Guid, wo);
            }
        }


        /// <summary>
        ///  Gets a list of Tracked Objects.
        /// </summary>
        public List<ObjectGuid> GetTrackedObjectGuids()
        {
            lock (clientObjectList)
                return clientObjectList.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known creatures
        /// </summary>
        private List<ObjectGuid> GetKnownCreatures()
        {
            lock (clientObjectList)
            {
                throw new NotImplementedException(); // We can't use the GUID to see if this is a creature, we need another way
                //return clientObjectList.Select(x => x.Key).Where(o => o.IsCreature()).ToList();
            }
        }

        /// <summary>
        /// returns a list of the ObjectGuids of all known objects
        /// </summary>
        public List<ObjectGuid> GetKnownObjects()
        {
            lock (clientObjectList)
                return clientObjectList.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// forces either an update or a create object to be sent to the client
        /// </summary>
        public void TrackObject(WorldObject worldObject, bool update = false)
        {
            bool sendUpdate;

            if (worldObject.Guid == Guid)
                return;

            // If Visibility is true, do not send object to client, object is meant for server side only, unless Adminvision is true.
            if ((worldObject.Visibility ?? false) && !Adminvision)
                return;

            lock (clientObjectList)
            {
                sendUpdate = clientObjectList.ContainsKey(worldObject.Guid);

                if (!sendUpdate)
                    clientObjectList.Add(worldObject.Guid, WorldManager.PortalYearTicks);
                else
                    clientObjectList[worldObject.Guid] = WorldManager.PortalYearTicks;
            }

            // TODO: Better handling of sending updates to client. The below line is causing much more problems than it is solving until we get proper movement.
            // Add this or something else back in when we handle movement better, until then, just send the create object once and move on.
            //if (!sendUpdate)
            //Console.WriteLine($"Telling {Name} about {worldObject.Name} - {worldObject.Guid.Full:X}");
            Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));

            // add creature equipped objects / wielded items
            if (worldObject is Creature)
                TrackEquippedObjects(worldObject as Creature);
        }

        /// <summary>
        /// Adds/updates the tracking list for creature wielded items
        /// </summary>
        public void TrackEquippedObjects(Creature creature)
        {
            var addList = new List<WorldObject>();

            foreach (var wieldedItem in creature.EquippedObjects.Values)
            {
                var selectable = (wieldedItem.ValidLocations.Value & EquipMask.Selectable) != 0;
                var missileCombat = creature.CombatMode == CombatMode.Missile && (wieldedItem.ValidLocations.Value & EquipMask.MissileAmmo) != 0;

                if (!selectable && !missileCombat)
                    continue;

                if (creature.Location == null || creature.Placement == null || creature.ParentLocation == null)
                    creature.SetChild(wieldedItem, (int)wieldedItem.CurrentWieldedLocation, out var placementId, out var parentLocation);

                lock (clientObjectList)
                {
                    var sendUpdate = clientObjectList.ContainsKey(wieldedItem.Guid);

                    if (!sendUpdate)
                        clientObjectList.Add(wieldedItem.Guid, WorldManager.PortalYearTicks);
                    else
                        clientObjectList[wieldedItem.Guid] = WorldManager.PortalYearTicks;
                }
                addList.Add(wieldedItem);
            }

            foreach (var item in addList)
            {
                //Console.WriteLine($"Telling {Name} about {item.Name} - {item.Guid.Full:X}");
                Session.Network.EnqueueSend(new GameMessageCreateObject(item));
            }
        }

        /// <summary>
        /// This will return true of the object was being tracked and has successfully been removed.
        /// </summary>
        /// <returns></returns>
        public bool StopTrackingObject(WorldObject worldObject, bool remove)
        {
            bool removed;

            lock (clientObjectList)
                removed = clientObjectList.Remove(worldObject.Guid);

            // Don't remove it if it went into our inventory...
            if (removed && remove)
                Session.Network.EnqueueSend(new GameMessageDeleteObject(worldObject));

            return removed;
        }
    }
}
