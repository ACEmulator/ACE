using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Creature : MutableWorldObject
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

        public Creature(AceCreatureStaticLocation aceC)
            : base((ObjectType)aceC.CreatureData.TypeId, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.Creature), aceC.CreatureData.Name, aceC.WeenieClassId,
                  (ObjectDescriptionFlag)aceC.CreatureData.WdescBitField, (WeenieHeaderFlag)aceC.CreatureData.WeenieFlags, aceC.Position)
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
            PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing);
            PhysicsData.MTableResourceId = aco.MotionTableId;
            PhysicsData.Stable = aco.SoundTableId;
            PhysicsData.CSetup = aco.ModelTableId;
            PhysicsData.Petable = aco.PhysicsTableId;
            PhysicsData.ObjScale = aco.ObjectScale;
            
            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aco.PhysicsBitField;
            PhysicsData.PhysicsState = (PhysicsState)aco.PhysicsState;

            // game data min required flags;
            Icon = (ushort)aco.IconId;

            GameData.Usable = (Usable)aco.Usability;
            // intersting finding: the radar color is influenced over the weenieClassId and NOT the blipcolor
            // the blipcolor in DB is 0 whereas the enum suggests it should be 2
            GameData.RadarColour = (RadarColor)aco.BlipColor;
            GameData.RadarBehavior = (RadarBehavior)aco.Radar;
            GameData.UseRadius = aco.UseRadius;

            aco.WeenieAnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)(ao.AnimationId - 0x01000000)));
            aco.WeenieTextureMapOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)(to.OldId - 0x05000000), (ushort)(to.NewId - 0x05000000)));
            aco.WeeniePaletteOverrides.ForEach(po => this.ModelData.AddPalette((ushort)(po.SubPaletteId - 0x04000000), (byte)po.Offset, (byte)(po.Length / 8)));
            ModelData.PaletteGuid = aco.PaletteId - 0x04000000;
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

        public void Kill(Session session)
        {
            IsAlive = false;

            // Send UpdateHealth Message with 0 Health - this is not necessary according to live pcaps
            // var healthLossEvent = new GameEventUpdateHealth(session, Guid.Full, 0f);
            // session.Network.EnqueueSend(healthLossEvent);

            // 1. step in retail: Create and send the death notice
            string killMessage = $"{session.Player.Name} has killed {Name}.";
            var creatureDeathEvent = new GameEventDeathNotice(session, killMessage);
            session.Network.EnqueueSend(creatureDeathEvent);

            // 2. step in retail: SoundEvent: sound 48 (HitFlesh1), volume 0,5
            session.Player.ActionApplySoundEffect(Sound.HitFlesh1, Guid);

            // 3. step in retail: SoundEvent: sound 14 (Wound3), volume 1
            session.Player.ActionApplySoundEffect(Sound.Wound3, Guid);

            // 4. step in retail: MovementEvent: (Hand-)Combat to Death
            // TODO: this isn't really working yet, we need more work on the motion stuff
            GeneralMotion motion1 = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            session.Network.EnqueueSend(new GameMessageUpdateMotion(this, session, motion1));

            // 5. step in retail: Play Script to show wound depending on where the last hit was
            // TODO: this is not showing yet, guess we need to give all the sounds and scripts some time to actually play
            var effectEvent = new GameMessageScript(Guid, Network.Enum.PlayScript.SplatterMidRightFront);
            session.Network.EnqueueSend(effectEvent);

            // 6. step in retail: UpdatePosition: gotta verify this on more examples
            // I guess it lowers the z axis, so the corpse isn't hovering in about the midth of a usual drudge

            // 7. step in retail: DeleteObject Creature
            // Remove Creature from Landblock this sends GameMessageRemoveObject
            LandblockManager.RemoveObject(this);

            // 8. step in retail: Create Corpse (dead for now - but guess is standing or in combat should be right)
            // TODO: set text of killer in description
            var corpse = CorpseObjectFactory.CreateCorpse(this, this.Location);
            LandblockManager.AddObject(corpse);

            // 9. step in retail: Death Animation of corpse
            GeneralMotion motion2 = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            session.Network.EnqueueSend(new GameMessageUpdateMotion(corpse, session, motion2));            
        }
    }
}
