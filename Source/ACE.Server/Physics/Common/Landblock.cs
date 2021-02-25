using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Extensions;
using ACE.Server.Managers;

namespace ACE.Server.Physics.Common
{
    public class Landblock: LandblockStruct
    {
        public Vector2 BlockCoord;
        public AFrame BlockFrame;
        public float MinZ;
        public float MaxZ;
        public bool DynObjsInitDone;
        public bool BlockInfoExists;
        public LandDefs.Direction Dir;
        public Vector2 Closest;
        public BoundingType InView;
        public CellLandblock _landblock;
        public LandblockInfo Info;
        public List<PhysicsObj> StaticObjects;
        public List<BuildingObj> Buildings;
        public List<ushort> StabList;
        public List<LandCell> DrawArray;
        public List<PhysicsObj> Scenery;
        public List<PhysicsObj> ServerObjects { get; set; }

        public static bool UseSceneFiles = true;

        public Landblock() : base()
        {
            Init();
        }

        public Landblock(CellLandblock landblock)
            : base(landblock)
        {
            Init();

            ID = landblock.Id;
            //Console.WriteLine("Loading landblock " + ID.ToString("X8"));
            BlockInfoExists = landblock.HasObjects;
            if (BlockInfoExists)
                Info = DBObj.GetLandblockInfo(ID - 1);
            BlockCoord = LandDefs.blockid_to_lcoord(landblock.Id).Value;
            _landblock = landblock;
            get_land_limits();
        }

        public new void Init()
        {
            InView = BoundingType.Outside;
            Dir = LandDefs.Direction.Unknown;
            Closest = new Vector2(-1, -1);
            BlockCoord = new Vector2();
            StaticObjects = new List<PhysicsObj>();
            Buildings = new List<BuildingObj>();
            ServerObjects = new List<PhysicsObj>();
        }

        public void PostInit()
        {
            init_landcell();

            init_buildings();
            init_static_objs();
        }

        public void add_static_object(PhysicsObj obj)
        {
            StaticObjects.Add(obj);
        }

