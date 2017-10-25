using System.IO;
using System.Numerics;

// PHYSICS PORTING NOTES:
// Vector3

// void Vec3_Copy(vec3t &a, vec3t &b);
// USE: Vector3 vectCopy = new vectCopy();  vectCopy = vectOrig;

// void Vec3_ToVec4(vec4t &a, vec3t &b, float w);
// USE: public Vector4(Vector3 value, float w) : Vector4 aVect4 = new Vector4(vect3, W);

// void Vec3_Add(vec3t &a, vec3t &b);
// USE: public static Vector3 Vector3.Add(Vector3 left, Vector3 right) : Vector3 sumVector = Vector3.Add(left, right);

// void Vec3_Sub(vec3t &a, vec3t &b);
// USE: public static Vector3 Vector3.Subtract(Vector3 left, Vector3 right) : Vector3 diffVector = Vector3.Subtract(left, right);

// void Vec3_Neg(vec3t &a);
// USE: public static Vector3 Vector3.Netate(Vector3 value) : Vector3 negVector = Vector3.Negate(value);

// void Vec3_Scale(vec3t &a, float s);
// USE:  public static Vector3 Vector3Scale(Vector3 a, float s) : Vector3 scaledVector = Vector3Scale(value, s);

// float Vec3_MagnitudeSQ(vec3t &a);
// USE:  public static float Vector3LengthSquared(Vector3 a) : float vectorLengthSq = Vector3LengthSquared(a);

// float Vec3_Magnitude(vec3t &a);
// USE:  public float Length() : Vector3 aVector.Length();

// void Vec3_Normalize(vec3t &a);
// Notes: make a vector unit length (length = 1).
// USE: public static Vector3 Normalize(Vector3 value) : Vector3 normVector = Vector3.Normalize(value);

// float Vec3_Dot(vec3t &a, vec3t &b);
// USE: public static float Dot(Vector3 vector1, Vector3 vector2) : float result = Vector3.Dot(vector1, vector2)
// Notes: dot product (aka inner product)
// *** if you need to use this for lighting or angle calculations, normalize the vectors first! ***
// for normalized vectors a & b, effectively returns the cosine of the angle between them.

// void Vec3_Cross(vec3t &result, vec3t &a, vec3t &b);
// USE: public static Vector3 Cross(Vector3 vector1, Vector3 vector2) : Vector3 crossVector = Vector3.Cross(vector1, vector2)
// Notes: cross product returns a vector perpendicular to the two input vectors (the "normal vector").
// If the inputs are of unit length, then the result vector is also of unit length.
// AC calculates the slope of a triangle by taking the Z value of the normal.

// void Vec3_Reflect(vec3t &r, vec3t &v, vec3t &n);
// USE: public static Vector3 Reflect(Vector3 vector, Vector3 normal) : Vector3 reflVector = Vector3.Reflect(vector, normal)
// Notes: // vector reflection  inputs: vector v, normal vector n (normal vector of the plane you want to reflect from)
// resulting vector in r.

// bool Vec3_EqZero(vec3t &a, float tolerance);
// USE:  public static bool Vector3EqualZero(Vector3 a) : bool isZero = Vector3EqualZero(vector);

// public const double M_PI = (3.1415926535897932384626433832795);
// USE: Math.PI




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
        /// This method is used to scale a vector3 by a float value.
        /// </summary>
        /// <param name="a">The vector3 to scale</param>
        /// <param name="s">float value used to scale the vector3 by</param>
        public static Vector3 Vector3Scale(Vector3 a, float s)
        {
            a.X *= s;
            a.Y *= s;
            a.Z *= s;
            return a;
        }

        /// <summary>
        /// This method returns the square of the length of the vector.
        /// It may not be used, it looked like it was called to get the length which we have built in.
        /// </summary>
        /// <param name="a">The length of the vector squared.</param>
        /// <returns></returns>
        public static float Vector3LengthSquared(Vector3 a)
        {
            return ((a.X * a.X) + (a.Y * a.Y) + (a.Z * a.Z));
        }

        /// <summary>
        /// Compare to see if a vector is 0 within a given fault tolerance.
        /// </summary>
        /// <param name="a">Vector3 you want to compare to zero.</param>
        /// <returns></returns>
        public static bool Vector3EqualZero(Vector3 a)
        {
            return (AceMath3d.FloatCompareEqual(a.X, 0.0f) && AceMath3d.FloatCompareEqual(a.Y, 0.0f) && AceMath3d.FloatCompareEqual(a.Z, 0.0f));
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
