using System;
using System.Collections.Generic;

using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Entity
{
    public static class PolygonCache
    {
        /// <summary>
        /// Default is false
        /// </summary>
        public static bool CacheEnabled;

        public static readonly HashSet<Polygon> Polygons = new HashSet<Polygon>();

        public static int Requests;
        public static int Hits;

        public static Polygon Get(Polygon p)
        {
            if (!CacheEnabled)
                return p;

            Requests++;

            //if (Requests % 10000 == 0)
                //Console.WriteLine($"PolygonCache: Requests={Requests}, Hits={Hits}");

            if (Polygons.TryGetValue(p, out var result))
            {
                Hits++;
                return result;
            }

            // not cached, add it
            Polygons.Add(p);
            return p;
        }

        public static Polygon Get(DatLoader.Entity.Polygon p, CVertexArray v)
        {
            var polygon = new Polygon(p, v);

            if (!CacheEnabled)
                return polygon;

            return Get(polygon);
        }
    }
}
