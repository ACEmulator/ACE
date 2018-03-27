using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class LandCell: SortCell
    {
        public List<Polygon> Polygons;
        public bool InView;

        public LandCell(): base()
        {
            Init();
        }

        public LandCell(uint cellID): base(cellID)
        {
            Init();
        }

        public override TransitionState FindCollisions(Transition transition)
        {
            var transitState = FindEnvCollisions(transition);
            if (transitState == TransitionState.OK)
            {
                transitState = base.FindCollisions(transition);
                if (transitState == TransitionState.OK)
                    transitState = FindObjCollisions(transition);
            }
            return transitState;
        }

        public override TransitionState FindEnvCollisions(Transition transition)
        {
            var transitState = check_entry_restrictions(transition);

            if (transitState != TransitionState.OK)
                return transitState;

            var path = transition.SpherePath;

            var blockOffset = LandDefs.GetBlockOffset(path.CheckPos.ObjCellID, ID);
            var localPoint = transition.SpherePath.GlobalLowPoint - blockOffset;

            Polygon walkable = null;
            if (!find_terrain_poly(localPoint, ref walkable))
                return transitState;

            var objInfo = transition.ObjectInfo;

            if (get_block_water_type() == LandDefs.WaterType.EntirelyWater &&
                !objInfo.State.HasFlag(ObjectInfoState.IsViewer) && !objInfo.Object.State.HasFlag(PhysicsState.Missile))
            {
                return TransitionState.Collided;
            }
            var waterDepth = get_water_depth(localPoint);

            var checkPos = path.GlobalSphere[0];
            checkPos.Center -= LandDefs.GetBlockOffset(path.CheckPos.ObjCellID, ID);

            return objInfo.ValidateWalkable(checkPos, walkable.Plane, WaterType != LandDefs.WaterType.NotWater, waterDepth, transition, ID);
        }

        public new static LandCell Get(uint cellID)
        {
            return (LandCell)LScape.get_landcell(cellID);
        }

        public new void Init()
        {
            base.Init();

            // always 2 polys?
            Polygons = new List<Polygon>(2);
            for (var i = 0; i < 2; i++) Polygons.Add(null);
        }

        public static void add_all_outside_cells(Position position, int numSphere, List<Sphere> spheres, CellArray cellArray)
        {
            if (cellArray.AddedOutside) return;

            if (numSphere != 0)
            {
                foreach (var sphere in spheres)
                {
                    var cellPoint = position.ObjCellID;
                    var center = sphere.Center;

                    if (!LandDefs.AdjustToOutside(ref cellPoint, ref center))
                        break;

                    var point = new Vector2();
                    point.X = center.X - (float)Math.Floor(center.X / 24.0f) * 24.0f;
                    point.Y = center.Y - (float)Math.Floor(center.Y / 24.0f) * 24.0f;
                    var minRad = sphere.Radius;
                    var maxRad = 24.0f - minRad;

                    var lcoord = LandDefs.gid_to_lcoord(cellPoint);
                    if (lcoord != null)
                    {
                        add_outside_cell(cellArray, lcoord.Value);
                        check_add_cell_boundary(cellArray, point, lcoord.Value, minRad, maxRad);
                    }
                }
            }
            else
            {
                if (!LandDefs.AdjustToOutside(position)) return;

                var lcoord = LandDefs.gid_to_lcoord(position.ObjCellID);
                if (lcoord != null)
                    add_outside_cell(cellArray, lcoord.Value);
            }
        }

        public static void add_all_outside_cells(int numParts, List<PhysicsPart> parts, CellArray cellArray)
        {
            // not implemented yet
        }

        public static void add_outside_cell(CellArray cellArray, float _x, float _y)
        {
            var x = (uint)_x;
            var y = (uint)_y;

            if (x >= 0 && y >= 0 && x < 2040 && y < 2040)
            {
                var cellID = (((y >> 3) | 32 * (x & 0xFFFFFFF8)) << 16) | ((y & 7) + 8 * (x & 7) + 1);
                var landCell = Get(cellID);
                if (landCell != null)
                    cellArray.add_cell(cellID, landCell);
            }
        }

        public static void add_outside_cell(CellArray cellArray, Vector2 lcoord)
        {
            add_outside_cell(cellArray, lcoord.X, lcoord.Y);
        }

        /// <summary>
        /// Checks if this sphere exceeds the boundaries of the cell
        /// if it does, adds the neighboring cells to cellArray
        /// </summary>
        public static void check_add_cell_boundary(CellArray cellArray, Vector2 point, Vector2 lcoord, float minRad, float maxRad)
        {
            float x = lcoord.X, y = lcoord.Y;

            if (point.X > maxRad)
            {
                add_outside_cell(cellArray, x + 1, y);
                if (point.Y > maxRad)
                    add_outside_cell(cellArray, x + 1, y + 1);
                else
                    add_outside_cell(cellArray, x + 1, y - 1);
            }
            if (point.X < minRad)
            {
                add_outside_cell(cellArray, x - 1, y);
                if (point.Y > maxRad)
                    add_outside_cell(cellArray, x - 1, y + 1);
                else
                    add_outside_cell(cellArray, x - 1, y - 1);
            }
            if (point.Y > maxRad)
                add_outside_cell(cellArray, x, y + 1);

            else if (point.Y < minRad)
                add_outside_cell(cellArray, x, y - 1);
        }

        public bool find_terrain_poly(Vector3 origin, ref Polygon walkable)
        {
            for (var i = 0; i < 2; i++)
            {
                if (Polygons[i].point_in_poly2D(origin, Sidedness.Positive))
                {
                    walkable = Polygons[i];
                    return true;
                }
            }
            return false;
        }

        public override bool point_in_cell(Vector3 point)
        {
            Polygon poly = null;
            return find_terrain_poly(point, ref poly);
        }
    }
}
