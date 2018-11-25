using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Common
{
    public class TextureMergeInfo
    {
        public TerrainTex TerrainBase;
        public List<TerrainTex> TerrainOverlays;
        public List<TerrainAlphaMap> TerrainAlphaOverlays;
        public List<int> TerrainAlphaIndices;
        public List<LandDefs.Rotation> TerrainRotations;

        public TerrainTex RoadOverlay;
        public List<RoadAlphaMap> RoadAlphaOverlays;
        public List<int> RoadAlphaIndices;
        public List<LandDefs.Rotation> RoadRotations;

        public List<uint> TCodes;

        public TextureMergeInfo()
        {
            TerrainOverlays = Enumerable.Repeat(TerrainTex.NULL, 3).ToList();
            TerrainAlphaOverlays = Enumerable.Repeat(TerrainAlphaMap.NULL, 3).ToList();
            TerrainRotations = Enumerable.Repeat(LandDefs.Rotation.Rot0, 3).ToList();
            TerrainAlphaIndices = Enumerable.Repeat(-1, 3).ToList();

            RoadAlphaOverlays = Enumerable.Repeat(RoadAlphaMap.NULL, 2).ToList();
            RoadRotations = Enumerable.Repeat(LandDefs.Rotation.Rot0, 2).ToList();
            RoadAlphaIndices = Enumerable.Repeat(-1, 2).ToList();
        }

        public void PostInit()
        {
            TerrainOverlays = TerrainOverlays.Where(t => t != null).ToList();
            TerrainAlphaOverlays = TerrainAlphaOverlays.Where(t => t != null).ToList();
            var numTerrains = TerrainOverlays.Count;
            TerrainRotations.RemoveRange(numTerrains, 3 - numTerrains);
            TerrainAlphaIndices = TerrainAlphaIndices.Where(t => t != -1).ToList();

            RoadAlphaOverlays = RoadAlphaOverlays.Where(r => r != null).ToList();
            var numRoads = RoadAlphaOverlays.Count;
            RoadRotations.RemoveRange(numRoads, 2 - numRoads);
            RoadAlphaIndices = RoadAlphaIndices.Where(r => r != -1).ToList();
        }

        public void Debug()
        {
            Console.WriteLine($"TerrainBase: {TerrainBase.TexGID:X8}");
            Console.WriteLine($"TerrainOverlays: {String.Join(",", TerrainOverlays.Where(t => t != null).Select(t => t.TexGID.ToString("X8")))}");
            Console.WriteLine($"TerrainAlphaOverlays: {String.Join(",", TerrainAlphaOverlays.Where(t => t != null).Select(t => t.TexGID.ToString("X8")))}");
            //Console.WriteLine($"TerrainAlphaIndices: {String.Join(",", TerrainAlphaIndices.Select(i => (AlphaIndex)i))}");
            Console.WriteLine($"TerrainAlphaIndices: {String.Join(",", TerrainAlphaIndices)}");
            Console.WriteLine($"TerrainRotations: {String.Join(",", TerrainRotations.Select(i => (int)i))}");
            //Console.WriteLine($"TerrainRotations: {String.Join(",", TerrainRotations)}");

            /*Console.WriteLine($"RoadOverlays: {TerrainBase.TexGID:X8}");
            Console.WriteLine($"RoadAlphaOverlays: {String.Join(",", RoadAlphaOverlays.Where(t => t != null).Select(t => t.RoadTexGID.ToString("X8")))}");
            Console.WriteLine($"RoadAlphaIndices: {String.Join(",", RoadAlphaIndices.Where(t => t != 0))}");
            Console.WriteLine($"RoadRotations: {String.Join(",", RoadRotations)}");*/

            Console.WriteLine($"TCodes: {String.Join(",", TCodes.Select(i => (SideCorner)i))}");
        }

        public enum RotationCorner
        {
            NW = 0,
            SW = 1,
            SE = 2,
            NE = 3,
        }
        
        // terrain rotations from NW

        public enum SideCorner
        {
            O = 0,
            SW = 1,
            SE = 2,
            S = 3,
            NE = 4,
            E = 6,
            NW = 8,
            W = 9,
            N = 12,
        };

        // terrain alpha indices:
        // -1 (null)
        // 0-3 (corners)
        // 4+ (sides?)

        public enum AlphaIndex
        {
            None = -1,
            Southwest = 0,
            Southeast = 1,
            Northeast = 2,
            Northwest = 3,
            //South = 4, // ?
            //East = 5,
            //West = 6,
            //North = 7
            Side = 5
        }

    }
}
