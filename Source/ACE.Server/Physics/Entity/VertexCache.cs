using System;
using System.Collections.Generic;

using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Entity
{
    public static class VertexCache
    {
        public static readonly HashSet<Vertex> Vertices = new HashSet<Vertex>();

        public static int Requests;
        public static int Hits;

        public static Vertex Get(Vertex v)
        {
            Requests++;

            //if (Requests % 100000 == 0)
                //Console.WriteLine($"VertexCache: Requests={Requests}, Hits={Hits}");

            Vertices.TryGetValue(v, out var result);
            if (result != null)
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

            return Get(vertex);
        }
    }
}
