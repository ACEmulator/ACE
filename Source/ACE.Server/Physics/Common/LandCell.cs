using System;
using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class LandCell
    {
        public List<Polygon> Polygons;
        public bool InView;

        public static ObjCell Get(int cellID)
        {
            //return LScape.get_landcell(ObjCell.Landscape, cellID);
            return null;
        }

        public static void add_all_outside_cells(Position position, int numSphere, List<Sphere> spheres, CellArray cellArray)
        {
            if (cellArray.AddedOutside)
                return;

            if (numSphere == 0)
            {
                foreach (var sphere in spheres)
                {
                    if (!LandDefs.AdjustToOutside(position)) // sphere.Center
                        break;

                    var center = sphere.Center;
                    var point = new Vector2();
                    point.X = center.X - (float)Math.Floor(center.X / 24.0f) * 24.0f;
                    point.Y = center.Y - (float)Math.Floor(center.Y / 24.0f) * 24.0f;
                    var minRad = sphere.Radius;
                    var maxRad = 24.0f - minRad;

                    var lcoord = LandDefs.gid_to_lcoord(position.ObjCellID);
                    if (lcoord != null)
                    {
                        add_outside_cell(cellArray, lcoord.Value);
                        check_add_cell_boundary(cellArray, point, lcoord.Value, minRad, maxRad);
                    }
                }
            }
            else
            {
                if (!LandDefs.AdjustToOutside(position)) return; // position.Frame.Origin

                var lcoord = LandDefs.gid_to_lcoord(position.ObjCellID);
                if (lcoord != null)
                    add_outside_cell(cellArray, lcoord.Value);
            }
        }

        public static void add_outside_cell(CellArray cellArray, float _x, float _y)
        {
            var x = (uint)_x;
            var y = (uint)_y;

            if (x >= 0 && y >= 0 && x < 2040 && y < 2040)
            {
                var cellID = (int)(y >> 3 | 32 * (x & 0xFFFFFFF8) << 16 | (y & 7) + 8 * (x & 7) + 1);
                var landCell = Get(cellID);
                if (landCell != null)
                    cellArray.add_cell(cellID, landCell);
            }
        }

        public static void add_outside_cell(CellArray cellArray, Vector2 lcoord)
        {
            add_outside_cell(cellArray, lcoord.X, lcoord.Y);
        }

        public static void check_add_cell_boundary(CellArray cellArray, Vector2 point, Vector2 lcoord, float minRad, float maxRad)
        {
            if (point.X > maxRad)
            {
                add_outside_cell(cellArray, point.X + 1, point.Y);
                if (point.Y > maxRad)
                    add_outside_cell(cellArray, point.X + 1, point.Y + 1);
                else
                    add_outside_cell(cellArray, point.X + 1, point.Y - 1);
            }
            if (point.X < minRad)
            {
                add_outside_cell(cellArray, point.X - 1, point.Y);
                if (point.Y > maxRad)
                    add_outside_cell(cellArray, point.X - 1, point.Y + 1);
                else
                    add_outside_cell(cellArray, point.X - 1, point.Y - 1);
            }
            if (point.Y > maxRad)
                add_outside_cell(cellArray, point.X, point.Y + 1);

            else if (point.Y < minRad)
                add_outside_cell(cellArray, point.X, point.Y - 1);
        }
    }
}
