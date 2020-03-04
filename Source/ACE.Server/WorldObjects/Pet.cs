using System;
using System.Collections.Generic;
using System.Numerics;

using log4net;

using ACE.Common;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
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

        public virtual bool Init(Player player, PetDevice petDevice)
        {
            if (player.CurrentActivePet != null)
            {
                if (player.CurrentActivePet is CombatPet)
                {
                    player.SendTransientError($"{player.CurrentActivePet.Name} is already active");
                    return false;
                }

                var stowPet = WeenieClassId == player.CurrentActivePet.WeenieClassId;

                // despawn passive pet
                player.CurrentActivePet.Destroy();

                if (stowPet) return false;
           }

            if (IsPassivePet)
            {
                // get physics radius of player and pet
                var playerRadius = player.PhysicsObj.GetPhysicsRadius();
                var petRadius = GetPetRadius();

                var spawnDist = playerRadius + petRadius + MinDistance;

                Location = player.Location.InFrontOf(spawnDist, true);
            }
            else
            {
                Location = player.Location.InFrontOf(5.0f);
            }

            Location.LandblockId = new LandblockId(Location.GetCell());

            Name = player.Name + "'s " + Name;

            player.CurrentActivePet = this;

            PetOwner = player.Guid.Full;
            P_PetOwner = player;

            var success = EnterWorld();

            if (!success)
            {
                player.SendTransientError($"Couldn't spawn {Name}");
                return false;
            }

            if (IsPassivePet)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(0.25f);
                actionChain.AddAction(this, () => TryCastSpells());
                actionChain.EnqueueChain();

                nextSlowTickTime = Time.GetUnixTime();
            }
            return true;
        }

        /// <summary>
        /// Called 5x per second for passive pets
        /// </summary>
        public void Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            if (IsMoving)
            {
                PhysicsObj.update_object();

                UpdatePosition_SyncLocation();

                SendUpdatePosition();
            }

            if (currentUnixTime >= nextSlowTickTime)
                SlowTick(currentUnixTime);
        }

        private static readonly double slowTickSeconds = 1.0;
        private double nextSlowTickTime;

        /// <summary>
        /// Called 1x per second
        /// </summary>
        public void SlowTick(double currentUnixTime)
        {
            //Console.WriteLine($"{Name}.HeartbeatStatic({currentUnixTime})");

            nextSlowTickTime += slowTickSeconds;

            var dist = GetCylinderDistance(P_PetOwner);

            if (dist > MaxDistance)
                Destroy();

            if (IsMoving) return;

            if (dist > MinDistance)
                StartFollow();

            if (dist < CastDistance && DateTime.UtcNow > NextCastTime)
                TryCastSpells();
        }

        // if the passive pet is between min-max distance to owner,
        // it will turn and start running torwards its owner

        private static readonly float MinDistance = 2.0f;
        private static readonly float MaxDistance = 192.0f;

        private static readonly float CastDistance = 96.0f;

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
            mvp.WalkRunThreshold = 0.0f;

            //mvp.UseFinalHeading = true;

            PhysicsObj.MoveToObject(P_PetOwner.PhysicsObj, mvp);

            // prevent snap forward
            PhysicsObj.UpdateTime = Physics.Common.PhysicsTimer.CurrentTime;
        }

        /// <summary>
        /// Broadcasts passive pet movement to clients
        /// </summary>
        public override void MoveTo(WorldObject target, float runRate = 1.0f)
        {
            if (!IsPassivePet)
            {
                base.MoveTo(target, runRate);
                return;
            }

            if (MoveSpeed == 0.0f)
                GetMovementSpeed();

            var motion = new Motion(this, target, MovementType.MoveToObject);

            motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge | MovementParams.StopCompletely;
            motion.MoveToParameters.DistanceToObject = MinDistance;
            motion.MoveToParameters.WalkRunThreshold = 0.0f;

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

            if (!IsPassivePet)
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
        /// A pet casts spells on its owner every 30 seconds?
        /// </summary>
        private static readonly TimeSpan CastInterval = TimeSpan.FromSeconds(30);

        private DateTime NextCastTime;

        /// <summary>
        /// Passive pet cast spells on its owner
        /// </summary>
        public void TryCastSpells()
        {
            // deprecated: should use Heartbeat emotes and PetCastSpellOnOwner
            foreach (var _spell in Biota.BiotaPropertiesSpellBook)
            {
                var spell = new Spell(_spell.Spell);

                if (spell.NotFound)
                {
                    log.Error($"{Name}.CastSpells(): spell {_spell.Spell} not found");
                    continue;
                }

                TryCastSpell(spell, P_PetOwner);
            }
            NextCastTime = DateTime.UtcNow + CastInterval;
        }

        public static Dictionary<uint, float> PetRadiusCache = new Dictionary<uint, float>();

        private float GetPetRadius()
        {
            if (PetRadiusCache.TryGetValue(WeenieClassId, out var radius))
                return radius;

            var setup = DatManager.PortalDat.ReadFromDat<SetupModel>(SetupTableId);

            var scale = ObjScale ?? 1.0f;

            return ProjectileRadiusCache[WeenieClassId] = setup.Spheres[0].Radius * scale;
        }
    }
}
