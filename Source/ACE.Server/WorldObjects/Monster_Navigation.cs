using System;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Util;
using Timer = ACE.Server.Entity.Timer;

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
        public static readonly float MaxMeleeRange = 0.5f;

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

            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid})StartTurn");
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
            {
                UpdatePosition();
                LastMoveTime = Timer.CurrentTime;
            }

            if (IsAttackRange())
                OnMoveComplete();

            if (GetDistanceToTarget() >= MaxChaseRange)
                Sleep();
        }

        public static bool ForcePos = false;

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

            //UpdatePosition_Inner(newPos, dir);
            UpdatePosition_PhysicsInner(newPos, dir);

            // set cached velocity
            var velocity = movement / deltaTime;
            PhysicsObj.CachedVelocity = velocity;

            SendUpdatePosition(ForcePos);
            //MoveTo(AttackTarget, RunRate, true);

            if (DebugMove)
                Console.WriteLine($"{Name} ({Guid}) UpdatePosition()");
        }

        public void UpdatePosition_Inner(Vector3 newPos, Vector3 dir)
        {
            // update position, and landblock if required
            Location.Rotate(dir);
            var blockCellUpdate = Location.SetPosition(newPos);
            if (Location.Indoors)
                UpdateIndoorCells(newPos);
            PhysicsObj.Position.Frame.Origin = newPos;
            if (blockCellUpdate.Item1)
                UpdateLandblock();
            if (blockCellUpdate.Item1 || blockCellUpdate.Item2)
                UpdateCell();
        }

        public void UpdatePosition_PhysicsInner(Vector3 requestPos, Vector3 dir)
        {
            Location.Rotate(dir);
            PhysicsObj.Position.Frame.Orientation = Location.Rotation;

            var cell = LScape.get_landcell(Location.Cell);

            PhysicsObj.set_request_pos(requestPos, Location.Rotation, null, Location.LandblockId.Raw);

            // simulate running forward for this amount of time
            PhysicsObj.update_object_server(false);

            // was the position successfully moved to?
            // use the physics position as the source-of-truth?
            var newPos = PhysicsObj.Position.Frame.Origin;
            if (Location.LandblockId.Raw != PhysicsObj.Position.ObjCellID)
            {
                var prevBlock = Location.LandblockId.Raw >> 16;
                var prevCell = Location.LandblockId.Raw & 0xFFFF;

                var newBlock = PhysicsObj.Position.ObjCellID >> 16;
                var newCell = PhysicsObj.Position.ObjCellID & 0xFFFF;

                Location.LandblockId = new LandblockId(PhysicsObj.Position.ObjCellID);

                if (prevBlock != newBlock)
                {
                    PreviousLocation = Location;
                    LandblockManager.RelocateObjectForPhysics(this);
                    PhysicsObj.Position.Frame.Origin = newPos;
                    //Console.WriteLine("Relocating " + Name + " to " + Location.LandblockId.Raw.ToString("X8"));
                }
                //else
                    //Console.WriteLine("Moving " + Name + " to " + Location.LandblockId.Raw.ToString("X8"));
            }
            Location.Pos = newPos;
            //DebugDistance();
        }

        public void DebugDistance()
        {
            var dist = GetDistanceToTarget();
            Console.WriteLine("Dist: " + dist);
        }

        public void UpdateIndoorCells(Vector3 newPos)
        {
            var adjustCell = AdjustCell.Get(Location.LandblockId.Raw >> 16);
            if (adjustCell == null) return;
            var newCell = adjustCell.GetCell(newPos);
            if (newCell == null) return;
            if (newCell.Value == Location.LandblockId.Raw) return;
            Location.LandblockId = new LandblockId(newCell.Value);
            //Console.WriteLine("Moving " + Name + " to indoor cell " + newCell.Value.ToString("X8"));
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
        /// Called when a monster changes landblocks
        /// </summary>
        public void UpdateLandblock()
        {
            PreviousLocation = Location;
            LandblockManager.RelocateObjectForPhysics(this);
            //Console.WriteLine("Relocating " + Name + " to " + Location.LandblockId);
        }

        /// <summary>
        /// Called when a monster changes cells
        /// </summary>
        public void UpdateCell()
        {
            var curCell = LScape.get_landcell(Location.LandblockId.Raw);
            //Console.WriteLine("Moving " + Name + " to " + curCell.ID.ToString("X8"));
            PhysicsObj.change_cell_server(curCell);
            //PhysicsObj.remove_shadows_from_cells();
            PhysicsObj.add_shadows_to_cell(curCell);
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
