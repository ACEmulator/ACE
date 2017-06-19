using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public CreatureAbility Health
        {
            get { return AceObject.Health; }
            set { AceObject.Health = value; }
        }

        public CreatureAbility Stamina
        {
            get { return AceObject.Stamina; }
            set { AceObject.Stamina = value; }
        }

        public CreatureAbility Mana
        {
            get { return AceObject.Mana; }
            set { AceObject.Mana = value; }
        }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Time after Creature dies until it respawns
        /// </summary>
        public double RespawnTime { get; set; }

        public Creature(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Creature(AceCreatureStaticLocation aceC)
            : base((ObjectType)aceC.WeenieObject.ItemType,
                  new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.Creature),
                  aceC.WeenieObject.Name,
                  aceC.WeenieClassId,
                  (ObjectDescriptionFlag)aceC.WeenieObject.AceObjectDescriptionFlags,
                  (WeenieHeaderFlag)aceC.WeenieObject.WeenieHeaderFlags,
                  aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            SetObjectData(aceC.WeenieObject);
            SetAbilities(aceC.WeenieObject);
        }

        public Creature(AceObject baseObject) : base(baseObject)
        {
            // FIXME(ddevec): Once physics data has been refactored this shouldn't be needed...
            SetObjectData(baseObject);
        }

        private void SetObjectData(AceObject aco)
        {
            PhysicsData.CurrentMotionState = new UniversalMotion(MotionStance.Standing);
            PhysicsData.MTableResourceId = aco.MotionTableId;
            PhysicsData.Stable = aco.SoundTableId;
            PhysicsData.CSetup = aco.ModelTableId;
            PhysicsData.Petable = aco.PhysicsTableId;
            PhysicsData.ObjScale = aco.DefaultScale;
            PhysicsData.PhysicsState = (PhysicsState)aco.PhysicsState;
            PhysicsData.Position = aco.Location;
        }

        protected void SetAbilities(AceObject aco)
        {
            // TODO: determine if this is necessary
            // recalculate the base value as the abilities end/will have an influence on those
            // Health.Base = aco.Health - Health.UnbuffedValue;
            // Stamina.Base = aco.Stamina - Stamina.UnbuffedValue;
            // Mana.Base = aco.Mana - Mana.UnbuffedValue;

            // FIXME(ddevec): Should we always start w/ max's? Probably not...
            Health.Current = Health.MaxValue;
            Stamina.Current = Stamina.MaxValue;
            Mana.Current = Mana.MaxValue;

            IsAlive = true;
        }

        public virtual void OnKill(Session session)
        {
            IsAlive = false;
            // This will determine if the derived type is a player
            var isDerivedPlayer = Guid.IsPlayer();
            // TODO: Implement some proper respawn timers, check the generators for that
            RespawnTime = WorldManager.PortalYearTicks + 10;

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
            QueuedGameAction actionDeath = new QueuedGameAction(this.Guid.Full, motionDeath, 2.0f, true, GameActionType.MovementEvent);
            session.Player.AddToActionQueue(actionDeath);

            // Create Corspe and set a location on the ground
            // TODO: set text of killer in description and find a better computation for the location, some corpse could end up in the ground
            var corpse = CorpseObjectFactory.CreateCorpse(this, this.Location);
            corpse.Location.PositionY -= corpse.PhysicsData.ObjScale ?? 0;
            corpse.Location.PositionZ -= (corpse.PhysicsData.ObjScale ?? 0) / 2;

            // Corpses stay on the ground for 5 * player level but minimum 1 hour
            // corpse.DespawnTime = Math.Max((int)session.Player.PropertiesInt[Enum.Properties.PropertyInt.Level] * 5, 360) + WorldManager.PortalYearTicks; // as in live
            corpse.DespawnTime = 20 + WorldManager.PortalYearTicks; // only for testing

            // If the object is a creature, Remove it from from Landblock
            if (!isDerivedPlayer)
            {
                QueuedGameAction removeCreature = new QueuedGameAction(this.Guid.Full, this, true, true, GameActionType.ObjectDelete);
                session.Player.AddToActionQueue(removeCreature);
            }

            // Add Corpse in that location via the ActionQueue to honor the motion delays
            QueuedGameAction addCorpse = new QueuedGameAction(this.Guid.Full, corpse, true, GameActionType.ObjectCreate);
            session.Player.AddToActionQueue(addCorpse);
        }
    }
}
