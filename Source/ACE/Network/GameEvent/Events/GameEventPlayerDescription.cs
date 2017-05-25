using System;
using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPlayerDescription : GameEventMessage
    {
        [Flags]
        private enum DescriptionPropertyFlag
        {
            None = 0x0000,
            PropertyInt32 = 0x0001,
            PropertyBool = 0x0002,
            PropertyDouble = 0x0004,
            Link = 0x0008,
            PropertyString = 0x0010,
            Position = 0x0020,
            Resource = 0x0040,
            PropertyInt64 = 0x0080,
        }

        [Flags]
        private enum DescriptionVectorFlag
        {
            None = 0x0000,
            Attribute = 0x0001,
            Skill = 0x0002,
            Spell = 0x0100,
            Enchantment = 0x0200
        }

        [Flags]
        private enum DescriptionAttributeFlag
        {
            None = 0x0000,
            Strength = 0x0001,
            Endurance = 0x0002,
            Quickness = 0x0004,
            Coordination = 0x0008,
            Focus = 0x0010,
            Self = 0x0020,
            Health = 0x0040,
            Stamina = 0x0080,
            Mana = 0x0100,
            // server always sends full mask (any cases where this shouldn't happen?)
            Full = Strength | Endurance | Quickness | Coordination | Focus | Self | Health | Stamina | Mana
        }

        [Flags]
        private enum DescriptionOptionFlag
        {
            None = 0x0000,
            Shortcut = 0x0001,
            Component = 0x0008,
            SpellTab = 0x0010,
            Unk20 = 0x0020,
            CharacterOption2 = 0x0040,
            Unk100 = 0x0100,
            WindowLayout = 0x0200,
            Unk400 = 0x0400,
        }

        public GameEventPlayerDescription(Session session)
            : base(GameEventType.PlayerDescription, GameMessageGroup.Group09, session)
        {
            WriteEventBody();
        }

        private void WriteEventBody()
        {
            var propertyFlags = DescriptionPropertyFlag.None;
            var propertyFlagsPos = Writer.BaseStream.Position;
            Writer.Write(0u);
            Writer.Write(0x0Au);

            var propertiesInt = Session.Player.PropertiesInt;
            if (propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                Writer.Write((ushort)propertiesInt.Count);
                Writer.Write((ushort)0x40);

                foreach (var uintProperty in propertiesInt)
                {
                    Writer.Write((uint)uintProperty.Key);
                    Writer.Write(uintProperty.Value);
                }
            }

            var propertiesInt64 = Session.Player.PropertiesInt64;
            if (propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                Writer.Write((ushort)propertiesInt64.Count);
                Writer.Write((ushort)0x40);

                foreach (var uint64Property in propertiesInt64)
                {
                    Writer.Write((uint)uint64Property.Key);
                    Writer.Write(uint64Property.Value);
                }
            }

            var propertiesBool = Session.Player.PropertiesBool;
            if (propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                Writer.Write((ushort)propertiesBool.Count);
                Writer.Write((ushort)0x20);

                foreach (var boolProperty in propertiesBool)
                {
                    Writer.Write((uint)boolProperty.Key);
                    Writer.Write(Convert.ToUInt32(boolProperty.Value)); // just as fast as inlining
                }
            }

            var propertiesDouble = Session.Player.PropertiesDouble;
            if (propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                Writer.Write((ushort)propertiesDouble.Count);
                Writer.Write((ushort)0x20);

                foreach (var doubleProperty in propertiesDouble)
                {
                    Writer.Write((uint)doubleProperty.Key);
                    Writer.Write(doubleProperty.Value);
                }
            }

            var propertiesString = Session.Player.PropertiesString;
            if (propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                Writer.Write((ushort)propertiesString.Count);
                Writer.Write((ushort)0x10);

                foreach (var stringProperty in propertiesString)
                {
                    Writer.Write((uint)stringProperty.Key);
                    Writer.WriteString16L(stringProperty.Value);
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

            Writer.WritePosition((uint)propertyFlags, propertyFlagsPos);

            var vectorFlags = DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;
            Writer.Write((uint)vectorFlags);
            Writer.Write(1u);

            if ((vectorFlags & DescriptionVectorFlag.Attribute) != 0)
            {
                var attributeFlags = DescriptionAttributeFlag.Full;
                Writer.Write((uint)attributeFlags);

                if ((attributeFlags & DescriptionAttributeFlag.Strength) != 0)
                {
                    Writer.Write(Session.Player.Strength.Ranks); // ranks
                    Writer.Write(Session.Player.Strength.Base);
                    Writer.Write(Session.Player.Strength.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Endurance) != 0)
                {
                    Writer.Write(Session.Player.Endurance.Ranks); // ranks
                    Writer.Write(Session.Player.Endurance.Base);
                    Writer.Write(Session.Player.Endurance.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Quickness) != 0)
                {
                    Writer.Write(Session.Player.Quickness.Ranks); // ranks
                    Writer.Write(Session.Player.Quickness.Base);
                    Writer.Write(Session.Player.Quickness.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Coordination) != 0)
                {
                    Writer.Write(Session.Player.Coordination.Ranks); // ranks
                    Writer.Write(Session.Player.Coordination.Base);
                    Writer.Write(Session.Player.Coordination.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Focus) != 0)
                {
                    Writer.Write(Session.Player.Focus.Ranks); // ranks
                    Writer.Write(Session.Player.Focus.Base);
                    Writer.Write(Session.Player.Focus.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Self) != 0)
                {
                    Writer.Write(Session.Player.Self.Ranks); // ranks
                    Writer.Write(Session.Player.Self.Base);
                    Writer.Write(Session.Player.Self.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Health) != 0)
                {
                    Writer.Write(Session.Player.Health.Ranks); // ranks
                    Writer.Write(0u); // unknown
                    Writer.Write(Session.Player.Health.ExperienceSpent); // xp spent
                    Writer.Write(Session.Player.Health.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Stamina) != 0)
                {
                    Writer.Write(Session.Player.Stamina.Ranks); // ranks
                    Writer.Write(0u); // unknown
                    Writer.Write(Session.Player.Stamina.ExperienceSpent); // xp spent
                    Writer.Write(Session.Player.Stamina.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Mana) != 0)
                {
                    Writer.Write(Session.Player.Mana.Ranks); // ranks
                    Writer.Write(0u); // unknown
                    Writer.Write(Session.Player.Mana.ExperienceSpent); // xp spent
                    Writer.Write(Session.Player.Mana.Current); // current value
                }
            }

            // TODO: need DB support for this
            if ((vectorFlags & DescriptionVectorFlag.Skill) != 0)
            {
                var skills = Session.Player.Skills;

                Writer.Write((ushort)skills.Count);
                Writer.Write((ushort)0x20); // unknown

                foreach (var skill in skills)
                {
                    Writer.Write((uint)skill.Key); // skill id
                    Writer.Write((ushort)skill.Value.Ranks); // points raised
                    Writer.Write((ushort)0);
                    Writer.Write((uint)skill.Value.Status); // skill state
                    Writer.Write((uint)skill.Value.ExperienceSpent); // xp spent on this skill
                    Writer.Write(0u); // current bonus points applied (buffs) - not implemented
                    Writer.Write(0u); // task difficulty
                    Writer.Write(0d);
                }
            }

            /*if ((vectorFlags & DescriptionVectorFlag.Spell) != 0)
            {
                writer.Write((ushort)spellCount);
                writer.Write((ushort)0x20);

                {
                    writer.Write(0u);
                    writer.Write(0.0f);
                }
            }*/

            /*if ((vectorFlags & DescriptionVectorFlag.Enchantment) != 0)
            {
            }*/

            var optionFlags = DescriptionOptionFlag.CharacterOption2;
            Writer.Write((uint)optionFlags);
            Writer.Write(Session.Player.CharacterOptions.GetCharacterOptions1Flag());

            /*if ((optionFlags & DescriptionOptionFlag.Shortcut) != 0)
            {
            }*/

            Writer.Write(0u);

            /*if ((optionFlags & DescriptionOptionFlag.SpellTab) != 0)
            {
                // additional spell tabs 2-7
                writer.Write(0u);
                writer.Write(0u);
                writer.Write(0u);
                writer.Write(0u);
                writer.Write(0u);
                writer.Write(0u);
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.Component) != 0)
            {
            }*/

            if ((optionFlags & DescriptionOptionFlag.Unk20) != 0)
                Writer.Write(0u);

            if ((optionFlags & DescriptionOptionFlag.CharacterOption2) != 0)
                Writer.Write(Session.Player.CharacterOptions.GetCharacterOptions2Flag());

            /*if ((optionFlags & DescriptionOptionFlag.Unk100) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.WindowLayout) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.Unk400) != 0)
            {
            }*/

            Writer.Write(0u);
            Writer.Write(0u);
        }
    }
}
