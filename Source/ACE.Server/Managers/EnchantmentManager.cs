using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.Physics.Extensions;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;
using SpellEnchantment = ACE.Server.Entity.SpellEnchantment;

namespace ACE.Server.Managers
{
    public enum StackType
    {
        Undef,
        None,
        Initial,
        Refresh,
        Surpassed,
        Surpass
    };

    public class EnchantmentManager
    {
        public WorldObject WorldObject { get; }
        public Player Player { get; }

        /// <summary>
        /// Returns TRUE if this object has any active enchantments in the registry
        /// </summary>
        public bool HasEnchantments => WorldObject.Biota.HasEnchantments(WorldObject.BiotaDatabaseLock);

        /// <summary>
        /// Returns TRUE If this object has a vitae penalty
        /// </summary>
        public bool HasVitae => WorldObject.Biota.HasEnchantment((uint)Spell.Vitae, WorldObject.BiotaDatabaseLock);

        /// <summary>
        /// Constructs a new EnchantmentManager for a WorldObject
        /// </summary>
        public EnchantmentManager(WorldObject obj)
        {
            WorldObject = obj;
            Player = obj as Player;
        }

        public Entity.Spell Surpass; // retval

        /// <summary>
        /// Add/update zero or more enchantments in this object's registry
        /// </summary>
        public IEnumerable<StackType> AddRange(IEnumerable<Enchantment> enchantment, WorldObject caster)
        {
            List<StackType> stacks = new List<StackType>();
            enchantment.ToList().ForEach(k => stacks.Add(Add(k, caster)));
            return stacks;
        }

        /// <summary>
        /// Add/update an enchantment in this object's registry
        /// </summary>
        public StackType Add(Enchantment enchantment, WorldObject caster)
        {
            StackType result = StackType.Undef;

            // check for existing spell in this category
            var entries = GetCategory(enchantment.Spell.Category);

            // if none, add new record
            if (entries.Count == 0)
            {
                var newEntry = BuildEntry(enchantment.Spell.Id, caster);
                newEntry.LayerId = enchantment.Layer;

                WorldObject.Biota.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);
                WorldObject.ChangesDetected = true;

                return StackType.Initial;
            }

            // Check for existing spells in registry that are superior
            foreach (var entry in entries)
            {
                if (enchantment.Spell.Power < entry.PowerLevel)
                {
                    // superior existing spell
                    Surpass = new Entity.Spell(entry.SpellId);

                    result = StackType.Surpassed;
                }
            }

            if (result != StackType.Surpassed)
            {
                // Check for existing spells in registry that are equal to
                foreach (var entry in entries)
                {
                    if (enchantment.Spell.Power == entry.PowerLevel)
                    {
                        if (entry.Duration == -1)
                        {
                            result = StackType.None;

                            break;
                        }

                        // item cast spell of equal power should override an existing spell, especially one with a duration
                        if ((caster as Creature) == null)
                        {
                            enchantment.Layer = entry.LayerId; // Should be a higher layer than existing enchant

                            var newEntry = BuildEntry(enchantment.Spell.Id, caster);
                            newEntry.LayerId = enchantment.Layer;
                            WorldObject.Biota.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);
                            WorldObject.ChangesDetected = true;
                            result = StackType.Refresh;
                            break;
                        }

                        // refresh existing spell
                        entry.StartTime = 0;

                        result = StackType.Refresh;

                        break;
                    }
                }

