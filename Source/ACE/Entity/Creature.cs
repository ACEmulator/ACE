using ACE.Entity.Enum;
using ACE.Entity.PlayerActions;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.StateMachines.Rules;
using ACE.StateMachines;
using ACE.StateMachines.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using log4net;

namespace ACE.Entity
{
    public class Creature : Container, ITickable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected Dictionary<Enum.Ability, CreatureAbility> abilities = new Dictionary<Enum.Ability, CreatureAbility>();

        public ReadOnlyDictionary<Enum.Ability, CreatureAbility> Abilities;

        public CreatureAbility Strength { get; set; }

        public CreatureAbility Endurance { get; set; }

        public CreatureAbility Coordination { get; set; }

        public CreatureAbility Quickness { get; set; }

        public CreatureAbility Focus { get; set; }

        public CreatureAbility Self { get; set; }

        public CreatureVital Health { get; set; }

        public CreatureVital Stamina { get; set; }

        public CreatureVital Mana { get; set; }

        /// <summary>
        /// This is used to allow us to queue up game actions that we are out of range to preform
        /// </summary>
        public QueuedGameAction BlockedGameAction { get; set; }

        /// <summary>
        /// If we need to move, where do we need to go?   This may be replaced by a property that Mogwai was discussing
        /// </summary>
        // TODO: Revisit to see if still needed
        public Position MoveToPosition { get; set; }

        /// <summary>
        /// What is the square of the sum of use radius plus csphear
        /// </summary>
        public float ArrivedRadiusSquared { get; set; }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Time after Creature dies until it respawns
        /// </summary>
        public double RespawnTime { get; set; }

        /// <summary>
        /// Track state of creature if their current action is blocked.   Either until they are no longer blocked or the action is abandoned.
        /// </summary>
        private readonly StateMachine movementStateMachine = new StateMachine();

        /// <summary>
        /// List of actions that don't have time delays -- like IDs or health queries
        /// We can have many of these running at one (but probably limited -- enforce at enqueue)
        /// </summary>
        protected List<ObjectAction> nonBlockingActions = new List<ObjectAction>();

        protected ObjectAction currentAction = null;
        protected ObjectAction nextAction = null;

        public MovementStates CreatureMovementStates
        { 
          get { return (MovementStates)movementStateMachine.CurrentState; }
          set { movementStateMachine.ChangeState((int)value); }
        }

        public Creature(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
            this.movementStateMachine.Initialize(MovementRules.GetRules(), MovementRules.GetInitState());
        }

