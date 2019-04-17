using System;
using System.Collections.Generic;

using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Entity
{
    public static class PolygonCache
    {
        public static bool Enabled = false;

        public static readonly HashSet<Polygon> Polygons = new HashSet<Polygon>();

        public static int Requests;
        public static int Hits;

        public static int Count => Polygons.Count;

        public static void Clear()
        {
            Polygons.Clear();
        }

        public static Polygon Get(Polygon p)
        {
            if (!Enabled)
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

            if (!Enabled)
                return polygon;

            return Get(polygon);
        }
    }
}
