using System;
using System.Collections.Generic;

using ACE.Server.Physics.BSP;

namespace ACE.Server.Physics.Entity
{
    public static class BSPCache
    {
        public static bool Enabled = false;

        public static readonly HashSet<BSPTree> BSPTrees = new HashSet<BSPTree>();

        public static int Requests;
        public static int Hits;

        public static int Count => BSPTrees.Count;

        public static void Clear()
        {
            BSPTrees.Clear();
        }

        public static BSPTree Get(BSPTree bspTree)
        {
            if (!Enabled)
                return bspTree;

            Requests++;

            //if (Requests % 1000 == 0)
                //Console.WriteLine($"BSPCache: Requests={Requests}, Hits={Hits}");

            if (BSPTrees.TryGetValue(bspTree, out var result))
            {
                Hits++;
                return result;
            }

            // not cached, add it
            BSPTrees.Add(bspTree);
            return bspTree;
        }

        public static BSPTree Get(DatLoader.Entity.BSPTree _bspTree, Dictionary<ushort, DatLoader.Entity.Polygon> polys, DatLoader.Entity.CVertexArray vertexArray)
        {
            var bspTree = new BSPTree(_bspTree, polys, vertexArray);

            if (!Enabled)
                return bspTree;

            return Get(bspTree);
        }
    }
}
