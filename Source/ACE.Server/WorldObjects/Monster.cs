using System;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;
using System.Numerics;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public enum State
        {
            Idle,
            Awake
        };

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

        public double LastMoveTime;

        public WorldObject AttackTarget;

        // TODO: determine from quickness / run speed
        public static readonly float RunSpeed = 2.5f;

        /// <summary>
        /// Construct a new monster from a weenie and a guid
        /// </summary>
        /*public Monster(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
        }*/

        /// <summary>
        /// Construct a new monster from a Biota
        /// </summary>
        /*public Monster(Biota biota) : base(biota)
        {
        }*/

        public void Think()
        {
            if (!IsMoving) return;
            Movement();
        }

        /// <summary>
        /// Called every ~1 second
        /// </summary>
        public void DoTick()
        {
            Think();
            QueueNextTick();
        }

        /// <summary>
        /// Called when player is in range
        /// </summary>
        public void WakeUp()
        {
            MonsterState = State.Awake;
            IsAwake = true;
            DoAttackStance();

            // start turning and moving towards target
            StartTurn();
        }

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

            /*var time = EstimateMoveTo();

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(time);
            actionChain.AddAction(this, () => OnMoveComplete());
            actionChain.EnqueueChain();*/
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
        /// Estimates the time it will take the monster to move towards target
        /// </summary>
        /// <returns></returns>
        public float EstimateMoveTo()
        {
            var dist = (AttackTarget.Location.GlobalPos - Location.GlobalPos).Length();
            return dist / RunSpeed;
        }

        public static readonly float MaxMeleeRange = 3.0f;

        /// <summary>
        /// Returns TRUE if monster is within target melee range
        /// </summary>
        public bool IsMeleeRange()
        {
            var dist = (AttackTarget.Location.GlobalPos - Location.GlobalPos).Length();
            return dist <= MaxMeleeRange;
        }

        public void Movement()
        {
            UpdatePosition();
            LastMoveTime = Timer.CurrentTime;
            if (IsMeleeRange())
                OnMoveComplete();
        }

        public void UpdatePosition()
        {
            var deltaTime = (float)(Timer.CurrentTime - LastMoveTime);
            var dir = Vector3.Normalize(AttackTarget.Location.GlobalPos - Location.GlobalPos);
            var movement = dir * deltaTime * RunSpeed;
            Location.Pos += movement;
            Location.Rotate(dir);
            SendUpdatePosition();
        }
    }
}
