using System;
using System.Collections.Generic;
using ACE.Server.Physics.BSP;

namespace ACE.Server.Physics.Entity
{
    public static class BSPCache
    {
        public static HashSet<BSPTree> BSPTrees;

        static BSPCache()
        {
            BSPTrees = new HashSet<BSPTree>();
        }

        public static int Requests;
        public static int Hits;

        public static BSPTree Get(BSPTree bspTree)
        {
            Requests++;

            //if (Requests % 1000 == 0)
                //Console.WriteLine($"BSPCache: Requests={Requests}, Hits={Hits}");

            BSPTrees.TryGetValue(bspTree, out var result);
            if (result != null)
            {
                Hits++;
                return result;
            }

            // not cached, add it
            BSPTrees.Add(bspTree);
            return bspTree;
        }

        public static BSPTree Get(DatLoader.Entity.BSPTree bspTree, Dictionary<ushort, DatLoader.Entity.Polygon> polys, DatLoader.Entity.CVertexArray vertexArray)
        {
            return Get(new BSPTree(bspTree, polys, vertexArray));
        }
    }
}
