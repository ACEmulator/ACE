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
    /// NOTE: the reason for not using the build int Matrix4X4 this was writen for column-major (i.e. OpenGL style, float[x][y])
    /// </summary>
    public class AceMatrix4X4
    {
        /// <summary>
        /// Gets the multiplicative identity matrix. x,y (col major)
        /// </summary>
        /// <param name="matrix">matrix by ref. x,y (col major)</param>
        public static void Mat4_Identity(ref AceMath3dUtil.mat4x4t matrix)
        {
            // x,y (col major)
            matrix.m4x4[0, 0] = 1.0f;
            matrix.m4x4[1, 0] = 0.0f;
            matrix.m4x4[2, 0] = 0.0f;
            matrix.m4x4[3, 0] = 0.0f;
            matrix.m4x4[0, 1] = 0.0f;
            matrix.m4x4[1, 1] = 1.0f;
            matrix.m4x4[2, 1] = 0.0f;
            matrix.m4x4[3, 1] = 0.0f;
            matrix.m4x4[0, 2] = 0.0f;
            matrix.m4x4[1, 2] = 0.0f;
            matrix.m4x4[2, 2] = 1.0f;
            matrix.m4x4[3, 2] = 0.0f;
            matrix.m4x4[0, 3] = 0.0f;
            matrix.m4x4[1, 3] = 0.0f;
            matrix.m4x4[2, 3] = 0.0f;
            matrix.m4x4[3, 3] = 1.0f;
        }

        /// <summary>
        /// Copies from the fromMatrix to the toMatrix
        /// I am not sure how this is different from toMatrix = fromMatrix; - but leaving it in for now.   Og II
        /// This could be just a c++ to c# difference.
        /// </summary>
        /// <param name="fromMatrix">matrix we are copying from</param>
        /// <param name="toMatrix">matrix we are copying to passed by ref</param>
        public static void Mat4_Copy(AceMath3dUtil.mat4x4t fromMatrix, ref AceMath3dUtil.mat4x4t toMatrix)
        {
            fromMatrix.m4x4.CopyTo(toMatrix.m4x4, 0);
        }

        /// <summary>
        /// Transposes the rows and columns of a matrix.
        /// </summary>
        /// <param name="matrix">matrix to transpose</param>
        public static void Mat4_Transpose(ref AceMath3dUtil.mat4x4t matrix)
        {
            AceMath3dUtil.mat4x4t temp = new AceMath3dUtil.mat4x4t();

            for (int i = 0; i < 4; i++)
            {
                temp.m4x4[i, 0] = matrix.m4x4[0, i];
                temp.m4x4[i, 1] = matrix.m4x4[1, i];
                temp.m4x4[i, 2] = matrix.m4x4[2, i];
                temp.m4x4[i, 3] = matrix.m4x4[3, i];
            }

            Mat4_Copy(matrix, ref temp);
        }

        /// <summary>
        /// a += b
        /// </summary>
        /// <param name="a">matrix by ref</param>
        /// <param name="b">matrix</param>
        public static void Mat4_Add(ref AceMath3dUtil.mat4x4t a, AceMath3dUtil.mat4x4t b)
        {
            for (int i = 0; i < 4; i++)
            {
                a.m4x4[i, 0] += b.m4x4[i, 0];
                a.m4x4[i, 1] += b.m4x4[i, 1];
                a.m4x4[i, 2] += b.m4x4[i, 2];
                a.m4x4[i, 3] += b.m4x4[i, 3];
            }
        }

        /// <summary>
        /// a -= b
        /// </summary>
        /// <param name="a">matrix by ref</param>
        /// <param name="b">matrix</param>
        public static void Mat4_Sub(ref AceMath3dUtil.mat4x4t a, AceMath3dUtil.mat4x4t b)
        {
            for (int i = 0; i < 4; i++)
            {
                a.m4x4[i, 0] -= b.m4x4[i, 0];
                a.m4x4[i, 1] -= b.m4x4[i, 1];
                a.m4x4[i, 2] -= b.m4x4[i, 2];
                a.m4x4[i, 3] -= b.m4x4[i, 3];
            }
        }

        /// <summary>
        /// multiply by a scalar
        /// </summary>
        /// <param name="a">matrix by ref</param>
        /// <param name="s">scaler value</param>
        public static void Mat4_Scale(ref AceMath3dUtil.mat4x4t a, float s)
        {
            for (int i = 0; i < 4; i++)
            {
                a.m4x4[i, 0] *= s;
                a.m4x4[i, 1] *= s;
                a.m4x4[i, 2] *= s;
                a.m4x4[i, 3] *= s;
            }
        }

        /// <summary>
        /// scale matrix - corresponds to glScalef().
        /// </summary>
        /// <param name="m">matrix by ref</param>
        /// <param name="x">x scaler</param>
        /// <param name="y">y scaler</param>
        /// <param name="z">z scaler</param>
        public static void Mat4_Scale(ref AceMath3dUtil.mat4x4t m, float x, float y, float z)
        {
            Mat4_Identity(ref m);
            m.m4x4[0, 0] = x;
            m.m4x4[1, 1] = y;
            m.m4x4[2, 2] = z;
        }


        /// <summary>
        /// Matrix multiplication.
        /// a *= b
        /// note that in general, A* B != B* A
        /// so ensure your transforms are in the right order first!
        /// generally to do forward transformations(local to world), this is scale -> rotate -> translate.
        /// </summary>
        /// <param name="a">matrix by ref</param>
        /// <param name="b">maxtrix to multiply by a*=b</param>
        public static void Mat4_MultiplyMat4_(ref AceMath3dUtil.mat4x4t a, AceMath3dUtil.mat4x4t b)
        {
            AceMath3dUtil.mat4x4t temp = new AceMath3dUtil.mat4x4t();

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    var accum = a.m4x4[0, row] * b.m4x4[col, 0];
                    accum += a.m4x4[1, row] * b.m4x4[col, 1];
                    accum += a.m4x4[2, row] * b.m4x4[col, 2];
                    accum += a.m4x4[3, row] * b.m4x4[col, 3];
                    temp.m4x4[col, row] = accum;
                }
            }
            //TODO: Verify this is correct - I changed the order on the copy. Og II
            Mat4_Copy(temp, ref a);
        }

        /// <summary>
        /// matrix/vector multiplication.
        /// v *= m
        /// Use these to apply a transformation matrix to a vector.
        /// </summary>
        /// <param name="v">vector4</param>
        /// <param name="m">matrix</param>
        public static void Mat4_MultiplyVector4(Vector4 v, ref AceMath3dUtil.mat4x4t m)
        {
            float[] temp = new float[4];
            //float accum;

            for (int j = 0; j < 4; j++)
            {
                temp[j] = m.m4x4[0, j] * v.X;
                temp[j] += m.m4x4[1, j] * v.Y;
                temp[j] += m.m4x4[2, j] * v.Z;
                temp[j] += m.m4x4[3, j] * v.W;
            }

            v.X = temp[0];
            v.Y = temp[1];
            v.Z = temp[2];
            v.W = temp[3];
        }

        /// <summary>
        /// convert to vec4, multiply, convert back.
        /// </summary>
        /// <param name="v3">Vector3</param>
        /// <param name="m">matrix by ref</param>
        public static void Mat4_MultiplyVector3(Vector3 v3, ref AceMath3dUtil.mat4x4t m)
        {
            Vector4 v4 = new Vector4(v3, 1.0f);
            Mat4_MultiplyVector4(v4, ref m);

            v3.X = v4.X;
            v3.Y = v4.Y;
            v3.Z = v4.Z;
        }

        /// <summary>
        /// Translation matrix (set object position)
        /// corresponds to glTranslatef() opengl call.
        /// </summary>
        /// <param name="m">matrix by ref</param>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <param name="z">z coord</param>
        public static void Mat4_Translate(ref AceMath3dUtil.mat4x4t m, float x, float y, float z)
        {
            Mat4_Identity(ref m);
            m.m4x4[3, 0] = x;
            m.m4x4[3, 1] = y;
            m.m4x4[3, 2] = z;
        }

        /// <summary>
        /// uniform scale (x=y=z). Should be true for most (all?) cases in AC.
        /// </summary>
        /// <param name="m">Matrix by ref</param>
        /// <param name="s">scaler value</param>
        public static void Mat4_ScaleUniform(ref AceMath3dUtil.mat4x4t m, float s)
        {
            Mat4_Scale(ref m, s, s, s);
        }

        /// <summary>
        /// rotate matrix - corresponds to glRotatef()
        /// Notes:  angle in radians.  x,y,z specify axis of rotation
        /// ** assumes axis vector is normalized; Don't call this with a non-unit-length axis!
        /// To use with quaternions: convert to axis/angle, input into this function.
        /// </summary>
        /// <param name="m">matrix by ref</param>
        /// <param name="angle">angle in radians</param>
        /// <param name="axis">axis of rotation Vector3</param>
        public static void Mat4_Rotate(ref AceMath3dUtil.mat4x4t m, float angle, Vector3 axis)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);
            float ci = 1.0f - c;

            Mat4_Identity(ref m);

            m.m4x4[0, 0] = (x * x * ci) + c;
            m.m4x4[1, 0] = (x * y * ci) - (z * s);
            m.m4x4[2, 0] = (x * z * ci) + (y * s);

            m.m4x4[0, 1] = (y * x * ci) + (z * s);
            m.m4x4[1, 1] = (y * y * ci) + c;
            m.m4x4[2, 1] = (y * z * ci) - (x * s);

            m.m4x4[0, 2] = (z * x * ci) - (y * s);
            m.m4x4[1, 2] = (z * y * ci) + (x * s);
            m.m4x4[2, 2] = (z * z * ci) + c;
        }

        /// <summary>
        /// matrix inversion, m = m^-1 - note Returns false if not invertable (det=0)
        /// </summary>
        /// <param name="m">matrix by ref</param>
        /// <returns></returns>
        public static bool Mat4_Invert(ref AceMath3dUtil.mat4x4t m)
        {
            AceMath3dUtil.mat4x4t temp = new AceMath3dUtil.mat4x4t();
            float[] s = new float[6];
            float[] c = new float[6];

            s[0] = m.m4x4[0, 0] * m.m4x4[1, 1] - m.m4x4[1, 0] * m.m4x4[0, 1];
            s[1] = m.m4x4[0, 0] * m.m4x4[1, 2] - m.m4x4[1, 0] * m.m4x4[0, 2];
            s[2] = m.m4x4[0, 0] * m.m4x4[1, 3] - m.m4x4[1, 0] * m.m4x4[0, 3];
            s[3] = m.m4x4[0, 1] * m.m4x4[1, 2] - m.m4x4[1, 1] * m.m4x4[0, 2];
            s[4] = m.m4x4[0, 1] * m.m4x4[1, 3] - m.m4x4[1, 1] * m.m4x4[0, 3];
            s[5] = m.m4x4[0, 2] * m.m4x4[1, 3] - m.m4x4[1, 2] * m.m4x4[0, 3];

            c[0] = m.m4x4[2, 0] * m.m4x4[3, 1] - m.m4x4[3, 0] * m.m4x4[2, 1];
            c[1] = m.m4x4[2, 0] * m.m4x4[3, 2] - m.m4x4[3, 0] * m.m4x4[2, 2];
            c[2] = m.m4x4[2, 0] * m.m4x4[3, 3] - m.m4x4[3, 0] * m.m4x4[2, 3];
            c[3] = m.m4x4[2, 1] * m.m4x4[3, 2] - m.m4x4[3, 1] * m.m4x4[2, 2];
            c[4] = m.m4x4[2, 1] * m.m4x4[3, 3] - m.m4x4[3, 1] * m.m4x4[2, 3];
            c[5] = m.m4x4[2, 2] * m.m4x4[3, 3] - m.m4x4[3, 2] * m.m4x4[2, 3];

            var det = (s[0] * c[5] - s[1] * c[4] + s[2] * c[3] + s[3] * c[2] - s[4] * c[1] + s[5] * c[0]);

            if (Math.Abs(det) < AceMath3dUtil.FloatTolerance)
                return false;

            var idet = 1.0f / det;

            temp.m4x4[0, 0] = (m.m4x4[1, 1] * c[5] - m.m4x4[1, 2] * c[4] + m.m4x4[1, 3] * c[3]) * idet;
            temp.m4x4[0, 1] = (-m.m4x4[0, 1] * c[5] + m.m4x4[0, 2] * c[4] - m.m4x4[0, 3] * c[3]) * idet;
            temp.m4x4[0, 2] = (m.m4x4[3, 1] * s[5] - m.m4x4[3, 2] * s[4] + m.m4x4[3, 3] * s[3]) * idet;
            temp.m4x4[0, 3] = (-m.m4x4[2, 1] * s[5] + m.m4x4[2, 2] * s[4] - m.m4x4[2, 3] * s[3]) * idet;

            temp.m4x4[1, 0] = (-m.m4x4[1, 0] * c[5] + m.m4x4[1, 2] * c[2] - m.m4x4[1, 3] * c[1]) * idet;
            temp.m4x4[1, 1] = (m.m4x4[0, 0] * c[5] - m.m4x4[0, 2] * c[2] + m.m4x4[0, 3] * c[1]) * idet;
            temp.m4x4[1, 2] = (-m.m4x4[3, 0] * s[5] + m.m4x4[3, 2] * s[2] - m.m4x4[3, 3] * s[1]) * idet;
            temp.m4x4[1, 3] = (m.m4x4[2, 0] * s[5] - m.m4x4[2, 2] * s[2] + m.m4x4[2, 3] * s[1]) * idet;

            temp.m4x4[2, 0] = (m.m4x4[1, 0] * c[4] - m.m4x4[1, 1] * c[2] + m.m4x4[1, 3] * c[0]) * idet;
            temp.m4x4[2, 1] = (-m.m4x4[0, 0] * c[4] + m.m4x4[0, 1] * c[2] - m.m4x4[0, 3] * c[0]) * idet;
            temp.m4x4[2, 2] = (m.m4x4[3, 0] * s[4] - m.m4x4[3, 1] * s[2] + m.m4x4[3, 3] * s[0]) * idet;
            temp.m4x4[2, 3] = (-m.m4x4[2, 0] * s[4] + m.m4x4[2, 1] * s[2] - m.m4x4[2, 3] * s[0]) * idet;

            temp.m4x4[3, 0] = (-m.m4x4[1, 0] * c[3] + m.m4x4[1, 1] * c[1] - m.m4x4[1, 2] * c[0]) * idet;
            temp.m4x4[3, 1] = (m.m4x4[0, 0] * c[3] - m.m4x4[0, 1] * c[1] + m.m4x4[0, 2] * c[0]) * idet;
            temp.m4x4[3, 2] = (-m.m4x4[3, 0] * s[3] + m.m4x4[3, 1] * s[1] - m.m4x4[3, 2] * s[0]) * idet;
            temp.m4x4[3, 3] = (m.m4x4[2, 0] * s[3] - m.m4x4[2, 1] * s[1] + m.m4x4[2, 2] * s[0]) * idet;

            //TODO: Verify this is correct - I changed the order on the copy. Og II
            Mat4_Copy(temp, ref m);
            return true;
        }
    }
}
