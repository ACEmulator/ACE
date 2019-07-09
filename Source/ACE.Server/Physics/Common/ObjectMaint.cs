using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Common
{
    /// <summary>
    /// Player visibility tracking
    /// Keeps track of which objects are currently known / visible to a player
    /// </summary>
    public class ObjectMaint
    {
        /// <summary>
        /// The client automatically removes known objects
        /// if they remain outside visibility for this amount of time
        /// </summary>
        public static readonly float DestructionTime = 25.0f;

        /// <summary>
        /// The owner of this ObjectMaint instance
        /// This is who we are tracking object visibility for, ie. a Player
        /// </summary>
        public PhysicsObj PhysicsObj { get; set; }

        /// <summary>
        /// This list of objects that are known to a player
        /// </summary>
        /// <remarks>
        /// 
        /// - When an object enters PVS / VisibleCell range of a player,
        /// it is added to this list, and the VisibleObject list
        /// 
        /// - When an object exits PVS / VisibleCell range of a player,
        /// it is removed from the VisibleObject list, and added to the destruction queue.
        /// if it remains outside PVS for DestructionTime, the client automatically culls the object,
        /// and it is removed from this list of known objects.
        ///
        /// - This is only maintained for players.
        /// </remarks>
        public Dictionary<uint, PhysicsObj> KnownObjects { get; set; }

        /// <summary>
        /// This list of objects that are currently within PVS / VisibleCell range
        /// only maintained for players
        /// </summary>
        public Dictionary<uint, PhysicsObj> VisibleObjects { get; set; }

        /// <summary>
        /// Objects that were previously visible to the client,
        /// but have been outside the PVS for less than 25 seconds
        /// only maintained for players
        /// </summary>
        public Dictionary<PhysicsObj, double> DestructionQueue { get; set; }

        /// <summary>
        /// A list of players that currently know about this object
        /// This is maintained for all server-spawned WorldObjects, and is used for broadcasting
        /// </summary>
        public Dictionary<uint, PhysicsObj> KnownPlayers { get; set; }

        /// <summary>
        /// For monster and CombatPet FindNextTarget
        /// - for monsters, contains players and combat pets
        /// - for combat pets, contains monsters
        /// </summary>
        public Dictionary<uint, PhysicsObj> VisibleTargets { get; set; }

        /// <summary>
        /// Custom lookup table of PhysicsObjs for the server
        /// </summary>
        public static ConcurrentDictionary<uint, PhysicsObj> ServerObjects { get; set; } = new ConcurrentDictionary<uint, PhysicsObj>();

        // Client structures -
        // When client unloads a cell/landblock, but still knows about objects in those cells?
        //public Dictionary<uint, LostCell> LostCellTable;
        //public List<PhysicsObj> NullObjectTable;
        //public Dictionary<uint, WeenieObject> WeenieObjectTable;
        //public List<WeenieObject> NullWeenieObjectTable;

        public ObjectMaint() { }

        public ObjectMaint(PhysicsObj obj)
        {
            PhysicsObj = obj;

            KnownObjects = new Dictionary<uint, PhysicsObj>();
            VisibleObjects = new Dictionary<uint, PhysicsObj>();
            DestructionQueue = new Dictionary<PhysicsObj, double>();

            KnownPlayers = new Dictionary<uint, PhysicsObj>();
            VisibleTargets = new Dictionary<uint, PhysicsObj>();
        }

        /// <summary>
        /// Adds an object to the list of known objects
        /// only maintained for players
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        public bool AddKnownObject(PhysicsObj obj)
        {
            if (KnownObjects.ContainsKey(obj.ID))
                return false;

            KnownObjects.Add(obj.ID, obj);

            // maintain KnownPlayers for both parties
            if (obj.IsPlayer) AddKnownPlayer(obj);

            obj.ObjMaint.AddKnownPlayer(PhysicsObj);

            return true;
        }

        /// <summary>
        /// Adds a list of objects to the known objects list
        /// only maintained for players
        /// </summary>
        /// <param name="objs">A list of currently visible objects</param>
        /// <returns>The list of visible objects that were previously unknown</returns>
        public List<PhysicsObj> AddKnownObjects(List<PhysicsObj> objs)
        {
            var newObjs = new List<PhysicsObj>();

            foreach (var obj in objs)
                if (AddKnownObject(obj)) newObjs.Add(obj);

            return newObjs;
        }

        public void RemoveKnownObject(PhysicsObj obj, bool inversePlayer = true)
        {
            KnownObjects.Remove(obj.ID);

            if (PhysicsObj.IsPlayer && inversePlayer)
                obj.ObjMaint.RemoveKnownPlayer(PhysicsObj);
        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a cell
        /// </summary>
        public List<PhysicsObj> GetVisibleObjects(ObjCell cell, VisibleObjectType type = VisibleObjectType.All)
        {
            if (PhysicsObj.CurLandblock == null || cell == null) return new List<PhysicsObj>();

            // use PVS / VisibleCells for EnvCells not seen outside
            // (mostly dungeons, also some large indoor areas ie. caves)
            if (cell is EnvCell envCell && !envCell.SeenOutside)
                return GetVisibleObjects(envCell, type);

            // use current landblock + adjacents for outdoors,
            // and envcells seen from outside (all buildings)
            var visibleObjs = new List<PhysicsObj>(PhysicsObj.CurLandblock.ServerObjects);

            var adjacents = PhysicsObj.CurLandblock.get_adjacents();
            if (adjacents != null)
            {
                foreach (var adjacent in adjacents)
                    visibleObjs.AddRange(adjacent.ServerObjects);
            }

            return ApplyFilter(visibleObjs, type).Where(i => i.ID != PhysicsObj.ID && (!(i.CurCell is EnvCell indoors) || indoors.SeenOutside)).ToList();
        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a dungeon cell
        /// </summary>
        public List<PhysicsObj> GetVisibleObjects(EnvCell cell, VisibleObjectType type)
        {
            var visibleObjs = new List<PhysicsObj>();

            // add objects from current cell
            visibleObjs.AddRange(cell.ObjectList);

            // add objects from visible cells
            foreach (var envCell in cell.VisibleCells.Values)
            {
                if (envCell != null)
                    visibleObjs.AddRange(envCell.ObjectList);
            }

            return ApplyFilter(visibleObjs, type).Where(i => !i.DatObject && i.ID != PhysicsObj.ID).Distinct().ToList();
        }

        public enum VisibleObjectType
        {
            All,
            Players,
            AttackTargets
        };

        public IEnumerable<PhysicsObj> ApplyFilter(List<PhysicsObj> objs, VisibleObjectType type)
        {
            IEnumerable<PhysicsObj> results = objs;

            if (type == VisibleObjectType.Players)
            {
                results = objs.Where(i => i.IsPlayer);
            }
            else if (type == VisibleObjectType.AttackTargets)
            {
                if (PhysicsObj.WeenieObj.IsCombatPet)
                    results = objs.Where(i => i.WeenieObj.IsMonster);
                else
                    results = objs.Where(i => i.IsPlayer || i.WeenieObj.IsCombatPet);
            }
            return results;
        }

        public static bool InitialClamp = true;

        public static float InitialClamp_Dist = 112.5f;
        public static float InitialClamp_DistSq = InitialClamp_Dist * InitialClamp_Dist;

        /// <summary>
        /// Adds an object to the list of visible objects
        /// only maintained for players
        /// </summary>
        /// <returns>TRUE if object was previously not visible, and added to the visible list</returns>
        public bool AddVisibleObject(PhysicsObj obj)
        {
            if (VisibleObjects.ContainsKey(obj.ID))
                return false;

            if (InitialClamp && !KnownObjects.ContainsKey(obj.ID))
            {
                var distSq = PhysicsObj.Position.Distance2DSquared(obj.Position);

                if (distSq > InitialClamp_DistSq)
                    return false;
            }

            //Console.WriteLine($"{PhysicsObj.Name}.AddVisibleObject({obj.Name})");
            VisibleObjects.Add(obj.ID, obj);

            if (obj.WeenieObj.IsMonster)
                obj.ObjMaint.AddVisibleTarget(PhysicsObj, false);

            return true;
        }

        /// <summary>
        /// Add a list of visible objects - maintains both known and visible objects
        /// only maintained for players
        /// </summary>
        public List<PhysicsObj> AddVisibleObjects(List<PhysicsObj> objs)
        {
            var visibleAdded = new List<PhysicsObj>();

            foreach (var obj in objs)
            {
                if (AddVisibleObject(obj))
                    visibleAdded.Add(obj);
            }
            RemoveObjectsToBeDestroyed(objs);

            return AddKnownObjects(visibleAdded);
        }

        /// <summary>
        /// Removes an object from the visible objects list
        /// only run for players, and also removes the player from
        /// the object's visible targets list
        /// </summary>
        public bool RemoveVisibleObject(PhysicsObj obj, bool inverseTarget = true)
        {
            var removed = VisibleObjects.Remove(obj.ID);

            if (inverseTarget)
                obj.ObjMaint.RemoveVisibleTarget(PhysicsObj);

            return removed;
        }

        /// <summary>
        /// Adds an object to the destruction queue
        /// Called when an object exits the PVS range
        /// only maintained for players
        /// </summary>
        public bool AddObjectToBeDestroyed(PhysicsObj obj)
        {
            RemoveVisibleObject(obj);

            if (DestructionQueue.ContainsKey(obj))
                return false;

            DestructionQueue.Add(obj, PhysicsTimer.CurrentTime + DestructionTime);

            return true;
        }

        /// <summary>
        /// Adds a list of objects to the destruction queue
        /// only maintained for players
        /// </summary>
        public List<PhysicsObj> AddObjectsToBeDestroyed(List<PhysicsObj> objs)
        {
            var queued = new List<PhysicsObj>();
            foreach (var obj in objs)
            {
                if (AddObjectToBeDestroyed(obj))
                    queued.Add(obj);
            }
            return queued;
        }

        /// <summary>
        /// Removes an object from the destruction queue
        /// if it has been invisible for less than 25s
        /// this is only used for players
        /// </summary>
        public bool RemoveObjectToBeDestroyed(PhysicsObj obj)
        {
            double time = -1;
            DestructionQueue.TryGetValue(obj, out time);
            if (time != -1 && time > PhysicsTimer.CurrentTime)
            {
                DestructionQueue.Remove(obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes objects from the destruction queue
        /// when they re-enter visibility within 25s
        /// this is only used for players
        /// </summary>
        public void RemoveObjectsToBeDestroyed(List<PhysicsObj> objs)
        {
            foreach (var obj in objs)
                RemoveObjectToBeDestroyed(obj);
        }

        /// <summary>
        /// Removes any objects that have been in the destruction queue
        /// for more than 25s
        /// this is only used for players
        /// </summary>
        public List<PhysicsObj> DestroyObjects()
        {
            // find the list of objects that have been in the destruction queue > 25s
            var expiredObjs = DestructionQueue.Where(kvp => kvp.Value <= PhysicsTimer.CurrentTime).ToDictionary(kvp => kvp.Key, kvp => kvp.Value).Keys.ToList();

            // remove expired objects from all lists
            foreach (var expiredObj in expiredObjs)
                RemoveObject(expiredObj);

            return expiredObjs;
        }

        /// <summary>
        /// Removes an object after it has expired from the destruction queue,
        /// or it has been destroyed
        /// </summary>
        public void RemoveObject(PhysicsObj obj, bool inverse = true)
        {
            if (obj == null) return;

            RemoveKnownObject(obj, inverse);
            RemoveVisibleObject(obj, inverse);
            DestructionQueue.Remove(obj);

            RemoveKnownPlayer(obj);
            RemoveVisibleTarget(obj);
        }

        /// <summary>
        /// Adds a player who currently knows about this object
        /// - this is maintained for all server-spawned objects
        /// 
        /// when an object broadcasts, it sends network messages
        /// to all players who currently know about this object
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        public bool AddKnownPlayer(PhysicsObj obj)
        {
            // only tracking players who know about this object
            if (!obj.IsPlayer)
            {
                Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddKnownPlayer({obj.Name}): tried to add a non-player");
                return false;
            }
            if (PhysicsObj.DatObject)
            {
                Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddKnownPlayer({obj.Name}): tried to add player for dat object");
                return false;
            }

            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.AddKnownPlayer({obj.Name})");

            // TryAdd for existing keys still modifies collection?
            if (KnownPlayers.ContainsKey(obj.ID))
                return false;

            KnownPlayers.Add(obj.ID, obj);
            return true;
        }

        /// <summary>
        /// Adds a list of players known to this object
        /// </summary>
        public List<PhysicsObj> AddKnownPlayers(List<PhysicsObj> objs)
        {
            var newObjs = new List<PhysicsObj>();

            foreach (var obj in objs)
                if (AddKnownPlayer(obj)) newObjs.Add(obj);

            return newObjs;
        }

        /// <summary>
        /// Removes a known player for this object
        /// </summary>
        public bool RemoveKnownPlayer(PhysicsObj obj)
        {
            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.RemoveKnownPlayer({obj.Name})");

            return KnownPlayers.Remove(obj.ID);
        }

        /// <summary>
        /// For monster and CombatPet FindNextTarget
        /// - for monsters, contains players and combat pets
        /// - for combat pets, contains monsters
        /// </summary>
        public bool AddVisibleTarget(PhysicsObj obj, bool clamp = true)
        {
            if (PhysicsObj.WeenieObj.IsCombatPet)
            {
                // only tracking monsters
                if (!obj.WeenieObj.IsMonster)
                {
                    Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddVisibleTarget({obj.Name}): tried to add a non-monster");
                    return false;
                }
            }
            else
            {
                // only tracking players and combat pets
                if (!obj.IsPlayer && !obj.WeenieObj.IsCombatPet)
                {
                    Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddVisibleTarget({obj.Name}): tried to add a non-player / non-combat pet");
                    return false;
                }
            }
            if (PhysicsObj.DatObject)
            {
                Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddVisibleTarget({obj.Name}): tried to add player for dat object");
                return false;
            }

            if (clamp && InitialClamp && obj.IsPlayer && !obj.ObjMaint.KnownObjects.ContainsKey(obj.ID))
            {
                var distSq = PhysicsObj.Position.Distance2DSquared(obj.Position);

                if (distSq > InitialClamp_DistSq)
                    return false;
            }

            // TryAdd for existing keys still modifies collection?
            if (VisibleTargets.ContainsKey(obj.ID))
                return false;

            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.AddVisibleTarget({obj.Name})");

            VisibleTargets.Add(obj.ID, obj);

            // maintain inverse for monsters / combat pets
            if (!obj.IsPlayer)
                obj.ObjMaint.AddVisibleTarget(PhysicsObj);

            return true;
        }

        /// <summary>
        /// For monster and CombatPet FindNextTarget
        /// - for monsters, contains players and combat pets
        /// - for combat pets, contains monsters
        /// </summary>
        public List<PhysicsObj> AddVisibleTargets(List<PhysicsObj> objs)
        {
            var visibleAdded = new List<PhysicsObj>();

            foreach (var obj in objs)
            {
                if (AddVisibleTarget(obj))
                    visibleAdded.Add(obj);
            }
            return AddKnownPlayers(visibleAdded.Where(o => o.IsPlayer).ToList());
        }

        public bool RemoveVisibleTarget(PhysicsObj obj)
        {
            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.RemoveVisibleTarget({obj.Name})");
            return VisibleTargets.Remove(obj.ID);
        }

        public List<PhysicsObj> GetVisibleObjectsDist(ObjCell cell, VisibleObjectType type)
        {
            var visibleObjs = GetVisibleObjects(cell, type);

            var dist = new List<PhysicsObj>();
            foreach (var obj in visibleObjs)
            {
                var distSq = PhysicsObj.Position.Distance2DSquared(obj.Position);

                if (distSq <= InitialClamp_DistSq)
                    dist.Add(obj);
            }
            return dist;
        }

        /// <summary>
        /// Adds a PhysicsObj to the static list of server-wide objects
        /// </summary>
        public static void AddServerObject(PhysicsObj obj)
        {
            if (obj != null)
                ServerObjects[obj.ID] = obj;
        }

        /// <summary>
        /// Removes a PhysicsObj from the static list of server-wide objects
        /// </summary>
        public static void RemoveServerObject(PhysicsObj obj)
        {
            if (obj != null)
                ServerObjects.TryRemove(obj.ID, out _);
        }

        /// <summary>
        /// Returns a PhysicsObj for an object ID
        /// </summary>
        public static PhysicsObj GetObjectA(uint objectID)
        {
            ServerObjects.TryGetValue(objectID, out var obj);
            return obj;
        }

        /// <summary>
        /// Clears all of the ObjMaint tables for an object
        /// </summary>
        public void RemoveAllObjects()
        {
            KnownObjects.Clear();
            VisibleObjects.Clear();
            DestructionQueue.Clear();
            KnownPlayers.Clear();
            VisibleTargets.Clear();
        }

        /// <summary>
        /// The destructor cleans up all ObjMaint references
        /// to this PhysicsObj
        /// </summary>
        public void DestroyObject()
        {
            foreach (var obj in KnownObjects.Values)
                obj.ObjMaint.RemoveObject(PhysicsObj);

            // we are maintaining the inverses here,
            // so passing false to iterate with modifying these collections
            foreach (var obj in KnownPlayers.Values)
                obj.ObjMaint.RemoveObject(PhysicsObj, false);

            foreach (var obj in VisibleTargets.Values)
                obj.ObjMaint.RemoveObject(PhysicsObj, false);

            RemoveAllObjects();

            RemoveServerObject(PhysicsObj);
        }
    }
}
