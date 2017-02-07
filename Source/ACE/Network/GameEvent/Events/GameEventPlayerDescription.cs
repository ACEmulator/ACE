using ACE.Entity;
using System;

namespace ACE.Network.GameEvent
{
    public class GameEventPlayerDescription : GameEventPacket
    {
        private Character _character;

        [Flags]
        private enum DescriptionPropertyFlag
        {
            None             = 0x0000,
            PropertyInt32    = 0x0001,
            PropertyBool     = 0x0002,
            PropertyDouble   = 0x0004,
            Link             = 0x0008,
            PropertyString   = 0x0010,
            Position         = 0x0020,
            Resource         = 0x0040,
            PropertyInt64    = 0x0080,
        }

        [Flags]
        private enum DescriptionVectorFlag
        {
            None             = 0x0000,
            Attribute        = 0x0001,
            Skill            = 0x0002,
            Spell            = 0x0100,
            Enchantment      = 0x0200
        }

        [Flags]
        private enum DescriptionAttributeFlag
        {
            None             = 0x0000,
            Strength         = 0x0001,
            Endurance        = 0x0002,
            Quickness        = 0x0004,
            Coordination     = 0x0008,
            Focus            = 0x0010,
            Self             = 0x0020,
            Health           = 0x0040,
            Stamina          = 0x0080,
            Mana             = 0x0100,
            // server always sends full mask (any cases where this shouldn't happen?)
            Full             = Strength | Endurance | Quickness | Coordination | Focus | Self | Health | Stamina | Mana
        }

        [Flags]
        private enum DescriptionOptionFlag
        {
            None             = 0x0000,
            Shortcut         = 0x0001,
            Component        = 0x0008,
            SpellTab         = 0x0010,
            Unk20            = 0x0020,
            CharacterOption2 = 0x0040,
            Unk100           = 0x0100,
            WindowLayout     = 0x0200,
            Unk400           = 0x0400,
        }

        public override GameEventOpcode Opcode { get { return GameEventOpcode.PlayerDescription; } }

        public GameEventPlayerDescription(Session session, Character character)
            : base(session)
        {
            _character = character;
        }

