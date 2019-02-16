using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects.Entity;

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
            var spellVuln = IgnoreMagicResist ? 1.0f : EnchantmentManager.GetVulnerabilityResistanceMod(damageType);    // ignore vuln?
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

        public float GetEffectiveArmorVsType(DamageType damageType, WorldObject damageSource)
        {
            var ignoreMagicArmor  = damageSource != null ? damageSource.IgnoreMagicArmor : false;
            var ignoreMagicResist = damageSource != null ? damageSource.IgnoreMagicResist : false;

            var enchantmentMod = ignoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod();

            var baseArmorMod = Biota.BaseArmor + enchantmentMod;

            // for creatures, can this be modified via enchantments?
            var armorVsType = Creature.GetArmorVsType(damageType);

            // handle negative baseArmorMod?
            if (baseArmorMod < 0)
                armorVsType = 1.0f + (1.0f - armorVsType);

            // TODO: handle monsters w/ multiple layers of armor
            return (float)(baseArmorMod * armorVsType);
        }

        public float GetArmorMod(DamageType damageType, WorldObject damageSource)
        {
            var effectiveArmorVsType = GetEffectiveArmorVsType(damageType, damageSource);

            return SkillFormula.CalcArmorMod(effectiveArmorVsType);
        }

        public float GetResistanceMod(DamageType damageType, WorldObject damageSource, float weaponResistanceMod)
        {
            var ignoreMagicResist = damageSource != null ? damageSource.IgnoreMagicResist : false;

            var resistanceType = Creature.GetResistanceType(damageType);

            if (ignoreMagicResist)
                return (float)Creature.GetNaturalResistance(resistanceType);
            else
                return (float)Creature.GetResistanceMod(resistanceType, weaponResistanceMod);
        }
    }
}
