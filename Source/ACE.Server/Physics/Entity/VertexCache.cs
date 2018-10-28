using System;
using System.Collections.Generic;

using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Entity
{
    public static class VertexCache
    {
        /// <summary>
        /// Default is false
        /// </summary>
        public static bool CacheEnabled;

        public static readonly HashSet<Vertex> Vertices = new HashSet<Vertex>();

        public static int Requests;
        public static int Hits;

        public static Vertex Get(Vertex v)
        {
            if (!CacheEnabled)
                return v;

            Requests++;

            //if (Requests % 100000 == 0)
                //Console.WriteLine($"VertexCache: Requests={Requests}, Hits={Hits}");

            if (Vertices.TryGetValue(v, out var result))
            {
                Hits++;
                return result;
            }

            // not cached, add it
            Vertices.Add(v);
            return v;
        }

        public static Vertex Get(SWVertex swv)
        {
            var vertex = new Vertex(swv);

            if (!CacheEnabled)
                return vertex;

            return Get(vertex);
        }
    }
}