        protected override void WriteEventBody()
        {
            var propertyFlags    = DescriptionPropertyFlag.None;
            var propertyFlagsPos = fragment.Payload.BaseStream.Position;
            fragment.Payload.Write(0u);
            fragment.Payload.Write(0x0Au);

            var propertiesInt = session.Character.PropertiesInt;
            if (propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                fragment.Payload.Write((ushort)propertiesInt.Count);
                fragment.Payload.Write((ushort)0x40);

                foreach (var uintProperty in propertiesInt)
                {
                    fragment.Payload.Write((uint)uintProperty.Key);
                    fragment.Payload.Write(uintProperty.Value);
                }
            }

            var propertiesInt64 = session.Character.PropertiesInt64;
            if (propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                fragment.Payload.Write((ushort)propertiesInt64.Count);
                fragment.Payload.Write((ushort)0x40);

                foreach (var uint64Property in propertiesInt64)
                {
                    fragment.Payload.Write((uint)uint64Property.Key);
                    fragment.Payload.Write(uint64Property.Value);
                }
            }

            var propertiesBool = session.Character.PropertiesBool;
            if (propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                fragment.Payload.Write((ushort)propertiesBool.Count);
                fragment.Payload.Write((ushort)0x20);

                foreach (var boolProperty in propertiesBool)
                {
                    fragment.Payload.Write((uint)boolProperty.Key);
                    fragment.Payload.Write(Convert.ToUInt32(boolProperty.Value)); // just as fast as inlining
                }
            }

            var propertiesDouble = session.Character.PropertiesDouble;
            if (propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                fragment.Payload.Write((ushort)propertiesDouble.Count);
                fragment.Payload.Write((ushort)0x20);

                foreach (var doubleProperty in propertiesDouble)
                {
                    fragment.Payload.Write((uint)doubleProperty.Key);
                    fragment.Payload.Write(doubleProperty.Value);
                }
            }

            var propertiesString = session.Character.PropertiesString;
            if (propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                fragment.Payload.Write((ushort)propertiesString.Count);
                fragment.Payload.Write((ushort)0x10);

                foreach (var stringProperty in propertiesString)
                {
                    fragment.Payload.Write((uint)stringProperty.Key);
                    fragment.Payload.WriteString16L(stringProperty.Value);
                }
            }

            /*if ((propertyFlags & DescriptionPropertyFlag.Resource) != 0)
            {
            }*/

            /*if ((propertyFlags & DescriptionPropertyFlag.Link) != 0)
            {
            }*/

            /*if ((propertyFlags & DescriptionPropertyFlag.Position) != 0)
            {
            }*/

            fragment.Payload.WritePosition((uint)propertyFlags, propertyFlagsPos);

            var vectorFlags = DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;
            fragment.Payload.Write((uint)vectorFlags);
            fragment.Payload.Write(1u);
            
            if ((vectorFlags & DescriptionVectorFlag.Attribute) != 0)
            {
                var attributeFlags = DescriptionAttributeFlag.Full;
                fragment.Payload.Write((uint)attributeFlags);

                if ((attributeFlags & DescriptionAttributeFlag.Strength) != 0)
                {
                    fragment.Payload.Write(_character.Strength.Ranks); // ranks
                    fragment.Payload.Write(_character.Strength.Value);
                    fragment.Payload.Write(_character.Strength.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Endurance) != 0)
                {
                    fragment.Payload.Write(_character.Endurance.Ranks); // ranks
                    fragment.Payload.Write(_character.Endurance.Value);
                    fragment.Payload.Write(_character.Endurance.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Quickness) != 0)
                {
                    fragment.Payload.Write(_character.Quickness.Ranks); // ranks
                    fragment.Payload.Write(_character.Quickness.Value);
                    fragment.Payload.Write(_character.Quickness.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Coordination) != 0)
                {
                    fragment.Payload.Write(_character.Coordination.Ranks); // ranks
                    fragment.Payload.Write(_character.Coordination.Value);
                    fragment.Payload.Write(_character.Coordination.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Focus) != 0)
                {
                    fragment.Payload.Write(_character.Focus.Ranks); // ranks
                    fragment.Payload.Write(_character.Focus.Value);
                    fragment.Payload.Write(_character.Focus.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Self) != 0)
                {
                    fragment.Payload.Write(_character.Self.Ranks); // ranks
                    fragment.Payload.Write(_character.Self.Value);
                    fragment.Payload.Write(_character.Self.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Health) != 0)
                {
                    fragment.Payload.Write(_character.Health.Ranks); // ranks
                    fragment.Payload.Write(0u); // unknown
                    fragment.Payload.Write(_character.Health.ExperienceSpent); // xp spent
                    fragment.Payload.Write(_character.Health.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Stamina) != 0)
                {
                    fragment.Payload.Write(_character.Stamina.Ranks); // ranks
                    fragment.Payload.Write(0u); // unknown
                    fragment.Payload.Write(_character.Stamina.ExperienceSpent); // xp spent
                    fragment.Payload.Write(_character.Stamina.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Mana) != 0)
                {
                    fragment.Payload.Write(_character.Mana.Ranks); // ranks
                    fragment.Payload.Write(0u); // unknown
                    fragment.Payload.Write(_character.Mana.ExperienceSpent); // xp spent
                    fragment.Payload.Write(_character.Mana.Current); // current value
                }
            }

            // TODO: need DB support for this
            if ((vectorFlags & DescriptionVectorFlag.Skill) != 0)
            {
                
                fragment.Payload.Write((ushort)_character.Skills.Count);
                fragment.Payload.Write((ushort)0x20); // unknown

                foreach (var skill in _character.Skills)
                {
                    fragment.Payload.Write((uint)skill.Key); // skill id
                    fragment.Payload.Write((ushort)skill.Value.Ranks); // points raised
                    fragment.Payload.Write((ushort)0);
                    fragment.Payload.Write((uint)skill.Value.Status); // skill state
                    fragment.Payload.Write((uint)skill.Value.ExperienceSpent); // xp spent on this skill
                    fragment.Payload.Write(0u); // current bonus points applied (buffs) - not implemented
                    fragment.Payload.Write(0u); // task difficulty
                    fragment.Payload.Write(0d);
                }
            }

            /*if ((vectorFlags & DescriptionVectorFlag.Spell) != 0)
            {
                fragment.Payload.Write((ushort)spellCount);
                fragment.Payload.Write((ushort)0x20);

                {
                    fragment.Payload.Write(0u);
                    fragment.Payload.Write(0.0f);
                }
            }*/

            /*if ((vectorFlags & DescriptionVectorFlag.Enchantment) != 0)
            {
            }*/

            var optionFlags = DescriptionOptionFlag.None;
            fragment.Payload.Write((uint)optionFlags);
            fragment.Payload.Write(0x11C4E56A);

            /*if ((optionFlags & DescriptionOptionFlag.Shortcut) != 0)
            {
            }*/

            fragment.Payload.Write(0u);

            /*if ((optionFlags & DescriptionOptionFlag.SpellTab) != 0)
            {
                // additional spell tabs 2-7
                fragment.Payload.Write(0u);
                fragment.Payload.Write(0u);
                fragment.Payload.Write(0u);
                fragment.Payload.Write(0u);
                fragment.Payload.Write(0u);
                fragment.Payload.Write(0u);
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.Component) != 0)
            {
            }*/

            if ((optionFlags & DescriptionOptionFlag.Unk20) != 0)
                fragment.Payload.Write(0u);

            if ((optionFlags & DescriptionOptionFlag.CharacterOption2) != 0)
                fragment.Payload.Write(0u);

            /*if ((optionFlags & DescriptionOptionFlag.Unk100) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.WindowLayout) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.Unk400) != 0)
            {
            }*/

            fragment.Payload.Write(0u);
            fragment.Payload.Write(0u);
        }
    }
}
