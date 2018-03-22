using System;
using System.Collections.Generic;
using System.Numerics;
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
        public List<uint> StabList;
        public List<LandCell> DrawArray;

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
            // already implemented in Scenery
        }

        public LandCell get_landcell(uint cellID)
        {
            var lcoord = LandDefs.gid_to_lcoord(cellID).Value;

            var idx = ((int)lcoord.X & 7) + ((int)lcoord.Y & 7) * SideCellCount;

            if (LandCells[idx].ID == cellID)
                return LandCells[idx];
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
            EnvCell.grab_visible(StabList);
        }

        public void init_buildings()
        {
            if (Info == null || SideCellCount != 8) return;

            int maxSize = 0, stabNum = 0;
            foreach (var info in Info.Buildings)
            {
                var building = BuildingObj.makeBuilding(info.ModelId, info.Portals, info.NumLeaves);
                var position = new Position(ID, new AFrame(info.Frame));
                var outside = LandDefs.AdjustToOutside(position);
                var cell = get_landcell(outside ? 0 : ID);
                if (cell == null) continue;
                building.set_initial_frame(position.Frame);
                building.add_to_cell(cell); // SortCell?
                Buildings.Add(building);
                building.add_to_stablist(StabList, ref maxSize, ref stabNum);
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
                    var position = new Position(info.Id, new AFrame(info.Frame));
                    var outside = LandDefs.AdjustToOutside(position);
                    var cell = get_landcell(outside ? 0 : info.Id);
                    if (cell == null) continue;
                    obj.add_obj_to_cell(cell, position.Frame);
                    add_static_object(obj);
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
            EnvCell.release_visible(StabList);
        }
    }
}
