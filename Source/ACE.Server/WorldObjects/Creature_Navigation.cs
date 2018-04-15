using System;
using System.Numerics;

using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public float GetDistance(WorldObject target)
        {
            return Location.DistanceTo(target.Location);
        }

        public float GetAngle(WorldObject target)
        {
            var currentDir = Location.GetCurrentDir();
            var targetDir = GetDirection(Location.Pos, target.Location.Pos);

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        /// <summary>
        /// Returns the 2D angle between 2 vectors
        /// </summary>
        static float GetAngle(Vector3 a, Vector3 b)
        {
            var cosTheta = a.Dot2D(b);
            var rads = Math.Acos(cosTheta);
            var angle = rads * (180.0f / Math.PI);
            return (float)angle;
        }

        static Vector3 GetDirection(Vector3 self, Vector3 target)
        {
            // draw a line from current location
            // to target location
            var offset = target - self;
            offset = offset.Normalize();

            return offset;
        }

        public void Rotate()
        {

        }

        public void MoveTo()
        {

        }
    }
}
