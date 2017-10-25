using System;
using System.Numerics;

// Matrix4x4
// void Mat4_Identity(mat4x4t &m);
// void Mat4_Copy(mat4x4t &a, mat4x4t &b);
// void Mat4_Transpose(mat4x4t &a);
// void Mat4_Add(mat4x4t &a, mat4x4t &b);
// void Mat4_Sub(mat4x4t &a, mat4x4t &b);
// void Mat4_Scale(mat4x4t &a, float s);
// void Mat4_Mul_Mat4(mat4x4t &a, mat4x4t &b);
// void Mat4_Mul_Vec4(Vector4 &v, mat4x4t &m);
// void Mat4_Mul_Vec3(Vector3 &v3, mat4x4t &m);
// void Mat4_Translate(mat4x4t &m, float x, float y, float z);
// void Mat4_Scale(mat4x4t &m, float x, float y, float z);
// void Mat4_ScaleUniform(mat4x4t &m, float s);
// void Mat4_Rotate(mat4x4t &m, float angle, Vector3 &axis);
// bool Mat4_Invert(mat4x4t &m);

namespace ACE.Entity
{
    /// <summary>
    /// Matrix4X4
    /// Adding methods ported from Anonymous' physics code.   Thanks for all the hard work! Og II
    /// </summary>
    public class AceMatrix4X4
    {
        //  4x4 matrix
        // internally stored as column-major (i.e. OpenGL style, float[x][y])

        /// <summary>
        /// Gets the multiplicative identity matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public static void Matrix4Identity(ref float[,] matrix)
        {
            // x,y (col major)
            matrix[0, 0] = 1.0f;
            matrix[1, 0] = 0.0f;
            matrix[2, 0] = 0.0f;
            matrix[3, 0] = 0.0f;
            matrix[0, 1] = 0.0f;
            matrix[1, 1] = 1.0f;
            matrix[2, 1] = 0.0f;
            matrix[3, 1] = 0.0f;
            matrix[0, 2] = 0.0f;
            matrix[1, 2] = 0.0f;
            matrix[2, 2] = 1.0f;
            matrix[3, 2] = 0.0f;
            matrix[0, 3] = 0.0f;
            matrix[1, 3] = 0.0f;
            matrix[2, 3] = 0.0f;
            matrix[3, 3] = 1.0f;
        }

        /// <summary>
        /// Matrix4 Copy - Copies from the array a to the array b
        /// </summary>
        /// <param name="fromArray">array we are copying from</param>
        /// <param name="toArray">array we are copying to</param>
        public static void Matrix4Copy(ref float[,] fromArray, ref float[,] toArray)
        {
            fromArray.CopyTo(toArray, 0);
        }

        public static void Matrix4Transpose(ref float[,] a)
        {
            float[,] temp = new float[4, 4];

            for (int i = 0; i < 4; i++)
            {
                temp[i, 0] = a[0, i];
                temp[i, 1] = a[1, i];
                temp[i, 2] = a[2, i];
                temp[i, 3] = a[3, i];
            }

            Matrix4Copy(ref a, ref temp);
        }

        // a += b
        public static void Matrix4Add(ref float[,] a, ref float[,] b)
        {
            for (int i = 0; i < 4; i++)
            {
                a[i, 0] += b[i, 0];
                a[i, 1] += b[i, 1];
                a[i, 2] += b[i, 2];
                a[i, 3] += b[i, 3];
            }
        }

        // a -= b
        public static void Matrix4Sub(ref float[,] a, ref float[,] b)
        {
            for (int i = 0; i < 4; i++)
            {
                a[i, 0] -= b[i, 0];
                a[i, 1] -= b[i, 1];
                a[i, 2] -= b[i, 2];
                a[i, 3] -= b[i, 3];
            }
        }

        // multiply by a scalar
        public static void Matrix4Scale(ref float[,] a, float s)
        {
            for (int i = 0; i < 4; i++)
            {
                a[i, 0] *= s;
                a[i, 1] *= s;
                a[i, 2] *= s;
                a[i, 3] *= s;
            }
        }

        // scale matrix
        // corresponds to glScalef().
        public static void Matrix4Scale(ref float[,] m, float x, float y, float z)
        {
            Matrix4Identity(ref m);
            m[0, 0] = x;
            m[1, 1] = y;
            m[2, 2] = z;
        }

        // matrix multiplication.
        // a *= b
        // note that in general, A*B != B*A
        // so ensure your transforms are in the right order first!
        // generally to do forward transformations (local to world), this is scale -> rotate -> translate.
        public static void Matrix4MultiplyMatrix4(ref float[,] a, ref float[,] b)
        {
            float[,] temp = new float[4, 4];

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    var accum = a[0, row] * b[col, 0];
                    accum += a[1, row] * b[col, 1];
                    accum += a[2, row] * b[col, 2];
                    accum += a[3, row] * b[col, 3];
                    temp[col, row] = accum;
                }
            }

