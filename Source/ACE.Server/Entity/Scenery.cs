using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Server.Physics;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Generates the scenery for a landblock
    /// </summary>
    public class Scenery
    {
        public static List<ModelMesh> Load(Landblock _landblock)
        {
            var landblock = _landblock.CellLandblock;

            // get the landblock cell offsets
            var blockX = (landblock.Id >> 24) * 8;
            var blockY = (landblock.Id >> 16 & 0xFF) * 8;

            var scenery = new List<ModelMesh>();

            for (var i = 0; i < landblock.Terrain.Count; i++)
            {
                var terrain = landblock.Terrain[i];

                var terrainType = terrain >> 2 & 0x1F;      // TerrainTypes table size = 32 (grass, desert, volcano, etc.)
                var sceneType = terrain >> 11;              // SceneTypes table size = 89 total, 32 which can be indexed for each terrain type

                var sceneInfo = (int)LandblockMesh.RegionDesc.TerrainInfo.TerrainTypes[terrainType].SceneTypes[sceneType];
                var scenes = LandblockMesh.RegionDesc.SceneInfo.SceneTypes[sceneInfo].Scenes;
                if (scenes.Count == 0) continue;

                var cellX = i / LandblockMesh.VertexDim;
                var cellY = i % LandblockMesh.VertexDim;

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
                    var noise = (uint)(cellXMat + cellYMat - cellMat * 23399) * 2.3283064e-10;

                    if (noise < obj.Freq && obj.WeenieObj == 0)
                    {
                        var position = Displace(obj, globalCellX, globalCellY, j);

                        // ensure within landblock range, and not near road
                        var lx = cellX * LandblockMesh.CellSize + position.X;
                        var ly = cellY * LandblockMesh.CellSize + position.Y;

                        // TODO: ensure walkable slope
                        if (lx < 0 || ly < 0 || lx > LandblockMesh.LandblockSize || ly > LandblockMesh.LandblockSize || OnRoad(obj, landblock, lx, ly)) continue;

                        // load scenery
                        var model = new ModelMesh(obj.ObjId, obj.BaseLoc);
                        model.ObjectDesc = obj;
                        model.Cell = new Vector2(cellX, cellY);
                        model.Position = new Vector3(position.X, position.Y, GetZ(_landblock, model));
                        model.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, RotateObj(obj, globalCellX, globalCellY, j));
                        model.Scale = ScaleObj(obj, globalCellX, globalCellY, j);
                        model.BuildPolygons();
                        model.BoundingBox = new BoundingBox(model);

                        // collision detection
                        if (Collision(_landblock.Buildings, model) || Collision(scenery, model))
                            continue;

                        scenery.Add(model);
                    }
                }
            }
            return scenery;
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
            if (quadrant >= 0.5)  return new Vector2(-x, -y);
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

            return (float)((1813693831 * y - (k + 63127) * (1360117743 * y * x + 1888038839) - 1109124029 * x) * 2.3283064e-10 * obj.MaxRotation * 0.0174533f);
        }

        /// <summary>
        /// Returns TRUE if x,y is located on a road cell
        /// </summary>
        public static bool OnRoad(ObjectDesc obj, CellLandblock landblock, float x, float y)
        {
            var cellX = (int)Math.Floor(x / LandblockMesh.CellSize);
            var cellY = (int)Math.Floor(y / LandblockMesh.CellSize);
            var terrain = landblock.Terrain[cellX * LandblockMesh.CellDim + cellY];     // ensure within bounds?
            return (terrain & 0x3) != 0;    // TODO: more complicated check for within road range
        }

        /// <summary>
        /// Returns TRUE is object intersects with any models
        /// </summary>
        public static bool Collision(List<ModelMesh> models, ModelMesh obj)
        {
            foreach (var model in models)
                if (obj.BoundingBox.Intersect2D(model.BoundingBox))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the landblock floor height for a scenery model
        /// </summary>
        public static float GetZ(Landblock landblock, ModelMesh modelMesh)
        {
            // get the landblock x/y position
            var x = modelMesh.Cell.X * LandblockMesh.CellSize + modelMesh.Position.X;
            var y = modelMesh.Cell.Y * LandblockMesh.CellSize + modelMesh.Position.Y;

            return landblock.LandblockMesh.GetZ(new Vector2(x, y));
        }
    }
}
