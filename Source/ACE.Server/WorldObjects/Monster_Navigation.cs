using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

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
        //public static readonly float MaxMeleeRange = 0.5f;
        public static readonly float MaxMeleeRange = 1.5f;

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

        public bool DebugMove;

        public bool InitSticky;
        public bool Sticky;

        /// <summary>
        /// Starts the process of monster turning towards target
        /// </summary>
        public void StartTurn()
        {
            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid}) - StartTurn");

            if (MoveSpeed == 0.0f)
                GetMovementSpeed();

            IsTurning = true;

            if (IsRanged)
                TurnTo(AttackTarget);
            else
                MoveTo(AttackTarget, RunRate);

            if (IsRanged) return;

            // need turning listener?
            IsTurning = false;
            IsMoving = true;

            var mvp = GetMovementParameters();

            PhysicsObj.MoveToObject(AttackTarget.PhysicsObj, mvp);
            PhysicsObj.add_moveto_listener(OnMoveComplete);

            if (!InitSticky)
            {
                PhysicsObj.add_sticky_listener(OnSticky);
                PhysicsObj.add_unsticky_listener(OnUnsticky);
                InitSticky = true;
            }
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
            //if (DebugMove)
                //Console.WriteLine($"{Name} ({Guid}) - OnMoveComplete");

            PhysicsObj.CachedVelocity = Vector3.Zero;
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
            return GetRotateDelay(AttackTarget);
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
                UpdatePosition();

            if (GetDistanceToTarget() >= MaxChaseRange)
                Sleep();
        }

        public static bool ForcePos = true;

        public void UpdatePosition()
        {
            PhysicsObj.update_object();
            UpdatePosition_SyncLocation();

            SendUpdatePosition(ForcePos);

            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid}) - UpdatePosition");
        }

        /// <summary>
        /// Synchronizes the WorldObject Location with the Physics Location
        /// </summary>
        public void UpdatePosition_SyncLocation()
        {
            // was the position successfully moved to?
            // use the physics position as the source-of-truth?
            var newPos = PhysicsObj.Position;
            if (Location.LandblockId.Raw != newPos.ObjCellID)
            {
                var prevBlock = Location.LandblockId.Raw >> 16;
                var prevCell = Location.LandblockId.Raw & 0xFFFF;

                var newBlock = newPos.ObjCellID >> 16;
                var newCell = newPos.ObjCellID & 0xFFFF;

                Location.LandblockId = new LandblockId(newPos.ObjCellID);

                if (prevBlock != newBlock)
                {
                    PreviousLocation = Location;
                    LandblockManager.RelocateObjectForPhysics(this);
                    //PhysicsObj.Position.Frame.Origin = newPos;
                    //Console.WriteLine("Relocating " + Name + " to " + Location.LandblockId.Raw.ToString("X8"));
                }
                //else
                //Console.WriteLine("Moving " + Name + " to " + Location.LandblockId.Raw.ToString("X8"));
            }
            Location.Pos = newPos.Frame.Origin;
            Location.Rotation = newPos.Frame.Orientation;

            //if (DebugMove)
                //DebugDistance();
        }

        public void DebugDistance()
        {
            var dist = GetDistanceToTarget();
            var angle = GetAngle(AttackTarget);
            Console.WriteLine("Dist: " + dist);
            Console.WriteLine("Angle: " + angle);
        }

        public void GetMovementSpeed()
        {
            var moveSpeed = MotionTable.GetRunSpeed(MotionTableId);
            if (moveSpeed == 0) moveSpeed = 2.5f;
            var scale = ObjScale ?? 1.0f;

            RunRate = GetRunRate();

            MoveSpeed = moveSpeed * RunRate * scale;

            //Console.WriteLine(Name + " - Run: " + runSkill + " - RunRate: " + RunRate + " - Move: " + MoveSpeed + " - Scale: " + scale);
        }

        /// <summary>
        /// Returns the RunRate that is sent to the client as myRunRate
        /// </summary>
        public float GetRunRate()
        {
            var runSkill = GetCreatureSkill(Skill.Run).Current;
            var runRate = MovementSystem.GetRunRate(0.0f, (int)runSkill, 1.0f);

            return (float)runRate;
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

        public MovementParameters GetMovementParameters()
        {
            var mvp = new MovementParameters();

            mvp.CanWalk = false;
            mvp.CanRun = true;
            mvp.CanSidestep = false;
            mvp.CanWalkBackwards = false;
            mvp.MoveAway = true;
            mvp.CanCharge = true;
            mvp.FailWalk = true;
            //mvp.UseFinalHeading = true;
            mvp.Sticky = true;

            mvp.MinDistance = 0.1f;
            mvp.DistanceToObject = 0.5f;
            mvp.Speed = 1.0f;

            mvp.SetHoldKey = true;
            mvp.HoldKeyToApply = HoldKey.Run;

            return mvp;
        }

        public void OnSticky()
        {
            //Console.WriteLine($"{Name} ({Guid}) - OnSticky");
            Sticky = true;
        }

        public void OnUnsticky()
        {
            //Console.WriteLine($"{Name} ({Guid}) - OnUnsticky");
            Sticky = false;
        }
    }
}
