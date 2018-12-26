using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class Creature_BodyPart
    {
        public Creature Creature;
        public BiotaPropertiesBodyPart Biota;

        public float WeaponResistanceMod;

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

        public int BaseArmorMod => Biota.BaseArmor + (IgnoreMagicResist ? 0 : EnchantmentManager.GetBodyArmorMod());
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
            var resistance = (float)armorVsType / Biota.BaseArmor;
            if (double.IsNaN(resistance))
                resistance = 1.0f;

            float mod;
            var spellVuln = IgnoreMagicResist ? 1.0f : EnchantmentManager.GetVulnerabilityResistanceMod(damageType);    // ignore vuln?
            var spellProt = IgnoreMagicResist ? 1.0f : EnchantmentManager.GetProtectionResistanceMod(damageType);

            if (WeaponResistanceMod < spellVuln)
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
    }
}
