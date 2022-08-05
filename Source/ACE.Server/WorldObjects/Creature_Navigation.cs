using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Creature navigation / position / rotation
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Returns the 3D distance between this creature and target
        /// </summary>
        public float GetDistance(WorldObject target)
        {
            return Location.DistanceTo(target.Location);
        }

        /// <summary>
        /// Returns the 2D angle between current direction
        /// and position from an input target
        /// </summary>
        public float GetAngle(WorldObject target)
        {
            var currentDir = Location.GetCurrentDir();

            var targetDir = Vector3.Zero;
            if (Location.Indoors == target.Location.Indoors)
                targetDir = GetDirection(Location.ToGlobal(), target.Location.ToGlobal());
            else
                targetDir = GetDirection(Location.Pos, target.Location.Pos);

            targetDir.Z = 0.0f;
            targetDir = Vector3.Normalize(targetDir);
            
            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        public float GetAngle_Physics(WorldObject target)
        {
            var currentDir = GetCurrentDir_Physics();

            var targetDir = Vector3.Zero;
            if (Location.Indoors == target.Location.Indoors)
                targetDir = GetDirection(Location.ToGlobal(), target.Location.ToGlobal());
            else
                targetDir = GetDirection(Location.Pos, target.Location.Pos);

            targetDir.Z = 0.0f;
            targetDir = Vector3.Normalize(targetDir);

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        public Vector3 GetCurrentDir_Physics()
        {
            return Vector3.Normalize(Vector3.Transform(Vector3.UnitY, PhysicsObj.Position.Frame.Orientation));
        }

        public float GetAngle_Physics2(WorldObject target)
        {
            return PhysicsObj.Position.heading_diff(target.PhysicsObj.Position);
        }

        /// <summary>
        /// Returns the 2D angle between current direction
        /// and rotation from an input position
        /// </summary>
        public float GetAngle(Position position)
        {
            var currentDir = Location.GetCurrentDir();
            var targetDir = position.GetCurrentDir();

            // get the 2D angle between these vectors
            return GetAngle(currentDir, targetDir);
        }

        /// <summary>
        /// Returns the 2D angle of the input vector
        /// </summary>
        public static float GetAngle(Vector3 dir)
        {
            var rads = Math.Atan2(dir.Y, dir.X);
            if (double.IsNaN(rads)) return 0.0f;

            var angle = rads * 57.2958f;
            return (float)angle;
        }

        /// <summary>
        /// Returns the 2D angle between 2 vectors
        /// </summary>
        public static float GetAngle(Vector3 a, Vector3 b)
        {
            var cosTheta = a.Dot2D(b);
            var rads = Math.Acos(cosTheta);
            if (double.IsNaN(rads)) return 0.0f;

            var angle = rads * (180.0f / Math.PI);
            return (float)angle;
        }

        /// <summary>
        /// Returns a normalized 2D vector from self to target
        /// </summary>
        public Vector3 GetDirection(Vector3 self, Vector3 target)
        {
            var target2D = new Vector3(self.X, self.Y, 0);
            var self2D = new Vector3(target.X, target.Y, 0);

            return Vector3.Normalize(target - self);
        }

        /// <summary>
        /// Sends a TurnToObject command to the client
        /// </summary>
        public void TurnToObject(WorldObject target, bool stopCompletely = true)
        {
            var turnToMotion = new Motion(this, target, MovementType.TurnToObject);

            if (!stopCompletely)
                turnToMotion.MoveToParameters.MovementParameters &= ~MovementParams.StopCompletely;

            EnqueueBroadcastMotion(turnToMotion);
        }

        /// <summary>
        /// Starts rotating a creature from its current direction
        /// so that it eventually is facing the target position
        /// </summary>
        /// <returns>The amount of time in seconds for the rotation to complete</returns>
        public virtual float Rotate(WorldObject target)
        {
            if (target == null || target.Location == null)
                return 0.0f;

            // send network message to start turning creature
            TurnToObject(target);

            var angle = GetAngle(target);
            //Console.WriteLine("Angle: " + angle);

            // estimate time to rotate to target
            var rotateDelay = GetRotateDelay(angle);
            //Console.WriteLine("RotateTime: " + rotateTime);

            // update server object rotation on completion
            // TODO: proper incremental rotation
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(rotateDelay);
            actionChain.AddAction(this, () =>
            {
                if (target == null || target.Location == null)
                    return;

                //var matchIndoors = Location.Indoors == target.Location.Indoors;

                //var globalLoc = matchIndoors ? Location.ToGlobal() : Location.Pos;
                //var targetLoc = matchIndoors ? target.Location.ToGlobal() : target.Location.Pos;
                var globalLoc = Location.ToGlobal();
                var targetLoc = target.Location.ToGlobal();

                var targetDir = GetDirection(globalLoc, targetLoc);

                Location.Rotate(targetDir);
            });
            actionChain.EnqueueChain();

            return rotateDelay;
        }

        /// <summary>
        /// Returns the amount of time for this creature to rotate by the # of degrees
        /// from the input angle, using the omega speed from its MotionTable
        /// </summary>
        public virtual float GetRotateDelay(float angle)
        {
            var turnSpeed = MotionTable.GetTurnSpeed(MotionTableId);
            if (turnSpeed == 0.0f) return 0.0f;

            var rotateTime = Math.PI / turnSpeed / 180.0f * angle;
            return (float)rotateTime;
        }

        /// <summary>
        /// Returns the amount of time for this creature to rotate
        /// towards its target, based on the omega speed from its MotionTable
        /// </summary>
        public float GetRotateDelay(WorldObject target)
        {
            var angle = GetAngle(target);
            return GetRotateDelay(angle);
        }

        /// <summary>
        /// Starts rotating a creature from its current direction
        /// so that it eventually is facing the rotation from the input position
        /// Used by the emote system, which has the target rotation stored in positions
        /// </summary>
        /// <returns>The amount of time in seconds for the rotation to complete</returns>
        public float TurnTo(Position position)
        {
            var frame = new AFrame(position.Pos, position.Rotation);
            var heading = frame.get_heading();

            // send network message to start turning creature
            var turnToMotion = new Motion(this, position, heading);
            EnqueueBroadcastMotion(turnToMotion);

            var angle = GetAngle(position);
            //Console.WriteLine("Angle: " + angle);

            // estimate time to rotate to target
            var rotateDelay = GetRotateDelay(angle);
            //Console.WriteLine("RotateTime: " + rotateTime);

            // update server object rotation on completion
            // TODO: proper incremental rotation
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(rotateDelay);
            actionChain.AddAction(this, () =>
            {
                var targetDir = position.GetCurrentDir();
                Location.Rotate(targetDir);
                PhysicsObj.Position.Frame.Orientation = Location.Rotation;
            });
            actionChain.EnqueueChain();

            return rotateDelay;
        }

        /// <summary>
        /// Returns the amount of time for this creature to rotate
        /// towards the rotation from the input position, based on the omega speed from its MotionTable
        /// Used by the emote system, which has the target rotation stored in positions
        /// </summary>
        /// <param name="position">Only the rotation information from this position is used here</param>
        public float GetRotateDelay(Position position)
        {
            var angle = GetAngle(position);
            return GetRotateDelay(angle);
        }

        /// <summary>
        /// This is called by the monster AI system for ranged attacks
        /// It is mostly a duplicate of Rotate(), and should be refactored eventually...
        /// It sets CurrentMotionState and AttackTarget here
        /// </summary>
        public float TurnTo(WorldObject target, bool debug = false)
        {
            if (DebugMove)
                Console.WriteLine($"{Name}.TurnTo({target.Name})");

            if (this is Player) return 0.0f;

            var turnToMotion = new Motion(this, target, MovementType.TurnToObject);
            EnqueueBroadcastMotion(turnToMotion);

            CurrentMotionState = turnToMotion;

            AttackTarget = target;
            var rotateDelay = EstimateTurnTo();
            if (debug)
                Console.WriteLine("TurnTime = " + rotateDelay);
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(rotateDelay);
            actionChain.AddAction(this, () =>
            {
                // fix me: in progress turn
                //var targetDir = GetDirection(Location.ToGlobal(), target.Location.ToGlobal());
                //Location.Rotate(targetDir);
                if (debug)
                    Console.WriteLine("Finished turning - " + rotateDelay + "s");
            });
            actionChain.EnqueueChain();
            return rotateDelay;
        }

        /// <summary>
        /// Used by the monster AI system to start turning / running towards a target
        /// </summary>
        public virtual void MoveTo(WorldObject target, float runRate = 1.0f)
        {
            if (DebugMove)
                Console.WriteLine($"{Name}.MoveTo({target.Name}, {runRate}) - CurPos: {Location.ToLOCString()} - DestPos: {AttackTarget.Location.ToLOCString()} - TargetDist: {Vector3.Distance(Location.ToGlobal(), AttackTarget.Location.ToGlobal())}");

            var motion = GetMoveToMotion(target, runRate);

            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);
        }

        public Motion GetMoveToMotion(WorldObject target, float runRate)
        {
            var motion = new Motion(this, target, MovementType.MoveToObject);
            motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.Sticky | MovementParams.MoveAway;
            motion.MoveToParameters.WalkRunThreshold = 1.0f;

            if (runRate > 0)
                motion.RunRate = runRate;
            else
                motion.MoveToParameters.MovementParameters &= ~MovementParams.CanRun;

            return motion;
        }

        public virtual void BroadcastMoveTo(Player player)
        {
            Motion motion = null;

            if (AttackTarget != null)
            {
                // move to object
                motion = GetMoveToMotion(AttackTarget, RunRate);
            }
            else
            {
                // move to position
                var home = GetPosition(PositionType.Home);

                motion = GetMoveToPosition(home, RunRate, 1.0f);
            }

            player.Session.Network.EnqueueSend(new GameMessageUpdateMotion(this, motion));
        }

        /// <summary>
        /// Sends a network message for moving a creature to a new position
        /// </summary>
        public void MoveTo(Position position, float runRate = 1.0f, bool setLoc = true, float? walkRunThreshold = null, float? speed = null)
        {
            // build and send MoveToPosition message to client
            var motion = GetMoveToPosition(position, runRate, walkRunThreshold, speed);
            EnqueueBroadcastMotion(motion);

            if (!setLoc) return;

            // start executing MoveTo iterator on server
            if (!PhysicsObj.IsMovingOrAnimating)
                PhysicsObj.UpdateTime = Physics.Common.PhysicsTimer.CurrentTime;

            var mvp = new MovementParameters(motion.MoveToParameters);
            PhysicsObj.MoveToPosition(new Physics.Common.Position(position), mvp);

            AddMoveToTick();
        }

        private void AddMoveToTick()
        {
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(monsterTickInterval);
            actionChain.AddAction(this, () =>
            {
                if (!IsDead && PhysicsObj?.MovementManager?.MoveToManager != null && PhysicsObj.IsMovingTo())
                {
                    PhysicsObj.update_object();
                    UpdatePosition_SyncLocation();
                    SendUpdatePosition();

                    if (PhysicsObj?.MovementManager?.MoveToManager?.FailProgressCount < 5)
                    {
                        AddMoveToTick();
                    }
                    else
                    {
                        if (PhysicsObj?.MovementManager?.MoveToManager != null)
                        {
                            PhysicsObj.MovementManager.MoveToManager.CancelMoveTo(WeenieError.ActionCancelled);
                            PhysicsObj.MovementManager.MoveToManager.FailProgressCount = 0;
                        }
                        EnqueueBroadcastMotion(new Motion(CurrentMotionState.Stance, MotionCommand.Ready));
                    }

                    //Console.WriteLine($"{Name}.Position: {Location}");
                }
            });
            actionChain.EnqueueChain();
        }

        public Motion GetMoveToPosition(Position position, float runRate = 1.0f, float? walkRunThreshold = null, float? speed = null)
        {
            // TODO: change parameters to accept an optional MoveToParameters

            var motion = new Motion(this, position);
            motion.MovementType = MovementType.MoveToPosition;
            //motion.Flag |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.MoveAway;
            if (walkRunThreshold != null)
                motion.MoveToParameters.WalkRunThreshold = walkRunThreshold.Value;
            if (speed != null)
                motion.MoveToParameters.Speed = speed.Value;

            // always use final heading?
            var frame = new AFrame(position.Pos, position.Rotation);
            motion.MoveToParameters.DesiredHeading = frame.get_heading();
            motion.MoveToParameters.MovementParameters |= MovementParams.UseFinalHeading;
            motion.MoveToParameters.DistanceToObject = 0.6f;

            if (runRate > 0)
                motion.RunRate = runRate;
            else
                motion.MoveToParameters.MovementParameters &= ~MovementParams.CanRun;

            return motion;
        }

        /// <summary>
        /// For monsters only -- blips to a new position within the same landblock
        /// </summary>
        public void FakeTeleport(Position _newPosition)
        {
            var newPosition = new Position(_newPosition);

            newPosition.PositionZ += 0.005f * (ObjScale ?? 1.0f);

            if (Location.Landblock != newPosition.Landblock)
            {
                log.Error($"{Name} tried to teleport from {Location} to a different landblock {newPosition}");
                return;
            }

            // force out of hotspots
            PhysicsObj.report_collision_end(true);

            //HandlePreTeleportVisibility(newPosition);

            // do the physics teleport
            var setPosition = new Physics.Common.SetPosition();
            setPosition.Pos = new Physics.Common.Position(newPosition);
            setPosition.Flags = Physics.Common.SetPositionFlags.SendPositionEvent | Physics.Common.SetPositionFlags.Slide | Physics.Common.SetPositionFlags.Placement | Physics.Common.SetPositionFlags.Teleport;

            PhysicsObj.SetPosition(setPosition);

            // update ace location
            SyncLocation();

            // broadcast blip to new position
            SendUpdatePosition(true);
        }
    }
}
