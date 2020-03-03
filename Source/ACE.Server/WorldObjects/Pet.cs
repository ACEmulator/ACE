using System;
using System.Numerics;

using log4net;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Physics.Animation;

using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// A passive summonable creature
    /// </summary>
    public class Pet : Creature
    {
        public Player P_PetOwner;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            // TODO: handle spawn failure
            var success = EnterWorld();

            if (IsPassivePet)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(0.25f);
                actionChain.AddAction(this, () => TryCastSpells());
                actionChain.EnqueueChain();
            }
        }

        // if the passive pet is between min-max distance to owner,
        // it will turn and start running torwards its owner

        private static readonly float MinDistance = 2.0f;
        private static readonly float MaxDistance = 192.0f;

        private static readonly float MinDistanceSq = MinDistance * MinDistance;
        private static readonly float MaxDistanceSq = MaxDistance * MaxDistance;

        private static readonly float CastDistance = 96.0f;
        private static readonly float CastDistanceSq = CastDistance * CastDistance;

        public override void Heartbeat(double currentUnixTime)
        {
            base.Heartbeat(currentUnixTime);

            if (IsMoving || this is CombatPet)
                return;

            var distSq = Location.SquaredDistanceTo(P_PetOwner.Location);

            if (distSq > MinDistanceSq && distSq < MaxDistanceSq)
                StartFollow();

            if (distSq < CastDistanceSq && DateTime.UtcNow > NextCastTime)
                TryCastSpells();
        }

        private void StartFollow()
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
            if (WeenieType == WeenieType.CombatPet)
            {
                base.MoveTo(target, runRate);
                return;
            }

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

            if (WeenieType == WeenieType.CombatPet)
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
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            if (IsMoving)
                PhysicsObj.update_object();
        }

        /// <summary>
        /// A pet casts spells on its owner every 30 seconds?
        /// </summary>
        private static TimeSpan CastInterval = TimeSpan.FromSeconds(30);

        private DateTime NextCastTime;

        /// <summary>
        /// Passive pet cast spells on its owner
        /// </summary>
        public void TryCastSpells()
        {
            foreach (var _spell in Biota.BiotaPropertiesSpellBook)
            {
                var spell = new Server.Entity.Spell(_spell.Spell);

                if (spell.NotFound)
                {
                    log.Error($"{Name}.CastSpells(): spell {_spell.Spell} not found");
                    continue;
                }

                TryCastSpell(spell, P_PetOwner);
            }
            NextCastTime = DateTime.UtcNow + CastInterval;
        }
    }
}
