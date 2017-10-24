using System.IO;
using System.Numerics;

namespace ACE.Entity
{
    /// <summary>
    /// Vector3 with ACE Serialize
    /// Adding methods ported from Anonymous' physics code.   Thanks for all the hard work! Og II
    /// </summary>
    public class AceVector3
    {
        private Vector3 vector;

        public AceVector3(float x, float y, float z)
        {
            vector.X = x; vector.Y = y; vector.Z = z;
        }

        /// <summary>
        /// Copy one vector3 to another.
        /// </summary>
        /// <param name="a">Vector3</param>
        /// <param name="b">Vector3</param>
        public static void Vector3Copy(Vector3 a, Vector3 b)
        {
            a.X = b.X;
            a.Y = b.Y;
            a.Z = b.Z;
        }

        /// <summary>
        /// Vector4 is used internally to convert vec3 vectors into a suitable form for multiplying with matrices.
        /// Generally you want to have a W of 0 for direction vectors, and a W of 1 for position vectors.
        /// This ensures that direction vectors aren't translated, and that position vectors are translated -
        /// </summary>
        /// <param name="a">Vector4</param>
        /// <param name="b">Vector3</param>
        /// <param name="w">W component</param>
        public static void Vector3ToVector4(out Vector4 a, Vector3 b, float w)
        {
            a.X = b.X;
            a.Y = b.Y;
            a.Z = b.Z;
            a.W = w;
        }

        /// <summary>
        /// Add two vector3's together
        /// </summary>
        /// <param name="a">Vector3</param>
        /// <param name="b">Vector3</param>
        public static void Vector3Add(Vector3 a, Vector3 b)
        {
            a.X += b.X;
            a.Y += b.Y;
            a.Z += b.Z;
        }

        /// <summary>
        /// Subtract two vector3's from each other
        /// </summary>
        /// <param name="a">Vector3</param>
        /// <param name="b">Vector3</param>
        public static void Vector3Subtract(Vector3 a, Vector3 b)
        {
            a.X -= b.X;
            a.Y -= b.Y;
            a.Z -= b.Z;
        }

        public void Update(float x, float y, float z)
        {
            vector.X = x; vector.Y = y; vector.Z = z;
        }

        public Vector3 Forward()
        {
            // todo figure out how to calculate vector forward for AC.
            return vector;
        }

        public Vector3 Get()
        {
            return vector;
        }

        public void Serialize(BinaryWriter payload)
        {
            payload.Write(vector.X);
            payload.Write(vector.Y);
            payload.Write(vector.Z);
        }
    }
}
