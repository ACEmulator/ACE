using System;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.WorldObjects
{
    public class Armor
    {
        public Player Player;
        public WorldObject ArmorClothing;
        public BiotaPropertiesBodyPart Biota;

        // TODO: differntiate between auras
        public EnchantmentManager EnchantmentManager => Player.EnchantmentManager;

        public Armor(Player player, WorldObject armorClothing, BiotaPropertiesBodyPart biota)
        {
            Player = player;
            ArmorClothing = armorClothing;
            Biota = biota;
        }

        public int BaseArmorMod => Biota.BaseArmor + EnchantmentManager.GetArmorMod();
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
            var mod = EnchantmentManager.GetResistanceMod(damageType);

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
