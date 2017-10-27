using System;
using System.Numerics;


// public const double M_PI = (3.1415926535897932384626433832795);
// USE: Math.PI

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

        internal static bool Fl_CmpEQ(float a, float b)
        {
            float delta = Math.Abs(a - b);
            return (delta < FloatTolerance);
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

        public class SPHERE_DEF
        {
            public Vector3 pos = new Vector3(); // pos of center
            public float radius; // needs to be > 0.0f
        }

        // Upright capped cylinder (Z-aligned)
        public class CYLINDER_DEF
        {
            public Vector3 pos = new Vector3(); // pos of cylinder at (x,y,z0), pos.z not used.
            public float z0;
            public float z1;
            public float radius;
        }

        // AABB (axis-aligned bounding box) ('BBox' class in AC)
        public class AABB_DEF
        {
            public Vector3 p0 = new Vector3();
            public Vector3 p1 = new Vector3();
        }

        public class CIRCLE_DEF
        {
            public float x;
            public float y;
            public float radius;
        }
    }
}
