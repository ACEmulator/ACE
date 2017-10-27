using System.Numerics;
using System;

namespace ACE.Entity
{
    public static class AceQuaternion
    {
        // Quaternion
        public static void Quat_Copy(Vector4 q1, Vector4 q2)
        {
            q1.W = q2.W;
            q1.X = q2.X;
            q1.Y = q2.Y;
            q1.Z = q2.Z;
        }
        public static void Quat_Identity(Vector4 q)
        {
            q.X = q.Y = q.Z = 0.0f;
            q.W = 1.0f;
        }
        public static void Quat_Add(Vector4 a, Vector4 b)
        {
            a.X += b.X;
            a.Y += b.Y;
            a.Z += b.Z;
            a.W += b.W;
        }
        public static void Quat_Sub(Vector4 a, Vector4 b)
        {
            a.X -= b.X;
            a.Y -= b.Y;
            a.Z -= b.Z;
            a.W -= b.W;
        }
        public static void Quat_Scale(Vector4 a, float s)
        {
            a.X *= s;
            a.Y *= s;
            a.Z *= s;
            a.W *= s;
        }

        // no zero-length checking.
        public static void Quat_Normalize(Vector4 q)
        {
            float fInvLen = AceMath3dUtil.InvSqrt((q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z) + (q.W * q.W));
            Quat_Scale(q, fInvLen);
        }

        // NOTE:  Multiplication is not generally commutative,
        // so in most cases p*q != q*p.
        public static void Quat_MulQuat(Vector4 q1, Vector4 rkQ)
        {
            Vector4 q1c = new Vector4();

            Quat_Copy(q1c, q1);

            q1.W = (q1c.W * rkQ.W) - (q1c.X * rkQ.X) - (q1c.Y * rkQ.Y) - (q1c.Z * rkQ.Z);
            q1.X = (q1c.W * rkQ.X) + (q1c.X * rkQ.W) + (q1c.Y * rkQ.Z) - (q1c.Z * rkQ.Y);
            q1.Y = (q1c.W * rkQ.Y) + (q1c.Y * rkQ.W) + (q1c.Z * rkQ.X) - (q1c.X * rkQ.Z);
            q1.Z = (q1c.W * rkQ.Z) + (q1c.Z * rkQ.W) + (q1c.X * rkQ.Y) - (q1c.Y * rkQ.X);
        }

        // doesn't do zero-length checking.
        public static void Quat_Invert(Vector4 q)
        {
            Vector4 qt = new Vector4();
            //bool retval;
            float fNorm = (q.W * q.W) + (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z);
            float fInvNorm = 1.0f / fNorm;

            qt.X = -q.X * fInvNorm;
            qt.Y = -q.Y * fInvNorm;
            qt.Z = -q.Z * fInvNorm;
            qt.W = q.W * fInvNorm;

            Quat_Copy(q, qt);
        }

        // axis is normalized automatically here.
        public static void Quat_FromAngleAxis(Vector4 q, float rfAngle, Vector3 rkAxis_in)
        {
            // axis[] needs to be unit length (i.e. normalized)
            Vector3 rkAxis = rkAxis_in;
            Vector3.Normalize(rkAxis);

            // The quaternion representing the rotation is
            //   q = cos(A/2)+sin(A/2)*(x*i+y*j+z*k)

            float fHalfAngle = 0.5f * rfAngle;
            float fSin = (float)Math.Sin(fHalfAngle);
            q.W = (float)Math.Cos(fHalfAngle);
            q.X = fSin * rkAxis.X;
            q.Y = fSin * rkAxis.Y;
            q.Z = fSin * rkAxis.Z;
        }
        public static void Quat_ToAngleAxis(Vector4 q, ref float prfAngle, Vector3 rkAxis_dest)
        {
            // The quaternion representing the rotation is
            //   q = cos(A/2)+sin(A/2)*(x*i+y*j+z*k)

            float fSqrLength = (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z);
            if (fSqrLength > 0.0f)
            {
                prfAngle = 2.0f * AceMath3dUtil.ACosZ(q.W);
                float fInvLength = AceMath3dUtil.InvSqrt(fSqrLength);
                rkAxis_dest.X = q.X * fInvLength;
                rkAxis_dest.Y = q.Y * fInvLength;
                rkAxis_dest.Z = q.Z * fInvLength;
            }
            else
            {
                // angle is 0 (mod 2*pi), so any axis will do
                prfAngle = 0.0f;
                rkAxis_dest.X = 0.0F;
                rkAxis_dest.Y = 0.0F;
                rkAxis_dest.Z = 1.0F;
            }
        }

