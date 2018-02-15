using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventPlayerDescription : GameEventMessage
    {
        [Flags]
        private enum DescriptionPropertyFlag
        {
            None            = 0x0000,
            PropertyInt32   = 0x0001,
            PropertyBool    = 0x0002,
            PropertyDouble  = 0x0004,
            PropertyDid     = 0x0008,
            PropertyString  = 0x0010,
            Position        = 0x0020,
            PropertyIid     = 0x0040,
            PropertyInt64   = 0x0080,
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
            : base(GameEventType.PlayerDescription, GameMessageGroup.UIQueue, session)
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

            var propertiesInt = Session.Player.GetAllPropertyInt().Where(x => ClientProperties.PropertiesInt.Contains((ushort)x.Key)).ToList();

            if (propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                Writer.Write((ushort)propertiesInt.Count);
                Writer.Write((ushort)0x40);

                foreach (var property in propertiesInt)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var propertiesInt64 = Session.Player.GetAllPropertyInt64().Where(x => ClientProperties.PropertiesInt64.Contains((ushort)x.Key)).ToList();

            if (propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                Writer.Write((ushort)propertiesInt64.Count);
                Writer.Write((ushort)0x40);

                foreach (var property in propertiesInt64)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var propertiesBool = Session.Player.GetAllPropertyBools().Where(x => ClientProperties.PropertiesBool.Contains((ushort)x.Key)).ToList();

            if (propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                Writer.Write((ushort)propertiesBool.Count);
                Writer.Write((ushort)0x20);

                foreach (var property in propertiesBool)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(Convert.ToUInt32(property.Value)); // just as fast as inlining
                }
            }

            var propertiesDouble = Session.Player.GetAllPropertyDouble().Where(x => ClientProperties.PropertiesDouble.Contains((ushort)x.Key)).ToList();

            if (propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                Writer.Write((ushort)propertiesDouble.Count);
                Writer.Write((ushort)0x20);

                foreach (var property in propertiesDouble)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var propertiesString = Session.Player.GetAllPropertyString().Where(x => ClientProperties.PropertiesString.Contains((ushort)x.Key)).ToList();

            if (propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                Writer.Write((ushort)propertiesString.Count);
                Writer.Write((ushort)0x10);

                foreach (var property in propertiesString)
                {
                    Writer.Write((uint)property.Key);
                    Writer.WriteString16L(property.Value);
                }
            }

            var propertiesDid = Session.Player.GetAllPropertyDataId().Where(x => ClientProperties.PropertiesDataId.Contains((ushort)x.Key)).ToList();

            if (propertiesDid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDid;

                Writer.Write((ushort)propertiesDid.Count);
                Writer.Write((ushort)0x20);

                foreach (var property in propertiesDid)
                {
                    Writer.Write((ushort)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var propertiesIid = Session.Player.GetAllPropertyInstanceId().Where(x => ClientProperties.PropertiesInstanceId.Contains((ushort)x.Key)).ToList();

            if (propertiesIid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyIid;

                Writer.Write((ushort)propertiesIid.Count);
                Writer.Write((ushort)0x20);

                foreach (var property in propertiesIid)
                {
                    Writer.Write((ushort)property.Key);
                    Writer.Write(property.Value);
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
var aceObj = Session.Player.GetAceObject() as AceCharacter;
            // todo fix to not use aceobj
            DescriptionVectorFlag vectorFlags = 0;//DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;

            /* todo fix for player, not use aceobj
            var propertiesSpells = aceObj.SpellIdProperties.ToList();
            if (propertiesSpells.Count > 0)
                vectorFlags |= DescriptionVectorFlag.Spell;*/

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
            {/* todo fix for not aceobj
                Writer.Write((ushort)propertiesSpells.Count);
                Writer.Write((ushort)64);

                {
                    foreach (AceObjectPropertiesSpell spell in propertiesSpells)
                    {
                        Writer.Write(spell.SpellId);
                        // This sets a flag to use new spell configuration always 2
                        Writer.Write(2f);
                    }
                }*/
            }

            /*if ((vectorFlags & DescriptionVectorFlag.Enchantment) != 0)
            {
            }*/

            // FIXME(ddevec): We have duplicated data everywhere.  There is an AceObject CharacterOption flag, and a Player.CharacterOptions...
            //    Which one is right?  I have no idea.  Right now the aceObject works...  we should probably do a refactoring once we've restored functionality

            // TODO: Refactor this to set all of these flags based on data. Og II
            CharacterOptionDataFlag optionFlags = CharacterOptionDataFlag.CharacterOptions2;
            // todo fix to not use aceobj
            //if (Session.Player.SpellsInSpellBars.Exists(x => x.AceObjectId == aceObj.AceObjectId))
            //    optionFlags |= CharacterOptionDataFlag.SpellLists8;

            Writer.Write((uint)optionFlags);
            /*
            Writer.Write(this.Session.Player.CharacterOptions.GetCharacterOptions1Flag());
            */
            Writer.Write(0 /* todo fix for not use aceobj aceObj.CharacterOptions1Mapping*/);

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
                Writer.Write(0 /* todo fix for not ueaceobj aceObj.CharacterOptions2Mapping*/);

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
            Writer.Write(0 /* todo fix for not useaceobj (uint)aceObj.Inventory.Count*/);
            // write out all of the non-containers and foci
            /*foreach (var inv in aceObj.Inventory.Where(i => !i.Value.UseBackpackSlot))
            {
                Writer.Write(inv.Value.AceObjectId);
                Writer.Write((uint)ContainerType.NonContainer);
            }
            // Containers and foci go in side slots, they come last with their own placement order.
            foreach (var inv in aceObj.Inventory.Where(i => i.Value.UseBackpackSlot))
            {
                Writer.Write(inv.Value.AceObjectId);
                if ((WeenieType)inv.Value.WeenieType == WeenieType.Container)
                    Writer.Write((uint)ContainerType.Container);
                else
                    Writer.Write((uint)ContainerType.Foci);
            }*/

            // TODO: This is where what we have equipped is sent.
            Writer.Write(0 /* todo fix for not use aceobj (uint)aceObj.WieldedItems.Count*/);
            /*foreach (var wieldedItem in aceObj.WieldedItems)
            {
                Writer.Write(wieldedItem.Value.AceObjectId);
                Writer.Write((uint)wieldedItem.Value.CurrentWieldedLocation);
                Writer.Write(wieldedItem.Value.ClothingPriority ?? 0);
            }*/
        }
    }
}
