using System;
using System.Numerics;

namespace ACE.Server.Physics
{
    public class Ray
    {
        public Vector3 Point;
        public Vector3 Dir;
        public float Length;

        public Ray()
        {

        }

        public Ray(Vector3 point, Vector3 dir, float length)
        {
            Point = point;
            Dir = dir;
            Length = length;
        }

        public Ray(Vector3 startPoint, Vector3 offset)
        {
            if (Math.Abs(offset.X - startPoint.X) > PhysicsGlobals.EPSILON ||
                Math.Abs(offset.Y - startPoint.Y) > PhysicsGlobals.EPSILON ||
                Math.Abs(offset.Z - startPoint.Z) > PhysicsGlobals.EPSILON)
            {
                Length = (float)Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y + offset.Z * offset.Z);

                var invLength = 1.0f / Length;

                Point = startPoint;
                Dir = new Vector3(offset.X * invLength, offset.Y * invLength, offset.Z * invLength);
            }
        }
    }
}
