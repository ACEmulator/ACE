using ACE.Entity;
using System;

namespace ACE.Network.GameEvent
{
    public class GameEventPlayerDescription : GameEventMessage
    {
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

        public override GameEventOpcode EventType { get { return GameEventOpcode.PlayerDescription; } }

        public GameEventPlayerDescription(Session session)
            : base(session)
        {
        }

        protected override void WriteEventBody()
        {
            var propertyFlags    = DescriptionPropertyFlag.None;
            var propertyFlagsPos = writer.BaseStream.Position;
            writer.Write(0u);
            writer.Write(0x0Au);

            var propertiesInt = session.Player.PropertiesInt;
            if (propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                writer.Write((ushort)propertiesInt.Count);
                writer.Write((ushort)0x40);

                foreach (var uintProperty in propertiesInt)
                {
                    writer.Write((uint)uintProperty.Key);
                    writer.Write(uintProperty.Value);
                }
            }

            var propertiesInt64 = session.Player.PropertiesInt64;
            if (propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                writer.Write((ushort)propertiesInt64.Count);
                writer.Write((ushort)0x40);

                foreach (var uint64Property in propertiesInt64)
                {
                    writer.Write((uint)uint64Property.Key);
                    writer.Write(uint64Property.Value);
                }
            }

            var propertiesBool = session.Player.PropertiesBool;
            if (propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                writer.Write((ushort)propertiesBool.Count);
                writer.Write((ushort)0x20);

                foreach (var boolProperty in propertiesBool)
                {
                    writer.Write((uint)boolProperty.Key);
                    writer.Write(Convert.ToUInt32(boolProperty.Value)); // just as fast as inlining
                }
            }

            var propertiesDouble = session.Player.PropertiesDouble;
            if (propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                writer.Write((ushort)propertiesDouble.Count);
                writer.Write((ushort)0x20);

                foreach (var doubleProperty in propertiesDouble)
                {
                    writer.Write((uint)doubleProperty.Key);
                    writer.Write(doubleProperty.Value);
                }
            }

            var propertiesString = session.Player.PropertiesString;
            if (propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                writer.Write((ushort)propertiesString.Count);
                writer.Write((ushort)0x10);

                foreach (var stringProperty in propertiesString)
                {
                    writer.Write((uint)stringProperty.Key);
                    writer.WriteString16L(stringProperty.Value);
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

            writer.WritePosition((uint)propertyFlags, propertyFlagsPos);

            var vectorFlags = DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;
            writer.Write((uint)vectorFlags);
            writer.Write(1u);
            
            if ((vectorFlags & DescriptionVectorFlag.Attribute) != 0)
            {
                var attributeFlags = DescriptionAttributeFlag.Full;
                writer.Write((uint)attributeFlags);

                if ((attributeFlags & DescriptionAttributeFlag.Strength) != 0)
                {
                    writer.Write(this.session.Player.Strength.Ranks); // ranks
                    writer.Write(this.session.Player.Strength.UnbuffedValue);
                    writer.Write(this.session.Player.Strength.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Endurance) != 0)
                {
                    writer.Write(this.session.Player.Endurance.Ranks); // ranks
                    writer.Write(this.session.Player.Endurance.UnbuffedValue);
                    writer.Write(this.session.Player.Endurance.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Quickness) != 0)
                {
                    writer.Write(this.session.Player.Quickness.Ranks); // ranks
                    writer.Write(this.session.Player.Quickness.UnbuffedValue);
                    writer.Write(this.session.Player.Quickness.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Coordination) != 0)
                {
                    writer.Write(this.session.Player.Coordination.Ranks); // ranks
                    writer.Write(this.session.Player.Coordination.UnbuffedValue);
                    writer.Write(this.session.Player.Coordination.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Focus) != 0)
                {
                    writer.Write(this.session.Player.Focus.Ranks); // ranks
                    writer.Write(this.session.Player.Focus.UnbuffedValue);
                    writer.Write(this.session.Player.Focus.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Self) != 0)
                {
                    writer.Write(this.session.Player.Self.Ranks); // ranks
                    writer.Write(this.session.Player.Self.UnbuffedValue);
                    writer.Write(this.session.Player.Self.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Health) != 0)
                {
                    writer.Write(this.session.Player.Health.Ranks); // ranks
                    writer.Write(0u); // unknown
                    writer.Write(this.session.Player.Health.ExperienceSpent); // xp spent
                    writer.Write(this.session.Player.Health.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Stamina) != 0)
                {
                    writer.Write(this.session.Player.Stamina.Ranks); // ranks
                    writer.Write(0u); // unknown
                    writer.Write(this.session.Player.Stamina.ExperienceSpent); // xp spent
                    writer.Write(this.session.Player.Stamina.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Mana) != 0)
                {
                    writer.Write(this.session.Player.Mana.Ranks); // ranks
                    writer.Write(0u); // unknown
                    writer.Write(this.session.Player.Mana.ExperienceSpent); // xp spent
                    writer.Write(this.session.Player.Mana.Current); // current value
                }
            }

            // TODO: need DB support for this
            if ((vectorFlags & DescriptionVectorFlag.Skill) != 0)
            {
                var skills = this.session.Player.Skills;

                writer.Write((ushort)skills.Count);
                writer.Write((ushort)0x20); // unknown

                foreach (var skill in skills)
                {
                    writer.Write((uint)skill.Key); // skill id
                    writer.Write((ushort)skill.Value.Ranks); // points raised
                    writer.Write((ushort)0);
                    writer.Write((uint)skill.Value.Status); // skill state
                    writer.Write((uint)skill.Value.ExperienceSpent); // xp spent on this skill
                    writer.Write(0u); // current bonus points applied (buffs) - not implemented
                    writer.Write(0u); // task difficulty
                    writer.Write(0d);
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

            var optionFlags = DescriptionOptionFlag.None;
            writer.Write((uint)optionFlags);
            writer.Write(0x11C4E56A);

            /*if ((optionFlags & DescriptionOptionFlag.Shortcut) != 0)
            {
            }*/

            writer.Write(0u);

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
                writer.Write(0u);

            if ((optionFlags & DescriptionOptionFlag.CharacterOption2) != 0)
                writer.Write(0u);

            /*if ((optionFlags & DescriptionOptionFlag.Unk100) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.WindowLayout) != 0)
            {
            }*/

            /*if ((optionFlags & DescriptionOptionFlag.Unk400) != 0)
            {
            }*/

            writer.Write(0u);
            writer.Write(0u);
        }
    }
}
