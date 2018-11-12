using System;
using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Managers
{
    public class EnchantmentManagerWithCaching : EnchantmentManager
    {
        /// <summary>
        /// Constructs a new EnchantmentManager for a WorldObject
        /// </summary>
        public EnchantmentManagerWithCaching(WorldObject obj) : base(obj)
        {
        }

        /// <summary>
        /// Add/update an enchantment in this object's registry
        /// </summary>
        public override (StackType stackType, Spell surpass) Add(Enchantment enchantment, WorldObject caster)
        {
            var result = base.Add(enchantment, caster);

            ClearCache();

            return result;
        }

        /// <summary>
        /// Removes a spell from the enchantment registry, and
        /// sends the relevant network messages for spell removal
        /// </summary>
        public override void Remove(BiotaPropertiesEnchantmentRegistry entry, bool sound = true)
        {
            base.Remove(entry, sound);

            ClearCache();
        }

        /// <summary>
        /// Removes all enchantments except for vitae
        /// Called on player death
        /// </summary>
        public override void RemoveAllEnchantments()
        {
            base.RemoveAllEnchantments();

            ClearCache();
        }


        /// <summary>
        /// Called on player death
        /// </summary>
        public override float UpdateVitae()
        {
            var result = base.UpdateVitae();

            ClearCache();

            return result;
        }

        /// <summary>
        /// Called when player crosses the VitaeCPPool threshold
        /// </summary>
        public override float ReduceVitae()
        {
            var result = base.ReduceVitae();

            ClearCache();

            return result;
        }


        /// <summary>
        /// Silently removes a spell from the enchantment registry, and sends the relevant network message for dispel
        /// </summary>
        public override void Dispel(BiotaPropertiesEnchantmentRegistry entry)
        {
            base.Dispel(entry);

            ClearCache();
        }

        /// <summary>
        /// Silently removes multiple spells from the enchantment registry, and sends the relevent network messages for dispel
        /// </summary>
        public override void Dispel(List<BiotaPropertiesEnchantmentRegistry> entries)
        {
            base.Dispel(entries);

            ClearCache();
        }


        private void ClearCache()
        {
            attributeModCache.Clear();
            vitalModCache.Clear();
            skillModCache.Clear();
            regenerationModCache = null;
            defenseModCache = null;
            armorModCache = null;
            armorModVsTypeModCache.Clear();
        }

        private readonly Dictionary<PropertyAttribute, int> attributeModCache = new Dictionary<PropertyAttribute, int>();

        /// <summary>
        /// Returns the bonus to an attribute from enchantments
        /// </summary>
        public override int GetAttributeMod(PropertyAttribute attribute)
        {
            if (attributeModCache.TryGetValue(attribute, out var value))
                return value;

            value = base.GetAttributeMod(attribute);

            attributeModCache[attribute] = value;

            return value;
        }

        private readonly Dictionary<CreatureVital, float> vitalModCache = new Dictionary<CreatureVital, float>();

        /// <summary>
        /// Gets the direct modifiers to a vital / secondary attribute
        /// </summary>
        public override float GetVitalMod(CreatureVital vital)
        {
            if (vitalModCache.TryGetValue(vital, out var value))
                return value;

            value = base.GetVitalMod(vital);

            vitalModCache[vital] = value;

            return value;
        }

        private readonly Dictionary<Skill, int> skillModCache = new Dictionary<Skill, int>();

        /// <summary>
        /// Returns the bonus to a skill from enchantments
        /// </summary>
        public override int GetSkillMod(Skill skill)
        {
            if (skillModCache.TryGetValue(skill, out var value))
                return value;

            value = base.GetSkillMod(skill);

            skillModCache[skill] = value;

            return value;
        }

        private float? regenerationModCache;

        /// <summary>
        /// Gets the regeneration modifier for a vital type
        /// (regeneration / rejuvenation / mana renewal)
        /// </summary>
        public override float GetRegenerationMod(CreatureVital vital)
        {
            if (regenerationModCache.HasValue)
                return regenerationModCache.Value;

            regenerationModCache = base.GetRegenerationMod(vital);

            return regenerationModCache.Value;
        }

        private float? defenseModCache;
        /// <summary>
        /// Returns the defense skill modifier, ie. Defender
        /// </summary>
        public override float GetDefenseMod()
        {
            if (defenseModCache.HasValue)
                return defenseModCache.Value;

            defenseModCache = base.GetDefenseMod();

            return defenseModCache.Value;
        }

        private int? armorModCache;

        /// <summary>
        /// Returns the additive armor level modifier, ie. Impenetrability
        /// </summary>
        public override int GetArmorMod()
        {
            if (armorModCache.HasValue)
                return armorModCache.Value;

            armorModCache = base.GetArmorMod();

            return armorModCache.Value;
        }

        private readonly Dictionary<DamageType, float> armorModVsTypeModCache = new Dictionary<DamageType, float>();

        /// <summary>
        /// Gets the additive armor level vs type modifier, ie. banes
        /// </summary>
        public override float GetArmorModVsType(DamageType damageType)
        {
            if (armorModVsTypeModCache.TryGetValue(damageType, out var value))
                return value;

            value = base.GetArmorModVsType(damageType);

            armorModVsTypeModCache[damageType] = value;

            return value;
        }
    }
}
