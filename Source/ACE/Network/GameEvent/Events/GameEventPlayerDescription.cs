using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPlayerDescription : GameEventMessage
    {
        [Flags]
        private enum DescriptionPropertyFlag
        {
            None           = 0x0000,
            PropertyInt32  = 0x0001,
            PropertyBool   = 0x0002,
            PropertyDouble = 0x0004,
            PropertyDid    = 0x0008,
            PropertyString = 0x0010,
            Position       = 0x0020,
            PropertyIid    = 0x0040,
            PropertyInt64  = 0x0080,
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

        /// <summary>
        /// TODO: convert this to serialize AceCharacter rather than do all this session.Player garbage
        /// </summary>
        private void WriteEventBody()
        {
            var propertyFlags = DescriptionPropertyFlag.None;
            var propertyFlagsPos = Writer.BaseStream.Position;
            Writer.Write(0u);
            Writer.Write(0x0Au);

            var aceObj = Session.Player.GetAceObject() as AceCharacter;

            // < 9000 to filter out our custom properties
            var propertiesInt = aceObj.IntProperties.Where(x => x.PropertyId < 9000).ToList();
            if (propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                Writer.Write((ushort)propertiesInt.Count);
                Writer.Write((ushort)0x40);

                var notNull = propertiesInt.Where(x => x != null);
                foreach (var uintProperty in notNull)
                {
                    Writer.Write((uint)uintProperty.PropertyId);
                    Writer.Write(uintProperty.PropertyValue.Value);
                }
            }

            var propertiesInt64 = aceObj.Int64Properties.Where(x => x.PropertyId < 9000).ToList();
            if (propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                Writer.Write((ushort)propertiesInt64.Count);
                Writer.Write((ushort)0x40);

                var notNull = propertiesInt64.Where(x => x != null);
                foreach (var uint64Property in notNull)
                {
                    Writer.Write((uint)uint64Property.PropertyId);
                    Writer.Write(uint64Property.PropertyValue.Value);
                }
            }

            var propertiesBool = aceObj.BoolProperties.Where(x => x.PropertyId < 9000).ToList();
            if (propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                Writer.Write((ushort)propertiesBool.Count);
                Writer.Write((ushort)0x20);

                foreach (var boolProperty in propertiesBool)
                {
                    Writer.Write((uint)boolProperty.PropertyId);
                    Writer.Write(Convert.ToUInt32(boolProperty.PropertyValue)); // just as fast as inlining
                }
            }

            var propertiesDouble = aceObj.DoubleProperties.Where(x => x.PropertyId < 9000).ToList();
            if (propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                Writer.Write((ushort)propertiesDouble.Count);
                Writer.Write((ushort)0x20);

                var notNull = propertiesDouble.Where(x => x != null);
                foreach (var doubleProperty in notNull)
                {
                    Writer.Write((uint)doubleProperty.PropertyId);
                    Writer.Write(doubleProperty.PropertyValue.Value);
                }
            }

            var propertiesString = aceObj.StringProperties.Where(x => x.PropertyId < 9000).ToList();
            if (propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                Writer.Write((ushort)propertiesString.Count);
                Writer.Write((ushort)0x10);

                foreach (var stringProperty in propertiesString)
                {
                    Writer.Write((uint)stringProperty.PropertyId);
                    Writer.WriteString16L(stringProperty.PropertyValue);
                }
            }

            var propertiesDid = aceObj.DataIdProperties.Where(x => x.PropertyId < 9000 && x.PropertyValue != null).ToList();
            if (propertiesDid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDid;

                Writer.Write((ushort)propertiesDid.Count);
                Writer.Write((ushort)0x20);

                foreach (var didProperty in propertiesDid)
                {
                    Writer.Write(didProperty.PropertyId);
                    Writer.Write(didProperty.PropertyValue.Value);
                }
            }

            var propertiesIid = aceObj.InstanceIdProperties.Where(x => x.PropertyId < 9000 && x.PropertyValue != null).ToList();
            if (propertiesIid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyIid;

                Writer.Write((ushort)propertiesIid.Count);
                Writer.Write((ushort)0x20);

                foreach (var iidProperty in propertiesIid)
                {
                    Writer.Write(iidProperty.PropertyId);
                    Writer.Write(iidProperty.PropertyValue.Value);
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
                    Writer.Write(this.Session.Player.Strength.Ranks); // ranks
                    Writer.Write(this.Session.Player.Strength.Base);
                    Writer.Write(this.Session.Player.Strength.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Endurance) != 0)
                {
                    Writer.Write(this.Session.Player.Endurance.Ranks); // ranks
                    Writer.Write(this.Session.Player.Endurance.Base);
                    Writer.Write(this.Session.Player.Endurance.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Quickness) != 0)
                {
                    Writer.Write(this.Session.Player.Quickness.Ranks); // ranks
                    Writer.Write(this.Session.Player.Quickness.Base);
                    Writer.Write(this.Session.Player.Quickness.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Coordination) != 0)
                {
                    Writer.Write(this.Session.Player.Coordination.Ranks); // ranks
                    Writer.Write(this.Session.Player.Coordination.Base);
                    Writer.Write(this.Session.Player.Coordination.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Focus) != 0)
                {
                    Writer.Write(this.Session.Player.Focus.Ranks); // ranks
                    Writer.Write(this.Session.Player.Focus.Base);
                    Writer.Write(this.Session.Player.Focus.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Self) != 0)
                {
                    Writer.Write(this.Session.Player.Self.Ranks); // ranks
                    Writer.Write(this.Session.Player.Self.Base);
                    Writer.Write(this.Session.Player.Self.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & DescriptionAttributeFlag.Health) != 0)
                {
                    Writer.Write(this.Session.Player.Health.Ranks); // ranks
                    Writer.Write(0u); // init_level - always appears to be 0
                    Writer.Write(this.Session.Player.Health.ExperienceSpent); // xp spent
                    Writer.Write(this.Session.Player.Health.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Stamina) != 0)
                {
                    Writer.Write(this.Session.Player.Stamina.Ranks); // ranks
                    Writer.Write(0u); // init_level - always appears to be 0
                    Writer.Write(this.Session.Player.Stamina.ExperienceSpent); // xp spent
                    Writer.Write(this.Session.Player.Stamina.Current); // current value
                }

                if ((attributeFlags & DescriptionAttributeFlag.Mana) != 0)
                {
                    Writer.Write(this.Session.Player.Mana.Ranks); // ranks
                    Writer.Write(0u); // init_level - always appears to be 0
                    Writer.Write(this.Session.Player.Mana.ExperienceSpent); // xp spent
                    Writer.Write(this.Session.Player.Mana.Current); // current value
                }
            }

            if ((vectorFlags & DescriptionVectorFlag.Skill) != 0)
            {
                // FIXME(ddevec): This should be a property of the player -- the AceObject does not track buffs.
                var skills = aceObj.GetSkills();

                Writer.Write((ushort)skills.Count);
                Writer.Write((ushort)0x20); // unknown

                foreach (var skill in skills)
                {
                    Writer.Write((uint)skill.Skill); // skill id
                    Writer.Write((ushort)skill.Ranks); // points raised
                    Writer.Write((ushort)1u);
                    Writer.Write((uint)skill.Status); // skill state
                    Writer.Write((uint)skill.ExperienceSpent); // xp spent on this skill
                    if (skill.Status == SkillStatus.Specialized)
                        Writer.Write(10u);  // init_level, for specialization bonus -- buffs are not part of this message
                    else
                        Writer.Write(0u); // no init_level
                    Writer.Write(0u); // task difficulty, aka "resistance_of_last_check"
                    Writer.Write(0d); // last_time_used
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

            // FIXME(ddevec): We have duplicated data everywhere.  There is an AceObject CharacterOption flag, and a Player.CharacterOptions...
            //    Which one is right?  I have no idea.  Right now the aceObject works...  we should probably do a refactoring once we've restored functionality
            var optionFlags = DescriptionOptionFlag.CharacterOption2;
            Writer.Write((uint)optionFlags);
            /*
            Writer.Write(this.Session.Player.CharacterOptions.GetCharacterOptions1Flag());
            */
            Writer.Write(aceObj.CharacterOptions1Mapping);

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
                // Writer.Write(this.Session.Player.CharacterOptions.GetCharacterOptions2Flag());
                Writer.Write(aceObj.CharacterOptions2Mapping);

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
