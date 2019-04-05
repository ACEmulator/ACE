using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class Creature_BodyPart
    {
        public Creature Creature;
        public BiotaPropertiesBodyPart Biota;

        public float WeaponArmorMod = 1.0f;
        public float WeaponResistanceMod = 1.0f;        // resistance cleaving, rends

        public bool IgnoreMagicArmor;   // impen, bane
        public bool IgnoreMagicResist;  // armor, protection

        public EnchantmentManager EnchantmentManager => Creature.EnchantmentManager;

        public Creature_BodyPart(Creature creature, BiotaPropertiesBodyPart biota, bool ignoreMagicArmor = false, bool ignoreMagicResist = false)
        {
            Creature = creature;
            Biota = biota;

            IgnoreMagicArmor = ignoreMagicArmor;
            IgnoreMagicResist = ignoreMagicResist;
        }

        public int BaseArmorMod
        {
            get
            {
                var armorMod = IgnoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod();

                return (int)Math.Round((Biota.BaseArmor + armorMod) * WeaponArmorMod);
            }
        }
        
        public int ArmorVsSlash => GetArmorVsType(DamageType.Slash, Biota.ArmorVsSlash);
        public int ArmorVsPierce => GetArmorVsType(DamageType.Pierce, Biota.ArmorVsPierce);
        public int ArmorVsBludgeon => GetArmorVsType(DamageType.Bludgeon, Biota.ArmorVsBludgeon);
        public int ArmorVsFire => GetArmorVsType(DamageType.Fire, Biota.ArmorVsFire);
        public int ArmorVsCold => GetArmorVsType(DamageType.Cold, Biota.ArmorVsCold);
        public int ArmorVsAcid => GetArmorVsType(DamageType.Acid, Biota.ArmorVsAcid);
        public int ArmorVsElectric => GetArmorVsType(DamageType.Electric, Biota.ArmorVsElectric);
        public int ArmorVsNether => GetArmorVsType(DamageType.Nether, Biota.ArmorVsNether);

        public int GetArmorVsType(DamageType damageType, int armorVsType)
        {
            // TODO: refactor this class
            var preScaled = (float)BaseArmorMod / Biota.BaseArmor;

            //var resistance = (float)armorVsType / Biota.BaseArmor;
            var resistance = (float)armorVsType / Biota.BaseArmor * preScaled;
            if (double.IsNaN(resistance))
                resistance = 1.0f;

            float mod;
            var spellVuln = IgnoreMagicResist ? 1.0f : EnchantmentManager.GetVulnerabilityResistanceMod(damageType);
            var spellProt = IgnoreMagicResist ? 1.0f : EnchantmentManager.GetProtectionResistanceMod(damageType);

            if (WeaponResistanceMod > spellVuln)
                mod = WeaponResistanceMod * spellProt;
            else
                mod = spellVuln * spellProt;

            var baseArmorMod = BaseArmorMod;

            var resistanceMod = resistance / mod;
            if (baseArmorMod < 0)
                resistanceMod = 1.0f + (1.0f - resistanceMod);

            /*Console.WriteLine("BaseArmor: " + Biota.BaseArmor);
            Console.WriteLine("BaseArmorMod: " + baseArmorMod);
            Console.WriteLine("Resistance: " + resistance);
            Console.WriteLine("ResistanceMod: " + resistanceMod);*/

            return (int)Math.Round(baseArmorMod * resistanceMod);
        }

        /// <summary>
        /// Main entry point for getting the armor mod
        /// </summary>
        public float GetArmorMod(DamageType damageType, List<WorldObject> armorLayers, WorldObject damageSource, float armorRendingMod = 1.0f)
        {
            var effectiveArmorVsType = GetEffectiveArmorVsType(damageType, armorLayers, damageSource, armorRendingMod);

            return SkillFormula.CalcArmorMod(effectiveArmorVsType);
        }

        public float GetEffectiveArmorVsType(DamageType damageType, List<WorldObject> armorLayers, WorldObject damageSource, float armorRendingMod = 1.0f)
        {
            var ignoreMagicArmor  = damageSource != null ? damageSource.IgnoreMagicArmor : false;
            var ignoreMagicResist = damageSource != null ? damageSource.IgnoreMagicResist : false;

            // get base AL / RL
            var enchantmentMod = ignoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod();

            var baseArmorMod = (float)(Biota.BaseArmor + enchantmentMod);

            // handle armor rending mod here?
            //if (baseArmorMod > 0)
                //baseArmorMod *= armorRendingMod;

            // for creatures, can this be modified via enchantments?
            var armorVsType = Creature.GetArmorVsType(damageType);

            // handle negative baseArmorMod?
            if (baseArmorMod < 0)
                armorVsType = 1.0f + (1.0f - armorVsType);

            var effectiveAL = (float)(baseArmorMod * armorVsType);

            // handle monsters w/ multiple layers of armor
            foreach (var armorLayer in armorLayers)
                effectiveAL += GetArmorMod(armorLayer, damageSource, damageType);

            // Armor Rending reduces physical armor too?
            if (effectiveAL > 0)
                effectiveAL *= armorRendingMod;

            return effectiveAL;
        }

        public List<WorldObject> GetArmorLayers(CombatBodyPart bodyPart)
        {
            var coverageMask = BodyParts.GetCoverageMask(bodyPart);

            var equipped = Creature.EquippedObjects.Values.Where(e => e is Clothing && (e.ClothingPriority & coverageMask) != 0).ToList();

            return equipped;
        }

        /// <summary>
        /// Returns the effective AL for 1 piece of armor/clothing
        /// </summary>
        /// <param name="armor">A piece of armor or clothing</param>
        public float GetArmorMod(WorldObject armor, WorldObject weapon, DamageType damageType)
        {
            // get base armor/resistance level
            var baseArmor = armor.GetProperty(PropertyInt.ArmorLevel) ?? 0;
            var armorType = armor.GetProperty(PropertyInt.ArmorType) ?? 0;
            var resistance = Creature.GetResistance(armor, damageType);

            /*Console.WriteLine(armor.Name);
            Console.WriteLine("--");
            Console.WriteLine("Base AL: " + baseArmor);
            Console.WriteLine("Base RL: " + resistance);*/

            var ignoreMagicArmor = weapon != null && weapon.IgnoreMagicArmor;

            // armor level additives
            var armorMod = ignoreMagicArmor ? 0 : armor.EnchantmentManager.GetArmorMod();
            // Console.WriteLine("Impen: " + armorMod);
            var effectiveAL = baseArmor + armorMod;

            // resistance additives
            var armorBane = ignoreMagicArmor ? 0 : armor.EnchantmentManager.GetArmorModVsType(damageType);
            // Console.WriteLine("Bane: " + armorBane);
            var effectiveRL = (float)(resistance + armorBane);

            // resistance clamp
            effectiveRL = Math.Clamp(effectiveRL, -2.0f, 2.0f);

            // TODO: could brittlemail / lures send a piece of armor or clothing's AL into the negatives?
            if (effectiveAL < 0)
                effectiveRL = 1.0f / effectiveRL;

            /*Console.WriteLine("Effective AL: " + effectiveAL);
            Console.WriteLine("Effective RL: " + effectiveRL);
            Console.WriteLine();*/

            return effectiveAL * effectiveRL;
        }
    }
}
