using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

using ACE.Server.Physics.Common;

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
        /// The link to this player's Object Maintenance
        /// This is the system from client physics that tracks known objects, visible objects,
        /// and objects that have been occluded for less than 25s that are pending destruction
        /// </summary>
        public ObjectMaint ObjMaint { get => PhysicsObj.ObjMaint; }

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
        /// Returns the list of WorldObjects this Player this player currently knows about
        /// </summary>
        public List<WorldObject> GetKnownObjects()
        {
            return ObjMaint.ObjectTable.Values.Select(o => o.WeenieObj.WorldObject).ToList();
        }

        /// <summary>
        /// Sends a network message to player for CreateObject, if applicable
        /// </summary>
        public void TrackObject(WorldObject worldObject)
        {
            //Console.WriteLine($"TrackObject({worldObject.Name})");

            if (worldObject == null || worldObject.Guid == Guid)
                return;

            // If Visibility is true, do not send object to client, object is meant for server side only, unless Adminvision is true.
            if (worldObject.Visibility && !Adminvision)
                return;

            //Console.WriteLine($"TrackObject({worldObject.Name})");
            Session.Network.EnqueueSend(new GameMessageCreateObject(worldObject));

            // add creature equipped objects / wielded items
            if (worldObject is Creature creature)
                TrackEquippedObjects(creature);
        }

        /// <summary>
        /// Adds/updates the tracking list for creature wielded items
        /// </summary>
        public void TrackEquippedObjects(Creature creature, bool remove = false)
        {
            foreach (var wieldedItem in creature.EquippedObjects.Values)
                TrackEquippedObject(creature, wieldedItem, remove);
        }

        public void TrackEquippedObject(Creature creature, WorldObject wieldedItem, bool remove = false)
        {
            //Console.WriteLine($"TrackEquippedObject({wieldedItem.Name})");

            var selectable = (wieldedItem.ValidLocations.Value & EquipMask.Selectable) != 0;
            var missileCombat = creature.CombatMode == CombatMode.Missile && (wieldedItem.ValidLocations.Value & EquipMask.MissileAmmo) != 0;

            if (!selectable && !missileCombat)
                return;

            if (creature.Location == null || creature.Placement == null || creature.ParentLocation == null)
                creature.SetChild(wieldedItem, (int)wieldedItem.CurrentWieldedLocation, out var placementId, out var parentLocation);

            if (!remove)
                Session.Network.EnqueueSend(new GameMessageCreateObject(wieldedItem));
            else
                Session.Network.EnqueueSend(new GameMessageDeleteObject(wieldedItem));
        }

        public bool AddTrackedObject(WorldObject worldObject)
        {
            // does this work for equipped objects?
            if (ObjMaint.ObjectTable.Values.Contains(worldObject.PhysicsObj))
                return false;

            ObjMaint.AddObject(worldObject.PhysicsObj);
            ObjMaint.AddVisibleObject(worldObject.PhysicsObj);

            TrackObject(worldObject);
            return true;
        }

        /// <summary>
        /// This will return true of the object was being tracked and has successfully been removed.
        /// </summary>
        public bool RemoveTrackedObject(WorldObject worldObject, bool remove)
        {
            //Console.WriteLine($"RemoveTrackedObject({remove})");

            ObjMaint.RemoveObject(worldObject.PhysicsObj);

            if (remove)
            {
                Session.Network.EnqueueSend(new GameMessageDeleteObject(worldObject));
                var creature = worldObject as Creature;
                if (creature != null)
                    TrackEquippedObjects(creature, true);
            }
            return true;
        }
    }
}
