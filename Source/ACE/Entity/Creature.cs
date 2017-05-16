using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Creature : Container
    {
        protected Dictionary<Enum.Ability, CreatureAbility> abilities = new Dictionary<Enum.Ability, CreatureAbility>();

        public ReadOnlyDictionary<Enum.Ability, CreatureAbility> Abilities;

        public CreatureAbility Strength { get; set; }

        public CreatureAbility Endurance { get; set; }

        public CreatureAbility Coordination { get; set; }

        public CreatureAbility Quickness { get; set; }

        public CreatureAbility Focus { get; set; }

        public CreatureAbility Self { get; set; }

        public CreatureAbility Health { get; set; }

        public CreatureAbility Stamina { get; set; }

        public CreatureAbility Mana { get; set; }

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
            : base((ObjectType)aceC.CreatureData.TypeId,
                  new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.Creature),
                  aceC.CreatureData.Name,
                  aceC.WeenieClassId,
                  (ObjectDescriptionFlag)aceC.CreatureData.AceObjectDescriptionFlags,
                  (WeenieHeaderFlag)aceC.CreatureData.WeenieHeaderFlags,
                  aceC.Position)
        {
            if (aceC.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceC.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceC.WeenieClassId - 0x8000);

            SetObjectData(aceC.CreatureData);
            SetAbilities(aceC.CreatureData);
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
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aco.PhysicsDescriptionFlag;
            PhysicsData.PhysicsState = (PhysicsState)aco.PhysicsState;

            // game data min required flags;
            Icon = aco.IconId;

            // TODO: Check to see if we should default a 0 to fix these possible null errors Og II
            GameData.ContainerCapacity = (byte)aco.ContainersCapacity;
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
            ModelData.PaletteGuid = (uint)aco.PaletteId;
        }

        private void SetAbilities(AceCreatureObject aco)
        {
            Strength = new CreatureAbility(aco, Enum.Ability.Strength);
            Endurance = new CreatureAbility(aco, Enum.Ability.Endurance);
            Coordination = new CreatureAbility(aco, Enum.Ability.Coordination);
            Quickness = new CreatureAbility(aco, Enum.Ability.Quickness);
            Focus = new CreatureAbility(aco, Enum.Ability.Focus);
            Self = new CreatureAbility(aco, Enum.Ability.Self);

            Health = new CreatureAbility(aco, Enum.Ability.Health);
            Stamina = new CreatureAbility(aco, Enum.Ability.Stamina);
            Mana = new CreatureAbility(aco, Enum.Ability.Mana);

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
            corpse.Location.PositionY -= corpse.PhysicsData.ObjScale;
            corpse.Location.PositionZ -= corpse.PhysicsData.ObjScale / 2;

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