        // mat4x4 directly from quat.
        public static void Quat_ToMat4(ref float[,] M, Vector4 q)
        {
            float a = q.W;
            float b = q.X;
            float c = q.Y;
            float d = q.Z;
            float a2 = a * a;
            float b2 = b * b;
            float c2 = c * c;
            float d2 = d * d;

            M[0, 0] = a2 + b2 - c2 - d2;
            M[0, 1] = 2.0f * (b * c + a * d);
            M[0, 2] = 2.0f * (b * d - a * c);
            M[0, 3] = 0.0f;

            M[1, 0] = (float)(2.0 * (b * c - a * d));
            M[1, 1] = a2 - b2 + c2 - d2;
            M[1, 2] = 2.0f * (c * d + a * b);
            M[1, 3] = 0.0f;

            M[2, 0] = 2.0f * (b * d + a * c);
            M[2, 1] = 2.0f * (c * d - a * b);
            M[2, 2] = a2 - b2 - c2 + d2;
            M[2, 3] = 0.0f;

            M[3, 0] = M[3, 1] = M[3, 2] = 0.0f;
            M[3, 3] = 1.0f;
        }
        public static void Quat_FromEuler(Vector4 q, float roll, float pitch, float yaw)
        {
            var cr = (float)Math.Cos(roll * 0.5f);
            var cp = (float)Math.Cos(pitch * 0.5f);
            var cy = (float)Math.Cos(yaw * 0.5f);
            var sr = (float)Math.Sin(roll * 0.5f);
            var sp = (float)Math.Sin(pitch * 0.5f);
            var sy = (float)Math.Sin(yaw * 0.5f);
            var cpcy = cp * cy;
            var spsy = sp * sy;

            q.W = cr * cpcy + sr * spsy;
            q.X = sr * cpcy - cr * spsy;
            q.Y = cr * sp * cy + sr * cp * sy;
            q.Z = cr * cp * sy - sr * sp * cy;
        }

        // fHomogenous should normally be true

        public static void Quat_ToEuler(Vector4 q, Vector3 euler, bool fHomogenous)
        {
            float sqw = q.W * q.W;
            float sqx = q.X * q.X;
            float sqy = q.Y * q.Y;
            float sqz = q.Z * q.Z;

            if (fHomogenous == true)
            {
                euler.X = (float)Math.Atan2(2.0f * (q.X * q.Y + q.Z * q.W), sqx - sqy - sqz + sqw);
                euler.Y = (float)Math.Asin(-2.0f * (q.X * q.Z - q.Y * q.W));
                euler.Z = (float)Math.Atan2(2.0f * (q.Y * q.Z + q.X * q.W), -sqx - sqy + sqz + sqw);
            }
            else
            {
                euler.X = (float)Math.Atan2(2.0f * (q.Z * q.Y + q.X * q.W), 1 - 2 * (sqx + sqy));
                euler.Y = (float)Math.Asin(-2.0f * (q.X * q.Z - q.Y * q.W));
                euler.Z = (float)Math.Atan2(2.0f * (q.X * q.Y + q.Z * q.W), 1 - 2 * (sqy + sqz));
            }
        }

    }
}
