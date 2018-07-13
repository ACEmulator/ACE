using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class AppraisalSpellBook
    {
        public enum _EnchantmentState : uint
        {
            Off = 0x00000000u,
            On = 0x80000000u
        }

        public ushort SpellId { get; set; }
        public _EnchantmentState EnchantmentState { get; set; }
    }
    /// <summary>
    /// Handles calculating and sending all object appraisal info
    /// </summary>
    public class AppraiseInfo
    {
        public IdentifyResponseFlags Flags;

        public bool Success;    // assessment successful?

        public Dictionary<PropertyInt, int> PropertiesInt;
        public Dictionary<PropertyInt64, long> PropertiesInt64;
        public Dictionary<PropertyBool, bool> PropertiesBool;
        public Dictionary<PropertyFloat, double> PropertiesFloat;
        public Dictionary<PropertyString, string> PropertiesString;
        public Dictionary<PropertyDataId, uint> PropertiesDID;

        public List<AppraisalSpellBook> SpellBook;

        public ArmorProfile ArmorProfile;
        public CreatureProfile CreatureProfile;
        public WeaponProfile WeaponProfile;
        public HookProfile HookProfile;

        public ArmorMask ArmorHighlight;
        public ArmorMask ArmorColor;
        public WeaponMask WeaponHighlight;
        public WeaponMask WeaponColor;
        public ResistMask ResistHighlight;
        public ResistMask ResistColor;

        public ArmorLevel ArmorLevels;

        /// <summary>
        /// Construct all of the info required for appraising any WorldObject
        /// </summary>
        public AppraiseInfo(WorldObject wo, bool success = true)
        {
            //Console.WriteLine("Appraise: " + wo.Guid);

            Success = success;
            if (!Success)
                return;

            // get wielder, if applicable
            var wielder = GetWielder(wo);

            BuildProperties(wo);
            BuildSpells(wo);

            // armor / clothing
            if (wo is Clothing)
                BuildArmor(wo);

            if (wo is Creature creature)
                BuildCreature(creature);

            if (wo is MeleeWeapon || wo is Missile || wo is MissileLauncher || wo is Caster)
                BuildWeapon(wo, wielder);

            BuildFlags();
        }

        private void BuildProperties(WorldObject wo)
        {
            PropertiesInt = wo.GetAllPropertyInt().Where(x => ClientProperties.PropertiesInt.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);
            PropertiesInt64 = wo.GetAllPropertyInt64().Where(x => ClientProperties.PropertiesInt64.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);
            PropertiesBool = wo.GetAllPropertyBools().Where(x => ClientProperties.PropertiesBool.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);
            PropertiesFloat = wo.GetAllPropertyFloat().Where(x => ClientProperties.PropertiesDouble.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);
            PropertiesString = wo.GetAllPropertyString().Where(x => ClientProperties.PropertiesString.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);
            PropertiesDID = wo.GetAllPropertyDataId().Where(x => ClientProperties.PropertiesDataId.Contains((ushort)x.Key)).ToDictionary(x => x.Key, x => x.Value);

            AddPropertyEnchantments(wo);
        }

        private void AddPropertyEnchantments(WorldObject wo)
        {
            if (wo == null) return;

            if (PropertiesInt.ContainsKey(PropertyInt.ArmorLevel))
                PropertiesInt[PropertyInt.ArmorLevel] += wo.EnchantmentManager.GetArmorMod();
        }

        private void BuildSpells(WorldObject wo)
        {
            SpellBook = new List<AppraisalSpellBook>();

            // add primary spell, if exists
            if (wo.SpellDID.HasValue)
                SpellBook.Add(new AppraisalSpellBook { SpellId = (ushort)wo.SpellDID.Value, EnchantmentState = AppraisalSpellBook._EnchantmentState.Off });

            foreach ( var biotaPropertiesSpellBook in wo.Biota.BiotaPropertiesSpellBook)
                SpellBook.Add(new AppraisalSpellBook { SpellId = (ushort)biotaPropertiesSpellBook.Spell, EnchantmentState = AppraisalSpellBook._EnchantmentState.Off });
        }

        private void AddSpells(List<AppraisalSpellBook> activeSpells, WorldObject worldObject, WorldObject wielder = null)
        {
            List<BiotaPropertiesEnchantmentRegistry> wielderEnchantments = null;
            if (worldObject == null) return;

            // get all currently active item enchantments on the item
            var woEnchantments = worldObject.EnchantmentManager.GetEnchantments(MagicSchool.ItemEnchantment);

            // get all currently active item enchantment auras on the player
            if (wielder != null)
                wielderEnchantments = wielder.EnchantmentManager.GetEnchantments(MagicSchool.ItemEnchantment);

            if (worldObject.WeenieType == WeenieType.Clothing)
            {
                // Only show Clothing type item enchantments
                foreach (var enchantment in woEnchantments)
                {
                    if ((enchantment.SpellCategory >= (ushort)SpellCategory.ArmorValueRaising) && (enchantment.SpellCategory <= (ushort)SpellCategory.AcidicResistanceLowering))
                        activeSpells.Add(new AppraisalSpellBook() { SpellId = (ushort)enchantment.SpellId, EnchantmentState = AppraisalSpellBook._EnchantmentState.On });
                }
            }
            else
            {
                // Show debuff item enchantments on weapons
                foreach (var enchantment in woEnchantments)
                {
                    if (worldObject is Caster)
                    {
                        // Caster weapon only item Auras
                        if ((enchantment.SpellCategory == (uint)SpellCategory.DefenseModLowering)
                            || (enchantment.SpellCategory == (uint)SpellCategory.AppraisalResistanceRaising)
                            || (GetSpellName((uint)enchantment.SpellId).Contains("Spirit"))) // Spirit Loather spells
                        {
                            activeSpells.Add(new AppraisalSpellBook() { SpellId = (ushort)enchantment.SpellId, EnchantmentState = AppraisalSpellBook._EnchantmentState.On });
                        }
                    }
                    else
                    {
                        // Other weapon type Auras
                        if ((enchantment.SpellCategory == (uint)SpellCategory.AttackModLowering)
                            || (enchantment.SpellCategory == (uint)SpellCategory.DamageLowering)
                            || (enchantment.SpellCategory == (uint)SpellCategory.DefenseModLowering)
                            || (enchantment.SpellCategory == (uint)SpellCategory.WeaponTimeLowering))
                        {
                            activeSpells.Add(new AppraisalSpellBook() { SpellId = (ushort)enchantment.SpellId, EnchantmentState = AppraisalSpellBook._EnchantmentState.On });
                        }
                    }
                }

                if (wielder != null)
                {
                    // Only show reflected Auras from player appropriate for wielded weapons
                    foreach (var enchantment in wielderEnchantments)
                    {
                        if (worldObject is Caster)
                        {
                            // Caster weapon only item Auras
                            if ((enchantment.SpellCategory == (uint)SpellCategory.DefenseModRaising)
                                || (enchantment.SpellCategory == (uint)SpellCategory.AppraisalResistanceLowering)
                                || (enchantment.SpellCategory == (uint)SpellCategory.SpellDamageRaising))
                            {
                                activeSpells.Add(new AppraisalSpellBook() { SpellId = (ushort)enchantment.SpellId, EnchantmentState = AppraisalSpellBook._EnchantmentState.On });
                            }
                        }
                        else
                        {
                            // Other weapon type Auras
                            if ((enchantment.SpellCategory == (uint)SpellCategory.AttackModRaising)
                                || (enchantment.SpellCategory == (uint)SpellCategory.DamageRaising)
                                || (enchantment.SpellCategory == (uint)SpellCategory.DefenseModRaising)
                                || (enchantment.SpellCategory == (uint)SpellCategory.WeaponTimeRaising))
                            {
                                activeSpells.Add(new AppraisalSpellBook() { SpellId = (ushort)enchantment.SpellId, EnchantmentState = AppraisalSpellBook._EnchantmentState.On });
                            }
                        }
                    }
                }
            }
        }

        private string GetSpellName(uint spellId)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
                return null;

            return spellTable.Spells[spellId].Name;
        }

        private void BuildArmor(WorldObject wo)
        {
            ArmorProfile = new ArmorProfile(wo);
            ArmorHighlight = ArmorMaskHelper.GetHighlightMask(wo);
            ArmorColor = ArmorMaskHelper.GetColorMask(wo);

            AddSpells(SpellBook, wo);
        }

        private void BuildCreature(Creature creature)
        {
            CreatureProfile = new CreatureProfile(creature);

            // only creatures?
            ResistHighlight = ResistMaskHelper.GetHighlightMask(creature);
            ResistColor = ResistMaskHelper.GetColorMask(creature);

            ArmorLevels = new ArmorLevel(creature);
        }

        private void BuildWeapon(WorldObject weapon, WorldObject wielder)
        {
            if ((weapon as Caster) == null)
                WeaponProfile = new WeaponProfile(weapon, wielder);

            WeaponHighlight = WeaponMaskHelper.GetHighlightMask(weapon, wielder);
            WeaponColor = WeaponMaskHelper.GetColorMask(weapon, wielder);

            // item enchantments can also be on wielder currently
            AddSpells(SpellBook, weapon, wielder);
        }

        private WorldObject GetWielder(WorldObject weapon)
        {
            if (weapon.WielderId == null)
                return null;

            return WorldManager.GetPlayerByGuidId(weapon.WielderId.Value);
        }

        /// <summary>
        /// Constructs the bitflags for appraising a WorldObject
        /// </summary>
        private void BuildFlags()
        {
            if (PropertiesInt.Count > 0)
                Flags |= IdentifyResponseFlags.IntStatsTable;
            if (PropertiesInt64.Count > 0)
                Flags |= IdentifyResponseFlags.Int64StatsTable;
            if (PropertiesBool.Count > 0)
                Flags |= IdentifyResponseFlags.BoolStatsTable;
            if (PropertiesFloat.Count > 0)
                Flags |= IdentifyResponseFlags.FloatStatsTable;
            if (PropertiesString.Count > 0)
                Flags |= IdentifyResponseFlags.StringStatsTable;
            if (PropertiesDID.Count > 0)
                Flags |= IdentifyResponseFlags.DidStatsTable;
            if (SpellBook.Count > 0)
                Flags |= IdentifyResponseFlags.SpellBook;
            if (ArmorProfile != null)
                Flags |= IdentifyResponseFlags.ArmorProfile;
            if (CreatureProfile != null)
                Flags |= IdentifyResponseFlags.CreatureProfile;
            if (WeaponProfile != null)
                Flags |= IdentifyResponseFlags.WeaponProfile;
            if (HookProfile != null)
                Flags |= IdentifyResponseFlags.HookProfile;
            if (ArmorHighlight != 0)
                Flags |= IdentifyResponseFlags.ArmorEnchantmentBitfield;
            if (WeaponHighlight != 0)
                Flags |= IdentifyResponseFlags.WeaponEnchantmentBitfield;
            if (ResistHighlight != 0)
                Flags |= IdentifyResponseFlags.ResistEnchantmentBitfield;
            if (ArmorLevels != null)
                Flags |= IdentifyResponseFlags.ArmorLevels;
        }
    }

    public static class AppraiseInfoExtensions
    {
        /// <summary>
        /// Writes the AppraiseInfo to the network stream
        /// </summary>
        public static void Write(this BinaryWriter writer, AppraiseInfo info)
        {
            writer.Write((uint)info.Flags);
            writer.Write(Convert.ToUInt32(info.Success));
            if (info.Flags.HasFlag(IdentifyResponseFlags.IntStatsTable))
                writer.Write(info.PropertiesInt);
            if (info.Flags.HasFlag(IdentifyResponseFlags.Int64StatsTable))
                writer.Write(info.PropertiesInt64);
            if (info.Flags.HasFlag(IdentifyResponseFlags.BoolStatsTable))
                writer.Write(info.PropertiesBool);
            if (info.Flags.HasFlag(IdentifyResponseFlags.FloatStatsTable))
                writer.Write(info.PropertiesFloat);
            if (info.Flags.HasFlag(IdentifyResponseFlags.StringStatsTable))
                writer.Write(info.PropertiesString);
            if (info.Flags.HasFlag(IdentifyResponseFlags.DidStatsTable))
                writer.Write(info.PropertiesDID);
            if (info.Flags.HasFlag(IdentifyResponseFlags.SpellBook))
                writer.Write(info.SpellBook);
            if (info.Flags.HasFlag(IdentifyResponseFlags.ArmorProfile))
                writer.Write(info.ArmorProfile);
            if (info.Flags.HasFlag(IdentifyResponseFlags.CreatureProfile))
                writer.Write(info.CreatureProfile);
            if (info.Flags.HasFlag(IdentifyResponseFlags.WeaponProfile))
                writer.Write(info.WeaponProfile);
            if (info.Flags.HasFlag(IdentifyResponseFlags.HookProfile))
                writer.Write(info.HookProfile);
            if (info.Flags.HasFlag(IdentifyResponseFlags.ArmorEnchantmentBitfield))
            {
                writer.Write((ushort)info.ArmorHighlight);
                writer.Write((ushort)info.ArmorColor);
            }
            if (info.Flags.HasFlag(IdentifyResponseFlags.WeaponEnchantmentBitfield))
            {
                writer.Write((ushort)info.WeaponHighlight);
                writer.Write((ushort)info.WeaponColor);
            }
            if (info.Flags.HasFlag(IdentifyResponseFlags.ResistEnchantmentBitfield))
            {
                writer.Write((ushort)info.ResistHighlight);
                writer.Write((ushort)info.ResistColor);
            }
            if (info.Flags.HasFlag(IdentifyResponseFlags.ArmorLevels))
                writer.Write(info.ArmorLevels);
        }

        // TODO: generics
        public static void Write(this BinaryWriter writer, Dictionary<PropertyInt, int> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<PropertyInt64, long> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<PropertyBool, bool> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.Write(Convert.ToUInt32(kvp.Value));
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<PropertyFloat, double> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<PropertyString, string> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.WriteString16L(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<PropertyDataId, uint> properties)
        {
            PHashTable.WriteHeader(writer, properties.Count);
            foreach (var kvp in properties)
            {
                writer.Write((uint)kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, List<AppraisalSpellBook> spells)
        {
            writer.Write((uint)spells.Count);
            foreach (var spell in spells)
            {
                writer.Write((uint)spell.EnchantmentState | spell.SpellId);
            }
        }
    }
}
