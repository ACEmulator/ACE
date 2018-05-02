using System;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.Motion;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// The exclusive states the monster can be in
        /// </summary>
        public enum State
        {
            Idle,
            Awake
        };

        /// <summary>
        /// Determines when a monster will attack
        /// </summary>
        [Flags]
        public enum Tolerance
        {
            None        = 0,    // attack targets in range
            NoAttack    = 1,    // never attack
            ID          = 2,    // attack when ID'd or attacked
            Unknown     = 4,    // unused?
            Provoke     = 8,    // used in conjunction with 32
            Unknown2    = 16,   // unused?
            Target      = 32,   // only target original attacker
            Retaliate   = 64    // only attack after attacked
        };

        public State MonsterState = State.Idle;
        public bool IsAwake = false;
        public bool IsMoving = false;
        public bool IsTurning = false;

        public bool IsDead => Health.Current <= 0;

        public double LastMoveTime;

        public WorldObject AttackTarget;

        // TODO: determine from quickness / run speed
        public static readonly float RunSpeed = 2.5f;

        /// <summary>
        /// Called every ~1 second
        /// </summary>
        public void DoTick()
        {
            Think();
            QueueNextTick();
        }

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        public void Think()
        {
            if (!IsAwake || IsDead) return;

            if (!IsTurning && !IsMoving && !IsMeleeRange())
                StartTurn();
            else
                Movement();
        }

        /// <summary>
        /// Called when player is in range
        /// </summary>
        public void WakeUp()
        {
            MonsterState = State.Awake;
            IsAwake = true;
            DoAttackStance();
        }

        /// <summary>
        /// Switch to attack stance
        /// </summary>
        public void DoAttackStance()
        {
            // TODO: get attack stance based on weapon type
            var attackStance = MotionStance.UaNoShieldAttack;

            var motion = new UniversalMotion(attackStance);
            motion.MovementData.CurrentStyle = (uint)attackStance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.Ready;

            CurrentMotionState = motion;
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        /// <summary>
        /// Starts the process of monster turning towards the attack target
        /// </summary>
        public void StartTurn()
        {
            IsTurning = true;
            var time = EstimateTurnTo();

            MoveTo(AttackTarget);

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
            IsTurning = false;
            StartMove();
        }

        /// <summary>
        /// Starts the process of monster moving towards the attack target
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
            StartAttack();
        }

        /// <summary>
        /// Starts the process of monster attacking target
        /// </summary>
        public void StartAttack()
        {
            //Console.WriteLine("StartAttack");
        }

        /// <summary>
        /// Estimates the time it will take the monster to turn and move towards target
        /// </summary>
        /// <returns></returns>
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
            var rotateTime = Math.PI / (360.0f / angle);
            return (float)rotateTime;
        }

        /// <summary>
        /// Gets the distance to target, with radius excluded
        /// </summary>
        public float GetDistanceToTarget()
        {
            var dist = (AttackTarget.Location.GlobalPos - Location.GlobalPos).Length();
            dist -= AttackTarget.PhysicsObj.GetRadius() - PhysicsObj.GetRadius();
            return dist;
        }

        /// <summary>
        /// Estimates the time it will take the monster to move towards target
        /// </summary>
        /// <returns></returns>
        public float EstimateMoveTo()
        {
            return GetDistanceToTarget() / RunSpeed;
        }

        public static readonly float MaxMeleeRange = 1.0f;
        public static readonly float MaxChaseRange = 192.0f;

        /// <summary>
        /// Returns TRUE if monster is within target melee range
        /// </summary>
        public bool IsMeleeRange()
        {
            return GetDistanceToTarget() <= MaxMeleeRange;
        }

        /// <summary>
        /// Primary movement handler, determines if target in range
        /// </summary>
        public void Movement()
        {
            UpdatePosition();
            LastMoveTime = Timer.CurrentTime;
            if (IsMeleeRange())
                OnMoveComplete();

            if (GetDistanceToTarget() >= MaxChaseRange)
                Sleep();
        }

        /// <summary>
        /// Updates monster position and rotation
        /// </summary>
        public void UpdatePosition()
        {
            var deltaTime = (float)(Timer.CurrentTime - LastMoveTime);
            if (deltaTime > 2.0f) return;   // FIXME: state persist?
            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            var movement = dir * deltaTime * RunSpeed;
            if (Location.SetPosition(Location.Pos + movement))
                UpdateLandblock();

            Location.Rotate(dir);
            SendUpdatePosition();
        }

        /// <summary>
        /// Sets the corpse to the final position
        /// </summary>
        public void SetFinalPosition()
        {
            var playerDir = AttackTarget.Location.GetCurrentDir();
            Location.Pos = AttackTarget.Location.Pos + playerDir * (AttackTarget.PhysicsObj.GetRadius() + PhysicsObj.GetRadius());
            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            Location.Rotate(dir);
            SendUpdatePosition();
        }

        /// <summary>
        /// Called when a monster changes landblocks
        /// </summary>
        public void UpdateLandblock()
        {
            //Console.WriteLine("Updating landblock for " + Name);
            PreviousLocation = Location;
            LandblockManager.RelocateObjectForPhysics(this);
        }

        /// <summary>
        /// Cleans up state on monster death
        /// </summary>
        public void OnDeath()
        {
            IsTurning = false;
            IsMoving = false;
        }

        /// <summary>
        /// Returns a monster to idle state
        /// </summary>
        public void Sleep()
        {
            MonsterState = State.Idle;
            IsAwake = false;
            IsMoving = false;
        }
    }
}
