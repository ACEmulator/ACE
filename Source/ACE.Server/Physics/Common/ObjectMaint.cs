using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    /// <summary>
    /// Player visibility tracking
    /// Keeps track of which objects are currently known / visible to a player
    /// </summary>
    public class ObjectMaint
    {
        /// <summary>
        /// Objects are removed from the client after this amount of time
        /// </summary>
        public static readonly float DestructionTime = 25.0f;

        /// <summary>
        /// The distance the server sends objects to players
        /// for non-dungeon landblocks
        /// </summary>
        public static readonly float RadiusOutside = 192.0f;

        /// <summary>
        /// The cell radius to send objects to players
        /// for non-dungeon landblocks (8 cell radius by default)
        /// </summary>
        public static readonly float CellRadiusOutside = RadiusOutside / LandDefs.CellLength;

        /// <summary>
        /// The owner of this ObjectMaint instance
        /// This is who we are tracking object visibility for, ie. a Player
        /// </summary>
        public PhysicsObj PhysicsObj;

        /// <summary>
        /// This list of objects that are known to the client
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
        /// </remarks>
        public Dictionary<uint, PhysicsObj> ObjectTable;

        /// <summary>
        /// This list of objects that are currently within PVS / VisibleCell range
        /// </summary>
        public Dictionary<uint, PhysicsObj> VisibleObjectTable;

        /// <summary>
        /// A list of objects that currently know about this object
        /// </summary>
        public Dictionary<uint, PhysicsObj> VoyeurTable;

        /// <summary>
        /// Objects that were previously visible to the client,
        /// but have been outside the PVS for less than 25 seconds
        /// </summary>
        public Dictionary<PhysicsObj, double> DestructionQueue;

        /// <summary>
        /// Custom lookup table of PhysicsObjs for the server
        /// </summary>
        public static ConcurrentDictionary<uint, PhysicsObj> ServerObjects = new ConcurrentDictionary<uint, PhysicsObj>();

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
            ObjectTable = new Dictionary<uint, PhysicsObj>();
            VisibleObjectTable = new Dictionary<uint, PhysicsObj>();
            VoyeurTable = new Dictionary<uint, PhysicsObj>();
            DestructionQueue = new Dictionary<PhysicsObj, double>();
        }

        /// <summary>
        /// Adds an object to the list of known objects
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        public bool AddObject(PhysicsObj obj)
        {
            if (!ObjectTable.ContainsKey(obj.ID))
            {
                ObjectTable.Add(obj.ID, obj);

                // add to target object's voyeurs
                obj.ObjMaint.AddVoyeur(PhysicsObj);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a list of objects to the known objects list
        /// </summary>
        /// <param name="objs">A list of newly visible objects</param>
        /// <returns>The list of visible objects that were previously unknown</returns>
        public List<PhysicsObj> AddObjects(List<PhysicsObj> objs)
        {
            var newObjs = new List<PhysicsObj>();

            foreach (var obj in objs)
                if (AddObject(obj)) newObjs.Add(obj);

            return newObjs;
        }

        public static bool InitialClamp = true;

        public static float InitialClamp_Dist = 112.5f;
        public static float InitialClamp_DistSq = InitialClamp_Dist * InitialClamp_Dist;

        /// <summary>
        /// Adds an object to the list of visible objects
        /// </summary>
        public bool AddVisibleObject(PhysicsObj obj)
        {
            if (!VisibleObjectTable.ContainsKey(obj.ID))
            {
                if (InitialClamp && !ObjectTable.ContainsKey(obj.ID))
                {
                    var distSq = PhysicsObj.WeenieObj.WorldObject.Location.Distance2DSquared(obj.WeenieObj.WorldObject.Location);
                    if (distSq > InitialClamp_DistSq)
                        return false;
                }

                VisibleObjectTable.Add(obj.ID, obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a list of visible objects - maintains both known and visible objects
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

            return AddObjects(visibleAdded);
        }

        /// <summary>
        /// Adds an object to the destruction queue
        /// Called when an object exits the PVS range
        /// </summary>
        public bool AddObjectToBeDestroyed(PhysicsObj obj)
        {
            var time = PhysicsTimer.CurrentTime + DestructionTime;
            if (!DestructionQueue.ContainsKey(obj))
            {
                DestructionQueue.Add(obj, time);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a list of objects to the destruction queue
        /// </summary>
        public List<PhysicsObj> AddObjectsToBeDestroyed(List<PhysicsObj> objs)
        {
            var queued = new List<PhysicsObj>();
            foreach (var obj in objs)
            {
                if (AddObjectToBeDestroyed(obj))
                    queued.Add(obj);

                VisibleObjectTable.Remove(obj.ID);
            }
            return queued;
        }

        /// <summary>
        /// Returns a list of objects that have been in the destruction queue
        /// for less than 25 seconds
        /// </summary>
        public List<PhysicsObj> GetCulledObjects(List<PhysicsObj> visibleObjects)
        {
            var culledObjects = DestructionQueue.Where(kvp => kvp.Value > PhysicsTimer.CurrentTime).ToDictionary(kvp => kvp.Key, kvp => kvp.Value).Keys.ToList();
            return culledObjects;
        }

        /// <summary>
        /// Returns a list of objects that have been in the destruction queue
        /// for more than 25 seconds
        /// </summary>
        public List<PhysicsObj> GetDestroyedObjects()
        {
            var destroyedObjects = DestructionQueue.Where(kvp => kvp.Value <= PhysicsTimer.CurrentTime).ToDictionary(kvp => kvp.Key, kvp => kvp.Value).Keys.ToList();
            return destroyedObjects;
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
        /// Returns a list of outdoor cells within visible range of player
        /// </summary>
        public List<ObjCell> GetOutdoorCells(ObjCell cell)
        {
            // get cell x/y global offset
            var lcoord = LandDefs.get_outside_lcoord(cell.ID, PhysicsObj.Position.Frame.Origin.X, PhysicsObj.Position.Frame.Origin.Y).Value;

            // includes the origin cell
            var blockLength = (int)CellRadiusOutside * 2 + 1;
            var cells = new List<ObjCell>(/*blockLength * blockLength*/);

            var start = new Vector2(lcoord.X - CellRadiusOutside, lcoord.Y - CellRadiusOutside);
            var end = new Vector2(lcoord.X + CellRadiusOutside, lcoord.Y + CellRadiusOutside);

            for (var cellX = start.X; cellX <= end.X; cellX++)
            {
                for (var cellY = start.Y; cellY <= end.Y; cellY++)
                {
                    var blockCellID = LandDefs.lcoord_to_gid(cellX, cellY);
                    var _cell = LScape.get_landcell((uint)blockCellID);
                    if (_cell == null)
                        continue;
                    cells.Add(_cell);

                    // does this outdoor cell contain a building?
                    // if so, add all of its cells
                    var landCell = _cell as LandCell;
                    if (landCell.has_building())
                    {
                        //Console.WriteLine($"Found building in cell {landCell.ID:X8}");
                        var buildingCells = landCell.Building.get_building_cells();
                        //Console.WriteLine("# cells: " + buildingCells.Count);
                        cells.AddRange(buildingCells);
                    }
                }
            }
            return cells;
        }

        public List<PhysicsObj> GetVisibleObjectsDist(ObjCell cell)
        {
            var visibleObjs = GetVisibleObjects(cell);

            var dist = new List<PhysicsObj>();
            foreach (var obj in visibleObjs)
            {
                var distSq = PhysicsObj.WeenieObj.WorldObject.Location.Distance2DSquared(obj.WeenieObj.WorldObject.Location);
                if (distSq <= InitialClamp_DistSq)
                    dist.Add(obj);
            }
            return dist;
        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a cell
        /// in an outdoor landblock
        /// </summary>
        public List<PhysicsObj> GetVisibleObjects(ObjCell cell)
        {
            if (PhysicsObj.CurLandblock == null || cell == null) return new List<PhysicsObj>();

            // use PVS / VisibleCells for EnvCells not seen outside
            // (mostly dungeons, also some large indoor areas ie. caves)
            var envCell = cell as EnvCell;
            if (envCell != null && !envCell.SeenOutside)
                return GetVisibleObjects(envCell);

            // use current landblock + adjacents for outdoors,
            // and envcells seen from outside (all buildings)
            var visibleObjs = new List<PhysicsObj>(PhysicsObj.CurLandblock.ServerObjects);

            var adjacents = PhysicsObj.CurLandblock.get_adjacents();
            if (adjacents != null)
            {
                foreach (var adjacent in adjacents)
                    visibleObjs.AddRange(adjacent.ServerObjects);
            }

            return visibleObjs.Where(i => i.ID != PhysicsObj.ID).ToList();

            /*var cells = GetOutdoorCells(cell);

            var visibleObjs = new List<PhysicsObj>();

            foreach (var _cell in cells)
                visibleObjs.AddRange(_cell.ObjectList);

            return visibleObjs.Where(obj => !obj.DatObject).Distinct().ToList();*/

        }

        /// <summary>
        /// Returns a list of objects that are currently visible from a dungeon cell
        /// </summary>
        public List<PhysicsObj> GetVisibleObjects(EnvCell cell)
        {
            var visibleObjs = new List<PhysicsObj>();

            // add objects from current cell
            visibleObjs.AddRange(cell.ObjectList);

            // add objects from visible cells
            foreach (var envCell in cell.VisibleCells.Values)
            {
                if (envCell == null) continue;

                visibleObjs.AddRange(envCell.ObjectList);
            }

            return visibleObjs.Where(i => !i.DatObject && i.ID != PhysicsObj.ID).Distinct().ToList();
        }

        /// <summary>
        /// Removes an object from the destruction queue
        /// if it has been invisible for less than 25s
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
        /// </summary>
        public void RemoveObjectsToBeDestroyed(List<PhysicsObj> objs)
        {
            foreach (var obj in objs)
                RemoveObjectToBeDestroyed(obj);
        }

        /// <summary>
        /// Removes any objects that have been in the destruction queue
        /// for more than 25s
        /// </summary>
        /// <returns></returns>
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
        /// Removes an object from all of the tables
        /// </summary>
        public void RemoveObject(PhysicsObj obj)
        {
            if (obj == null) return;

            ObjectTable.Remove(obj.ID);
            VisibleObjectTable.Remove(obj.ID);
            DestructionQueue.Remove(obj);
            VoyeurTable.Remove(obj.ID);

            obj.ObjMaint.RemoveVoyeur(PhysicsObj);
        }

        /// <summary>
        /// Clears all of the ObjMaint tables for an object
        /// </summary>
        public void RemoveAllObjects()
        {
            foreach (var obj in ObjectTable.Values)
                obj.ObjMaint.RemoveVoyeur(PhysicsObj);

            ObjectTable.Clear();
            VisibleObjectTable.Clear();
            DestructionQueue.Clear();
        }

        /// <summary>
        /// Adds a PhysicsObj to the static list of server-wide objects
        /// </summary>
        public static void AddServerObject(PhysicsObj obj)
        {
            if (obj == null) return;

            ServerObjects[obj.ID] = obj;
        }

        /// <summary>
        /// Removes a PhysicsObj from the static list of server-wide objects
        /// </summary>
        public static void RemoveServerObject(PhysicsObj obj)
        {
            if (obj == null) return;

            ServerObjects.TryRemove(obj.ID, out _);
        }

        /// <summary>
        /// Adds an object to the list of voyeurs
        /// </summary>
        /// <returns>true if previously an unknown object</returns>
        public bool AddVoyeur(PhysicsObj obj)
        {
            // only tracking players who know about each object
            if (!obj.IsPlayer)
                return false;

            if (!VoyeurTable.ContainsKey(obj.ID))
            {
                VoyeurTable.Add(obj.ID, obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a list of objects watching this object
        /// </summary>
        public List<PhysicsObj> AddVoyeurs(List<PhysicsObj> objs)
        {
            var newVoyeurs = new List<PhysicsObj>();

            foreach (var obj in objs)
                if (AddVoyeur(obj)) newVoyeurs.Add(obj);

            return newVoyeurs;
        }

        /// <summary>
        /// Removes an object from the voyeurs table
        /// </summary>
        public bool RemoveVoyeur(PhysicsObj obj)
        {
            return VoyeurTable.Remove(obj.ID);
        }

        /// <summary>
        /// Called when a new PhysicsObj is first instantiated
        /// Gets the list of visible players to this PhysicsObj,
        /// and adds them to the voyeurs list
        /// </summary>
        public void get_voyeurs()
        {
            if (PhysicsObj.DatObject) return;

            var visiblePlayers = GetVisibleObjectsDist(PhysicsObj.CurCell).Where(o => o.IsPlayer).ToList();
            AddVoyeurs(visiblePlayers);
        }

        /// <summary>
        /// The destructor cleans up all ObjMaint references
        /// to this PhysicsObj
        /// </summary>
        public void DestroyObject()
        {
            foreach (var obj in ObjectTable.Values)
                obj.ObjMaint.RemoveObject(PhysicsObj);

            RemoveServerObject(PhysicsObj);
        }
    }
}
