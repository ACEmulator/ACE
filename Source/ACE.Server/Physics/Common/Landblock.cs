using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Extensions;

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
            BlockInfoExists = landblock.HasObjects;
            if (BlockInfoExists)
                Info = (LandblockInfo)DBObj.Get(new QualifiedDataID(2, ID - 1));
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
        }

        public void PostInit()
        {
            init_buildings();
            init_static_objs();
        }

        public void add_static_object(PhysicsObj obj)
        {
            StaticObjects.Add(obj);
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
            // ported from Scenery
            Scenery = new List<PhysicsObj>();

            // get the landblock cell offsets
            var blockX = (ID >> 24) * 8;
            var blockY = (ID >> 16 & 0xFF) * 8;

            for (var i = 0; i < Terrain.Count; i++)
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
                        var position = Displace(obj, globalCellX, globalCellY, j);

                        // ensure within landblock range, and not near road
                        var lx = cellX * LandDefs.CellLength + position.X;
                        var ly = cellY * LandDefs.CellLength + position.Y;

                        // TODO: ensure walkable slope
                        if (lx < 0 || ly < 0 || lx >= LandDefs.BlockLength || ly >= LandDefs.BlockLength || OnRoad(obj, lx, ly)) continue;

                        // load scenery
                        var pos = new Position(ID);
                        pos.Frame.Origin = new Vector3(lx, ly, 0);
                        pos.Frame.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, RotateObj(obj, globalCellX, globalCellY, j));
                        var outside = LandDefs.AdjustToOutside(pos);
                        var cell = get_landcell(pos.ObjCellID);
                        //if (cell == null) continue;

                        Polygon walkable = null;
                        var terrainPoly = cell.find_terrain_poly(pos.Frame.Origin, ref walkable);
                        walkable.Plane.set_height(ref pos.Frame.Origin);

                        // todo: collision detection
                        var physicsObj = PhysicsObj.makeObject(obj.ObjId, 0, false);
                        //physicsObj.set_initial_frame(pos.Frame);
                        physicsObj.add_obj_to_cell(cell, pos.Frame);
                        Scenery.Add(physicsObj);
                    }
                }
            }
            //Console.WriteLine("Landblock " + ID.ToString("X8") + " scenery count: " + Scenery.Count);
        }

        /// <summary>
        /// Displaces a scenery object into a pseudo-randomized location
        /// </summary>
        /// <param name="obj">The object description</param>
        /// <param name="ix">The global cell X-offset</param>
        /// <param name="iy">The global cell Y-offset</param>
        /// <param name="iq">The scene index of the object</param>
        /// <returns>The new location of the object</returns>
        public static Vector2 Displace(ObjectDesc obj, uint ix, uint iy, uint iq)
        {
            float x;
            float y;

            var loc = obj.BaseLoc.Origin;

            if (obj.DisplaceX <= 0)
                x = loc.X;
            else
                x = (float)((1813693831 * iy - (iq + 45773) * (1360117743 * iy * ix + 1888038839) - 1109124029 * ix)
                    * 2.3283064e-10 * obj.DisplaceX + loc.X);

            if (obj.DisplaceY <= 0)
                y = loc.Y;
            else
                y = (float)((1813693831 * iy - (iq + 72719) * (1360117743 * iy * ix + 1888038839) - 1109124029 * ix)
                    * 2.3283064e-10 * obj.DisplaceY + loc.Y);

            var quadrant = (1813693831 * iy - ix * (1870387557 * iy + 1109124029) - 402451965) * 2.3283064e-10;

            if (quadrant >= 0.75) return new Vector2(y, -x);
            if (quadrant >= 0.5) return new Vector2(-x, -y);
            if (quadrant >= 0.25) return new Vector2(-y, x);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns the scale for a scenery object
        /// </summary>
        /// <param name="obj">The object decription</param>
        /// <param name="x">The global cell X-offset</param>
        /// <param name="y">The global cell Y-offset</param>
        /// <param name="k">The scene index of the object</param>
        public static float ScaleObj(ObjectDesc obj, uint x, uint y, uint k)
        {
            var scale = 1.0f;

            var minScale = obj.MinScale;
            var maxScale = obj.MaxScale;

            if (minScale == maxScale)
                scale = maxScale;
            else
                scale = (float)(Math.Pow(maxScale / minScale,
                    (1813693831 * y - (k + 32593) * (1360117743 * y * x + 1888038839) - 1109124029 * x) * 2.3283064e-10) * minScale);

            return scale;
        }

        /// <summary>
        /// Returns the rotation for a scenery object
        /// </summary>
        public static float RotateObj(ObjectDesc obj, uint x, uint y, uint k)
        {
            if (obj.MaxRotation <= 0.0f)
                return 0.0f;

            return (float)((1813693831 * y - (k + 63127) * (1360117743 * y * x + 1888038839) - 1109124029 * x) * 2.3283064e-10 * obj.MaxRotation * -0.0174533f);
        }

        /// <summary>
        /// Returns TRUE if x,y is located on a road cell
        /// </summary>
        public bool OnRoad(ObjectDesc obj, float x, float y)
        {
            var cellX = (int)Math.Floor(x / LandDefs.CellLength);
            var cellY = (int)Math.Floor(y / LandDefs.CellLength);
            var terrain = Terrain[cellX * LandDefs.BlockSide + cellY];     // ensure within bounds?
            return (terrain & 0x3) != 0;    // TODO: more complicated check for within road range
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
                    var position = new Position(ID, new AFrame(info.Frame));
                    var outside = LandDefs.AdjustToOutside(position);
                    var cell = get_landcell(position.ObjCellID);
                    if (cell == null) continue;
                    obj.add_obj_to_cell(cell, position.Frame);
                    add_static_object(obj);
                }
                if (UseSceneFiles)
                    get_land_scenes();
            }
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
        /// Returns TRUE if this landblock is a dungeon
        /// </summary>
        public bool IsDungeon
        {
            get
            {
                // return cached value
                if (isDungeon != null)
                    return isDungeon.Value;

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
                isDungeon = Info != null && Info.NumCells > 0 && Info.Buildings != null && Info.Buildings.Count > 0;
                return isDungeon.Value;
            }
        }
    }
}