            Matrix4Copy(ref a, ref temp);
        }

        // matrix/vector multiplication.
        // v *= m
        // Use these to apply a transformation matrix to a vector.
        public static void Matrix4MultiplyVector4(Vector4 v, ref float[,] m)
        {
            float[] temp = new float[4];
            //float accum;

            for (int j = 0; j < 4; j++)
            {
                temp[j] = m[0, j] * v.X;
                temp[j] += m[1, j] * v.Y;
                temp[j] += m[2, j] * v.Z;
                temp[j] += m[3, j] * v.W;
            }

            v.X = temp[0];
            v.Y = temp[1];
            v.Z = temp[2];
            v.W = temp[3];
        }

        public static void Matrix4MultiplyVector3(Vector3 v3, ref float[,] m)
        {
            // convert to vec4, multiply, convert back.
            Vector4 v4 = new Vector4(v3, 1.0f);
            Matrix4MultiplyVector4(v4, ref m);

            v3.X = v4.X;
            v3.Y = v4.Y;
            v3.Z = v4.Z;
        }

        // translation matrix (set object position)
        // corresponds to glTranslatef() opengl call.
        public static void Matrix4Translate(ref float[,] m, float x, float y, float z)
        {
            Matrix4Identity(ref m);
            m[3, 0] = x;
            m[3, 1] = y;
            m[3, 2] = z;
        }

        // uniform scale (x=y=z). Should be true for most (all?) cases in AC.
        public static void Matrix4ScaleUniform(ref float[,] m, float s)
        {
            Matrix4Scale(ref m, s, s, s);
        }

        // rotate matrix
        // corresponds to glRotatef()
        //
        // Notes:
        //    angle in radians.
        //    x,y,z specify axis of rotation
        //    ** assumes axis vector is normalized; Don't call this with a non-unit-length axis!
        //
        // To use with quaternions: convert to axis/angle, input into this function.
        public static void Matrix4Rotate(ref float[,] m, float angle, Vector3 axis)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);
            float ci = 1.0f - c;

            Matrix4Identity(ref m);

            m[0, 0] = (x * x * ci) + c;
            m[1, 0] = (x * y * ci) - (z * s);
            m[2, 0] = (x * z * ci) + (y * s);

            m[0, 1] = (y * x * ci) + (z * s);
            m[1, 1] = (y * y * ci) + c;
            m[2, 1] = (y * z * ci) - (x * s);

            m[0, 2] = (z * x * ci) - (y * s);
            m[1, 2] = (z * y * ci) + (x * s);
            m[2, 2] = (z * z * ci) + c;
        }

        // matrix inversion, m = m^-1
        // Returns false if not invertable (det=0)
        public static bool Matrix4Invert(ref float[,] m)
        {
            float[,] temp = new float[4, 4];
            float[] s = new float[6];
            float[] c = new float[6];

            s[0] = m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1];
            s[1] = m[0, 0] * m[1, 2] - m[1, 0] * m[0, 2];
            s[2] = m[0, 0] * m[1, 3] - m[1, 0] * m[0, 3];
            s[3] = m[0, 1] * m[1, 2] - m[1, 1] * m[0, 2];
            s[4] = m[0, 1] * m[1, 3] - m[1, 1] * m[0, 3];
            s[5] = m[0, 2] * m[1, 3] - m[1, 2] * m[0, 3];

            c[0] = m[2, 0] * m[3, 1] - m[3, 0] * m[2, 1];
            c[1] = m[2, 0] * m[3, 2] - m[3, 0] * m[2, 2];
            c[2] = m[2, 0] * m[3, 3] - m[3, 0] * m[2, 3];
            c[3] = m[2, 1] * m[3, 2] - m[3, 1] * m[2, 2];
            c[4] = m[2, 1] * m[3, 3] - m[3, 1] * m[2, 3];
            c[5] = m[2, 2] * m[3, 3] - m[3, 2] * m[2, 3];

            var det = (s[0] * c[5] - s[1] * c[4] + s[2] * c[3] + s[3] * c[2] - s[4] * c[1] + s[5] * c[0]);

            if (Math.Abs(det) < AceMath3d.FloatTolerance)
                return false;

            var idet = 1.0f / det;

            temp[0, 0] = (m[1, 1] * c[5] - m[1, 2] * c[4] + m[1, 3] * c[3]) * idet;
            temp[0, 1] = (-m[0, 1] * c[5] + m[0, 2] * c[4] - m[0, 3] * c[3]) * idet;
            temp[0, 2] = (m[3, 1] * s[5] - m[3, 2] * s[4] + m[3, 3] * s[3]) * idet;
            temp[0, 3] = (-m[2, 1] * s[5] + m[2, 2] * s[4] - m[2, 3] * s[3]) * idet;

            temp[1, 0] = (-m[1, 0] * c[5] + m[1, 2] * c[2] - m[1, 3] * c[1]) * idet;
            temp[1, 1] = (m[0, 0] * c[5] - m[0, 2] * c[2] + m[0, 3] * c[1]) * idet;
            temp[1, 2] = (-m[3, 0] * s[5] + m[3, 2] * s[2] - m[3, 3] * s[1]) * idet;
            temp[1, 3] = (m[2, 0] * s[5] - m[2, 2] * s[2] + m[2, 3] * s[1]) * idet;

            temp[2, 0] = (m[1, 0] * c[4] - m[1, 1] * c[2] + m[1, 3] * c[0]) * idet;
            temp[2, 1] = (-m[0, 0] * c[4] + m[0, 1] * c[2] - m[0, 3] * c[0]) * idet;
            temp[2, 2] = (m[3, 0] * s[4] - m[3, 1] * s[2] + m[3, 3] * s[0]) * idet;
            temp[2, 3] = (-m[2, 0] * s[4] + m[2, 1] * s[2] - m[2, 3] * s[0]) * idet;

            temp[3, 0] = (-m[1, 0] * c[3] + m[1, 1] * c[1] - m[1, 2] * c[0]) * idet;
            temp[3, 1] = (m[0, 0] * c[3] - m[0, 1] * c[1] + m[0, 2] * c[0]) * idet;
            temp[3, 2] = (-m[3, 0] * s[3] + m[3, 1] * s[1] - m[3, 2] * s[0]) * idet;
            temp[3, 3] = (m[2, 0] * s[3] - m[2, 1] * s[1] + m[2, 2] * s[0]) * idet;

            Matrix4Copy(ref m, ref temp);
            return true;
        }
    }
}
