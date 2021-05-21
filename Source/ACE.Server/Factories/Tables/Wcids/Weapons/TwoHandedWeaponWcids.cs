using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class TwoHandedWeaponWcids
    {
        private static ChanceTable<WeenieClassName> GreatAxes = new ChanceTable<WeenieClassName>()
        {
            // two-handed - axe
            ( WeenieClassName.ace41052_greataxe,          0.40f ),
            ( WeenieClassName.ace41053_acidgreataxe,      0.15f ),
            ( WeenieClassName.ace41054_lightninggreataxe, 0.15f ),
            ( WeenieClassName.ace41055_flaminggreataxe,   0.15f ),
            ( WeenieClassName.ace41056_frostgreataxe,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> GreatStarMaces = new ChanceTable<WeenieClassName>()
        {
            // two-handed - mace
            ( WeenieClassName.ace41057_greatstarmace,          0.40f ),
            ( WeenieClassName.ace41058_acidgreatstarmace,      0.15f ),
            ( WeenieClassName.ace41059_lightninggreatstarmace, 0.15f ),
            ( WeenieClassName.ace41060_flaminggreatstarmace,   0.15f ),
            ( WeenieClassName.ace41061_frostgreatstarmace,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> KhandaHandledMaces = new ChanceTable<WeenieClassName>()
        {
            // two-handed - mace
            ( WeenieClassName.ace41062_khandahandledmace,          0.40f ),
            ( WeenieClassName.ace41063_acidkhandahandledmace,      0.15f ),
            ( WeenieClassName.ace41064_lightningkhandahandledmace, 0.15f ),
            ( WeenieClassName.ace41065_flamingkhandahandledmace,   0.15f ),
            ( WeenieClassName.ace41066_frostkhandahandledmace,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Quadrelles = new ChanceTable<WeenieClassName>()
        {
            // two-handed - mace
            ( WeenieClassName.ace40623_quadrelle,          0.40f ),
            ( WeenieClassName.ace40624_acidquadrelle,      0.15f ),
            ( WeenieClassName.ace40625_lightningquadrelle, 0.15f ),
            ( WeenieClassName.ace40626_flamingquadrelle,   0.15f ),
            ( WeenieClassName.ace40627_frostquadrelle,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Tetsubos = new ChanceTable<WeenieClassName>()
        {
            // two-handed - mace
            ( WeenieClassName.ace40635_tetsubo,          0.40f ),
            ( WeenieClassName.ace40636_acidtetsubo,      0.15f ),
            ( WeenieClassName.ace40637_lightningtetsubo, 0.15f ),
            ( WeenieClassName.ace40638_flamingtetsubo,   0.15f ),
            ( WeenieClassName.ace40639_frosttetsubo,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Assagais = new ChanceTable<WeenieClassName>()
        {
            // two-handed - spear
            ( WeenieClassName.ace41036_assagai,          0.40f ),
            ( WeenieClassName.ace41037_acidassagai,      0.15f ),
            ( WeenieClassName.ace41038_lightningassagai, 0.15f ),
            ( WeenieClassName.ace41039_flamingassagai,   0.15f ),
            ( WeenieClassName.ace41040_frostassagai,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Corsecas = new ChanceTable<WeenieClassName>()
        {
            // two-handed - spear
            ( WeenieClassName.ace40818_corsesca,          0.40f ),
            ( WeenieClassName.ace40819_acidcorsesca,      0.15f ),
            ( WeenieClassName.ace40820_lightningcorsesca, 0.15f ),
            ( WeenieClassName.ace40821_flamingcorsesca,   0.15f ),
            ( WeenieClassName.ace40822_frostcorsesca,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> MagariYaris = new ChanceTable<WeenieClassName>()
        {
            // two-handed - spear
            ( WeenieClassName.ace41041_magariyari,          0.40f ),
            ( WeenieClassName.ace41042_acidmagariyari,      0.15f ),
            ( WeenieClassName.ace41043_lightningmagariyari, 0.15f ),
            ( WeenieClassName.ace41044_flamingmagariyari,   0.15f ),
            ( WeenieClassName.ace41045_frostmagariyari,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Pikes = new ChanceTable<WeenieClassName>()
        {
            // two-handed - spear
            ( WeenieClassName.ace41046_pike,          0.40f ),
            ( WeenieClassName.ace41047_acidpike,      0.15f ),
            ( WeenieClassName.ace41048_lightningpike, 0.15f ),
            ( WeenieClassName.ace41049_flamingpike,   0.15f ),
            ( WeenieClassName.ace41050_frostpike,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Nodachis = new ChanceTable<WeenieClassName>()
        {
            // two-handed - sword
            ( WeenieClassName.ace40760_nodachi,          0.40f ),
            ( WeenieClassName.ace40761_acidnodachi,      0.15f ),
            ( WeenieClassName.ace40762_lightningnodachi, 0.15f ),
            ( WeenieClassName.ace40763_flamingnodachi,   0.15f ),
            ( WeenieClassName.ace40764_frostnodachi,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Shashqas = new ChanceTable<WeenieClassName>()
        {
            // two-handed - sword
            ( WeenieClassName.ace41067_shashqa,          0.40f ),
            ( WeenieClassName.ace41068_acidshashqa,      0.15f ),
            ( WeenieClassName.ace41069_lightningshashqa, 0.15f ),
            ( WeenieClassName.ace41070_flamingshashqa,   0.15f ),
            ( WeenieClassName.ace41071_frostshashqa,     0.15f ),
        };

        private static ChanceTable<WeenieClassName> Spadones = new ChanceTable<WeenieClassName>()
        {
            // two-handed - sword
            ( WeenieClassName.ace40618_spadone,          0.40f ),
            ( WeenieClassName.ace40619_acidspadone,      0.15f ),
            ( WeenieClassName.ace40620_lightningspadone, 0.15f ),
            ( WeenieClassName.ace40621_flamingspadone,   0.15f ),
            ( WeenieClassName.ace40622_frostspadone,     0.15f ),
        };

        private static readonly List<(ChanceTable<WeenieClassName> table, TreasureWeaponType weaponType)> twoHandedWeaponTables = new List<(ChanceTable<WeenieClassName>, TreasureWeaponType)>()
        {
            ( GreatAxes,          TreasureWeaponType.TwoHandedAxe ),
            ( GreatStarMaces,     TreasureWeaponType.TwoHandedMace ),
            ( KhandaHandledMaces, TreasureWeaponType.TwoHandedMace ),
            ( Quadrelles,         TreasureWeaponType.TwoHandedMace ),
            ( Tetsubos,           TreasureWeaponType.TwoHandedMace ),
            ( Assagais,           TreasureWeaponType.TwoHandedSpear ),
            ( Corsecas,           TreasureWeaponType.TwoHandedSpear ),
            ( MagariYaris,        TreasureWeaponType.TwoHandedSpear ),
            ( Pikes,              TreasureWeaponType.TwoHandedSpear ),
            ( Nodachis,           TreasureWeaponType.TwoHandedSword ),
            ( Shashqas,           TreasureWeaponType.TwoHandedSword ),
            ( Spadones,           TreasureWeaponType.TwoHandedSword ),
        };

        public static WeenieClassName Roll(out TreasureWeaponType weaponType)
        {
            // even chance of selecting each weapon table
            var weaponTable = twoHandedWeaponTables[ThreadSafeRandom.Next(0, twoHandedWeaponTables.Count - 1)];

            weaponType = weaponTable.weaponType;

            return weaponTable.table.Roll();
        }

        private static readonly Dictionary<WeenieClassName, TreasureWeaponType> _combined = new Dictionary<WeenieClassName, TreasureWeaponType>();

        static TwoHandedWeaponWcids()
        {
            foreach (var twoHandedWeaponsTable in twoHandedWeaponTables)
            {
                foreach (var wcid in twoHandedWeaponsTable.table)
                    _combined.TryAdd(wcid.result, twoHandedWeaponsTable.weaponType);
            }
        }

        public static bool TryGetValue(WeenieClassName wcid, out TreasureWeaponType weaponType)
        {
            return _combined.TryGetValue(wcid, out weaponType);
        }
    }
}
