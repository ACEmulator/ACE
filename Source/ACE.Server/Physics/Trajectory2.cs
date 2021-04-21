using System;
using System.Numerics;

using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public static class Trajectory2
    {
        public static Vector3 CalculateTrajectory(Vector3 startPos, Vector3 endPos, Vector3 targetVelocity, float speed, bool gravity)
        {
            var targetOffset = endPos - startPos;

            Vector3 result;
            float t = 0.0f;

            if (targetVelocity == Vector3.Zero)
            {
                var targetDist = targetOffset.Length();

                t = targetDist / speed;
                result = targetOffset / t;
            }
            else
            {
                var p0 = targetOffset;
                var p1 = Vector3.Zero;

                var v0 = targetVelocity;
                if (Vec.NormalizeCheckSmall(ref v0))
                    v0 = Vector3.Zero;

                var s1 = speed;

                var a = (v0.X * v0.X) + (v0.Y * v0.Y) - (s1 * s1);
                var b = 2 * ((p0.X * v0.X) + (p0.Y * v0.Y) - (p1.X * v0.X) - (p1.Y * v0.Y));
                var c = (p0.X * p0.X) + (p0.Y * p0.Y) + (p1.X * p1.X) + (p1.Y * p1.Y) - (2 * p1.X * p0.X) - (2 * p1.Y * p0.Y);

                var t0 = Math.Sqrt((b * b) - (4 * a * c));
                var t1 = (-b + t0) / (2 * a);
                var t2 = (-b - t0) / (2 * a);

                if (t1 < 0)
                    t1 = float.MaxValue;
                if (t2 < 0)
                    t2 = float.MaxValue;

                t = (float)Math.Min(t1, t2);

                if (t >= 100)
                    return CalculateTrajectory(startPos, endPos, Vector3.Zero, speed, gravity);

                var s0 = targetVelocity.Length();

                result = (p0 + t * s0 * v0) / t;
            }

            if (gravity)
                result.Z -= PhysicsGlobals.Gravity * t * 0.5f;

            return result;
        }
    }
}
