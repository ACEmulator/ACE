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

        public EnchantmentManager EnchantmentManager => Creature.EnchantmentManager;

        public Creature_BodyPart(Creature creature, BiotaPropertiesBodyPart biota)
        {
            Creature = creature;
            Biota = biota;
        }

        /// <summary>
        /// Main entry point for getting the armor mod
        /// </summary>
        public float GetArmorMod(DamageType damageType, List<WorldObject> armorLayers, WorldObject weapon, float armorRendingMod = 1.0f)
        {
            var effectiveArmorVsType = GetEffectiveArmorVsType(damageType, armorLayers, weapon, armorRendingMod);

            return SkillFormula.CalcArmorMod(effectiveArmorVsType);
        }

        public float GetEffectiveArmorVsType(DamageType damageType, List<WorldObject> armorLayers, WorldObject weapon, float armorRendingMod = 1.0f)
        {
            var ignoreMagicArmor  = weapon != null ? weapon.IgnoreMagicArmor : false;
            var ignoreMagicResist = weapon != null ? weapon.IgnoreMagicResist : false;

            // get base AL / RL
            var enchantmentMod = ignoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod();

            var baseArmorMod = (float)(Biota.BaseArmor + enchantmentMod);

            // for creatures, can this be modified via enchantments?
            var armorVsType = Creature.GetArmorVsType(damageType);

            // handle negative baseArmorMod?
            if (baseArmorMod < 0)
                armorVsType = 1.0f + (1.0f - armorVsType);

            var effectiveAL = (float)(baseArmorMod * armorVsType);

            // handle monsters w/ multiple layers of armor
            foreach (var armorLayer in armorLayers)
                effectiveAL += GetArmorMod(armorLayer, damageType, ignoreMagicArmor);

            // armor rending reduces base armor + all physical armor too?
            if (effectiveAL > 0)
                effectiveAL *= armorRendingMod;

            return effectiveAL;
        }

        /// <summary>
        /// Returns the effective AL for 1 piece of armor/clothing
        /// </summary>
        /// <param name="armor">A piece of armor or clothing</param>
        public float GetArmorMod(WorldObject armor, DamageType damageType, bool ignoreMagicArmor)
        {
            // get base armor/resistance level
            var baseArmor = armor.GetProperty(PropertyInt.ArmorLevel) ?? 0;
            var armorType = armor.GetProperty(PropertyInt.ArmorType) ?? 0;
            var resistance = Creature.GetResistance(armor, damageType);

            /*Console.WriteLine(armor.Name);
            Console.WriteLine("--");
            Console.WriteLine("Base AL: " + baseArmor);
            Console.WriteLine("Base RL: " + resistance);*/

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

        public List<WorldObject> GetArmorLayers(CombatBodyPart bodyPart)
        {
            var coverageMask = BodyParts.GetCoverageMask(bodyPart);

            var equipped = Creature.EquippedObjects.Values.Where(e => e is Clothing && (e.ClothingPriority & coverageMask) != 0).ToList();

            return equipped;
        }
    }
}
