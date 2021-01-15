using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Server.Physics.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Physics.Common
{
    /// <summary>
    /// Player visibility tracking
    /// Keeps track of which objects are currently known / visible to a player
    /// </summary>
    public class ObjectMaint
    {
        /// <summary>
        /// The client automatically removes known objects if they remain outside visibility for this amount of time
        /// </summary>
        public static readonly float DestructionTime = 25.0f;

        private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// The owner of this ObjectMaint instance
        /// This is who we are tracking object visibility for, ie. a Player
        /// </summary>
        public PhysicsObj PhysicsObj { get; }

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
        private Dictionary<uint, PhysicsObj> KnownObjects { get; } = new Dictionary<uint, PhysicsObj>();

        /// <summary>
        /// This list of objects that are currently within PVS / VisibleCell range
        /// only maintained for players
        /// </summary>
        private Dictionary<uint, PhysicsObj> VisibleObjects { get; } = new Dictionary<uint, PhysicsObj>();

        /// <summary>
        /// Objects that were previously visible to the client,
        /// but have been outside the PVS for less than 25 seconds
        /// only maintained for players
        /// </summary>
        private Dictionary<PhysicsObj, double> DestructionQueue { get; } = new Dictionary<PhysicsObj, double>();

        /// <summary>
        /// A list of players that currently know about this object
        /// This is maintained for all server-spawned WorldObjects, and is used for broadcasting
        /// </summary>
        private Dictionary<uint, PhysicsObj> KnownPlayers { get; } = new Dictionary<uint, PhysicsObj>();

        /// <summary>
        /// For monster and CombatPet FindNextTarget
        /// - for monsters, contains players and combat pets
        /// - for combat pets, contains monsters
        /// </summary>
        private Dictionary<uint, PhysicsObj> VisibleTargets { get; } = new Dictionary<uint, PhysicsObj>();

        /// <summary>
        /// Handles monsters targeting things they would not normally target
        /// - For faction mobs, retaliate against same-faction players and combat pets
        /// - For regular monsters, retaliate against faction mobs
        /// - For regular monsters that do *not* have a FoeType, retaliate against monsters that are foes with this creature
        /// </summary>
        private Dictionary<uint, PhysicsObj> RetaliateTargets { get; } = new Dictionary<uint, PhysicsObj>();

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
        }


        public PhysicsObj GetKnownObject(uint objectGuid)
        {
            rwLock.EnterReadLock();
            try
            {
                KnownObjects.TryGetValue(objectGuid, out var obj);
                return obj;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public int GetKnownObjectsCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public bool KnownObjectsContainsKey(uint guid)
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.ContainsKey(guid);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public bool KnownObjectsContainsValue(PhysicsObj value)
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Values.Contains(value);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<KeyValuePair<uint, PhysicsObj>> GetKnownObjectsWhere(Func<KeyValuePair<uint, PhysicsObj>, bool> predicate)
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetKnownObjectsValues()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Values.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetKnownObjectsValuesWhere(Func<PhysicsObj, bool> predicate)
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Values.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds an object to the list of known objects
        /// only maintained for players
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        public bool AddKnownObject(PhysicsObj obj)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (KnownObjects.ContainsKey(obj.ID))
                    return false;

                KnownObjects.TryAdd(obj.ID, obj);

                // maintain KnownPlayers for both parties
                if (obj.IsPlayer)
                    AddKnownPlayer(obj);

                obj.ObjMaint.AddKnownPlayer(PhysicsObj);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds a list of objects to the known objects list
        /// only maintained for players
        /// </summary>
        /// <param name="objs">A list of currently visible objects</param>
        /// <returns>The list of visible objects that were previously unknown</returns>
        private List<PhysicsObj> AddKnownObjects(IEnumerable<PhysicsObj> objs)
        {
            var newObjs = new List<PhysicsObj>();

            foreach (var obj in objs)
            {
                if (AddKnownObject(obj))
                    newObjs.Add(obj);
            }

            return newObjs;
        }

        public bool RemoveKnownObject(PhysicsObj obj, bool inversePlayer = true)
        {
            rwLock.EnterWriteLock();
            try
            {
                var result = KnownObjects.Remove(obj.ID, out _);

                if (inversePlayer && PhysicsObj.IsPlayer)
                    obj.ObjMaint.RemoveKnownPlayer(PhysicsObj);

                return result;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public int GetVisibleObjectsCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public bool VisibleObjectsContainsKey(uint key)
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.ContainsKey(key);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<KeyValuePair<uint, PhysicsObj>> GetVisibleObjectsWhere(Func<KeyValuePair<uint, PhysicsObj>, bool> predicate)
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetVisibleObjectsValues()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.Values.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetVisibleObjectsValuesWhere(Func<PhysicsObj, bool> predicate)
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.Values.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<Creature> GetVisibleObjectsValuesOfTypeCreature()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleObjects.Values.Select(v => v.WeenieObj.WorldObject).OfType<Creature>().ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a cell
        /// </summary>
        public List<PhysicsObj> GetVisibleObjects(ObjCell cell, VisibleObjectType type = VisibleObjectType.All)
        {
            rwLock.EnterReadLock();
            try
            {
                if (PhysicsObj.CurLandblock == null || cell == null)
                    return new List<PhysicsObj>();

                // use PVS / VisibleCells for EnvCells not seen outside
                // (mostly dungeons, also some large indoor areas ie. caves)
                if (cell is EnvCell envCell)
                    return GetVisibleObjects(envCell, type);

                // use current landblock + adjacents for outdoors,
                // and envcells seen from outside (all buildings)
                var visibleObjs = PhysicsObj.CurLandblock.GetServerObjects(true);

                return ApplyFilter(visibleObjs, type).Where(i => i.ID != PhysicsObj.ID && (!(i.CurCell is EnvCell indoors) || indoors.SeenOutside)).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a dungeon cell
        /// </summary>
        private List<PhysicsObj> GetVisibleObjects(EnvCell cell, VisibleObjectType type)
        {
            var visibleObjs = new List<PhysicsObj>();

            // add objects from current cell
            cell.AddObjectListTo(visibleObjs);

            // add objects from visible cells
            foreach (var envCell in cell.VisibleCells.Values)
            {
                if (envCell != null)
                    envCell.AddObjectListTo(visibleObjs);
            }

            // if SeenOutside, add objects from outdoor landblock
            if (cell.SeenOutside)
            {
                var outsideObjs = PhysicsObj.CurLandblock.GetServerObjects(true).Where(i => !(i.CurCell is EnvCell indoors) || indoors.SeenOutside);

                visibleObjs.AddRange(outsideObjs);
            }

            return ApplyFilter(visibleObjs, type).Where(i => !i.DatObject && i.ID != PhysicsObj.ID).Distinct().ToList();
        }

        public enum VisibleObjectType
        {
            All,
            Players,
            AttackTargets
        };

        private IEnumerable<PhysicsObj> ApplyFilter(List<PhysicsObj> objs, VisibleObjectType type)
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
                else if (PhysicsObj.WeenieObj.IsFactionMob)
                    results = objs.Where(i => i.IsPlayer || i.WeenieObj.IsCombatPet || i.WeenieObj.IsMonster && !i.WeenieObj.SameFaction(PhysicsObj));
                else
                {
                    // adding faction mobs here, even though they are retaliate-only, for inverse visible targets
                    results = objs.Where(i => i.IsPlayer || i.WeenieObj.IsCombatPet || i.WeenieObj.IsFactionMob || i.WeenieObj.PotentialFoe(PhysicsObj));
                }
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
            rwLock.EnterWriteLock();
            try
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
                VisibleObjects.TryAdd(obj.ID, obj);

                if (obj.WeenieObj.IsMonster)
                    obj.ObjMaint.AddVisibleTarget(PhysicsObj, false);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Add a list of visible objects - maintains both known and visible objects
        /// only maintained for players
        /// </summary>
        public List<PhysicsObj> AddVisibleObjects(ICollection<PhysicsObj> objs)
        {
            rwLock.EnterWriteLock();
            try
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
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes an object from the visible objects list
        /// only run for players
        /// and also removes the player from the object's visible targets list
        /// </summary>
        public bool RemoveVisibleObject(PhysicsObj obj, bool inverseTarget = true)
        {
            rwLock.EnterWriteLock();
            try
            {
                var removed = VisibleObjects.Remove(obj.ID, out _);

                if (inverseTarget)
                    obj.ObjMaint.RemoveVisibleTarget(PhysicsObj);

                return removed;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public int GetDestructionQueueCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return DestructionQueue.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public Dictionary<PhysicsObj, double> GetDestructionQueueCopy()
        {
            rwLock.EnterReadLock();
            try
            {
                var result = new Dictionary<PhysicsObj, double>();

                foreach (var kvp in DestructionQueue)
                    result[kvp.Key] = kvp.Value;

                return result;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds an object to the destruction queue
        /// Called when an object exits the PVS range
        /// only maintained for players
        /// </summary>
        public bool AddObjectToBeDestroyed(PhysicsObj obj)
        {
            rwLock.EnterWriteLock();
            try
            {
                RemoveVisibleObject(obj);

                if (DestructionQueue.ContainsKey(obj))
                    return false;

                DestructionQueue.TryAdd(obj, PhysicsTimer.CurrentTime + DestructionTime);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds a list of objects to the destruction queue
        /// only maintained for players
        /// </summary>
        public List<PhysicsObj> AddObjectsToBeDestroyed(List<PhysicsObj> objs)
        {
            rwLock.EnterWriteLock();
            try
            {
                var queued = new List<PhysicsObj>();
                foreach (var obj in objs)
                {
                    if (AddObjectToBeDestroyed(obj))
                        queued.Add(obj);
                }

                return queued;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes an object from the destruction queue if it has been invisible for less than 25s
        /// this is only used for players
        /// </summary>
        public bool RemoveObjectToBeDestroyed(PhysicsObj obj)
        {
            rwLock.EnterWriteLock();
            try
            {
                double time = -1;
                DestructionQueue.TryGetValue(obj, out time);
                if (time != -1 && time > PhysicsTimer.CurrentTime)
                {
                    DestructionQueue.Remove(obj, out _);
                    return true;
                }

                return false;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes objects from the destruction queue when they re-enter visibility within 25s
        /// this is only used for players
        /// </summary>
        private void RemoveObjectsToBeDestroyed(IEnumerable<PhysicsObj> objs)
        {
            foreach (var obj in objs)
                RemoveObjectToBeDestroyed(obj);
        }

        /// <summary>
        /// Removes any objects that have been in the destruction queue for more than 25s
        /// this is only used for players
        /// </summary>
        public List<PhysicsObj> DestroyObjects()
        {
            rwLock.EnterWriteLock();
            try
            {
                // find the list of objects that have been in the destruction queue > 25s
                var expiredObjs = DestructionQueue.Where(kvp => kvp.Value <= PhysicsTimer.CurrentTime)
                    .Select(kvp => kvp.Key).ToList();

                // remove expired objects from all lists
                foreach (var expiredObj in expiredObjs)
                    RemoveObject(expiredObj);

                return expiredObjs;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes an object after it has expired from the destruction queue, or it has been destroyed
        /// </summary>
        public void RemoveObject(PhysicsObj obj, bool inverse = true)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (obj == null) return;

                RemoveKnownObject(obj, inverse);
                RemoveVisibleObject(obj, inverse);
                DestructionQueue.Remove(obj, out _);

                if (obj.IsPlayer)
                    RemoveKnownPlayer(obj);

                RemoveVisibleTarget(obj);
                RemoveRetaliateTarget(obj);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public int GetKnownPlayersCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownPlayers.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<KeyValuePair<uint, PhysicsObj>> GetKnownPlayersWhere(Func<KeyValuePair<uint, PhysicsObj>, bool> predicate)
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownPlayers.Where(predicate).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetKnownPlayersValues()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownPlayers.Values.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<Player> GetKnownPlayersValuesAsPlayer()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownPlayers.Values.Select(v => v.WeenieObj.WorldObject).OfType<Player>().ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<Creature> GetKnownObjectsValuesAsCreature()
        {
            rwLock.EnterReadLock();
            try
            {
                return KnownObjects.Values.Select(v => v.WeenieObj.WorldObject).OfType<Creature>().ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a player who currently knows about this object
        /// - this is maintained for all server-spawned objects
        /// 
        /// when an object broadcasts, it sends network messages
        /// to all players who currently know about this object
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        private bool AddKnownPlayer(PhysicsObj obj)
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

            KnownPlayers.TryAdd(obj.ID, obj);
            return true;
        }

        /// <summary>
        /// Adds a list of players known to this object
        /// </summary>
        public List<PhysicsObj> AddKnownPlayers(IEnumerable<PhysicsObj> objs)
        {
            rwLock.EnterWriteLock();
            try
            {
                var newObjs = new List<PhysicsObj>();

                foreach (var obj in objs)
                {
                    if (AddKnownPlayer(obj))
                        newObjs.Add(obj);
                }

                return newObjs;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes a known player for this object
        /// </summary>
        public bool RemoveKnownPlayer(PhysicsObj obj)
        {
            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.RemoveKnownPlayer({obj.Name})");

            rwLock.EnterReadLock();
            try
            {
                return KnownPlayers.Remove(obj.ID, out _);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public int GetVisibleTargetsCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleTargets.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public int GetRetaliateTargetsCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return RetaliateTargets.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public bool VisibleTargetsContainsKey(uint key)
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleTargets.ContainsKey(key);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public bool RetaliateTargetsContainsKey(uint key)
        {
            rwLock.EnterReadLock();
            try
            {
                return RetaliateTargets.ContainsKey(key);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetVisibleTargetsValues()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleTargets.Values.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<PhysicsObj> GetRetaliateTargetsValues()
        {
            rwLock.EnterReadLock();
            try
            {
                return RetaliateTargets.Values.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public List<Creature> GetVisibleTargetsValuesOfTypeCreature()
        {
            rwLock.EnterReadLock();
            try
            {
                return VisibleTargets.Values.Select(v => v.WeenieObj.WorldObject).OfType<Creature>().ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// For monster and CombatPet FindNextTarget
        /// - for monsters, contains players and combat pets
        /// - for combat pets, contains monsters
        /// </summary>
        private bool AddVisibleTarget(PhysicsObj obj, bool clamp = true, bool foeType = false)
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
            else if (PhysicsObj.WeenieObj.IsFactionMob)
            {
                // only tracking players, combat pets, and monsters of differing faction
                if (!obj.IsPlayer && !obj.WeenieObj.IsCombatPet && (!obj.WeenieObj.IsMonster || PhysicsObj.WeenieObj.SameFaction(obj)))
                {
                    Console.WriteLine($"{PhysicsObj.Name}.ObjectMaint.AddVisibleTarget({obj.Name}): tried to add a non-player / non-combat pet / non-opposing faction mob");
                    return false;
                }
            }
            else
            {
                // handle special case:
                // we want to select faction mobs for monsters inverse targets,
                // but not add to the original monster
                if (obj.WeenieObj.IsFactionMob)
                {
                    obj.ObjMaint.AddVisibleTarget(PhysicsObj);
                    return false;
                }

                // handle special case:
                // if obj has a FoeType of this creature, and this creature doesn't have a FoeType for obj,
                // we only want to perform the inverse
                if (obj.WeenieObj.FoeType != null && obj.WeenieObj.FoeType == PhysicsObj.WeenieObj.WorldObject?.CreatureType &&
                    (PhysicsObj.WeenieObj.FoeType == null || obj.WeenieObj.WorldObject != null && PhysicsObj.WeenieObj.FoeType != obj.WeenieObj.WorldObject.CreatureType))
                {
                    obj.ObjMaint.AddVisibleTarget(PhysicsObj);
                    return false;
                }

                // only tracking players and combat pets
                if (!obj.IsPlayer && !obj.WeenieObj.IsCombatPet && PhysicsObj.WeenieObj.FoeType == null)
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
        public List<PhysicsObj> AddVisibleTargets(IEnumerable<PhysicsObj> objs)
        {
            rwLock.EnterWriteLock();
            try
            {
                var visibleAdded = new List<PhysicsObj>();

                foreach (var obj in objs)
                {
                    if (AddVisibleTarget(obj))
                        visibleAdded.Add(obj);
                }

                return AddKnownPlayers(visibleAdded.Where(o => o.IsPlayer));
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private bool RemoveVisibleTarget(PhysicsObj obj)
        {
            //Console.WriteLine($"{PhysicsObj.Name} ({PhysicsObj.ID:X8}).ObjectMaint.RemoveVisibleTarget({obj.Name})");
            return VisibleTargets.Remove(obj.ID);
        }

        public List<PhysicsObj> GetVisibleObjectsDist(ObjCell cell, VisibleObjectType type)
        {
            rwLock.EnterReadLock();
            try
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
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a retaliate target for a monster that it would not normally attack
        /// </summary>
        public void AddRetaliateTarget(PhysicsObj obj)
        {
            rwLock.EnterWriteLock();
            try
            {
                if (RetaliateTargets.ContainsKey(obj.ID))
                {
                    //Console.WriteLine($"{PhysicsObj.Name}.AddRetaliateTarget({obj.Name}) - retaliate target already exists");
                    return;
                }
                //Console.WriteLine($"{PhysicsObj.Name}.AddRetaliateTarget({obj.Name})");
                RetaliateTargets.Add(obj.ID, obj);

                // we're going to add retaliate targets to the list of visible targets as well,
                // so that we don't have to traverse both VisibleTargets and RetaliateTargets
                // in all of the logic based on VisibleTargets
                if (VisibleTargets.ContainsKey(obj.ID))
                {
                    //Console.WriteLine($"{PhysicsObj.Name}.AddRetaliateTarget({obj.Name}) - visible target already exists");
                    return;
                }
                VisibleTargets.Add(obj.ID, obj);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Called when a monster goes back to sleep
        /// </summary>
        public void ClearRetaliateTargets()
        {
            rwLock.EnterWriteLock();
            try
            {
                // remove retaliate targets from visible targets
                foreach (var retaliateTarget in RetaliateTargets)
                    VisibleTargets.Remove(retaliateTarget.Key);

                RetaliateTargets.Clear();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private bool RemoveRetaliateTarget(PhysicsObj obj)
        {
            return RetaliateTargets.Remove(obj.ID);
        }

        /// <summary>
        /// Clears all of the ObjMaint tables for an object
        /// </summary>
        private void RemoveAllObjects()
        {
            KnownObjects.Clear();
            VisibleObjects.Clear();
            DestructionQueue.Clear();
            KnownPlayers.Clear();
            VisibleTargets.Clear();
            RetaliateTargets.Clear();
        }

        /// <summary>
        /// The destructor cleans up all ObjMaint references
        /// to this PhysicsObj
        /// </summary>
        public void DestroyObject()
        {
            rwLock.EnterWriteLock();
            try
            {
                foreach (var obj in KnownObjects.Values)
                    obj.ObjMaint.RemoveObject(PhysicsObj);

                // we are maintaining the inverses here,
                // so passing false to iterate with modifying these collections
                foreach (var obj in KnownPlayers.Values)
                    obj.ObjMaint.RemoveObject(PhysicsObj, false);

                foreach (var obj in VisibleTargets.Values)
                    obj.ObjMaint.RemoveObject(PhysicsObj, false);

                foreach (var obj in RetaliateTargets.Values)
                    obj.ObjMaint.RemoveObject(PhysicsObj, false);

                RemoveAllObjects();

                ServerObjectManager.RemoveServerObject(PhysicsObj);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
