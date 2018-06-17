using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public enum StackType
    {
        None,
        Initial,
        Refresh,
        Surpassed,
        Surpass
    };

    public class EnchantmentManager
    {
        public WorldObject WorldObject { get; }
        public ICollection<BiotaPropertiesEnchantmentRegistry> Enchantments { get; }

        /// <summary>
        /// Returns TRUE if this object has any active enchantments in the registry
        /// </summary>
        public bool HasEnchantments => WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Any();

        /// <summary>
        /// Returns TRUE If this object has a vitae penalty
        /// </summary>
        public bool HasVitae => WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Any(e => e.SpellId == (uint)Spell.Vitae);

        /// <summary>
        /// Constructs a new EnchantmentManager for a WorldObject
        /// </summary>
        public EnchantmentManager(WorldObject obj)
        {
            WorldObject = obj;
            Enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry;
        }

        public Database.Models.World.Spell Surpass; // retval

        /// <summary>
        /// Add/update an enchantment in this object's registry
        /// </summary>
        public StackType Add(Enchantment enchantment, bool castByItem)
        {
            // check for existing spell in this category
            var entry = GetCategory(enchantment.Spell.Category);

/*
                if (enchantment.Spell.Power > entry.PowerLevel)
                {
                    // surpass existing spell
                    Surpass = DatabaseManager.World.GetCachedSpell((uint)entry.SpellId);
                    Remove(entry, false);
                    entry = BuildEntry(enchantment.Spell.SpellId);
                    entry.Duration = -1;
                    entry.StartTime = 0;
                    entry.LayerId = enchantment.Layer;
                    WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(entry);
                    return StackType.Surpass;
                }

                // TODO: the refresh spell case may need some additional functionality to get working correctly
                if (enchantment.Spell.Power == entry.PowerLevel)
                {
                    if (entry.Duration != -1)
                    {
                        // refresh existing spell with a no countdown timer
                        entry.LayerId++;
                        entry.Duration = -1;
                        entry.StartTime = 0;
                        return StackType.Refresh;
                    }
                    
                    return StackType.None;
                }

                // superior existing spell
                Surpass = DatabaseManager.World.GetCachedSpell((uint)entry.SpellId);
                return StackType.Surpassed;
            }
            */
            // if none, add new record
            if (entry == null)
            {
                entry = BuildEntry(enchantment.Spell.SpellId, castByItem);
                entry.LayerId = enchantment.Layer;
                var type = (EnchantmentTypeFlags)entry.StatModType;
                WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(entry);

                return StackType.Initial;
            }

            if (enchantment.Spell.Power > entry.PowerLevel)
            {
                // surpass existing spell
                Surpass = DatabaseManager.World.GetCachedSpell((uint)entry.SpellId);
                Remove(entry, false);
                entry = BuildEntry(enchantment.Spell.SpellId, castByItem);
                entry.LayerId = enchantment.Layer;
                WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(entry);
                return StackType.Surpass;
            }

            if (enchantment.Spell.Power == entry.PowerLevel)
            {
                if (entry.Duration != -1)
                {
                    // refresh existing spell
                    entry.LayerId++;
                    entry.StartTime = 0;
                    return StackType.Refresh;
                }

                return StackType.None;
            }

            // superior existing spell
            Surpass = DatabaseManager.World.GetCachedSpell((uint)entry.SpellId);
            return StackType.Surpassed;
        }

        /// <summary>
        /// Writes the EnchantmentRegistry to the network stream
        /// </summary>
        public void SendRegistry(BinaryWriter writer)
        {
            var player = WorldObject as Player;
            var enchantmentRegistry = new EnchantmentRegistry(player);
            writer.Write(enchantmentRegistry);
        }

        /// <summary>
        /// Writes UpdateEnchantment vitae to the network stream
        /// </summary>
        public void SendUpdateVitae()
        {
            var player = WorldObject as Player;
            var vitae = new Enchantment(player, GetVitae());
            player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(player.Session, vitae));
        }

        /// <summary>
        /// Called on player death
        /// </summary>
        public float UpdateVitae()
        {
            BiotaPropertiesEnchantmentRegistry vitae = null;

            if (!HasVitae)
            {
                // add entry for new vitae
                vitae = BuildEntry((uint)Spell.Vitae);
                vitae.EnchantmentCategory = (uint)EnchantmentMask.Vitae;
                vitae.LayerId = 0;
                vitae.StatModValue = 1.0f - (float)PropertyManager.GetDouble("vitae_penalty").Item;

                WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(vitae);
            }
            else
            {
                // update existing vitae
                vitae = GetVitae();
                vitae.StatModValue -= (float)PropertyManager.GetDouble("vitae_penalty").Item;
            }

            var player = WorldObject as Player;
            var minVitae = GetMinVitae((uint)player.Level);
            if (vitae.StatModValue < minVitae)
                vitae.StatModValue = minVitae;

            SaveDatabase();

            return vitae.StatModValue;
        }

        /// <summary>
        /// Called when player crosses the VitaeCPPool threshold
        /// </summary>
        public float ReduceVitae()
        {
            var vitae = GetVitae();
            vitae.StatModValue += 0.01f;
            //SaveDatabase();

            var player = WorldObject as Player;

            if (Math.Abs(vitae.StatModValue - 1.0f) < PhysicsGlobals.EPSILON)
                return 1.0f;

            return vitae.StatModValue;
        }

        /// <summary>
        /// Removes the vitae penalty for a player
        /// </summary>
        public void RemoveVitae()
        {
            WorldObject.RemoveEnchantment((int)Spell.Vitae);
            var player = WorldObject as Player;
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(2.0f);
            actionChain.AddAction(player, () =>
            {
                player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(player.Session, (ushort)Spell.Vitae, 0));
                player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.SpellExpire, 1.0f));
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Returns TRUE if registry contains spellId for this creature
        /// </summary>
        public bool HasSpell(uint spellId)
        {
            return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Any(e => e.SpellId == spellId);
        }

        /// <summary>
        /// Returns all of the enchantments for a category
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry GetCategory(uint categoryID)
        {
            return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(e => e.SpellCategory == categoryID);
        }

        /// <summary>
        /// Returns the enchantments for a specific spell
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry GetSpell(uint spellID)
        {
            return WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(e => e.SpellId == spellID);
        }

        /// <summary>
        /// Returns the vitae enchantment
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry GetVitae()
        {
            return GetSpell((uint)Spell.Vitae);
        }

        /// <summary>
        /// Called on player death
        /// </summary>
        public void SaveDatabase()
        {
            var player = WorldObject as Player;
            var saveChain = player.GetSaveChain();
            saveChain.EnqueueChain();
        }

        /// <summary>
        /// Builds an enchantment registry entry from a spell ID
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry BuildEntry(uint spellID, bool castByItem = false)
        {
            var spellBase = DatManager.PortalDat.SpellTable.Spells[spellID];
            var spell = DatabaseManager.World.GetCachedSpell(spellID);

            var entry = new BiotaPropertiesEnchantmentRegistry();

            entry.EnchantmentCategory = (uint)spell.MetaSpellType;
            var enchantmentType = (EnchantmentTypeFlags)entry.EnchantmentCategory;
            entry.ObjectId = WorldObject.Guid.Full;
            entry.Object = WorldObject.Biota;
            entry.SpellId = (int)spell.SpellId;
            entry.SpellCategory = (ushort)spell.Category;
            entry.PowerLevel = spell.Power;

            if (castByItem)
            {
                entry.Duration = -1.0;
                entry.StartTime = 0;
            }
            else
                entry.Duration = spell.Duration ?? 0.0;

            entry.CasterObjectId = WorldObject.Guid.Full;   // only works for self?
            entry.DegradeModifier = spell.DegradeModifier ?? 0.0f;
            entry.DegradeLimit = spell.DegradeLimit ?? 0.0f;
            entry.StatModType = spell.StatModType ?? 0;
            entry.StatModKey = spell.StatModKey ?? 0;
            entry.StatModValue = spell.StatModVal ?? 0.0f;

            return entry;
        }

        /// <summary>
        /// Called every ~5 seconds for active object
        /// </summary>
        public void HeartBeat()
        {
            var expired = new List<BiotaPropertiesEnchantmentRegistry>();

            foreach (var enchantment in Enchantments)
            {
                enchantment.StartTime -= 5;

                // StartTime ticks backwards to -Duration
                if (enchantment.Duration > 0 && enchantment.StartTime <= -enchantment.Duration)
                    expired.Add(enchantment);
            }

            foreach (var enchantment in expired)
                Remove(enchantment);
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public void Remove(BiotaPropertiesEnchantmentRegistry entry, bool sound = true)
        {
            var spellID = entry.SpellId;
            var spell = DatabaseManager.World.GetCachedSpell((uint)spellID);
            WorldObject.RemoveEnchantment(spellID);
            var player = WorldObject as Player;
            player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(player.Session, (ushort)entry.SpellId, entry.LayerId));

            if (sound)
                player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.SpellExpire, 1.0f));
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public void Dispel(BiotaPropertiesEnchantmentRegistry entry)
        {
            var spellID = entry.SpellId;
            var spell = DatabaseManager.World.GetCachedSpell((uint)spellID);
            WorldObject.RemoveEnchantment(spellID);
            var player = WorldObject as Player;
            player.Session.Network.EnqueueSend(new GameEventMagicDispelEnchantment(player.Session, (ushort)entry.SpellId, entry.LayerId));

        }

        /// <summary>
        /// Returns the minimum vitae for a player level
        /// </summary>
        public float GetMinVitae(uint level)
        {
            var propVitae = PropertyManager.GetDouble("vitae_min").Item;
            var maxPenalty = (level - 1) * 3;
            if (maxPenalty < 1)
                maxPenalty = 1;
            var globalMax = 100 - (uint)Math.Round(propVitae * 100);
            if (maxPenalty > globalMax)
                maxPenalty = globalMax;

            var minVitae = (100 - maxPenalty) / 100.0f;
            if (minVitae < propVitae)
                minVitae = (float)propVitae;

            return minVitae;
        }

        /// <summary>
        /// Returns the bonus to a skill from enchantments
        /// </summary>
        public int GetSkillMod(Skill skill)
        {
            var typeFlags = EnchantmentTypeFlags.Skill;
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(typeFlags) && e.StatModKey == (uint)skill);
            if (enchantments == null) return 0;

            var skillMod = 0;
            foreach (var enchantment in enchantments)
                skillMod += (int)enchantment.StatModValue;

            return skillMod;
        }

        /// <summary>
        /// Returns the bonus to an attribute from enchantments
        /// </summary>
        public int GetAttributeMod(PropertyAttribute attribute)
        {
            var typeFlags = EnchantmentTypeFlags.Attribute;
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(typeFlags) && e.StatModKey == (uint)attribute);
            if (enchantments == null) return 0;

            var attributeMod = 0;
            foreach (var enchantment in enchantments)
                attributeMod += (int)enchantment.StatModValue;

            return attributeMod;
        }

        /// <summary>
        /// Returns the base armor modifier from enchantments
        /// </summary>
        /// <returns></returns>
        public int GetBodyArmorMod()
        {
            return GetModifier(EnchantmentTypeFlags.BodyArmorValue);
        }

        /// <summary>
        /// Returns the sum of the StatModValues for an EnchantmentTypeFlag
        /// </summary>
        public int GetModifier(EnchantmentTypeFlags type)
        {
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(type));
            if (enchantments == null) return 0;

            var modifier = 0;
            foreach (var enchantment in enchantments)
                modifier += (int)enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the sum of the modifiers for a StatModKey
        /// </summary>
        public int GetAdditiveMod(PropertyInt statModKey)
        {
            var type = EnchantmentTypeFlags.Additive;
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(type) && e.StatModKey == (int)statModKey);
            if (enchantments == null) return 0;

            // additive
            var modifier = 0;
            foreach (var enchantment in enchantments)
                modifier += (int)enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the product of the modifiers for a StatModKey
        /// </summary>
        public float GetMultiplicativeMod(PropertyFloat statModKey)
        {
            var type = EnchantmentTypeFlags.Multiplicative;
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(type) && e.StatModKey == (int)statModKey);
            if (enchantments == null) return 1.0f;

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the resistance modifier for a damage type
        /// </summary>
        public float GetResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(typeFlags) && e.StatModKey == (int)resistance);
            if (enchantments == null) return 1.0f;

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the multiplicative armor level modifier, ie. banes
        /// </summary>
        public float GetArmorModVsType(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var key = GetImpenBaneKey(damageType);
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Where(e => ((EnchantmentTypeFlags)e.StatModType).HasFlag(typeFlags) && e.StatModKey == (int)key);
            if (enchantments == null) return 0.0f;

            // additive
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the ArmorModVsType key for a DamageType
        /// </summary>
        public PropertyFloat GetImpenBaneKey(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return PropertyFloat.ArmorModVsSlash;
                case DamageType.Pierce:
                    return PropertyFloat.ArmorModVsPierce;
                case DamageType.Bludgeon:
                    return PropertyFloat.ArmorModVsBludgeon;
                case DamageType.Fire:
                    return PropertyFloat.ArmorModVsFire;
                case DamageType.Cold:
                    return PropertyFloat.ArmorModVsCold;
                case DamageType.Acid:
                    return PropertyFloat.ArmorModVsAcid;
                case DamageType.Electric:
                    return PropertyFloat.ArmorModVsElectric;
                case DamageType.Nether:
                    return PropertyFloat.ArmorModVsNether;
            }
            return 0;
        }

        /// <summary>
        /// Gets the resistance PropertyFloat for a DamageType
        /// </summary>
        public PropertyFloat GetResistanceKey(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Slash:
                    return PropertyFloat.ResistSlash;
                case DamageType.Pierce:
                    return PropertyFloat.ResistPierce;
                case DamageType.Bludgeon:
                    return PropertyFloat.ResistBludgeon;
                case DamageType.Fire:
                    return PropertyFloat.ResistFire;
                case DamageType.Cold:
                    return PropertyFloat.ResistCold;
                case DamageType.Acid:
                    return PropertyFloat.ResistAcid;
                case DamageType.Electric:
                    return PropertyFloat.ResistElectric;
                case DamageType.Nether:
                    return PropertyFloat.ResistNether;
            }
            return 0;
        }

        /// <summary>
        /// Returns the weapon damage modifier, ie. Blood Drinker
        /// </summary>
        public int GetDamageMod()
        {
            return GetAdditiveMod(PropertyInt.Damage);
        }

        /// <summary>
        /// Returns the attack skill modifier, ie. Heart Seeker
        /// </summary>
        public float GetAttackMod()
        {
            return GetMultiplicativeMod(PropertyFloat.WeaponOffense);
        }

        /// <summary>
        /// Returns the weapon speed modifier, ie. Swift Killer
        /// </summary>
        public int GetWeaponSpeedMod()
        {
            return GetAdditiveMod(PropertyInt.WeaponTime);
        }

        /// <summary>
        /// Returns the defense skill modifier, ie. Defender
        /// </summary>
        public float GetDefenseMod()
        {
            return GetMultiplicativeMod(PropertyFloat.WeaponDefense);
        }

        /// <summary>
        /// Returns the weapon damage variance modifier
        /// </summary>
        /// <returns></returns>
        public float GetVarianceMod()
        {
            return GetMultiplicativeMod(PropertyFloat.DamageVariance);
        }

        /// <summary>
        /// Returns the additive armor level modifier, ie. Impenetrability
        /// </summary>
        public int GetArmorMod()
        {
            return GetAdditiveMod(PropertyInt.ArmorLevel);
        }
    }
}
