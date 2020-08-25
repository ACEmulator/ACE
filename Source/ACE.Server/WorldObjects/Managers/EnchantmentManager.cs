using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects.Managers
{
    public enum StackType
    {
        None,
        Initial,
        Surpass,
        Refresh,
        Surpassed,
    };

    public class EnchantmentManager
    {
        public WorldObject WorldObject { get; }
        public Player Player { get; }

        /// <summary>
        /// Returns TRUE if this object has any active enchantments in the registry
        /// </summary>
        public virtual bool HasEnchantments => WorldObject.Biota.PropertiesEnchantmentRegistry.HasEnchantments(WorldObject.BiotaDatabaseLock);

        /// <summary>
        /// Returns TRUE If this object has a vitae penalty
        /// </summary>
        public bool HasVitae => WorldObject.Biota.PropertiesEnchantmentRegistry.HasEnchantment((uint)SpellId.Vitae, WorldObject.BiotaDatabaseLock);

        /// <summary>
        /// Constructs a new EnchantmentManager for a WorldObject
        /// </summary>
        public EnchantmentManager(WorldObject obj)
        {
            WorldObject = obj;
            Player = obj as Player;
        }


        /// <summary>
        /// Returns TRUE if registry contains spellId for this creature
        /// </summary>
        public bool HasSpell(uint spellId)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.HasEnchantment(spellId, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the enchantments for a specific spell
        /// </summary>
        public PropertiesEnchantmentRegistry GetEnchantment(uint spellID, uint? casterGuid = null)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentBySpell((int)spellID, casterGuid, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the enchantments for a specific spell from an equipment set
        /// </summary>
        public PropertiesEnchantmentRegistry GetEnchantment(uint spellID, EquipmentSet equipmentSet)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentBySpellSet((int)spellID, equipmentSet, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns a list of all the active enchantments for a magic school
        /// </summary>
        public List<PropertiesEnchantmentRegistry> GetEnchantments(MagicSchool magicSchool)
        {
            var spells = new List<PropertiesEnchantmentRegistry>();

            var enchantments = from e in WorldObject.Biota.PropertiesEnchantmentRegistry.Clone(WorldObject.BiotaDatabaseLock)
                group e by e.SpellCategory
                into categories
                select categories.OrderByDescending(c => c.LayerId).First();

            foreach (var enchantment in enchantments)
            {
                if (enchantment.SpellId > SpellCategory_Cooldown)
                    continue;

                var spell = new Spell(enchantment.SpellId);

                if (spell.NotFound)
                {
                    Console.WriteLine($"EnchantmentManager.GetEnchantments({magicSchool}): couldn't find spell {enchantment.SpellId} for {WorldObject.Name}");
                    continue;
                }

                if (spell.School == magicSchool)
                    spells.Add(enchantment);
            }

            return spells;
        }

        /// <summary>
        /// Returns all of the enchantments for a category
        /// </summary>
        public List<PropertiesEnchantmentRegistry> GetEnchantments(SpellCategory spellCategory)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentsByCategory(spellCategory, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type
        /// </summary>
        public List<PropertiesEnchantmentRegistry> GetEnchantments_TopLayer(EnchantmentTypeFlags statModType)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentsTopLayerByStatModType(statModType, WorldObject.BiotaDatabaseLock);
        }

        /// <summary>
        /// Returns the top layers in each spell category for a StatMod type + key
        /// </summary>
        public List<PropertiesEnchantmentRegistry> GetEnchantments_TopLayer(EnchantmentTypeFlags statModType, uint statModKey, bool handleMultiple = false)
        {
            return WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentsTopLayerByStatModType(statModType, statModKey, WorldObject.BiotaDatabaseLock, handleMultiple);
        }

        /// <summary>
        /// Add/update an enchantment in this object's registry
        /// </summary>
        public virtual AddEnchantmentResult Add(Spell spell, WorldObject caster, bool equip = false)
        {
            var result = new AddEnchantmentResult();

            // check for existing spell in this category
            var entries = GetEnchantments(spell.Category);

            // if none, add new record
            if (entries.Count == 0)
            {
                var newEntry = BuildEntry(spell, caster, equip);
                newEntry.LayerId = 1;
                WorldObject.Biota.PropertiesEnchantmentRegistry.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);
                WorldObject.ChangesDetected = true;

                result.Enchantment = newEntry;
                result.StackType = StackType.Initial;
                return result;
            }

            result.BuildStack(entries, spell, caster, equip);

            // handle cases:
            // surpassing: new spell is written to next layer
            // refreshing: - key by caster guid
            // surpassed:  - underpowered spell is written to next layer?

            // note that these cases are not exclusive,
            // consider case: strength 3 -> strength 6 -> strength 3
            // for the 2nd cast of strength 3, it would have 1 refresh and 1 surpassed
            // would 2nd cast of strength 3 refresh the 1st, but still be surpassed by 6?

            var refreshSpell = result.Refresh.Count > 0 ? result.RefreshCaster : null;

            if (refreshSpell == null)
            {
                var newEntry = BuildEntry(spell, caster, equip);
                newEntry.LayerId = result.NextLayerId;
                WorldObject.Biota.PropertiesEnchantmentRegistry.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);

                result.Enchantment = newEntry;
            }
            else
            {
                // for multiple void casters casting the same DoT,
                // we might want to sort by StatModValue in GetEnchantments_TopLayer()

                // for the same void caster re-casting the same DoT,
                // should be update the StatModVal here?

                var duration = spell.Duration;
                if (caster is Player player && player.AugmentationIncreasedSpellDuration > 0 && spell.DotDuration == 0)
                    duration *= 1.0f + player.AugmentationIncreasedSpellDuration * 0.2f;

                var timeRemaining = refreshSpell.Duration + refreshSpell.StartTime;

                if (duration > timeRemaining)
                {
                    refreshSpell.StartTime = 0;
                    refreshSpell.Duration = duration;
                }

                result.Enchantment = refreshSpell;
            }
            WorldObject.ChangesDetected = true;

            // output message is from StackType,
            // which is the largest of the combined StackTypes

            return result;
        }

        /// <summary>
        /// Builds an enchantment registry entry from a spell ID
        /// </summary>
        private PropertiesEnchantmentRegistry BuildEntry(Spell spell, WorldObject caster = null, bool equip = false)
        {
            var entry = new PropertiesEnchantmentRegistry();

            entry.EnchantmentCategory = (uint)spell.MetaSpellType;
            entry.SpellId = (int)spell.Id;
            entry.SpellCategory = spell.Category;
            entry.PowerLevel = spell.Power;

            if (caster is Creature)
            {
                entry.Duration = spell.Duration;

                if (caster is Player player && player.AugmentationIncreasedSpellDuration > 0 && spell.DotDuration == 0)
                    entry.Duration *= 1.0f + player.AugmentationIncreasedSpellDuration * 0.2f;
            }
            else
            {
                if (!equip)
                {
                    entry.Duration = spell.Duration;
                }
                else
                {
                    // enchantments from equipping items are active until the item is dequipped
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
            entry.StatModType = spell.StatModType;
            entry.StatModKey = spell.StatModKey;
            entry.StatModValue = spell.StatModVal;

            if (spell.DotDuration != 0)
            {
                var heartbeatInterval = WorldObject.HeartbeatInterval ?? 5.0f;

                // scale StatModValue for HeartbeatIntervals other than the default 5s
                if (heartbeatInterval != 5.0f)
                {
                    entry.StatModValue = spell.GetDamagePerTick((float)heartbeatInterval);
                }

                // calculate runtime StatModValue for enchantment
                if (caster != null)
                {
                    entry.StatModValue = caster.CalculateDotEnchantment_StatModValue(spell, WorldObject, entry.StatModValue);
                }
                //Console.WriteLine($"enchantment_statModVal: {entry.StatModValue}");
            }

            // handle equipment sets
            if (caster != null && caster.HasItemSet && caster.ItemSetContains(spell.Id))
            {
                entry.HasSpellSetId = true;
                entry.SpellSetId = (EquipmentSet)caster.EquipmentSetId;
            }

            return entry;
        }

        /// <summary>
        /// Adds a cooldown spell to the enchantment registry
        /// </summary>
        public virtual bool StartCooldown(WorldObject item)
        {
            var cooldownID = item.CooldownId;
            if (cooldownID == null)
                return false;

            var newEntry = new PropertiesEnchantmentRegistry();

            // TODO: BiotaPropertiesEnchantmentRegistry.SpellId should be uint
            newEntry.SpellId = (int)GetCooldownSpellID(cooldownID.Value);
            newEntry.SpellCategory = (SpellCategory)SpellCategory_Cooldown;
            newEntry.HasSpellSetId = true;
            newEntry.Duration = item.CooldownDuration ?? 0.0f;
            newEntry.CasterObjectId = item.Guid.Full;
            newEntry.DegradeLimit = -666;
            newEntry.StatModType = EnchantmentTypeFlags.Cooldown;
            newEntry.EnchantmentCategory = (uint)EnchantmentMask.Cooldown;

            newEntry.LayerId = 1;      // cooldown at layer 1, any spells at layer 2?
            WorldObject.Biota.PropertiesEnchantmentRegistry.AddEnchantment(newEntry, WorldObject.BiotaDatabaseLock);
            WorldObject.ChangesDetected = true;

            Player.Session.Network.EnqueueSend(new GameEventMagicUpdateEnchantment(Player.Session, new Enchantment(Player, newEntry)));

            return true;
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public virtual void Remove(PropertiesEnchantmentRegistry entry, bool sound = true)
        {
            if (entry == null)
                return;

            var spellID = entry.SpellId;

            if (WorldObject.Biota.PropertiesEnchantmentRegistry.TryRemoveEnchantment(entry.SpellId, entry.CasterObjectId, WorldObject.BiotaDatabaseLock))
                WorldObject.ChangesDetected = true;

            if (Player != null)
            {
                var layer = (entry.SpellId == (uint)SpellId.Vitae) ? (ushort)0 : entry.LayerId; // this line is to force vitae to be layer 0 to match retail pcaps. We save it as layer 1 to make EF Core happy.
                Player.Session.Network.EnqueueSend(new GameEventMagicRemoveEnchantment(Player.Session, (ushort)entry.SpellId, layer));

                if (sound && entry.SpellCategory != (SpellCategory)SpellCategory_Cooldown)
                    Player.Session.Network.EnqueueSend(new GameMessageSound(Player.Guid, Sound.SpellExpire, 1.0f));
            }
            else
            {
                var ownerID = WorldObject.OwnerId ?? WorldObject.WielderId;

                if (ownerID != null)
                {
                    var owner = PlayerManager.GetOnlinePlayer((uint)ownerID);

                    if (owner != null)
                    {
                        var spell = new Spell(spellID);

                        owner.Session.Network.EnqueueSend(new GameMessageSystemChat($"The spell {spell.Name} on {WorldObject.Name} has expired.", ChatMessageType.Magic));

                        if (sound)
                            owner.Session.Network.EnqueueSend(new GameMessageSound(owner.Guid, Sound.SpellExpire, 1.0f));
                    }
                }
            }
        }

        /// <summary>
        /// Removes all enchantments except for vitae and item spells
        /// Called on player death
        /// </summary>
        public virtual void RemoveAllEnchantments()
        {
            // exclude cooldowns and enchantments from items
            var spellsToExclude = WorldObject.Biota.PropertiesEnchantmentRegistry.Clone(WorldObject.BiotaDatabaseLock).Where(i => i.Duration == -1 || i.SpellId > short.MaxValue).Select(i => i.SpellId);

            WorldObject.Biota.PropertiesEnchantmentRegistry.RemoveAllEnchantments(spellsToExclude, WorldObject.BiotaDatabaseLock);
            WorldObject.ChangesDetected = true;
        }

        /// <summary>
        /// Returns the vitae enchantment
        /// </summary>
        public PropertiesEnchantmentRegistry GetVitae()
        {
            return GetEnchantment((uint)SpellId.Vitae);
        }

        /// <summary>
        /// Returns the minimum vitae for a player level
        /// </summary>
        public float GetMinVitae(uint level)
        {
            var propVitae = 1.0 - PropertyManager.GetDouble("vitae_penalty_max").Item;

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
        /// Called on player death
        /// </summary>
        public virtual float UpdateVitae()
        {
            if (Player == null) return 0;
            PropertiesEnchantmentRegistry vitae;

            if (!HasVitae)
            {
                // TODO refactor this so it uses the existing Add() method.

                // add entry for new vitae
                var spell = new Spell(SpellId.Vitae);

                vitae = BuildEntry(spell);
                vitae.EnchantmentCategory = (uint)EnchantmentMask.Vitae;
                vitae.LayerId = 1; // This should be 0 but EF Core seems to be very unhappy with 0 as the layer id now that we're using layer as part of the composite key.
                vitae.StatModValue = 1.0f - (float)PropertyManager.GetDouble("vitae_penalty").Item;
                WorldObject.Biota.PropertiesEnchantmentRegistry.AddEnchantment(vitae, WorldObject.BiotaDatabaseLock);
                WorldObject.ChangesDetected = true;
            }
            else
            {
                // update existing vitae
                vitae = GetVitae();
                vitae.StatModValue -= (float)PropertyManager.GetDouble("vitae_penalty").Item;
                WorldObject.ChangesDetected = true;
            }

            var minVitae = GetMinVitae((uint)Player.Level);

            if (vitae.StatModValue < minVitae)
                vitae.StatModValue = minVitae;
            if (vitae.StatModValue > 1.0f)
                vitae.StatModValue = 1.0f;

            return vitae.StatModValue;
        }

        /// <summary>
        /// Called when player crosses the VitaeCPPool threshold
        /// </summary>
        public virtual float ReduceVitae()
        {
            var vitae = GetVitae();
            vitae.StatModValue += 0.01f;

            if (vitae.StatModValue.EpsilonEquals(1.0f) || vitae.StatModValue > 1.0f)
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

            var vitae = GetVitae();

            Remove(vitae);
        }


        /// <summary>
        /// Silently removes a spell from the enchantment registry, and sends the relevant network message for dispel
        /// </summary>
        public virtual void Dispel(PropertiesEnchantmentRegistry entry)
        {
            if (entry == null)
                return;

            var spellID = entry.SpellId;

            if (WorldObject.Biota.PropertiesEnchantmentRegistry.TryRemoveEnchantment(entry.SpellId, entry.CasterObjectId, WorldObject.BiotaDatabaseLock))
                WorldObject.ChangesDetected = true;

            if (Player != null)
                Player.Session.Network.EnqueueSend(new GameEventMagicDispelEnchantment(Player.Session, (ushort)entry.SpellId, entry.LayerId));
        }

        /// <summary>
        /// Silently removes multiple spells from the enchantment registry, and sends the relevent network messages for dispel
        /// </summary>
        public virtual void Dispel(List<PropertiesEnchantmentRegistry> entries)
        {
            if (entries == null || entries.Count == 0)
                return;

            foreach (var entry in entries)
            {
                if (WorldObject.Biota.PropertiesEnchantmentRegistry.TryRemoveEnchantment(entry.SpellId, entry.CasterObjectId, WorldObject.BiotaDatabaseLock))
                    WorldObject.ChangesDetected = true;
            }
            if (Player != null)
                Player.Session.Network.EnqueueSend(new GameEventMagicDispelMultipleEnchantments(Player.Session, entries));
        }

        /// <summary>
        /// Removes all enchantments from the player on server, and sends network messages to silently dispel the enchantments
        /// </summary>
        public void DispelAllEnchantments()
        {
            var enchantments = WorldObject.Biota.PropertiesEnchantmentRegistry.Clone(WorldObject.BiotaDatabaseLock);

            Dispel(enchantments);
        }

        /// <summary>
        /// Selects a list of spells to dispel
        /// </summary>
        /// <param name="spell">The dispel spell</param>
        public List<SpellEnchantment> SelectDispel(Spell spell)
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
            // number_variance - number * number_variance = the minimum # of spells to dispel
            var minPower = spell.MinPower;
            var maxPower = spell.MaxPower;
            var powerVariance = spell.PowerVariance;
            var dispelSchool = spell.DispelSchool;
            var align = spell.Align;
            var number = spell.Number;
            var numberVariance = spell.NumberVariance;

            //var enchantments = GetEnchantments_TopLayer(WorldObject.Biota.GetEnchantments(WorldObject.BiotaDatabaseLock));
            var enchantments = WorldObject.Biota.PropertiesEnchantmentRegistry.Clone(WorldObject.BiotaDatabaseLock);

            var filtered = enchantments.Where(e => e.PowerLevel <= maxPower);

            // no dispel for enchantments from item sources (and vitae)
            filtered = filtered.Where(e => e.Duration != -1);

            // for dispelSchool and align,
            // we probably could do some calculations to figure out these values directly from the enchantments
            // but it would be far easier and more reliable to just do them through the spells
            // since dispels are not a time-critical function, this should still be fine
            var spells = new List<SpellEnchantment>();
            foreach (var filter in filtered)
            {
                var spellEnchantment = new SpellEnchantment(filter);

                if (!spellEnchantment.Spell.NotFound)
                    spells.Add(spellEnchantment);
            }

            var filterSpells = spells;
            if (dispelSchool != MagicSchool.None)
                filterSpells = filterSpells.Where(s => s.Spell != null && s.Spell.School == dispelSchool).ToList();

            if (align != DispelType.All)
            {
                if (align == DispelType.Positive)
                    filterSpells = filterSpells.Where(s => s.Spell != null && s.Spell.IsBeneficial).ToList();
                else if (align == DispelType.Negative)
                    filterSpells = filterSpells.Where(s => s.Spell != null && s.Spell.IsHarmful).ToList();
            }

            // dispel all
            if (number == -1)
                return filterSpells;

            // get number of spells to dispel
            var dispelNum = number;
            if (numberVariance != 1.0f)
            {
                var maxDispelNum = dispelNum;
                var minDispelNum = (int)Math.Round(dispelNum * (1.0f - numberVariance));

                // factor in rng variance
                dispelNum = ThreadSafeRandom.Next(minDispelNum, maxDispelNum);
            }

            // randomize the filtered spell list
            filterSpells.Shuffle();

            // select the required # of spells
            return filterSpells.Take(dispelNum).ToList();
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
        /// Returns the bonus to an attribute from enchantments
        /// </summary>
        public virtual int GetAttributeMod(PropertyAttribute attribute)
        {
            var enchantments = GetEnchantments_TopLayer(EnchantmentTypeFlags.Attribute, (uint)attribute, true);

            var attributeMod = 0;
            foreach (var enchantment in enchantments)
                attributeMod += (int)enchantment.StatModValue;

            return attributeMod;
        }

        /// <summary>
        /// Gets the direct modifiers to a vital / secondary attribute
        /// </summary>
        public virtual float GetVitalMod_Additives(CreatureVital vital)
        {
            var typeFlags = EnchantmentTypeFlags.SecondAtt | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)vital.Vital);

            // additive
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        public virtual float GetVitalMod_Multiplier(CreatureVital vital)
        {
            // multiplicatives (asheron's lesser benediction)
            var typeFlags = EnchantmentTypeFlags.SecondAtt | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)vital.Vital);

            var multiplier = 1.0f;
            foreach (var enchantment in enchantments)
                multiplier *= enchantment.StatModValue;

            return multiplier;
        }

        /// <summary>
        /// Returns the additive bonus from XP enchantments, such as Augmented Understanding
        /// </summary>
        public virtual float GetXPBonus()
        {
            var enchantments = GetEnchantments(SpellCategory.TrinketXPRaising);

            // TODO: temporary code to handle both additive and multiplicative mods
            // should be additive in database, update when everything is in sync
            var modifier = 0.0f;

            foreach (var enchantment in enchantments)
            {
                if (enchantment.StatModType.HasFlag(EnchantmentTypeFlags.Multiplicative))
                    modifier += enchantment.StatModValue - 1.0f;
                else
                    modifier += enchantment.StatModValue;
            }
            return modifier;
        }

        /// <summary>
        /// Returns the bonus to a skill from enchantments
        /// </summary>
        public virtual int GetSkillMod(Skill skill)
        {
            var enchantments = GetEnchantments_TopLayer(EnchantmentTypeFlags.Skill, (uint)skill, true);

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
        /// Returns the sum of the StatModValues for an EnchantmentTypeFlag
        /// </summary>
        public int GetModifier(EnchantmentTypeFlags type, bool? positive = null)
        {
            var enchantments = GetEnchantments_TopLayer(type);

            var modifier = 0;
            foreach (var enchantment in enchantments)
            {
                var statModVal = (int)enchantment.StatModValue;

                if (positive == null || positive.Value && statModVal > 0 || !positive.Value && statModVal < 0)
                {
                    modifier += statModVal;
                }
            }
            return modifier;
        }

        /// <summary>
        /// Returns the sum of the modifiers for a StatModKey
        /// </summary>
        public int GetAdditiveMod(PropertyInt statModKey)
        {
            var enchantments = GetEnchantments_TopLayer(EnchantmentTypeFlags.Additive, (uint)statModKey);

            var modifier = 0;
            foreach (var enchantment in enchantments.Where(e => ((EnchantmentTypeFlags)e.StatModType & EnchantmentTypeFlags.Skill) == 0))
                modifier += (int)enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the sum of the enchantment statmod values
        /// </summary>
        public float GetAdditiveMod(List<PropertiesEnchantmentRegistry> enchantments)
        {
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        public float GetAdditiveMod(PropertyFloat statModKey)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;

            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)statModKey);

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
            var enchantments = GetEnchantments_TopLayer(EnchantmentTypeFlags.Multiplicative, (uint)statModKey);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }


        /// <summary>
        /// Returns the base armor modifier from enchantments
        /// </summary>
        public virtual int GetBodyArmorMod()
        {
            return GetModifier(EnchantmentTypeFlags.BodyArmorValue);
        }

        /// <summary>
        /// Returns either the positive body armor from life spells (ie. Armor Self)
        /// or the negative body armor (ie. Imperil)
        /// </summary>
        public virtual int GetBodyArmorMod(bool positive)
        {
            return GetModifier(EnchantmentTypeFlags.BodyArmorValue, positive);
        }

        /// <summary>
        /// Gets the resistance modifier for a damage type
        /// </summary>
        public virtual float GetResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)resistance);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Gets the resistance modifier for a damage type
        /// </summary>
        public virtual float GetProtectionResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)resistance);

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
        public virtual float GetVulnerabilityResistanceMod(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var resistance = GetResistanceKey(damageType);
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)resistance);

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
        public virtual float GetRegenerationMod(CreatureVital vital)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Multiplicative;
            var vitalKey = GetVitalRateKey(vital);
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)vitalKey);

            // multiplicative
            var modifier = 1.0f;
            foreach (var enchantment in enchantments)
                modifier *= enchantment.StatModValue;

            return modifier;
        }


        /// <summary>
        /// Returns the weapon damage bonus, ie. Blood Drinker
        /// </summary>
        public virtual int GetDamageBonus()
        {
            var damageMod = GetAdditiveMod(PropertyInt.Damage);
            var auraDamageMod = GetAdditiveMod(PropertyInt.WeaponAuraDamage);

            // there is an unfortunate situation in the spell db,
            // where blood drinker 1-7 are defined as PropertyInt.Damage
            // (possibly from also being cast as direct item spells elsewhere?)
            // and blood drinker 8 is properly defined as aura...

            /*if (WorldObject is Creature && auraDamageMod != 0)
                return auraDamageMod;
            else
                return damageMod;*/

            return auraDamageMod + damageMod;
        }

        /// <summary>
        /// Returns the DamageMod for bow / crossbow
        /// </summary>
        public virtual float GetDamageMod()
        {
            return GetAdditiveMod(PropertyFloat.DamageMod);
        }

        /// <summary>
        /// Returns the attack skill modifier, ie. Heart Seeker
        /// </summary>
        public virtual float GetAttackMod()
        {
            var offenseMod = GetAdditiveMod(PropertyFloat.WeaponOffense);
            var auraOffenseMod = GetAdditiveMod(PropertyFloat.WeaponAuraOffense);

            /*if (WorldObject is Creature && auraOffenseMod != 0)
                return auraOffenseMod;
            else
                return offenseMod;*/

            return auraOffenseMod + offenseMod;
        }

        /// <summary>
        /// Returns the weapon speed modifier, ie. Swift Killer
        /// </summary>
        public virtual int GetWeaponSpeedMod()
        {
            var speedMod = GetAdditiveMod(PropertyInt.WeaponTime);
            var auraSpeedMod = GetAdditiveMod(PropertyInt.WeaponAuraSpeed);

            /*if (WorldObject is Creature && auraSpeedMod != 0)
                return auraSpeedMod;
            else
                return speedMod;*/

            return auraSpeedMod + speedMod;
        }

        /// <summary>
        /// Returns the defense skill modifier, ie. Defender
        /// </summary>
        public virtual float GetDefenseMod()
        {
            var defenseMod = GetAdditiveMod(PropertyFloat.WeaponDefense);
            var auraDefenseMod = GetAdditiveMod(PropertyFloat.WeaponAuraDefense);

            /*if (WorldObject is Creature && auraDefenseMod != 0)
                return auraDefenseMod;
            else
                return defenseMod;*/

            return auraDefenseMod + defenseMod;
        }

        /// <summary>
        /// Returns the mana conversion bonus modifier, ie. Hermetic Link / Void
        /// </summary>
        public virtual float GetManaConvMod()
        {
            var manaConvMod = GetMultiplicativeMod(PropertyFloat.ManaConversionMod);
            var manaConvAuraMod = GetMultiplicativeMod(PropertyFloat.WeaponAuraManaConv);

            /*if (WorldObject is Creature && manaConvAuraMod != 1.0f)
                return manaConvAuraMod;
            else
                return manaConvMod;*/

            return manaConvAuraMod * manaConvMod;
        }

        /// <summary>
        /// Returns the elemental damage bonus modifier, ie. Spirit Drinker / Loather
        /// </summary>
        public virtual float GetElementalDamageMod()
        {
            var elementalDamageMod = GetAdditiveMod(PropertyFloat.ElementalDamageMod);
            var elementalDamageAuraMod = GetAdditiveMod(PropertyFloat.WeaponAuraElemental);

            /*if (WorldObject is Creature && elementalDamageAuraMod != 0)
                return elementalDamageAuraMod;
            else
                return elementalDamageMod;*/

            return elementalDamageAuraMod + elementalDamageMod;
        }

        /// <summary>
        /// Returns the weapon damage variance modifier
        /// </summary>
        public virtual float GetVarianceMod()
        {
            return GetMultiplicativeMod(PropertyFloat.DamageVariance);
        }

        /// <summary>
        /// Returns the additive armor level modifier, ie. Impenetrability
        /// </summary>
        public virtual int GetArmorMod()
        {
            return GetAdditiveMod(PropertyInt.ArmorLevel);
        }

        /// <summary>
        /// Gets the additive armor level vs type modifier, ie. banes
        /// </summary>
        public virtual float GetArmorModVsType(DamageType damageType)
        {
            var typeFlags = EnchantmentTypeFlags.Float | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var key = GetImpenBaneKey(damageType);
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)key);

            // additive
            var modifier = 0.0f;
            foreach (var enchantment in enchantments)
                modifier += enchantment.StatModValue;

            return modifier;
        }

        /// <summary>
        /// Returns the defense skill debuffs for Dirty Fighting
        /// </summary>
        public int GetDefenseDebuffMod()
        {
            var typeFlags = EnchantmentTypeFlags.Skill | EnchantmentTypeFlags.Additive | EnchantmentTypeFlags.DefenseSkills;
            var enchantments = GetEnchantments_TopLayer(typeFlags, 0);

            // additive
            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        /// <summary>
        /// Returns the attack skill debuffs for Dirty Fighting
        /// </summary>
        public int GetAttackDebuffMod()
        {
            var typeFlags = EnchantmentTypeFlags.Skill | EnchantmentTypeFlags.Additive | EnchantmentTypeFlags.AttackSkills;
            var enchantments = GetEnchantments_TopLayer(typeFlags, 0);

            // additive
            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        /// <summary>
        /// Returns the ResistLockpick enchantment additives, ie. Strengthen/Weaken Lock
        /// </summary>
        /// <returns></returns>
        public virtual int GetResistLockpick()
        {
            return GetAdditiveMod(PropertyInt.ResistLockpick);
        }


        /// <summary>
        /// Returns a rating enchantment modifier
        /// </summary>
        /// <param name="property">The rating to return an enchantment modifier</param>
        public virtual int GetRating(PropertyInt property)
        {
            var typeFlags = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)property);

            return (int)Math.Round(GetAdditiveMod(enchantments));
        }

        public virtual int GetNetherDotDamageRating()
        {
            var type = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var netherDots = GetEnchantments_TopLayer(type, (uint)PropertyInt.NetherOverTime);

            // this function produces a similar value to the original ACE function,
            // but is using the actual retail calculation method
            var totalBaseDamage = 0.0f;
            foreach (var netherDot in netherDots)
            {
                // normally we could just use netherDot.StatModValue here,
                // but in case WorldObject has a non-default HeartbeatInterval,
                // we want this value to still be based on the damage per default heartbeat interval
                totalBaseDamage += GetDamagePerTick(netherDot, 5.0);
            }
            var rating = (int)Math.Round(totalBaseDamage / 8.0f);   // thanks to Xenocide for this formula!
            //Console.WriteLine($"{WorldObject.Name}.NetherDotDamageRating: {rating}");
            return rating;
        }

        /// <summary>
        ///  Returns the damage over time (DoT) enchantment mod
        /// </summary>
        public float GetDamageOverTimeMod()
        {
            var typeFlags = EnchantmentTypeFlags.Int | EnchantmentTypeFlags.SingleStat | EnchantmentTypeFlags.Additive;
            var enchantments = GetEnchantments_TopLayer(typeFlags, (uint)PropertyInt.DamageOverTime);

            // additive float
            return GetAdditiveMod(enchantments);
        }

        public static ushort SpellCategory_Cooldown = 0x8000;

        /// <summary>
        /// Adds 0x8000 to the sharedCooldownID
        /// </summary>
        public uint GetCooldownSpellID(int sharedCooldownID)
        {
            return (uint)(SpellCategory_Cooldown | sharedCooldownID);
        }

        /// <summary>
        /// Returns the seconds until this item's cooldown expires
        /// </summary>
        public float GetCooldown(int sharedCooldownID)
        {
            var cooldownSpellID = GetCooldownSpellID(sharedCooldownID);

            var cooldown = GetEnchantment(cooldownSpellID);

            if (cooldown != null)
                return (float)(cooldown.Duration - Math.Abs(cooldown.StartTime));
            else
                return 0.0f;
        }

        /// <summary>
        /// Returns TRUE if this item can be activated at this time
        /// </summary>
        public bool CheckCooldown(int? sharedCooldownID)
        {
            if (sharedCooldownID == null)
                return true;

            return GetCooldown(sharedCooldownID.Value) == 0.0f;
        }

        /// <summary>
        /// Called every ~5 seconds for active object
        /// </summary>
        public void HeartBeat(double heartbeatInterval)
        {
            var topLayerEnchantments = WorldObject.Biota.PropertiesEnchantmentRegistry.GetEnchantmentsTopLayer(WorldObject.BiotaDatabaseLock);

            HeartBeat_DamageOverTime(topLayerEnchantments);

            var expired = WorldObject.Biota.PropertiesEnchantmentRegistry.HeartBeatEnchantmentsAndReturnExpired(heartbeatInterval, WorldObject.BiotaDatabaseLock);

            foreach (var enchantment in expired)
                Remove(enchantment);
        }

        /// <summary>
        /// Applies damage from DoTs every ~5 seconds
        /// </summary>
        /// <param name="enchantments">A list of active enchantments at the top layers</param>
        public void HeartBeat_DamageOverTime(List<PropertiesEnchantmentRegistry> enchantments)
        {
            var dots = new List<PropertiesEnchantmentRegistry>();
            var netherDots = new List<PropertiesEnchantmentRegistry>();
            var aetheriaDots = new List<PropertiesEnchantmentRegistry>();
            var heals = new List<PropertiesEnchantmentRegistry>();

            foreach (var enchantment in enchantments)
            {
                // combine DoTs from multiple sources
                if (enchantment.StatModKey == (int)PropertyInt.DamageOverTime)
                {
                    if (enchantment.SpellCategory == SpellCategory.AetheriaProcDamageOverTimeRaising)
                        aetheriaDots.Add(enchantment);
                    else
                        dots.Add(enchantment);
                }
                else if (enchantment.StatModKey == (int)PropertyInt.NetherOverTime)
                    netherDots.Add(enchantment);

                else if (enchantment.StatModKey == (int)PropertyInt.HealOverTime)
                    heals.Add(enchantment);
            }

            // apply damage over time (DoTs)
            if (dots.Count > 0)
                ApplyDamageTick(dots, DamageType.Undef);

            if (netherDots.Count > 0)
                ApplyDamageTick(netherDots, DamageType.Nether);

            if (aetheriaDots.Count > 0)
                ApplyDamageTick(aetheriaDots, DamageType.Undef, true);

            // apply healing over time (HoTs)
            if (heals.Count > 0)
                ApplyHealingTick(heals);
        }

        public void ApplyHealingTick(List<PropertiesEnchantmentRegistry> enchantments)
        {
            var creature = WorldObject as Creature;
            if (creature == null || creature.IsDead) return;

            // get the total tick amount
            var tickAmountTotal = 0.0f;
            foreach (var enchantment in enchantments)
            {
                //var totalAmount = enchantment.StatModValue;
                //var totalTicks = GetNumTicks(enchantment);
                var tickAmount = enchantment.StatModValue;

                tickAmountTotal += tickAmount;
            }

            // apply healing ratings?
            tickAmountTotal *= creature.GetHealingRatingMod();

            // do healing
            var healAmount = creature.UpdateVitalDelta(creature.Health, (int)Math.Round(tickAmountTotal));
            creature.DamageHistory.OnHeal((uint)healAmount);

            if (creature is Player player)
                player.SendMessage($"You receive {healAmount} points of periodic healing.", PropertyManager.GetBool("aetheria_heal_color").Item ? ChatMessageType.Broadcast : ChatMessageType.Combat);
        }

        /// <summary>
        /// Applies 1 tick of damage from a DoT spell
        /// </summary>
        /// <param name="enchantments">The damage over time (DoT) spells</param>
        public void ApplyDamageTick(List<PropertiesEnchantmentRegistry> enchantments, DamageType damageType, bool aetheria = false)
        {
            var creature = WorldObject as Creature;
            if (creature == null || creature.IsDead) return;

            bool isDead = false;
            var damagers = new Dictionary<WorldObject, float>();

            var targetPlayer = WorldObject as Player;

            // get the total tick amount
            var tickAmountTotal = 0.0f;
            foreach (var enchantment in enchantments)
            {
                //var totalAmount = enchantment.StatModValue;
                //var totalTicks = GetNumTicks(enchantment);
                var tickAmount = enchantment.StatModValue;

                // run tick amount through damage calculation functions?
                // it appears retail might have done an initial damage calc,
                // and then applied that to the enchantment StatModVal beforehand
                // for each damage tick, this pre-calc would then be multiplied
                // against the realtime resistances

                var damager = WorldObject.CurrentLandblock?.GetObject(enchantment.CasterObjectId) as Creature;
                if (damager == null)
                {
                    Console.WriteLine($"{WorldObject.Name}.ApplyDamageTick() - couldn't find damager {enchantment.CasterObjectId:X8}");
                    continue;
                }

                // if a PKType with Enduring Enchantment has died, ensure they don't continue to take DoT from PK sources
                if (targetPlayer != null && damager is Player && !targetPlayer.IsPKType)
                    continue;

                var resistanceMod = creature.GetResistanceMod(damageType, damager, null);

                // with the halvening, this actually seems like the fairest balance currently..
                var useNetherDotDamageRating = targetPlayer != null;

                var damageResistRatingMod = creature.GetDamageResistRatingMod(CombatType.Magic, useNetherDotDamageRating);   // df?

                var dotResistRatingMod = Creature.GetNegativeRatingMod(creature.GetDotResistanceRating());  // should this be here, or somewhere else?
                                                                                                            // should this affect NetherDotDamageRating?

                //Console.WriteLine("DR: " + Creature.ModToRating(damageRatingMod));
                //Console.WriteLine("DRR: " + Creature.NegativeModToRating(damageResistRatingMod));
                //Console.WriteLine("NRR: " + Creature.NegativeModToRating(netherResistRatingMod));

                tickAmount *= resistanceMod * damageResistRatingMod * dotResistRatingMod;

                // make sure the target's current health is not exceeded
                if (tickAmountTotal + tickAmount >= creature.Health.Current)
                {
                    tickAmount = creature.Health.Current - tickAmountTotal;
                    isDead = true;
                }

                if (damagers.ContainsKey(damager))
                    damagers[damager] += tickAmount;
                else
                    damagers.Add(damager, tickAmount);

                creature.DamageHistory.Add(damager, damageType, (uint)Math.Round(tickAmount));

                tickAmountTotal += tickAmount;

                if (isDead) break;
            }

            creature.TakeDamageOverTime(tickAmountTotal, damageType);

            if (!creature.IsAlive) return;

            foreach (var kvp in damagers)
            {
                var damager = kvp.Key;
                var amount = kvp.Value;

                if (creature.Invincible)
                    amount = 0;

                var damageSourcePlayer = damager as Player;
                if (damageSourcePlayer != null)
                {
                    creature.TakeDamageOverTime_NotifySource(damageSourcePlayer, damageType, amount, aetheria);

                    if (creature.IsAlive)
                        creature.EmoteManager.OnDamage(damageSourcePlayer);
                }
            }
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
        /// Returns the number of ticks for a DoT enchantment
        /// </summary>
        public int GetNumTicks(PropertiesEnchantmentRegistry enchantment, double? heartbeatInterval = null)
        {
            // assumed to be DoT enchantment

            if (heartbeatInterval == null)
                heartbeatInterval = WorldObject.HeartbeatInterval ?? 5.0;

            // it's possible retail had a separate ticking mechanism for these,
            // that ensured ticks every 5s, instead of heartbeat intervals
            return (int)Math.Ceiling(enchantment.Duration / heartbeatInterval.Value);
        }

        /// <summary>
        /// Returns the total damage for a DoT enchantment
        /// </summary>
        public float GetTotalDamage(PropertiesEnchantmentRegistry enchantment)
        {
            // assumed to be DoT enchantment
            return enchantment.StatModValue * GetNumTicks(enchantment);
        }

        public float GetDamagePerTick(PropertiesEnchantmentRegistry enchantment, double? heartbeatInterval = null)
        {
            // assumed to be DoT enchantment

            var creatureHeartbeatInterval = WorldObject.HeartbeatInterval ?? 5.0;

            if (heartbeatInterval == null)
                heartbeatInterval = creatureHeartbeatInterval;

            if (heartbeatInterval == creatureHeartbeatInterval)
                return enchantment.StatModValue;

            // calculate the total damage w/ creature heartbeat interval
            var totalDamage = GetTotalDamage(enchantment);

            // divide totalDamage by the requested tick interval
            var numTicks = GetNumTicks(enchantment, heartbeatInterval);

            return totalDamage / numTicks;
        }
    }
}