        /// <summary>
        /// Called when a server object to be broadcast to clients
        /// enters this landblock
        /// </summary>
        public bool add_server_object(PhysicsObj obj)
        {
            if (!ServerObjects.Contains(obj))
            {
                ServerObjects.Add(obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when a server object to be broadcast to clients
        /// is removed from this landblock
        /// </summary>
        public bool remove_server_object(PhysicsObj obj)
        {
            return ServerObjects.Remove(obj);
        }

        public void adjust_scene_obj_height()
        {
            foreach (var obj in StaticObjects)
            {
                var cell = (LandCell)obj.CurCell;
                Polygon walkable = null;
                var objPos = obj.Position.Frame.Origin;
                if (!cell.find_terrain_poly(objPos, ref walkable))
                    continue;
                var adjZ = objPos.Z;
                if (Math.Abs(walkable.Plane.Normal.Z) > PhysicsGlobals.EPSILON)
                    adjZ = (objPos.Dot2D(walkable.Plane.Normal) + walkable.Plane.D) / walkable.Plane.Normal.Z * -1;
                if (Math.Abs(objPos.Z - adjZ) > PhysicsGlobals.EPSILON)
                {
                    objPos.Z = adjZ;
                    obj.set_initial_frame(obj.Position.Frame);
                }
            }
        }

        public float GetZ(Vector3 point)
        {
            var cell = GetCell(point);
            if (cell == null)
                return point.Z;
            Polygon walkable = null;
            if (!cell.find_terrain_poly(point, ref walkable))
                return point.Z;
            var adjZ = point.Z;
            if (Math.Abs(walkable.Plane.Normal.Z) > PhysicsGlobals.EPSILON)
                adjZ = (point.Dot2D(walkable.Plane.Normal) + walkable.Plane.D) / walkable.Plane.Normal.Z * -1;
            return adjZ;
        }

        public LandCell GetCell(Vector3 point)
        {
            if (point.X < 0 || point.Y < 0 || point.X > 192 || point.Y > 192)
                return null;

            var cellX = (int)point.X / 24;
            var cellY = (int)point.Y / 24;

            var blockCellID = (ID & 0xFFFF0000) | (uint)(cellX * 8 + cellY) + 1;
            return (LandCell)LScape.get_landcell((uint)blockCellID);
        }

        public void destroy_buildings()
        {
            foreach (var building in Buildings)
                building.remove();

            Buildings.Clear();
            StabList.Clear();
        }

        public void destroy_static_objects()
        {
            foreach (var obj in StaticObjects)
                obj.leave_world();

            StaticObjects.Clear();
        }

        public void get_land_limits()
        {
            var minHeight = byte.MaxValue;
            var maxHeight = byte.MinValue;

            foreach (var height in Height)
            {
                if (height < minHeight) minHeight = height;
                if (height > maxHeight) maxHeight = height;
            }
            MinZ = LandDefs.LandHeightTable[minHeight]/* + 1.0f*/;
            MaxZ = LandDefs.LandHeightTable[maxHeight]/* + 200.0f*/;
        }

        public void get_land_scenes()
        {
            //Console.WriteLine("Loading scenery for " + ID.ToString("X8"));
            
            // ported from Scenery
            Scenery = new List<PhysicsObj>();

            // get the landblock cell offsets
            var blockX = (ID >> 24) * 8;
            var blockY = (ID >> 16 & 0xFF) * 8;

            for (uint i = 0; i < Terrain.Count; i++)
            {
                var terrain = Terrain[(int)i];

                var terrainType = terrain >> 2 & 0x1F;      // TerrainTypes table size = 32 (grass, desert, volcano, etc.)
                var sceneType = terrain >> 11;              // SceneTypes table size = 89 total, 32 which can be indexed for each terrain type

                var sceneInfo = (int)DatManager.PortalDat.RegionDesc.TerrainInfo.TerrainTypes[terrainType].SceneTypes[sceneType];
                var scenes = DatManager.PortalDat.RegionDesc.SceneInfo.SceneTypes[sceneInfo].Scenes;
                if (scenes.Count == 0) continue;

                var cellX = i / LandDefs.VertexDim;
                var cellY = i % LandDefs.VertexDim;

                var globalCellX = (uint)(cellX + blockX);
                var globalCellY = (uint)(cellY + blockY);

                var cellMat = globalCellY * (712977289 * globalCellX + 1813693831) - 1109124029 * globalCellX + 2139937281;
                var offset = cellMat * 2.3283064e-10;
                var scene_idx = (int)(scenes.Count * offset);
                if (scene_idx >= scenes.Count) scene_idx = 0;

                var sceneId = scenes[scene_idx];

                var scene = DatManager.PortalDat.ReadFromDat<Scene>(sceneId);

                var cellXMat = -1109124029 * globalCellX;
                var cellYMat = 1813693831 * globalCellY;
                cellMat = 1360117743 * globalCellX * globalCellY + 1888038839;

                for (uint j = 0; j < scene.Objects.Count; j++)
                {
                    var obj = scene.Objects[(int)j];
                    var noise = (uint)(cellXMat + cellYMat - cellMat * (23399 + j)) * 2.3283064e-10;

                    if (noise < obj.Freq && obj.WeenieObj == 0)
                    {
                        // pseudo-randomized placement
                        var position = ObjectDesc.Displace(obj, globalCellX, globalCellY, j);

                        var lx = cellX * LandDefs.CellLength + position.X;
                        var ly = cellY * LandDefs.CellLength + position.Y;
                        var loc = new Vector3(lx, ly, position.Z);

                        // ensure within landblock range, and not near road
                        if (lx < 0 || ly < 0 || lx >= LandDefs.BlockLength || ly >= LandDefs.BlockLength || OnRoad(loc)) continue;

                        // load scenery
                        var pos = new Position(ID);
                        pos.Frame.Origin = loc;
                        var outside = LandDefs.AdjustToOutside(pos);
                        var cell = get_landcell(pos.ObjCellID);
                        if (cell == null) continue;

                        // check for buildings
                        var sortCell = (SortCell)cell;
                        if (sortCell != null && sortCell.has_building()) continue;

                        Polygon walkable = null;
                        var terrainPoly = cell.find_terrain_poly(pos.Frame.Origin, ref walkable);
                        if (walkable == null) continue;

                        // ensure walkable slope
                        if (!ObjectDesc.CheckSlope(obj, walkable.Plane.Normal.Z)) continue;

                        walkable.Plane.set_height(ref pos.Frame.Origin);

                        // rotation
                        if (obj.Align != 0)
                            pos.Frame = ObjectDesc.ObjAlign(obj, walkable.Plane, pos.Frame.Origin);
                        else
                            pos.Frame = ObjectDesc.RotateObj(obj, globalCellX, globalCellY, j, pos.Frame.Origin);

                        // build object
                        var physicsObj = PhysicsObj.makeObject(obj.ObjId, 0, false);
                        physicsObj.DatObject = true;
                        physicsObj.set_initial_frame(pos.Frame);
                        if (!physicsObj.obj_within_block())
                        {
                            //Console.WriteLine($"Landblock {ID:X8} scenery: failed to spawn {obj.ObjId:X8}");
                            physicsObj.DestroyObject();
                            continue;
                        }

                        physicsObj.add_obj_to_cell(cell, pos.Frame);
                        var scale = ObjectDesc.ScaleObj(obj, globalCellX, globalCellY, j);
                        physicsObj.SetScaleStatic(scale);
                        Scenery.Add(physicsObj);
                    }
                }
            }
            //Console.WriteLine("Landblock " + ID.ToString("X8") + " scenery count: " + Scenery.Count);
        }

        public static float RoadWidth = 5.0f;
        public static float TileLength = 24.0f;

        /// <summary>
        /// Returns TRUE if x,y is located on a road cell
        /// </summary>
        public bool IsRoad(DatLoader.Entity.ObjectDesc obj, float x, float y)
        {
            var cellX = (int)Math.Floor(x / LandDefs.CellLength);
            var cellY = (int)Math.Floor(y / LandDefs.CellLength);
            var terrain = Terrain[cellX * LandDefs.BlockSide + cellY];     // ensure within bounds?
            return (terrain & 0x3) != 0;    // TODO: more complicated check for within road range
        }

        public bool OnRoad(Vector3 obj)
        {
            int x = (int)(obj.X / TileLength);
            int y = (int)(obj.Y / TileLength);

            float rMin = RoadWidth;
            float rMax = TileLength - RoadWidth;

            int x0 = x;
            int x1 = x0 + 1;
            int y0 = y;
            int y1 = y0 + 1;

            uint r0 = GetRoad(x0, y0);
            uint r1 = GetRoad(x0, y1);
            uint r2 = GetRoad(x1, y0);
            uint r3 = GetRoad(x1, y1);

            if (r0 == 0 && r1 == 0 && r2 == 0 && r3 == 0)
                return false;

            float dx = obj.X - x * TileLength;
            float dy = obj.Y - y * TileLength;

            if (r0 > 0)
            {
                if (r1 > 0)
                {
                    if (r2 > 0)
                    {
                        if (r3 > 0)
                            return true;
                        else
                            return (dx < rMin || dy < rMin);
                    }
                    else
                    {
                        if (r3 > 0)
                            return (dx < rMin || dy > rMax);
                        else
                            return (dx < rMin);
                    }
                }
                else
                {
                    if (r2 > 0)
                    {
                        if (r3 > 0)
                            return (dx > rMax || dy < rMin);
                        else
                            return (dy < rMin);
                    }
                    else
                    {
                        if (r3 > 0)
                            return (Math.Abs(dx - dy) < rMin);
                        else
                            return (dx + dy < rMin);
                    }
                }
            }
            else
            {
                if (r1 > 0)
                {
                    if (r2 > 0)
                    {
                        if (r3 > 0)
                            return (dx > rMax || dy > rMax);
                        else
                            return (Math.Abs(dx + dy - TileLength) < rMin);
                    }
                    else
                    {
                        if (r3 > 0)
                            return (dy > rMax);
                        else
                            return (TileLength + dx - dy < rMin);
                    }
                }
                else
                {
                    if (r2 > 0)
                    {
                        if (r3 > 0)
                            return (dx > rMax);
                        else
                            return (TileLength - dx + dy < rMin);
                    }
                    else
                    {
                        if (r3 > 0)
                            return (TileLength * 2f - dx - dy < rMin);
                        else
                            return false;
                    }
                }
            }
        }

        public uint GetRoad(int x, int y)
        {
            ushort t0 = Terrain[x * 9 + y];
            t0 = (ushort)(t0 & ((1 << 2) - 1));
            return t0;
        }

        public LandCell get_landcell(uint cellID)
        {
            var lcoord = LandDefs.gid_to_lcoord(cellID).Value;

            var idx = ((int)lcoord.Y & 7) + ((int)lcoord.X & 7) * SideCellCount;

            if (LandCells[idx].ID == cellID)
                return (LandCell)LandCells[idx];
            else
                return null;
        }

        public uint get_terrain(uint cellID, Vector3 point)
        {
            var lcoord = LandDefs.gid_to_lcoord(cellID).Value;

            return Terrain[(int)lcoord.X * 255 * 9 + (int)lcoord.Y];
        }

        public void grab_visible_cells()
        {
            // legacy method
            //EnvCell.grab_visible(StabList);
        }

        public void init_buildings()
        {
            if (Info == null || SideCellCount != 8) return;

            uint maxSize = 0, stabNum = 0;
            foreach (var info in Info.Buildings)
            {
                var building = BuildingObj.makeBuilding(info.ModelId, info.Portals, info.NumLeaves);
                var position = new Position(ID, new AFrame(info.Frame));
                var outside = LandDefs.AdjustToOutside(position);
                var cell = get_landcell(position.ObjCellID);
                if (cell == null) continue;
                building.set_initial_frame(position.Frame);

                // hack
                building.PartArray.Parts[0].Pos = position;
                building.Position = position;
                cell.Building = building;

                building.add_to_cell(cell); // SortCell?
                Buildings.Add(building);
                building.add_to_stablist(ref StabList, ref maxSize, ref stabNum);
            }
        }

        public void init_dyn_objs()
        {
            if (SideCellCount != 8 || DynObjsInitDone)
                return;

            for (var i = 0; i < SideCellCount; i++)
            {
                var cell = (ObjCell)LandCells[i];
                var offset = i * SideCellCount * 11;    // ?
                cell.init_objects();
            }
            DynObjsInitDone = true;
        }

        public void init_landcell()
        {
            // should be length SideCellCount ^ 2
            foreach (var landCell in LandCells.Values)
                landCell.CurLandblock = this;
        }

        public void init_static_objs()
        {
            if (SideCellCount != 8) return;
            if (StaticObjects.Count > 0)
            {
                adjust_scene_obj_height();
                foreach (var obj in StaticObjects)
                    if (!obj.is_completely_visible())
                        obj.calc_cross_cells_static();
            }
            else if (Info != null)
            {
                foreach (var info in Info.Objects)
                {
                    var obj = PhysicsObj.makeObject(info.Id, 0, false);
                    obj.DatObject = true;
                    var position = new Position(ID, new AFrame(info.Frame));
                    var outside = LandDefs.AdjustToOutside(position);
                    var cell = get_landcell(position.ObjCellID);
                    if (cell == null)
                    {
                        //Console.WriteLine($"Landblock {ID:X8} - failed to spawn static object {info.Id:X8}");
                        obj.DestroyObject();
                        continue;
                    }
                    obj.add_obj_to_cell(cell, position.Frame);
                    add_static_object(obj);
                }

                if (Info.RestrictionTables != null)
                {
                    foreach (var kvp in Info.RestrictionTables)
                    {
                        var lcoord = LandDefs.gid_to_lcoord(kvp.Key);

                        if (lcoord == null) continue;

                        var idx = ((int)lcoord.Value.Y & 7) + ((int)lcoord.Value.X & 7) * SideCellCount;

                        LandCells[idx].RestrictionObj = kvp.Value;
                    }
                }
            }
            if (UseSceneFiles)
                get_land_scenes();
        }

        public void notify_change_size()
        {
            release_visible_cells();
            release_objs();
            destroy_static_objects();
            destroy_buildings();
            Closest = new Vector2(-1, -1);
        }

        public void release_all()
        {
            release_objs();
            release_visible_cells();
        }

        public void release_objs()
        {
            if (SideVertexCount != 9) return;

            for (var i = 0; i < SideCellCount; i++)
            {
                var cell = (ObjCell)LandCells[i];
                var offset = i * SideCellCount * 11;    // ?
                cell.release_objects();
            }
            DynObjsInitDone = false;
        }

        public void release_visible_cells()
        {
            // legacy method
            //EnvCell.release_visible(StabList);
        }

        private bool? isDungeon;

        /// <summary>
        /// Returns TRUE if this landblock is a dungeon,
        /// with no traversable overworld
        /// </summary>
        public bool IsDungeon
        {
            get
            {
                // return cached value
                if (isDungeon != null)
                    return isDungeon.Value;

                // hack for NW island
                // did a worldwide analysis for adding watercells into the formula,
                // but they are inconsistently defined for some of the edges of map unfortunately
                if (BlockCoord.X < 64 && BlockCoord.Y > 1976)
                {
                    //Console.WriteLine($"Allowing {ID:X8}");
                    isDungeon = false;
                    return isDungeon.Value;
                }

                // a dungeon landblock is determined by:
                // - all heights being 0
                // - having at least 1 EnvCell (0x100+)
                // - contains no buildings
                foreach (var height in Height)
                {
                    if (height != 0)
                    {
                        isDungeon = false;
                        return isDungeon.Value;
                    }
                }
                isDungeon = Info != null && Info.NumCells > 0 && Info.Buildings != null && Info.Buildings.Count == 0;
                return isDungeon.Value;
            }
        }

        private bool? hasDungeon;

        /// <summary>
        /// Returns TRUE if this landblock contains a dungeon
        //
        /// If a landblock contains both a dungeon + traversable overworld,
        /// this field will return TRUE, whereas IsDungeon will return FALSE
        /// 
        /// This property should only be used in very specific scenarios,
        /// such as determining if a landblock contains a mansion basement
        /// </summary>
        public bool HasDungeon
        {
            get
            {
                // return cached value
                if (hasDungeon != null)
                    return hasDungeon.Value;

                hasDungeon = Info != null && Info.NumCells > 0 && Info.Buildings != null && Info.Buildings.Count == 0;
                return hasDungeon.Value;
            }
        }


        private List<Landblock> adjacents;

        /// <summary>
        /// Returns the list of adjacent landblocks
        /// </summary>
        public List<Landblock> get_adjacents()
        {
            return adjacents;
        }

        /// <summary>
        /// This should only be used when PhysicsEngine.Instance.Server is true
        /// </summary>
        /// <param name="landblocks"></param>
        public void SetAdjacents(List<Server.Entity.Landblock> landblocks)
        {
            if (adjacents == null)
                adjacents = new List<Landblock>(landblocks.Count);
            else
                adjacents.Clear();

            foreach (var landblock in landblocks)
                adjacents.Add(landblock.PhysicsLandblock);
        }

        public List<PhysicsObj> GetServerObjects(bool includeAdjacents)
        {
            var results = new List<PhysicsObj>(ServerObjects);

            if (includeAdjacents)
            {
                foreach (var adjacent in adjacents)
                    results.AddRange(adjacent.ServerObjects);
            }

            return results;
        }


        private List<EnvCell> envcells;

        public List<EnvCell> get_envcells()
        {
            if (envcells != null) return envcells;

            envcells = new List<EnvCell>();

            if (Info == null) return envcells;

            var startCell = ID & 0xFFFF0000 | 0x100;
            var cellID = startCell;
            for (var i = 0; i < Info.NumCells; i++)
            {
                var envCell = (EnvCell)LScape.get_landcell(cellID++);
                if (envCell != null)
                    envcells.Add(envCell);
                else
                    break;
            }
            return envcells;
        }

        public void SortObjects()
        {
            ServerObjects = ServerObjects.OrderBy(i => i.Order).ToList();
        }
    }
}
