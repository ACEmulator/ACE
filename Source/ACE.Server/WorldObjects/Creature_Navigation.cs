using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;
using ACE.Server.Physics;
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
            var targetDir = GetDirection(Location.ToGlobal(), target.Location.ToGlobal());
            
            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        public float GetAngle(Position position)
        {
            var currentDir = Location.GetCurrentDir();
            var targetDir = position.GetCurrentDir();

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        public static float GetAngle(Vector3 dir)
        {
            var rads = Math.Atan2(dir.Y, dir.X);
            var angle = rads * 57.2958f;
            return (float)angle;
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

        public float Rotate(WorldObject target)
        {
            // execute the TurnToObject motion
            var turnToMotion = new UniversalMotion(CurrentMotionState.Stance, target.Location, target.Guid);
            turnToMotion.MovementTypes = MovementTypes.TurnToObject;
            CurrentLandblock?.EnqueueBroadcastMotion(this, turnToMotion);

            return GetRotateDelay(target);
        }

        public float GetRotateDelay(WorldObject target)
        {
            // get inner angle between current heading and target
            var angle = GetAngle(target);

            if (angle < PhysicsGlobals.EPSILON) return 0.0f;

            // calculate time to complete the rotation
            var rotateTime = Math.PI / (360.0f / angle);

            var waitTime = 0.25f;
            return (float)rotateTime + waitTime;
        }

        public void MoveTo(WorldObject target, float runRate = 1.0f)
        {
            if (this is Player)
                return;

            var motion = new UniversalMotion(CurrentMotionState.Stance, target.Location, target.Guid);
            motion.MovementTypes = MovementTypes.MoveToObject;
            motion.Flag |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.Sticky | MovementParams.MoveAway;
            motion.WalkRunThreshold = 1.0f;
            motion.RunRate = runRate;

            CurrentMotionState = motion;

            if (CurrentLandblock != null)
                CurrentLandblock?.EnqueueBroadcastMotion(this, motion);
        }

        public void TurnTo(WorldObject target)
        {
            if (this is Player) return;

            var motion = new UniversalMotion(CurrentMotionState.Stance, target.Location, target.Guid);
            motion.MovementTypes = MovementTypes.TurnToObject;

            CurrentMotionState = motion;

            if (CurrentLandblock != null)
                CurrentLandblock?.EnqueueBroadcastMotion(this, motion);

            AttackTarget = target;
            var turnTime = EstimateTurnTo();
            Console.WriteLine("TurnTime = " + turnTime);
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(turnTime);
            actionChain.AddAction(this, () =>
            {
                // fix me: in progress turn
                var targetDir = GetDirection(Location.ToGlobal(), target.Location.ToGlobal());
                Location.Rotate(targetDir);
                Console.WriteLine("Finished turning - " + turnTime + "s");
            });
            actionChain.EnqueueChain();
        }

        public float TurnTo(Position position)
        {
            // execute the TurnToObject motion
            var turnToMotion = new UniversalMotion(CurrentMotionState.Stance, position);
            turnToMotion.MovementTypes = MovementTypes.TurnToHeading;
            CurrentLandblock?.EnqueueBroadcastMotion(this, turnToMotion);

            return GetTurnToDelay(position);
        }

        public float GetTurnToDelay(Position position)
        {
            // get inner angle between current heading and target
            var angle = GetAngle(position);

            if (angle < PhysicsGlobals.EPSILON) return 0.0f;

            // calculate time to complete the rotation
            var rotateTime = Math.PI / (360.0f / angle);

            var waitTime = 0.25f;
            return (float)rotateTime + waitTime;
        }
    }
}
