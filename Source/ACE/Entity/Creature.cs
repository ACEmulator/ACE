using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            this.PhysicsData.MTableResourceId = aco.MotionTableId;
            this.PhysicsData.Stable = aco.SoundTableId;
            this.PhysicsData.CSetup = aco.ModelTableId;
            this.PhysicsData.ObjScale = aco.ObjectScale;
            
            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aco.PhysicsBitField;
            this.PhysicsData.PhysicsState = (PhysicsState)aco.PhysicsState;

            // game data min required flags;
            this.Icon = (ushort)aco.IconId;

            this.GameData.Usable = (Usable)aco.Usability;
            // intersting finding: the radar color is influenced over the weenieClassId and NOT the blipcolor
            // the blipcolor in DB is 0 whereas the enum suggests it should be 2
            this.GameData.RadarColour = (RadarColor)aco.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aco.Radar;
            this.GameData.UseRadius = aco.UseRadius;

            aco.WeenieAnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)(ao.AnimationId - 0x01000000)));
            aco.WeenieTextureMapOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)(to.OldId - 0x05000000), (ushort)(to.NewId - 0x05000000)));
            aco.WeeniePaletteOverrides.ForEach(po => this.ModelData.AddPalette((ushort)(po.SubPaletteId - 0x04000000), (byte)po.Offset, (byte)(po.Length / 8)));
            this.ModelData.PaletteGuid = aco.PaletteId - 0x04000000;
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
        }
    }
}
