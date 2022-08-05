using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;

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

        private static readonly PropertyIntComparer PropertyIntComparer = new PropertyIntComparer(64);
        private static readonly PropertyInt64Comparer PropertyInt64Comparer = new PropertyInt64Comparer(64);
        private static readonly PropertyBoolComparer PropertyBoolComparer = new PropertyBoolComparer(32);
        private static readonly PropertyFloatComparer PropertyDoubleComparer = new PropertyFloatComparer(32);
        private static readonly PropertyStringComparer PropertyStringComparer = new PropertyStringComparer(32);  // 16 in client, 32 sent across wire
        private static readonly PropertyDataIdComparer PropertyDataIdComparer = new PropertyDataIdComparer(32);
        private static readonly PropertyInstanceIdComparer PropertyInstanceIdComparer = new PropertyInstanceIdComparer(32);
        private static readonly SkillComparer SkillComparer = new SkillComparer(32);
        private static readonly SpellComparer SpellComparer = new SpellComparer(64);

        private void WriteEventBody()
        {
            // refactor this -- it is kind of ridiculous for this to all be in 1 giant function.
            // this is a combination of CACQualities, BaseQualities, PlayerModule, and other structures

            var propertyFlags = DescriptionPropertyFlag.None;
            var propertyFlagsPos = Writer.BaseStream.Position;

            Writer.Write(0u);
            Writer.Write((uint)Session.Player.WeenieType);

            var _propertiesInt = Session.Player.GetAllPropertyInt().Where(x => SendOnLoginProperties.PropertiesInt.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesInt.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt32;

                PackableHashTable.WriteHeader(Writer, _propertiesInt.Count, PropertyIntComparer.NumBuckets);

                var propertiesInt = new SortedDictionary<PropertyInt, int>(_propertiesInt, PropertyIntComparer);

                foreach (var property in propertiesInt)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var _propertiesInt64 = Session.Player.GetAllPropertyInt64().Where(x => ClientProperties.PropertiesInt64.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesInt64.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyInt64;

                PackableHashTable.WriteHeader(Writer, _propertiesInt64.Count, PropertyInt64Comparer.NumBuckets);

                var propertiesInt64 = new SortedDictionary<PropertyInt64, long>(_propertiesInt64, PropertyInt64Comparer);

                foreach (var property in propertiesInt64)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var _propertiesBool = Session.Player.GetAllPropertyBools().Where(x => SendOnLoginProperties.PropertiesBool.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesBool.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyBool;

                PackableHashTable.WriteHeader(Writer, _propertiesBool.Count, PropertyBoolComparer.NumBuckets);

                var propertiesBool = new SortedDictionary<PropertyBool, bool>(_propertiesBool, PropertyBoolComparer);

                foreach (var property in propertiesBool)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(Convert.ToUInt32(property.Value)); // just as fast as inlining
                }
            }

            var _propertiesDouble = Session.Player.GetAllPropertyFloat().Where(x => SendOnLoginProperties.PropertiesDouble.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesDouble.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDouble;

                PackableHashTable.WriteHeader(Writer, _propertiesDouble.Count, PropertyDoubleComparer.NumBuckets);

                var propertiesDouble = new SortedDictionary<PropertyFloat, double>(_propertiesDouble, PropertyDoubleComparer);

                foreach (var property in propertiesDouble)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var _propertiesString = Session.Player.GetAllPropertyString().Where(x => SendOnLoginProperties.PropertiesString.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesString.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyString;

                PackableHashTable.WriteHeader(Writer, _propertiesString.Count, PropertyStringComparer.NumBuckets);

                var propertiesString = new SortedDictionary<PropertyString, string>(_propertiesString, PropertyStringComparer);

                foreach (var property in propertiesString)
                {
                    Writer.Write((uint)property.Key);
                    if (property.Key == PropertyString.Name)
                    {
                        if (Session.Player.IsPlussed && Session.Player.CloakStatus < CloakStatus.Player)
                            Writer.WriteString16L("+" + property.Value);
                        else
                            Writer.WriteString16L(property.Value);
                    }
                    else
                        Writer.WriteString16L(property.Value);
                }
            }

            var _propertiesDid = Session.Player.GetAllPropertyDataId().Where(x => SendOnLoginProperties.PropertiesDataId.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesDid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyDid;

                PackableHashTable.WriteHeader(Writer, _propertiesDid.Count, PropertyDataIdComparer.NumBuckets);

                var propertiesDid = new SortedDictionary<PropertyDataId, uint>(_propertiesDid, PropertyDataIdComparer);

                foreach (var property in propertiesDid)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            var _propertiesIid = Session.Player.GetAllPropertyInstanceId().Where(x => SendOnLoginProperties.PropertiesInstanceId.Contains((ushort)x.Key)).ToDictionary(i => i.Key, i => i.Value);

            if (_propertiesIid.Count != 0)
            {
                propertyFlags |= DescriptionPropertyFlag.PropertyIid;

                PackableHashTable.WriteHeader(Writer, _propertiesIid.Count, PropertyInstanceIdComparer.NumBuckets);

                var propertiesIid = new SortedDictionary<PropertyInstanceId, uint>(_propertiesIid, PropertyInstanceIdComparer);

                foreach (var property in propertiesIid)
                {
                    Writer.Write((uint)property.Key);
                    Writer.Write(property.Value);
                }
            }

            /*if ((propertyFlags & DescriptionPropertyFlag.Resource) != 0)
            {
            }*/

            /*if ((propertyFlags & DescriptionPropertyFlag.Link) != 0)
            {
            }*/

            var lastOutsideDeath = Session.Player.GetPosition(PositionType.LastOutsideDeath);

            if (lastOutsideDeath != null)
            {
                propertyFlags |= DescriptionPropertyFlag.Position;

                PackableHashTable.WriteHeader(Writer, 1, 16);

                Writer.Write((uint)PositionType.LastOutsideDeath);
                lastOutsideDeath.Serialize(Writer);
            }

            Writer.WritePosition((uint)propertyFlags, propertyFlagsPos);

            DescriptionVectorFlag vectorFlags = DescriptionVectorFlag.Attribute | DescriptionVectorFlag.Skill;

            var knownSpells = Session.Player.Biota.CloneSpells(Session.Player.BiotaDatabaseLock);

            if (knownSpells.Count > 0)
                vectorFlags |= DescriptionVectorFlag.Spell;

            if (Session.Player.EnchantmentManager.HasEnchantments)
                vectorFlags |= DescriptionVectorFlag.Enchantment;

            Writer.Write((uint)vectorFlags);

            Writer.Write(Convert.ToUInt32(Session.Player.Health != null));

            if ((vectorFlags & DescriptionVectorFlag.Attribute) != 0)
            {
                var attributeFlags = AttributeCache.Full;
                Writer.Write((uint)attributeFlags);

                if ((attributeFlags & AttributeCache.Strength) != 0)
                {
                    Writer.Write(Session.Player.Strength.Ranks);
                    Writer.Write(Session.Player.Strength.StartingValue);
                    Writer.Write(Session.Player.Strength.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Endurance) != 0)
                {
                    Writer.Write(Session.Player.Endurance.Ranks);
                    Writer.Write(Session.Player.Endurance.StartingValue);
                    Writer.Write(Session.Player.Endurance.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Quickness) != 0)
                {
                    Writer.Write(Session.Player.Quickness.Ranks);
                    Writer.Write(Session.Player.Quickness.StartingValue);
                    Writer.Write(Session.Player.Quickness.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Coordination) != 0)
                {
                    Writer.Write(Session.Player.Coordination.Ranks);
                    Writer.Write(Session.Player.Coordination.StartingValue);
                    Writer.Write(Session.Player.Coordination.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Focus) != 0)
                {
                    Writer.Write(Session.Player.Focus.Ranks);
                    Writer.Write(Session.Player.Focus.StartingValue);
                    Writer.Write(Session.Player.Focus.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Self) != 0)
                {
                    Writer.Write(Session.Player.Self.Ranks);
                    Writer.Write(Session.Player.Self.StartingValue);
                    Writer.Write(Session.Player.Self.ExperienceSpent);
                }

                if ((attributeFlags & AttributeCache.Health) != 0)
                {
                    Writer.Write(Session.Player.Health.Ranks);
                    Writer.Write(Session.Player.Health.StartingValue); // init_level - always appears to be 0
                    Writer.Write(Session.Player.Health.ExperienceSpent);
                    Writer.Write(Session.Player.Health.Current);
                }

                if ((attributeFlags & AttributeCache.Stamina) != 0)
                {
                    Writer.Write(Session.Player.Stamina.Ranks);
                    Writer.Write(Session.Player.Stamina.StartingValue); // init_level - always appears to be 0
                    Writer.Write(Session.Player.Stamina.ExperienceSpent);
                    Writer.Write(Session.Player.Stamina.Current);
                }

                if ((attributeFlags & AttributeCache.Mana) != 0)
                {
                    Writer.Write(Session.Player.Mana.Ranks);
                    Writer.Write(Session.Player.Mana.StartingValue); // init_level - always appears to be 0
                    Writer.Write(Session.Player.Mana.ExperienceSpent);
                    Writer.Write(Session.Player.Mana.Current);
                }
            }

            if ((vectorFlags & DescriptionVectorFlag.Skill) != 0)
            {
                PackableHashTable.WriteHeader(Writer, Session.Player.Skills.Count, SkillComparer.NumBuckets);

                var skills = new SortedDictionary<Skill, CreatureSkill>(Session.Player.Skills, SkillComparer);

                foreach (var kvp in skills)
                {
                    // TODO: Network.Structure.Skill

                    Writer.Write((uint)kvp.Key); // skill id
                    Writer.Write(kvp.Value.Ranks); // points raised
                    Writer.Write((ushort)1u);
                    Writer.Write((uint)kvp.Value.AdvancementClass); // skill state
                    Writer.Write(kvp.Value.ExperienceSpent); // xp spent on this skill
                    Writer.Write(kvp.Value.InitLevel);  // init_level, for training/specialized bonus from character creation
                    Writer.Write(0u); // task difficulty, aka "resistance_of_last_check"
                    Writer.Write(0d); // last_time_used
                }
            }

            if ((vectorFlags & DescriptionVectorFlag.Spell) != 0)
            {
                PackableHashTable.WriteHeader(Writer, knownSpells.Count, SpellComparer.NumBuckets);

                var spells = new SortedDictionary<int, float>(knownSpells, SpellComparer);

                foreach (var spell in spells)
                {
                    Writer.Write(spell.Key);
                    // This sets a flag to use new spell configuration always 2
                    Writer.Write(2f);
                }
            }

            if ((vectorFlags & DescriptionVectorFlag.Enchantment) != 0)
                Session.Player.EnchantmentManager.SendRegistry(Writer);

            // TODO: Refactor this to set all of these flags based on data. Og II
            var optionFlags = CharacterOptionDataFlag.CharacterOptions2;

            optionFlags |= CharacterOptionDataFlag.SpellLists8;

            var shortcuts = Session.Player.GetShortcuts();
            if (shortcuts.Count > 0)
                optionFlags |= CharacterOptionDataFlag.Shortcut;

            if (Session.Player.Character.GameplayOptions != null && Session.Player.Character.GameplayOptions.Length > 0)
                optionFlags |= CharacterOptionDataFlag.GameplayOptions;

            var fillComps = Session.Player.Character.GetFillComponents(Session.Player.CharacterDatabaseLock);
            if (fillComps.Count > 0)
                optionFlags |= CharacterOptionDataFlag.DesiredComps;

            optionFlags |= CharacterOptionDataFlag.SpellbookFilters;

            Writer.Write((uint)optionFlags);
            Writer.Write(Session.Player.Character.CharacterOptions1);

            if (shortcuts.Count > 0)
                Writer.Write(shortcuts);

            if ((optionFlags & CharacterOptionDataFlag.SpellLists8) != 0)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var spells = Session.Player.GetSpellsInSpellBar(i);

                    Writer.Write(spells.Count);
                    foreach (var spell in spells)
                        Writer.Write(spell.SpellId);
                }
            }
            else
            {
                Writer.Write(0u);
            }

            if ((optionFlags & CharacterOptionDataFlag.DesiredComps) != 0)
                Writer.WriteOld(fillComps);     // verify

            //if ((optionFlags & CharacterOptionDataFlag.SpellbookFilters) != 0)
            Writer.Write(Session.Player.Character.SpellbookFilters);

            if ((optionFlags & CharacterOptionDataFlag.CharacterOptions2) != 0)
                Writer.Write(Session.Player.Character.CharacterOptions2);

            /*if ((optionFlags & DescriptionOptionFlag.Unk100) != 0)
            {
            }*/

            if ((optionFlags & CharacterOptionDataFlag.GameplayOptions) != 0)
                Writer.Write(Session.Player.Character.GameplayOptions);

            /*if ((optionFlags & DescriptionOptionFlag.Unk400) != 0)
            {
            }*/

            // Write total count.
            Writer.Write((uint)Session.Player.Inventory.Count);
            // write out all of the non-containers and foci
            foreach (var item in Session.Player.Inventory.Values.Where(i => !i.UseBackpackSlot).OrderBy(i => i.PlacementPosition))
            {
                Writer.Write(item.Guid.Full);
                Writer.Write((uint)ContainerType.NonContainer);
            }
            // Containers and foci go in side slots, they come last with their own placement order.
            foreach (var item in Session.Player.Inventory.Values.Where(i => i.UseBackpackSlot).OrderBy(i => i.PlacementPosition))
            {
                Writer.Write(item.Guid.Full);
                if (item.WeenieType == WeenieType.Container)
                    Writer.Write((uint)ContainerType.Container);
                else
                    Writer.Write((uint)ContainerType.Foci);
            }

            Writer.Write((uint)Session.Player.EquippedObjects.Values.Count);
            foreach (var item in Session.Player.EquippedObjects.Values)
            {
                Writer.Write(item.Guid.Full);
                Writer.Write((uint)(item.CurrentWieldedLocation ?? 0));
                Writer.Write((uint)(item.ClothingPriority ?? 0));
            }
        }
    }
}
