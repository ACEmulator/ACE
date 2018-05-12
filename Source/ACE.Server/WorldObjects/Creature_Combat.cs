using System;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public string GetSplatterDir(WorldObject target)
        {
            var sourcePos = new Vector3(Location.PositionX, Location.PositionY, 0);
            var targetPos = new Vector3(target.Location.PositionX, target.Location.PositionY, 0);
            var targetDir = new AFrame(target.Location.Pos, target.Location.Rotation).get_vector_heading();

            targetDir.Z = 0;
            targetDir = targetDir.Normalize();

            var sourceToTarget = (sourcePos - targetPos).Normalize();

            var dir = Vector3.Dot(sourceToTarget, targetDir);
            var angle = Vector3.Cross(sourceToTarget, targetDir);

            var frontBack = dir >= 0 ? "Front" : "Back";
            var leftRight = angle.Z <= 0 ? "Left" : "Right";

            return leftRight + frontBack;
        }
    }
}
