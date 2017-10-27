using System.Numerics;
using System;

namespace ACE.Entity
{
    /// <summary>
    /// Math routines used by a number of classes.
    /// Adding methods ported from Anonymous' physics code.   Thanks for all the hard work! Og II
    /// </summary>
    public static class AceMath3dUtil
    {
        /// <summary>
        /// 4x4 matrix - internally stored as column-major (i.e. OpenGL style, float[x][y])
        /// </summary>
        public class mat4x4t
        {
            public float[,] m4x4 = new float[4, 4];
        }

        public const double FloatTolerance = (1e-3f);

        internal static bool FloatCompareEqual(float a, float b)
        {
            float delta = Math.Abs(a - b);
            return (delta < FloatTolerance);
        }

        /// <summary>
        /// Compare to see if a vector is 0 within a given fault tolerance.
        /// </summary>
        /// <param name="a">Vector3 you want to compare to zero.</param>
        /// <returns></returns>
        public static bool Vector3EqualZero(Vector3 a)
        {
            return (FloatCompareEqual(a.X, 0.0f) && FloatCompareEqual(a.Y, 0.0f) && FloatCompareEqual(a.Z, 0.0f));
        }

        internal static float InvSqrt(float f)
        {
            return (1.0f / (float)Math.Sqrt(f));
        }

        internal static float ACosZ(float fValue)
        {
            if (!(-1.0f < fValue)) return ((float) Math.PI);
            return fValue < 1.0f ? (float) Math.Acos(fValue) : 0.0f;
        }
    }
}
