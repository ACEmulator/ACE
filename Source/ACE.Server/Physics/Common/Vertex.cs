using System;
using System.Numerics;

namespace ACE.Server.Physics.Entity
{
    public class Vertex: IEquatable<Vertex>
    {
        public ushort Index;
        public Vector3 Origin;
        public Vector3 Normal;

        public Vertex() { }

        public Vertex(ushort index)
        {
            Index = index;
        }

        public Vertex(DatLoader.Entity.SWVertex v)
        {
            Origin = v.Origin;
            Normal = v.Normal;

            // omitted UV texture coordinates
        }

        public Vertex(Vector3 origin)
        {
            Origin = origin;
        }

        public static Vector3 operator+ (Vertex a, Vertex b)
        {
            return a.Origin + b.Origin;
        }

        public static Vector3 operator- (Vertex a, Vertex b)
        {
            return a.Origin - b.Origin;
        }

        public static Vector3 operator* (Vertex a, Vertex b)
        {
            return a.Origin * b.Origin;
        }

        public static Vector3 operator/ (Vertex a, Vertex b)
        {
            return a.Origin / b.Origin;
        }

        public bool Equals(Vertex v)
        {
            if (v == null) return false;

            return Index == v.Index && Origin.X == v.Origin.X && Origin.Y == v.Origin.Y && Origin.Z == v.Origin.Z &&
                Normal.X == v.Normal.X && Normal.Y == v.Normal.Y && Normal.Z == v.Normal.Z;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            hash = (hash * 397) ^ Index.GetHashCode();
            hash = (hash * 397) ^ Origin.GetHashCode();
            hash = (hash * 397) ^ Normal.GetHashCode();

            return hash;
        }
    }
}
