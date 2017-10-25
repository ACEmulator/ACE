using System.IO;
using System.Numerics;

// Matrix4x4
// void Mat4_Identity(mat4x4t &m);
// void Mat4_Copy(mat4x4t &a, mat4x4t &b);
// void Mat4_Transpose(mat4x4t &a);
// void Mat4_Add(mat4x4t &a, mat4x4t &b);
// void Mat4_Sub(mat4x4t &a, mat4x4t &b);
// void Mat4_Scale(mat4x4t &a, float s);
// void Mat4_Mul_Mat4(mat4x4t &a, mat4x4t &b);
// void Mat4_Mul_Vec4(vec4t &v, mat4x4t &m);
// void Mat4_Mul_Vec3(vec3t &v3, mat4x4t &m);
// void Mat4_Translate(mat4x4t &m, float x, float y, float z);
// void Mat4_Scale(mat4x4t &m, float x, float y, float z);
// void Mat4_ScaleUniform(mat4x4t &m, float s);
// void Mat4_Rotate(mat4x4t &m, float angle, vec3t &axis);
// bool Mat4_Invert(mat4x4t &m);
namespace ACE.Entity
{
    /// <summary>
    /// Matrix4X4
    /// Adding methods ported from Anonymous' physics code.   Thanks for all the hard work! Og II
    /// </summary>
    public class AceMatrix4X4
    {
        /// <summary>
        ///  4x4 matrix
        /// internally stored as column-major (i.e. OpenGL style, float[x][y])
        /// </summary>
        private readonly float[,] matrix4X4 = new float[4, 4];

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

    }
}
