using System;
using System.Collections.Generic;

using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Entity
{
    public static class VertexCache
    {
        public static bool Enabled = false;

        public static readonly HashSet<Vertex> Vertices = new HashSet<Vertex>();

        public static int Requests;
        public static int Hits;

        public static int Count => Vertices.Count;

        public static void Clear()
        {
            Vertices.Clear();
        }

        public static Vertex Get(Vertex v)
        {
            if (!Enabled) return v;

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

            if (!Enabled)
                return vertex;

            return Get(vertex);
        }
    }
}
