using System;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;
using System.Diagnostics;

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
            None        = 0x0000,
            Attribute   = 0x0001,
            Skill       = 0x0002,
            Spell       = 0x0100,
            Enchantment = 0x0200
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
                    Debug.Assert(uintProperty.PropertyValue != null, "uintProperty.PropertyValue != null");
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
                    Debug.Assert(uint64Property.PropertyValue != null, "uint64Property.PropertyValue != null");
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
                    Debug.Assert(doubleProperty.PropertyValue != null, "doubleProperty.PropertyValue != null");
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
                    Debug.Assert(didProperty.PropertyValue != null, "didProperty.PropertyValue != null");
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
                    Debug.Assert(iidProperty.PropertyValue != null, "iidProperty.PropertyValue != null");
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

            DescriptionVectorFlag vectorFlags = DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;

            var propertiesSpells = aceObj.SpellIdProperties.ToList();
            if (propertiesSpells.Count > 0)
                vectorFlags |= DescriptionVectorFlag.Spell;

            Writer.Write((uint)vectorFlags);
            Writer.Write(1u);

            if ((vectorFlags & DescriptionVectorFlag.Attribute) != 0)
            {
                var attributeFlags = Ability.Full;
                Writer.Write((uint)attributeFlags);

                if ((attributeFlags & Ability.Strength) != 0)
                {
                    Writer.Write(this.Session.Player.Strength.Ranks); // ranks
                    Writer.Write(this.Session.Player.Strength.Base);
                    Writer.Write(this.Session.Player.Strength.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Endurance) != 0)
                {
                    Writer.Write(this.Session.Player.Endurance.Ranks); // ranks
                    Writer.Write(this.Session.Player.Endurance.Base);
                    Writer.Write(this.Session.Player.Endurance.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Quickness) != 0)
                {
                    Writer.Write(this.Session.Player.Quickness.Ranks); // ranks
                    Writer.Write(this.Session.Player.Quickness.Base);
                    Writer.Write(this.Session.Player.Quickness.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Coordination) != 0)
                {
                    Writer.Write(this.Session.Player.Coordination.Ranks); // ranks
                    Writer.Write(this.Session.Player.Coordination.Base);
                    Writer.Write(this.Session.Player.Coordination.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Focus) != 0)
                {
                    Writer.Write(this.Session.Player.Focus.Ranks); // ranks
                    Writer.Write(this.Session.Player.Focus.Base);
                    Writer.Write(this.Session.Player.Focus.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Self) != 0)
                {
                    Writer.Write(this.Session.Player.Self.Ranks); // ranks
                    Writer.Write(this.Session.Player.Self.Base);
                    Writer.Write(this.Session.Player.Self.ExperienceSpent); // xp spent
                }

                if ((attributeFlags & Ability.Health) != 0)
                {
                    Writer.Write(this.Session.Player.Health.Ranks); // ranks
                    Writer.Write(0u); // init_level - always appears to be 0
                    Writer.Write(this.Session.Player.Health.ExperienceSpent); // xp spent
                    Writer.Write(this.Session.Player.Health.Current); // current value
                }

                if ((attributeFlags & Ability.Stamina) != 0)
                {
                    Writer.Write(this.Session.Player.Stamina.Ranks); // ranks
                    Writer.Write(0u); // init_level - always appears to be 0
                    Writer.Write(this.Session.Player.Stamina.ExperienceSpent); // xp spent
                    Writer.Write(this.Session.Player.Stamina.Current); // current value
                }

                if ((attributeFlags & Ability.Mana) != 0)
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

            if ((vectorFlags & DescriptionVectorFlag.Spell) != 0)
            {
                Writer.Write((ushort)propertiesSpells.Count);
                Writer.Write((ushort)64);

                {
                    foreach (AceObjectPropertiesSpell spell in propertiesSpells)
                    {
                        Writer.Write(spell.SpellId);
                        // This sets a flag to use new spell configuration always 2
                        Writer.Write(2f);
                    }
                }
            }

            /*if ((vectorFlags & DescriptionVectorFlag.Enchantment) != 0)
            {
            }*/

            // FIXME(ddevec): We have duplicated data everywhere.  There is an AceObject CharacterOption flag, and a Player.CharacterOptions...
            //    Which one is right?  I have no idea.  Right now the aceObject works...  we should probably do a refactoring once we've restored functionality

            // TODO: Refactor this to set all of these flags based on data. Og II
            CharacterOptionDataFlag optionFlags = CharacterOptionDataFlag.CharacterOptions2;
            if (Session.Player.SpellsInSpellBars.Exists(x => x.AceObjectId == aceObj.AceObjectId))
            optionFlags |= CharacterOptionDataFlag.SpellLists8;

            Writer.Write((uint)optionFlags);
            /*
            Writer.Write(this.Session.Player.CharacterOptions.GetCharacterOptions1Flag());
            */
            Writer.Write(aceObj.CharacterOptions1Mapping);

            /*if ((optionFlags & DescriptionOptionFlag.Shortcut) != 0)
            {
            }*/
            if ((optionFlags & CharacterOptionDataFlag.SpellLists8) != 0)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var sorted =
                        Session.Player.SpellsInSpellBars.FindAll(x => x.AceObjectId == aceObj.AceObjectId && x.SpellBarId == i)
                            .OrderBy(s => s.SpellBarPositionId);
                    Writer.Write(sorted.Count());
                    foreach (AceObjectPropertiesSpellBarPositions spells in sorted)
                    {
                        Writer.Write(spells.SpellId);
                    }
                }
            }
            else
            {
                Writer.Write(0u);
            }

            /*if ((optionFlags & DescriptionOptionFlag.Component) != 0)
            {
            }*/

            if ((optionFlags & CharacterOptionDataFlag.SpellbookFilters) != 0)
                Writer.Write(0u);

            if ((optionFlags & CharacterOptionDataFlag.CharacterOptions2) != 0)
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

            // Write total count.
            Writer.Write((uint)aceObj.Inventory.Count);
            // write out all of the non-containers and foci
            foreach (var inv in aceObj.Inventory.Where(i => i.Value.WeenieType != (uint)WeenieType.Container && i.Value.RequiresBackpackSlot == false))
            {
                Writer.Write(inv.Value.AceObjectId);
                Writer.Write((uint)ContainerType.NonContainer);
            }
            // Containers and foci go in side slots, they come last with their own placment order.
            foreach (var inv in aceObj.Inventory.Where(i => i.Value.WeenieType == (uint)WeenieType.Container || i.Value.RequiresBackpackSlot == true))
            {
                Writer.Write(inv.Value.AceObjectId);
                if ((WeenieType)inv.Value.WeenieType == WeenieType.Container)
                    Writer.Write((uint)ContainerType.Container);
                if (inv.Value.RequiresBackpackSlot == true && (WeenieType)inv.Value.WeenieType != WeenieType.Container)
                    Writer.Write((uint)ContainerType.Foci);
            }

            // TODO: This is where what we have equipped is sent.
            // format is count
            // foreach (var wieldedItem in aceObj.WieldedItem)
            // {
            //    Writer.Write(wieldedItem.AceObjectId)
            //    Writer.Write(wieldedItem.CurrentWieldedLocation);
            //    Writer.Write(wieldedItem.ClothingPriority);
            // }
            // Replace with this code the place holder 0u that follows Og II

            Writer.Write(0u);
        }
    }
}
