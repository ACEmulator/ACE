using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.WorldObjects
{
    public class Creature_BodyPart
    {
        public Creature Creature;
        public KeyValuePair<CombatBodyPart, PropertiesBodyPart> Biota;

        public EnchantmentManager EnchantmentManager => Creature.EnchantmentManager;

        public Creature_BodyPart(Creature creature, KeyValuePair<CombatBodyPart, PropertiesBodyPart> biota)
        {
            Creature = creature;
            Biota = biota;
        }

        /// <summary>
        /// Main entry point for getting the armor mod
        /// </summary>
        public float GetArmorMod(DamageType damageType, List<WorldObject> armorLayers, Creature attacker, WorldObject weapon, float armorRendingMod = 1.0f)
        {
            var effectiveArmorVsType = GetEffectiveArmorVsType(damageType, armorLayers, attacker, weapon, armorRendingMod);

            return SkillFormula.CalcArmorMod(effectiveArmorVsType);
        }

        public float GetEffectiveArmorVsType(DamageType damageType, List<WorldObject> armorLayers, Creature attacker, WorldObject weapon, float armorRendingMod = 1.0f)
        {
            var ignoreMagicArmor = (weapon?.IgnoreMagicArmor ?? false) || (attacker?.IgnoreMagicArmor ?? false);
            var ignoreMagicResist = (weapon?.IgnoreMagicResist ?? false) || (attacker?.IgnoreMagicResist ?? false);

            // get base AL / RL
            var armorVsType = Biota.Value.BaseArmor * (float)Creature.GetArmorVsType(damageType);

            // additive enchantments:
            // imperil / armor
            var enchantmentMod = ignoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod();

            var effectiveAL = armorVsType + enchantmentMod;

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
            //if (effectiveAL < 0 && effectiveRL != 0)
                //effectiveRL = 1.0f / effectiveRL;

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
