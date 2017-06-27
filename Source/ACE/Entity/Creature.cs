using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class Creature : Container
    {
        public CreatureAbility Strength {
            get { return AceObject.StrengthAbility; }
            set { AceObject.StrengthAbility = value; }
        }

        public CreatureAbility Endurance
        {
            get { return AceObject.EnduranceAbility; }
            set { AceObject.EnduranceAbility = value; }
        }

        public CreatureAbility Coordination
        {
            get { return AceObject.CoordinationAbility; }
            set { AceObject.CoordinationAbility = value; }
        }

        public CreatureAbility Quickness
        {
            get { return AceObject.QuicknessAbility; }
            set { AceObject.QuicknessAbility = value; }
        }

        public CreatureAbility Focus
        {
            get { return AceObject.FocusAbility; }
            set { AceObject.FocusAbility = value; }
        }

        public CreatureAbility Self
        {
            get { return AceObject.SelfAbility; }
            set { AceObject.SelfAbility = value; }
        }

        public CreatureVital Health
        {
            get { return AceObject.Health; }
            set { AceObject.Health = value; }
        }

        public CreatureVital Stamina
        {
            get { return AceObject.Stamina; }
            set { AceObject.Stamina = value; }
        }

        public CreatureVital Mana
        {
            get { return AceObject.Mana; }
            set { AceObject.Mana = value; }
        }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get; set; }

        public double RespawnTime { get; set; }

        public Creature(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        protected void SetupVitals()
        {
            if (Health.Current != Health.MaxValue)
            {
                VitalTick(Health);
            }
            if (Stamina.Current != Stamina.MaxValue)
            {
                VitalTick(Stamina);
            }
            if (Mana.Current != Mana.MaxValue)
            {
                VitalTick(Mana);
            }
        }

        public Creature(AceObject baseObject)
            : base(baseObject)
        {
        }

        public virtual ActionChain GetOnKillChain(Session killerSession)
        {
            ActionChain onKillChain = new ActionChain();
            /*
            // Wait to respawn
            onKillChain.AddDelayTicks(10);
            */

            // Will start death animation
            onKillChain.AddAction(this, () => OnKill(killerSession));
            // Wait for kill animation
            onKillChain.AddDelaySeconds(2);
            onKillChain.AddChain(GetCreateCorpseChain());

            return onKillChain;
        }

        virtual protected float GetCorpseSpawnTime()
        {
            return 60;
        }

        public ActionChain GetCreateCorpseChain()
        {
            ActionChain createCorpseChain = new ActionChain(this, () =>
            {
                // Create Corspe and set a location on the ground
                // TODO: set text of killer in description and find a better computation for the location, some corpse could end up in the ground
                var corpse = CorpseObjectFactory.CreateCorpse(this, this.Location);
                // FIXME(ddevec): We don't have a real corpse yet, so these come in null -- this hack just stops them from crashing the game
                corpse.Location.PositionY -= (corpse.ObjScale ?? 0);
                corpse.Location.PositionZ -= (corpse.ObjScale ?? 0) / 2;

                // Corpses stay on the ground for 5 * player level but minimum 1 hour
                // corpse.DespawnTime = Math.Max((int)session.Player.PropertiesInt[Enum.Properties.PropertyInt.Level] * 5, 360) + WorldManager.PortalYearTicks; // as in live
                // corpse.DespawnTime = 20 + WorldManager.PortalYearTicks; // only for testing
                float despawnTime = GetCorpseSpawnTime();

                // Create corpse
                CurrentLandblock.AddWorldObject(corpse);
                // Create corpse decay
                ActionChain despawnChain = new ActionChain();
                despawnChain.AddDelaySeconds(despawnTime);
                despawnChain.AddAction(CurrentLandblock, () => corpse.CurrentLandblock.RemoveWorldObject(corpse.Guid, false));
                despawnChain.EnqueueChain();
            });

            return createCorpseChain;
        }

        private void OnKill(Session session)
        {
            IsAlive = false;
            // This will determine if the derived type is a player
            var isDerivedPlayer = Guid.IsPlayer();

            if (!isDerivedPlayer)
            {
                // Create and send the death notice
                string killMessage = $"{session.Player.Name} has killed {Name}.";
                var creatureDeathEvent = new GameEventDeathNotice(session, killMessage);
                session.Network.EnqueueSend(creatureDeathEvent);
            }

            // MovementEvent: (Hand-)Combat or in the case of smite: from Standing to Death
            // TODO: Check if the duration of the motion can somehow be computed
            UniversalMotion motionDeath = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            CurrentLandblock.EnqueueBroadcastMotion(this, motionDeath);

            // If the object is a creature, Remove it from from Landblock
            if (!isDerivedPlayer)
            {
                CurrentLandblock.RemoveWorldObject(Guid, false);
            }
        }

        /// <summary>
        /// Updates a vital, returns true if vital is now < max
        /// </summary>
        /// <param name="vital"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public void UpdateVital(CreatureVital vital, uint newVal)
        {
            EnqueueAction(new ActionEventDelegate(() => UpdateVitalInternal(vital, newVal)));
        }

        public void DeltaVital(CreatureVital vital, long delta)
        {
            EnqueueAction(new ActionEventDelegate(() => DeltaVitalInternal(vital, delta)));
        }

        private void DeltaVitalInternal(CreatureVital vital, long delta)
        {
            uint absVal;

            if (delta < 0 && Math.Abs(delta) > vital.Current)
            {
                absVal = (uint)(-1 * vital.Current);
            }
            else if (delta + vital.Current > vital.MaxValue)
            {
                absVal = (uint)(vital.MaxValue - vital.Current);
            }
            else
            {
                absVal = (uint)(vital.Current + delta);
            }

            UpdateVitalInternal(vital, absVal);
        }

        private void VitalTick(CreatureVital vital)
        {
            double tickTime = vital.NextTickTime;
            if (tickTime == double.NegativeInfinity)
            {
                tickTime = vital.RegenRate;
            }
            else
            {
                tickTime -= WorldManager.PortalYearTicks;
            }

            // Set up our next tick
            ActionChain tickChain = new ActionChain();
            tickChain.AddDelayTicks(tickTime);
            tickChain.AddAction(this, () => VitalTickInternal(vital));
            tickChain.EnqueueChain();
        }

        protected virtual void VitalTickInternal(CreatureVital vital)
        {
            vital.Tick(WorldManager.PortalYearTicks);
            if (vital.Current != vital.MaxValue)
            {
                VitalTick(vital);
            }
        }

        protected virtual void UpdateVitalInternal(CreatureVital vital, uint newVal)
        {
            uint old = vital.Current;

            if (newVal > vital.MaxValue)
            {
                newVal = (vital.MaxValue - vital.Current);
            }

            vital.Current = newVal;

            // Check for amount
            if (old == vital.MaxValue && vital.Current != vital.MaxValue)
            {
                // Start up a vital ticker
                new ActionChain(this, () => VitalTickInternal(vital)).EnqueueChain();
            }
        }
    }
}
