using System;
using System.Numerics;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// A passive summonable creature
    /// </summary>
    public class Pet : Creature
    {
        public Player P_PetOwner;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Pet(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Pet(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            Ethereal = true;
            RadarBehavior = ACE.Entity.Enum.RadarBehavior.ShowNever;
            Usable = ACE.Entity.Enum.Usable.No;

            SuppressGenerateEffect = true;
        }

        public virtual void Init(Player player, PetDevice petDevice)
        {
            Location = player.Location.InFrontOf(5f);
            Location.LandblockId = new LandblockId(Location.GetCell());

            Name = player.Name + "'s " + Name;

            player.CurrentActivePet = this;

            PetOwner = player.Guid.Full;
            P_PetOwner = player;

            EnterWorld();
        }

        // if the passive pet is between min-max distance to owner,
        // it will turn and start running torwards its owner

        private static readonly float MinDistance = 2.0f;
        private static readonly float MaxDistance = 192.0f;

        private static readonly float MinDistanceSq = MinDistance * MinDistance;
        private static readonly float MaxDistanceSq = MaxDistance * MaxDistance;

        public override void Heartbeat(double currentUnixTime)
        {
            base.Heartbeat(currentUnixTime);

            if (IsMoving || this is CombatPet)
                return;

            var distSq = Location.SquaredDistanceTo(P_PetOwner.Location);

            if (distSq > MinDistanceSq && distSq < MaxDistanceSq)
                StartFollow();
        }

        public void StartFollow()
        {
            // similar to Monster_Navigation.StartTurn()

            //Console.WriteLine($"{Name}.StartFollow()");

            IsMoving = true;

            // broadcast to clients
            MoveTo(P_PetOwner, RunRate);

            // perform movement on server
            var mvp = new MovementParameters();
            mvp.DistanceToObject = MinDistance;

            PhysicsObj.MoveToObject(P_PetOwner.PhysicsObj, mvp);
        }

        /// <summary>
        /// Broadcasts passive pet movement to clients
        /// </summary>
        public override void MoveTo(WorldObject target, float runRate = 1.0f)
        {
            if (MoveSpeed == 0.0f)
                GetMovementSpeed();

            var motion = new Motion(this, target, MovementType.MoveToObject);

            motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge | MovementParams.FailWalk | MovementParams.UseFinalHeading | MovementParams.MoveAway;
            motion.MoveToParameters.DistanceToObject = MinDistance;
            motion.MoveToParameters.WalkRunThreshold = 1.0f;

            motion.RunRate = RunRate;

            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);
        }

        /// <summary>
        /// Called when the MoveTo process has completed
        /// </summary>
        public override void OnMoveComplete(WeenieError status)
        {
            //Console.WriteLine($"{Name}.OnMoveComplete({status})");

            if (this is CombatPet)
            {
                base.OnMoveComplete(status);
                return;
            }

            if (status != WeenieError.None)
                return;

            PhysicsObj.CachedVelocity = Vector3.Zero;
            IsMoving = false;
        }

        /// <summary>
        /// Called ~5x per second
        /// </summary>
        public void Tick(double currentUnixTime)
        {
            if (IsMoving)
                PhysicsObj.update_object();
        }
    }
}