        public Creature(AceCreatureStaticLocation aceC)
            : base((ObjectType)aceC.CreatureData.TypeId,
                  new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.Creature),
                  aceC.CreatureData.Name,
                  aceC.WeenieClassId,
                  (ObjectDescriptionFlag)aceC.CreatureData.WdescBitField,
                  (WeenieHeaderFlag)aceC.CreatureData.WeenieFlags,
                  aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            SetObjectData(aceC.CreatureData);
            SetAbilities(aceC.CreatureData);
        }

        public void SetDestinationInformation(Position position, float arrivedRadiusSquared)
        {
            MoveToPosition = position;
            ArrivedRadiusSquared = arrivedRadiusSquared;
        }

        public void ClearDestinationInformation()
        {
            MoveToPosition = null;
            ArrivedRadiusSquared = 0.0f;
        }

        private void SetObjectData(AceCreatureObject aco)
        {
            PhysicsData.CurrentMotionState = new UniversalMotion(MotionStance.Standing);
            PhysicsData.MTableResourceId = aco.MotionTableId;
            PhysicsData.Stable = aco.SoundTableId;
            PhysicsData.CSetup = aco.ModelTableId;
            PhysicsData.Petable = aco.PhysicsTableId;
            PhysicsData.ObjScale = aco.ObjectScale;

            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aco.PhysicsBitField;
            PhysicsData.PhysicsState = (PhysicsState)aco.PhysicsState;

            // game data min required flags;
            Icon = aco.IconId;

            GameData.ContainerCapacity = aco.ContainersCapacity;
            GameData.ItemCapacity = aco.ItemsCapacity;
            GameData.Usable = (Usable)aco.Usability;
            // intersting finding: the radar color is influenced over the weenieClassId and NOT the blipcolor
            // the blipcolor in DB is 0 whereas the enum suggests it should be 2
            GameData.RadarColour = (RadarColor)aco.BlipColor;
            GameData.RadarBehavior = (RadarBehavior)aco.Radar;
            GameData.UseRadius = aco.UseRadius;

            aco.WeenieAnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, ao.AnimationId));
            aco.WeenieTextureMapOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, to.OldId, to.NewId));
            aco.WeeniePaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
            ModelData.PaletteGuid = aco.PaletteId;
        }

        private void SetAbilities(AceCreatureObject aco)
        {
            Strength = new CreatureAbility(Enum.Ability.Strength);
            Endurance = new CreatureAbility(Enum.Ability.Endurance);
            Coordination = new CreatureAbility(Enum.Ability.Coordination);
            Quickness = new CreatureAbility(Enum.Ability.Quickness);
            Focus = new CreatureAbility(Enum.Ability.Focus);
            Self = new CreatureAbility(Enum.Ability.Self);

            // TODO: Real regen rates?
            Health = new CreatureVital(aco, Enum.Ability.Health, .5);
            Stamina = new CreatureVital(aco, Enum.Ability.Stamina, 1.0);
            Mana = new CreatureVital(aco, Enum.Ability.Mana, .7);

            Strength.Base = aco.Strength;
            Endurance.Base = aco.Endurance;
            Coordination.Base = aco.Coordination;
            Quickness.Base = aco.Quickness;
            Focus.Base = aco.Focus;
            Self.Base = aco.Self;

            // recalculate the base value as the abilities end/will have an influence on those
            Health.Base = aco.Health - Health.UnbuffedValue;
            Stamina.Base = aco.Stamina - Stamina.UnbuffedValue;
            Mana.Base = aco.Mana - Mana.UnbuffedValue;

            Health.Current = Health.MaxValue;
            Stamina.Current = Stamina.MaxValue;
            Mana.Current = Mana.MaxValue;

            abilities.Add(Enum.Ability.Strength, Strength);
            abilities.Add(Enum.Ability.Endurance, Endurance);
            abilities.Add(Enum.Ability.Coordination, Coordination);
            abilities.Add(Enum.Ability.Quickness, Quickness);
            abilities.Add(Enum.Ability.Focus, Focus);
            abilities.Add(Enum.Ability.Self, Self);

            abilities.Add(Enum.Ability.Health, Health);
            abilities.Add(Enum.Ability.Stamina, Stamina);
            abilities.Add(Enum.Ability.Mana, Mana);

            Abilities = new ReadOnlyDictionary<Enum.Ability, CreatureAbility>(abilities);

            IsAlive = true;
        }

        public virtual IEnumerator ActOnKill(Session session)
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
            session.Player.SendMovementEvent(motionDeath);

            // Create Corspe and set a location on the ground
            // TODO: set text of killer in description and find a better computation for the location, some corpse could end up in the ground
            var corpse = CorpseObjectFactory.CreateCorpse(this, this.Location);
            corpse.Location.PositionY -= corpse.PhysicsData.ObjScale;
            corpse.Location.PositionZ -= corpse.PhysicsData.ObjScale / 2;

            // Corpses stay on the ground for 5 * player level but minimum 1 hour
            // corpse.DespawnTime = Math.Max((int)session.Player.PropertiesInt[Enum.Properties.PropertyInt.Level] * 5, 360) + WorldManager.PortalYearTicks; // as in live
            corpse.DespawnTime = 20 + WorldManager.PortalYearTicks; // only for testing

            // If the object is a creature, Remove it from from Landblock
            // FIXME(ddevec): This is the landblock's job? -- 
            if (!isDerivedPlayer)
            {
                LandblockManager.RemoveObject(this);
            }

            // Add Corpse in that location via the ActionQueue to honor the motion delays
            LandblockManager.AddObject(corpse);
            yield break;
        }

        /// <summary>
        /// Called on the main loop of the Landblock, intended to do time-based maintenance of creatures
        /// </summary>
        // FIXME(ddevec): Perhaps world-objects should have this and this should be an override?
        public virtual void Tick(double tickTime, LazyBroadcastList broadcaster)
        {
            // TODO: Realistic rates && adjusting rates for spells...
            Health.Tick(tickTime);
            Stamina.Tick(tickTime);
            Mana.Tick(tickTime);

            List<ObjectAction> newNBActions = new List<ObjectAction>();
            foreach (ObjectAction actn in nonBlockingActions)
            {
                actn.RunNext();
                if (!actn.Done)
                {
                    // Not strictly an error, but unexpected
                    log.Warn("Non-blocking action takes more than 1 Tick to run?");
                    newNBActions.Add(actn);
                }
            }

            nonBlockingActions = newNBActions;

            if (currentAction != null)
            {
                currentAction.RunNext();
                if (currentAction.Done)
                {
                    currentAction = nextAction;
                    nextAction = null;
                }
            }

            DoBroadcast(broadcaster);
        }

        /// <summary>
        /// Updates the current action -- Locks "this" implicitly
        /// </summary>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RequestAction(ObjectAction action)
        {
            // FIXME(ddevec): If curAction != null it needs to be canceled, etc.
            currentAction = action;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RequestChainAction(ObjectAction action)
        {
            // FIXME(ddevec): If curAction != null it needs to be canceled, etc.
            nextAction = action;
        }

        /// <summary>
        /// Updates the current action -- Locks "this" implicitly
        /// </summary>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RequestAction(Func<IEnumerator> actFunc)
        {
            // FIXME(ddevec): If curAction != null it needs to be canceled, etc.
            RequestAction(new DelegateAction(actFunc));
        }

        /// <summary>
        /// Updates the current action -- Locks "this" implicitly
        /// </summary>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RequestChainAction(Func<IEnumerator> actFunc)
        {
            // FIXME(ddevec): If curAction != null it needs to be canceled, etc.
            RequestChainAction(new DelegateAction(actFunc));
        }
    }
}
