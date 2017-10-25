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

// float Vec3_Dot(vec3t &a, vec3t &b);

// void Vec3_Cross(vec3t &result, vec3t &a, vec3t &b);

// void Vec3_Reflect(vec3t &r, vec3t &v, vec3t &n);

// bool Vec3_EqZero(vec3t &a, float tolerance);



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
        /// This method retures the square of the length of the vector.
        /// It may not be used, it looked like it was called to get the length which we have built in.
        /// </summary>
        /// <param name="a">The lenght of the vector squared.</param>
        /// <returns></returns>
        public static float Vector3LengthSquared(Vector3 a)
        {
            return ((a.X * a.X) + (a.Y * a.Y) + (a.Z * a.Z));
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