                // Previous check didn't return any result
                if (result == StackType.Undef)
                {
                    ushort layerBuffer = 1;
                    // Check for highest existing spell in registry that is inferior
                    foreach (var entry in entries)
                    {
                        if (enchantment.Spell.Power > entry.PowerLevel)
                        {
                            // surpass existing spell
                            Surpass = new Entity.Spell((uint)entry.SpellId);
                            layerBuffer = entry.LayerId;
                        }
                    }

                    enchantment.Layer = (ushort)(layerBuffer + 1); // Should be a higher layer than existing enchant

                    var newEntry = BuildEntry(enchantment.Spell.Id, caster);
                    newEntry.LayerId = enchantment.Layer;
                    WorldObject.Biota.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);
                    WorldObject.ChangesDetected = true;
                    result = StackType.Surpass;
                }
            }

            return result;
        }

        /// <summary>
        /// Writes the EnchantmentRegistry to the network stream
        /// </summary>
        public void SendRegistry(BinaryWriter writer)
        {
            if (Player == null) return;
            var enchantmentRegistry = new EnchantmentRegistry(Player);
            writer.Write(enchantmentRegistry);
        }

        /// <summary>
        /// Writes UpdateEnchantment vitae to the network stream
        /// </summary>
        public void SendUpdateVitae()
        {
            if (Player == null) return;
            var vitae = new Enchantment(Player, GetVitae());
            Player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(Player.Session, vitae));
        }

        /// <summary>
        /// Called on player death
        /// </summary>
        public float UpdateVitae()
        {
            if (Player == null) return 0;
            BiotaPropertiesEnchantmentRegistry vitae;

            if (!HasVitae)
            {
                // add entry for new vitae
                vitae = BuildEntry((uint)Spell.Vitae);
                vitae.EnchantmentCategory = (uint)EnchantmentMask.Vitae;
                vitae.LayerId = 0;
                vitae.StatModValue = 1.0f - (float)PropertyManager.GetDouble("vitae_penalty").Item;
                WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.Add(vitae);
                WorldObject.ChangesDetected = true;
            }
            else
            {
                // update existing vitae
                vitae = GetVitae();
                vitae.StatModValue -= (float)PropertyManager.GetDouble("vitae_penalty").Item;
            }

            var minVitae = GetMinVitae((uint)Player.Level);

            if (vitae.StatModValue < minVitae)
                vitae.StatModValue = minVitae;

            RemoveAllEnchantments();

            return vitae.StatModValue;
        }

        /// <summary>
        /// Removes all enchantments except for vitae
        /// Called on player death
        /// </summary>
        public void RemoveAllEnchantments()
        {
            var spellsToExclude = new Collection<int> { (int)Spell.Vitae };

            WorldObject.Biota.RemoveAllEnchantments(spellsToExclude, WorldObject.BiotaDatabaseLock);
            WorldObject.ChangesDetected = true;
        }

        /// <summary>
        /// Called when player crosses the VitaeCPPool threshold
        /// </summary>
        public float ReduceVitae()
        {
            var vitae = GetVitae();
            vitae.StatModValue += 0.01f;

            if (Math.Abs(vitae.StatModValue - 1.0f) < PhysicsGlobals.EPSILON)
                return 1.0f;

            return vitae.StatModValue;
        }

        /// <summary>
        /// Removes the vitae penalty for a player
        /// </summary>
        public void RemoveVitae()
        {
            if (Player == null)
                return;

            if (WorldObject.Biota.TryRemoveEnchantment((int)Spell.Vitae, out _, WorldObject.BiotaDatabaseLock))
                WorldObject.ChangesDetected = true;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(2.0f);
            actionChain.AddAction(Player, () =>
            {
                Player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(Player.Session, (ushort)Spell.Vitae, 0));
                Player.Session.Network.EnqueueSend(new GameMessageSound(Player.Guid, Sound.SpellExpire, 1.0f));
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Returns TRUE if registry contains spellId for this creature
        /// </summary>
        public bool HasSpell(uint spellId)
        {
            return WorldObject.Biota.HasEnchantment(spellId, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns all of the enchantments for a category
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> GetCategory(uint categoryID)
        {
            return WorldObject.Biota.GetEnchantmentsByCategory((ushort)categoryID, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the enchantments for a specific spell
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry GetSpell(uint spellID)
        {
            return WorldObject.Biota.GetEnchantmentBySpell((int)spellID, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the vitae enchantment
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry GetVitae()
        {
            return GetSpell((uint)Spell.Vitae);
        }

        /// <summary>
        /// Builds an enchantment registry entry from a spell ID
        /// </summary>
        public BiotaPropertiesEnchantmentRegistry BuildEntry(uint spellID, WorldObject caster = null)
        {
            var spell = new Entity.Spell(spellID);

            var entry = new BiotaPropertiesEnchantmentRegistry();

            entry.EnchantmentCategory = (uint)spell.MetaSpellType;
            entry.ObjectId = WorldObject.Guid.Full;
            entry.Object = WorldObject.Biota;
            entry.SpellId = (int)spell.Id;
            entry.SpellCategory = (ushort)spell.Category;
            entry.PowerLevel = spell.Power;

            // should default duration be 0 or -1 here?
            // changed from spellBase -> spell for void..
            if (caster is Creature)
                entry.Duration = spell.Duration;
            else
            {
                if (caster?.WeenieType == WeenieType.Gem)
                    entry.Duration = spell.Duration;
                else
                {
                    entry.Duration = -1.0;
                    entry.StartTime = 0;
                }
            }

            if (caster == null)
                entry.CasterObjectId = WorldObject.Guid.Full;
            else
                entry.CasterObjectId = caster.Guid.Full;

            entry.DegradeModifier = spell.DegradeModifier;
            entry.DegradeLimit = spell.DegradeLimit;
            entry.StatModType = (uint)spell.StatModType;
            entry.StatModKey = spell.StatModKey;
            entry.StatModValue = spell.StatModVal;

            return entry;
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public void Remove(BiotaPropertiesEnchantmentRegistry entry, bool sound = true)
        {
            var spellID = entry.SpellId;
            var spell = new Entity.Spell(spellID);

            if (WorldObject.Biota.TryRemoveEnchantment(spellID, out _, WorldObject.BiotaDatabaseLock))
                WorldObject.ChangesDetected = true;

            if (Player != null)
            {
                Player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(Player.Session, (ushort)entry.SpellId, entry.LayerId));

                if (sound)
                    Player.Session.Network.EnqueueSend(new GameMessageSound(Player.Guid, Sound.SpellExpire, 1.0f));
            }
            else
            {
                var ownerID = WorldObject.OwnerId ?? WorldObject.WielderId;

                if (ownerID != null)
                {
                    var owner = WorldManager.GetPlayerByGuidId((uint)ownerID);

                    if (owner != null)
                    {
                        owner.Session.Network.EnqueueSend(new GameMessageSystemChat($"The spell {spell.Name} on {WorldObject.Name} has expired.", ChatMessageType.Magic));

                        if (sound)
                            owner.Session.Network.EnqueueSend(new GameMessageSound(owner.Guid, Sound.SpellExpire, 1.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Silently removes a spell from the enchantment registry,
        /// and sends the relevant network message for dispel
        /// </summary>
        public void Dispel(BiotaPropertiesEnchantmentRegistry entry)
        {
            var spellID = entry.SpellId;

            if (WorldObject.Biota.TryRemoveEnchantment(spellID, out _, WorldObject.BiotaDatabaseLock))
                WorldObject.ChangesDetected = true;

            if (Player != null)
                Player.Session.Network.EnqueueSend(new GameEventMagicDispelEnchantment(Player.Session, (ushort)entry.SpellId, entry.LayerId));
        }

        /// <summary>
        /// Silently removes multiple spells from the enchantment registry,
        /// and sends the relevent network messages for dispel
        /// </summary>
        public void Dispel(List<BiotaPropertiesEnchantmentRegistry> entries)
        {
            foreach (var entry in entries)
            {
                if (WorldObject.Biota.TryRemoveEnchantment(entry.SpellId, out _, WorldObject.BiotaDatabaseLock))
                    WorldObject.ChangesDetected = true;
            }

            if (Player != null)
                Player.Session.Network.EnqueueSend(new GameEventMagicDispelMultipleEnchantments(Player.Session, entries));
        }

        /// <summary>
        /// Removes all enchantments from the player on server,
        /// and sends network messages to silently dispel the enchantments
        /// </summary>
        public void DispelAllEnchantments(bool showMsg = false)
        {
            var enchantments = WorldObject.Biota.BiotaPropertiesEnchantmentRegistry.ToList();

            foreach (var enchantment in enchantments)
            {
                if (showMsg)
                {
                    var spell = new Entity.Spell(enchantment.SpellId, false);
                    Console.WriteLine("Removing " + spell.Name);
                }
                Dispel(enchantment);
            }
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
        /// Returns the top layers in each spell category
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> GetEnchantments_TopLayer(List<BiotaPropertiesEnchantmentRegistry> enchantments)
        {
            var results = from e in enchantments
                group e by e.SpellCategory
                into categories
                select categories.OrderByDescending(c => c.LayerId).First();

            return results.ToList();
        }

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> GetEnchantments(EnchantmentTypeFlags statModType)
        {
            return GetEnchantments_TopLayer(WorldObject.Biota.GetEnchantmentsByStatModType((uint)statModType, WorldObject.BiotaDatabaseLock));
        }

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type + key
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> GetEnchantments(EnchantmentTypeFlags statModType, uint statModKey)
        {
            return GetEnchantments_TopLayer(WorldObject.Biota.GetEnchantmentsByStatModType((uint)statModType, WorldObject.BiotaDatabaseLock).Where(e => e.StatModKey == statModKey).ToList());
        }

        /// <summary>
        /// Returns the bonus to a skill from enchantments
        /// </summary>
        public int GetSkillMod(Skill skill)
        {
            var enchantments = GetEnchantments(EnchantmentTypeFlags.Skill, (uint)skill);

            var skillMod = 0;
            foreach (var enchantment in enchantments)
                skillMod += (int)enchantment.StatModValue;

            if (SkillHelper.DefenseSkills.Contains(skill))
                skillMod += GetDefenseDebuffMod();

            if (SkillHelper.AttackSkills.Contains(skill))
                skillMod += GetAttackDebuffMod();

            return skillMod;
        }

        /// <summary>
        /// Returns the bonus to an attribute from enchantments
        /// </summary>
        public int GetAttributeMod(PropertyAttribute attribute)
        {
            var enchantments = GetEnchantments(EnchantmentTypeFlags.Attribute, (uint)attribute);

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
            var enchantments = GetEnchantments(type);

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
            var enchantments = GetEnchantments(EnchantmentTypeFlags.Additive, (uint)statModKey);

            var modifier = 0;
            foreach (var enchantment in enchantments)
                modifier += (int)enchantment.StatModValue;

            return modifier;
        }

        public float GetAdditiveMod(PropertyFloat statModKey)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;

            var enchantments = GetEnchantments(typeFlags, (uint)statModKey);

            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the product of the modifiers for a StatModKey
        /// </summary>
        public float GetMultiplicativeMod(PropertyFloat statModKey)
        {
            var enchantments = GetEnchantments(EnchantmentTypeFlags.Multiplicative, (uint)statModKey);

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
            var enchantments = GetEnchantments(typeFlags, (uint)resistance);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the resistance modifier for a damage type
        /// </summary>
        public float GetProtectionResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = GetEnchantments(typeFlags, (uint)resistance);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
            {
                if (enchantment.StatModValue < 1.0f)
                    modifier *= enchantment.StatModValue;
            }

            return modifier;
        }

        /// <summary>
        /// Gets the resistance modifier for a damage type
        /// </summary>
        public float GetVulnerabilityResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = GetEnchantments(typeFlags, (uint)resistance);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
            {
                if (enchantment.StatModValue > 1.0f)
                    modifier *= enchantment.StatModValue;
            }

            return modifier;
        }

        /// <summary>
        /// Gets the regeneration modifier for a vital type
        /// (regeneration / rejuvenation / mana renewal)
        /// </summary>
        public float GetRegenerationMod(CreatureVital vital)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var vitalKey = GetVitalRateKey(vital);
            var enchantments = GetEnchantments(typeFlags, (uint)vitalKey);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the direct modifiers to a vital / secondary attribute
        /// </summary>
        public float GetVitalMod(CreatureVital vital)
        {
            var typeFlags = EnchantmentTypeFlags.SecondAtt | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments(typeFlags, (uint)vital.Vital);

            // additive
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the VitalRate key for a CreatureVital
        /// </summary>
        public PropertyFloat GetVitalRateKey(CreatureVital vital)
        {
            switch (vital.Vital)
            {
                case PropertyAttribute2nd.MaxHealth:
                    return PropertyFloat.HealthRate;
                case PropertyAttribute2nd.MaxStamina:
                    return PropertyFloat.StaminaRate;
                case PropertyAttribute2nd.MaxMana:
                    return PropertyFloat.ManaRate;
            }
            return 0;
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
            // BD8 seems to be the only one with aura in db?
            var aura = GetAdditiveMod(PropertyInt.WeaponAuraDamage);
            if (aura != 0) return aura;

            return GetAdditiveMod(PropertyInt.Damage);
        }

        /// <summary>
        /// Returns the DamageMod for bow / crossbow
        /// </summary>
        /// <returns></returns>
        public float GetDamageModifier()
        {
            return GetMultiplicativeMod(PropertyFloat.DamageMod);
        }

        /// <summary>
        /// Returns the attack skill modifier, ie. Heart Seeker
        /// </summary>
        public float GetAttackMod()
        {
            var aura = GetAdditiveMod(PropertyFloat.WeaponAuraOffense);
            if (aura != 0) return aura;

            return GetAdditiveMod(PropertyFloat.WeaponOffense);
        }

        /// <summary>
        /// Returns the weapon speed modifier, ie. Swift Killer
        /// </summary>
        public int GetWeaponSpeedMod()
        {
            var aura = GetAdditiveMod(PropertyInt.WeaponAuraSpeed);
            if (aura != 0) return aura;

            return GetAdditiveMod(PropertyInt.WeaponTime);
        }

        /// <summary>
        /// Returns the defense skill modifier, ie. Defender
        /// </summary>
        public float GetDefenseMod()
        {
            var aura = GetAdditiveMod(PropertyFloat.WeaponAuraDefense);
            if (aura != 0) return aura;

            return GetAdditiveMod(PropertyFloat.WeaponDefense);
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

        /// <summary>
        /// Gets the additive armor level vs type modifier, ie. banes
        /// </summary>
        public float GetArmorModVsType(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var key = GetImpenBaneKey(damageType);
            var enchantments = GetEnchantments(typeFlags, (uint)key);

            // additive
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the sum of the enchantment statmod values
        /// </summary>
        public float GetAdditiveMod(List<BiotaPropertiesEnchantmentRegistry> enchantments)
        {
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the defense skill debuffs for Dirty Fighting
        /// </summary>
        /// <returns></returns>
        public int GetDefenseDebuffMod()
        {
            var typeFlags = EnchantmentTypeFlags.Skill | EnchantmentTypeFlags.Additive | EnchantmentTypeFlags.DefenseSkills;
            var enchantments = GetEnchantments(typeFlags, 0);

            // additive
            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        /// <summary>
        /// Returns the attack skill debuffs for Dirty Fighting
        /// </summary>
        /// <returns></returns>
        public int GetAttackDebuffMod()
        {
            var typeFlags = EnchantmentTypeFlags.Skill | EnchantmentTypeFlags.Additive | EnchantmentTypeFlags.AttackSkills;
            var enchantments = GetEnchantments(typeFlags, 0);

            // additive
            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        /// <summary>
        /// Returns a rating enchantment modifier
        /// </summary>
        /// <param name="property">The rating to return an enchantment modifier</param>
        public int GetRating(PropertyInt property)
        {
            var typeFlags = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments(typeFlags, (uint)property);

            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        /// <summary>
        /// Returns the damage rating modifier from enchantments
        /// as an int rating (additive)
        /// </summary>
        public int GetDamageRating()
        {
            var damageRating = GetRating(PropertyInt.DamageRating);

            // weakness as negative damage rating?
            var weaknessRating = GetRating(PropertyInt.WeaknessRating);

            return damageRating - weaknessRating;
        }

        public int GetDamageResistRating()
        {
            var damageResistanceRating = GetRating(PropertyInt.DamageResistRating);

            // nether DoTs as negative DRR?
            var netherDotDamageRating = GetNetherDotDamageRating();

            return damageResistanceRating - netherDotDamageRating;
        }

        public int GetNetherDotDamageRating()
        {
            var type = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var netherDots = GetEnchantments(type, (uint)PropertyInt.NetherOverTime);

            var totalRating = 0.0f;
            foreach (var netherDot in netherDots)
            {
                var spell = new Entity.Spell(netherDot.SpellId);

                var baseDamage = Math.Max(0.5f, spell.Formula.Level - 1);

                // destructive curse / corruption
                if (netherDot.SpellCategory == 636 || netherDot.SpellCategory == 638)
                    totalRating += baseDamage;
                else if (netherDot.SpellCategory == 637)    // corrosion
                    totalRating += Math.Max(baseDamage * 2 - 1, 2);
            }
            return totalRating.Round();
        }

        /// <summary>
        /// Returns the healing resistance rating enchantment modifier
        /// </summary>
        public float GetHealingResistRatingMod()
        {
            var rating = GetRating(PropertyInt.HealingResistRating);

            // return as rating mod
            return 100.0f / (100 + rating);
        }

        /// <summary>
        ///  Returns the damage over time (DoT) enchantment mod
        /// </summary>
        public float GetDamageOverTimeMod()
        {
            var typeFlags = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments(typeFlags, (uint)PropertyInt.DamageOverTime);

            // additive float
            return GetAdditiveMod(enchantments);
        }

        /// <summary>
        /// Returns a list of all the active enchantments for a magic school
        /// </summary>
        public List<BiotaPropertiesEnchantmentRegistry> GetEnchantments(MagicSchool magicSchool)
        {
            var spells = new List<BiotaPropertiesEnchantmentRegistry>();

            var enchantments = from e in WorldObject.Biota.GetEnchantments(WorldObject.BiotaDatabaseLock)
                group e by e.SpellCategory
                into categories
                select categories.OrderByDescending(c => c.LayerId).First();

            foreach (var enchantment in enchantments)
            {
                var spell = new Entity.Spell(enchantment.SpellId);

                if (spell.School == magicSchool)
                    spells.Add(enchantment);
            }

            return spells;
        }

        /// <summary>
        /// Called every ~5 seconds for active object
        /// </summary>
        public void HeartBeat()
        {
            var expired = new List<BiotaPropertiesEnchantmentRegistry>();

            var enchantments = WorldObject.Biota.GetEnchantments(WorldObject.BiotaDatabaseLock);
            HeartBeat_DamageOverTime(GetEnchantments_TopLayer(enchantments));

            foreach (var enchantment in enchantments)
            {
                enchantment.StartTime -= WorldObject.HeartbeatInterval ?? 5;

                // StartTime ticks backwards to -Duration
                if (enchantment.Duration > 0 && enchantment.StartTime <= -enchantment.Duration)
                    expired.Add(enchantment);
            }

            foreach (var enchantment in expired)
                Remove(enchantment);
        }

        /// <summary>
        /// Applies damage from DoTs every ~5 seconds
        /// </summary>
        /// <param name="enchantments">A list of active enchantments at the top layers</param>
        public void HeartBeat_DamageOverTime(List<BiotaPropertiesEnchantmentRegistry> enchantments)
        {
            var dots = new List<BiotaPropertiesEnchantmentRegistry>();
            var netherDots = new List<BiotaPropertiesEnchantmentRegistry>();

            foreach (var enchantment in enchantments)
            {
                // combine DoTs from multiple sources
                if (enchantment.StatModKey == (int)PropertyInt.DamageOverTime)
                    dots.Add(enchantment);

                if (enchantment.StatModKey == (int)PropertyInt.NetherOverTime)
                    netherDots.Add(enchantment);
            }

            // apply damage over time (DoTs)
            if (dots.Count > 0)
                ApplyDamageTick(dots, DamageType.Undef);

            if (netherDots.Count > 0)
                ApplyDamageTick(netherDots, DamageType.Nether);
        }

        /// <summary>
        /// Applies 1 tick of damage from a DoT spell
        /// </summary>
        /// <param name="enchantment">The damage over time (DoT) spell</param>
        public void ApplyDamageTick(List<BiotaPropertiesEnchantmentRegistry> enchantments, DamageType damageType)
        {
            var creature = WorldObject as Creature;
            if (creature == null) return;

            var damagers = new Dictionary<WorldObject, float>();

            // get the total tick amount
            var tickAmountTotal = 0.0f;
            foreach (var enchantment in enchantments)
            {
                var totalAmount = enchantment.StatModValue;
                var totalTicks = (int)Math.Ceiling(enchantment.Duration / (WorldObject.HeartbeatInterval ?? 5));
                var tickAmount = totalAmount / totalTicks;

                // run tick amount through damage calculation functions?
                // it appears retail might have done an initial damage calc,
                // and then applied that to the enchantment StatModVal beforehand
                // for each damage tick, this pre-calc would then be multiplied
                // against the realtime resistances

                var damager = WorldObject.CurrentLandblock?.GetObject(enchantment.CasterObjectId);
                if (damager == null)
                {
                    Console.WriteLine($"{WorldObject.Name}.ApplyDamageTick() - couldn't find damager {enchantment.CasterObjectId:X8}");
                    continue;
                }

                // get damage / damage resistance rating here for now?
                var damageRatingMod = Creature.GetRatingMod(damager.EnchantmentManager.GetDamageRating());
                var damageResistRatingMod = Creature.GetNegativeRatingMod(GetDamageResistRating());
                //Console.WriteLine("DR: " + Creature.ModToRating(damageRatingMod));
                //Console.WriteLine("DRR: " + Creature.NegativeModToRating(damageResistRatingMod));
                tickAmount *= damageRatingMod * damageResistRatingMod;

                if (damagers.ContainsKey(damager))
                    damagers[damager] += tickAmount;
                else
                    damagers.Add(damager, tickAmount);

                creature.DamageHistory.Add(damager, damageType, (uint)Math.Round(tickAmount));

                tickAmountTotal += tickAmount;
            }

            creature.TakeDamageOverTime(tickAmountTotal, damageType);

            if (!creature.IsAlive) return;

            foreach (var kvp in damagers)
            {
                var damager = kvp.Key;
                var amount = kvp.Value;

                var damageSourcePlayer = damager as Player;
                if (damageSourcePlayer != null)
                    creature.TakeDamageOverTime_NotifySource(damageSourcePlayer, damageType, amount);
            }
        }

        /// <summary>
        /// Selects a list of spells to dispel
        /// </summary>
        /// <param name="spell">The dispel spell</param>
        public List<SpellEnchantment> SelectDispel(Entity.Spell spell)
        {
            // NOTE: in the default 16PY db,
            // there are a lot of dispels where the actual #s do not match up with the spell descriptions...
            // ie. the description will say it dispels 3-6 spells, and it will only dispel 2-4 etc.

            // dispel factors:
            // min_power - the minimum power level of spell to dispel (unused?)
            // max_power - the maximum power level of spell to dispel
            // power_variance - rng for power level, unused?
            // dispel_school - the magic school to dispel, 0 if all
            // align - type of spells to dispel: positive, negative, or all
            // number - the maximum # of spells to dispel
            // number_variance - number * number_variance = the minum # of spells to dispel
            var minPower = spell.MinPower;
            var maxPower = spell.MaxPower;
            var powerVariance = spell.PowerVariance;
            var dispelSchool = spell.DispelSchool;
            var align = spell.Align;
            var number = spell.Number;
            var numberVariance = spell.NumberVariance;

            var enchantments = GetEnchantments_TopLayer(WorldObject.Biota.GetEnchantments(WorldObject.BiotaDatabaseLock));

            var filtered = enchantments.Where(e => e.PowerLevel <= maxPower);

            // for dispelSchool and align,
            // we probably could do some calculations to figure out these values directly from the enchantments
            // but it would be far easier and more reliable to just do them through the spells
            // since dispels are not a time-critical function, this should still be fine
            var spells = new List<SpellEnchantment>();
            foreach (var filter in filtered)
                spells.Add(new SpellEnchantment(filter));

            var filterSpells = spells;
            if (dispelSchool != MagicSchool.None)
                filterSpells = filterSpells.Where(s => s.Spell.School == dispelSchool).ToList();

            if (align != DispelType.All)
            {
                if (align == DispelType.Positive)
                    filterSpells = filterSpells.Where(s => s.Spell.IsBeneficial).ToList();
                else if (align == DispelType.Negative)
                    filterSpells = filterSpells.Where(s => s.Spell.IsHarmful).ToList();
            }

            // dispel all
            if (number == -1)
                return filterSpells;

            // get number of spells to dispel
            var dispelNum = number;
            if (numberVariance != 1.0f)
            {
                var maxDispelNum = dispelNum;
                var minDispelNum = (int)Math.Round(dispelNum * numberVariance);

                // factor in rng variance
                dispelNum = Physics.Common.Random.RollDice(minDispelNum, maxDispelNum);
            }

            // randomize the filtered spell list
            filterSpells.Shuffle();

            // select the required # of spells
            return filterSpells.Take(dispelNum).ToList();
        }
    }
}
