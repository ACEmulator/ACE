using System;
using System.Numerics;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Return to home if target distance exceeds this range
        /// </summary>
        public static readonly float MaxChaseRange = 192.0f;

        /// <summary>
        /// Determines if a monster is within melee range of target
        /// </summary>
        public static readonly float MaxMeleeRange = 1.0f;

        /// <summary>
        /// The maximum range for a monster missile attack
        /// </summary>
        //public static readonly float MaxMissileRange = 80.0f;
        public static readonly float MaxMissileRange = 40.0f;   // for testing

        /// <summary>
        /// The distance per second from running animation
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// The run skill via MovementSystem GetRunRate()
        /// </summary>
        public float RunRate;

        /// <summary>
        /// Flag indicates monster is turning towards target
        /// </summary>
        public bool IsTurning = false;

        /// <summary>
        /// Flag indicates monster is moving towards target
        /// </summary>
        public bool IsMoving = false;

        /// <summary>
        /// The last time a movement tick was processed
        /// </summary>
        public double LastMoveTime;

        /// <summary>
        /// Starts the process of monster turning towards target
        /// </summary>
        public void StartTurn()
        {
            if (MoveSpeed == 0.0f)
                GetMovementSpeed();

            IsTurning = true;
            var time = EstimateTurnTo();

            if (IsRanged)
                TurnTo(AttackTarget);
            else
                MoveTo(AttackTarget, RunRate);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);
            actionChain.AddAction(this, () => OnTurnComplete());
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Called when the TurnTo process has completed
        /// </summary>
        public void OnTurnComplete()
        {
            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            Location.Rotate(dir);

            IsTurning = false;

            if (!IsRanged)
                StartMove();
        }

        /// <summary>
        /// Starts the process of monster moving towards target
        /// </summary>
        public void StartMove()
        {
            LastMoveTime = Timer.CurrentTime;
            IsMoving = true;
        }

        /// <summary>
        /// Called when the MoveTo process has completed
        /// </summary>
        public void OnMoveComplete()
        {
            IsMoving = false;
        }

        /// <summary>
        /// Estimates the time it will take the monster to turn and move towards target
        /// </summary>
        public float EstimateTargetTime()
        {
            return EstimateTurnTo() + EstimateMoveTo();
        }

        /// <summary>
        /// Estimates the time it will take the monster to turn towards target
        /// </summary>
        public float EstimateTurnTo()
        {
            var angle = GetAngle(AttackTarget);
            var rotateTime = Math.PI / (360.0f / angle) / MoveSpeed;
            return (float)rotateTime;
        }

        /// <summary>
        /// Estimates the time it will take the monster to move towards target
        /// </summary>
        public float EstimateMoveTo()
        {
            return GetDistanceToTarget() / MoveSpeed;
        }

        /// <summary>
        /// Returns TRUE if monster is within target melee range
        /// </summary>
        public bool IsMeleeRange()
        {
            return GetDistanceToTarget() <= MaxMeleeRange;
        }

        /// <summary>
        /// Returns TRUE if monster in range for current attack type
        /// </summary>
        public bool IsAttackRange()
        {
            return GetDistanceToTarget() <= MaxRange;
        }

        /// <summary>
        /// Gets the distance to target, with radius excluded
        /// </summary>
        public float GetDistanceToTarget()
        {
            var dist = (AttackTarget.Location.GlobalPos - Location.GlobalPos).Length();
            dist -= AttackTarget.PhysicsObj.GetRadius() + PhysicsObj.GetRadius();
            return dist;
        }

        /// <summary>
        /// Returns the destination position the monster is attempting to move to
        /// to perform a melee attack
        /// </summary>
        public Vector3 GetDestination()
        {
            var dir = Vector3.Normalize(Location.GlobalPos - AttackTarget.Location.GlobalPos);
            return AttackTarget.Location.Pos + dir * (AttackTarget.PhysicsObj.GetRadius() + PhysicsObj.GetRadius());
        }

        /// <summary>
        /// Primary movement handler, determines if target in range
        /// </summary>
        public void Movement()
        {
            if (!IsRanged)
            {
                UpdatePosition();
                LastMoveTime = Timer.CurrentTime;
            }

            if (IsAttackRange())
                OnMoveComplete();

            if (GetDistanceToTarget() >= MaxChaseRange)
                Sleep();
        }

        /// <summary>
        /// Updates monster position and rotation
        /// </summary>
        public void UpdatePosition()
        {
            // determine the time interval for this movement
            var deltaTime = (float)(Timer.CurrentTime - LastMoveTime);
            if (deltaTime > 2.0f) return;   // FIXME: state persist?

            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            var movement = dir * deltaTime * MoveSpeed;
            var newPos = Location.Pos + movement;

            // stop at destination
            var dist = GetDistanceToTarget();
            if (movement.Length() > dist)
                newPos = GetDestination();

            // update position, and landblock if required
            Location.Rotate(dir);
            if (Location.SetPosition(newPos))
                UpdateLandblock();

            SendUpdatePosition();
        }

        public void GetMovementSpeed()
        {
            var moveSpeed = MotionTable.GetRunSpeed(MotionTableId);
            if (moveSpeed == 0) moveSpeed = 2.5f;
            var scale = ObjScale ?? 1.0f;

            var runSkill = GetCreatureSkill(Skill.Run).Current;
            RunRate = (float)MovementSystem.GetRunRate(0.0f, (int)runSkill, 1.0f);
            MoveSpeed = moveSpeed * RunRate * scale;

            //Console.WriteLine(Name + " - Run: " + runSkill + " - RunRate: " + RunRate + " - Move: " + MoveSpeed + " - Scale: " + scale);
        }

        /// <summary>
        /// Called when a monster changes landblocks
        /// </summary>
        public void UpdateLandblock()
        {
            PreviousLocation = Location;
            LandblockManager.RelocateObjectForPhysics(this);
        }

        /// <summary>
        /// Sets the corpse to the final position
        /// </summary>
        public void SetFinalPosition()
        {
            if (AttackTarget == null) return;

            var playerDir = AttackTarget.Location.GetCurrentDir();
            Location.Pos = AttackTarget.Location.Pos + playerDir * (AttackTarget.PhysicsObj.GetRadius() + PhysicsObj.GetRadius());
            SendUpdatePosition();
        }

        /// <summary>
        /// Returns TRUE if monster is facing towards the target
        /// </summary>
        public bool IsFacing(WorldObject target)
        {
            var angle = GetAngle(target);
            var dist = Math.Max(0, GetDistanceToTarget());

            //Console.WriteLine("Angle: " + angle);
            //Console.WriteLine("Dist: " + dist);

            // rotation accuracy?
            var threshold = 10.0f;

            var minDist = 10.0f;

            if (dist < minDist)
                threshold += (minDist - dist) * 2.0f;

            return angle < threshold;
        }
    }
}
