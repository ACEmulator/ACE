using System.Collections.Generic;

using ACE.Common;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids.Weapons
{
    public static class TwoHandedWeaponWcids
    {
        private static readonly List<WeenieClassName> GreatAxes = new List<WeenieClassName>()
        {
            // two-handed - axe
            WeenieClassName.ace41052_greataxe,
            WeenieClassName.ace41053_acidgreataxe,
            WeenieClassName.ace41054_lightninggreataxe,
            WeenieClassName.ace41055_flaminggreataxe,
            WeenieClassName.ace41056_frostgreataxe,
        };

        private static readonly List<WeenieClassName> GreatStarMaces = new List<WeenieClassName>()
        {
            // two-handed - mace
            WeenieClassName.ace41057_greatstarmace,
            WeenieClassName.ace41058_acidgreatstarmace,
            WeenieClassName.ace41059_lightninggreatstarmace,
            WeenieClassName.ace41060_flaminggreatstarmace,
            WeenieClassName.ace41061_frostgreatstarmace,
        };

        private static readonly List<WeenieClassName> KhandaHandledMaces = new List<WeenieClassName>()
        {
            // two-handed - mace
            WeenieClassName.ace41062_khandahandledmace,
            WeenieClassName.ace41063_acidkhandahandledmace,
            WeenieClassName.ace41064_lightningkhandahandledmace,
            WeenieClassName.ace41065_flamingkhandahandledmace,
            WeenieClassName.ace41066_frostkhandahandledmace,
        };

        private static readonly List<WeenieClassName> Quadrelles = new List<WeenieClassName>()
        {
            // two-handed - mace
            WeenieClassName.ace40623_quadrelle,
            WeenieClassName.ace40624_acidquadrelle,
            WeenieClassName.ace40625_lightningquadrelle,
            WeenieClassName.ace40626_flamingquadrelle,
            WeenieClassName.ace40627_frostquadrelle,
        };

        private static readonly List<WeenieClassName> Tetsubos = new List<WeenieClassName>()
        {
            // two-handed - mace
            WeenieClassName.ace40635_tetsubo,
            WeenieClassName.ace40636_acidtetsubo,
            WeenieClassName.ace40637_lightningtetsubo,
            WeenieClassName.ace40638_flamingtetsubo,
            WeenieClassName.ace40639_frosttetsubo,
        };

        private static readonly List<WeenieClassName> Assagais = new List<WeenieClassName>()
        {
            // two-handed - spear
            WeenieClassName.ace41036_assagai,
            WeenieClassName.ace41037_acidassagai,
            WeenieClassName.ace41038_lightningassagai,
            WeenieClassName.ace41039_flamingassagai,
            WeenieClassName.ace41040_frostassagai,
        };

        private static readonly List<WeenieClassName> Corsecas = new List<WeenieClassName>()
        {
            // two-handed - spear
            WeenieClassName.ace40818_corsesca,
            WeenieClassName.ace40819_acidcorsesca,
            WeenieClassName.ace40820_lightningcorsesca,
            WeenieClassName.ace40821_flamingcorsesca,
            WeenieClassName.ace40822_frostcorsesca,
        };

        private static readonly List<WeenieClassName> MagariYaris = new List<WeenieClassName>()
        {
            // two-handed - spear
            WeenieClassName.ace41041_magariyari,
            WeenieClassName.ace41042_acidmagariyari,
            WeenieClassName.ace41043_lightningmagariyari,
            WeenieClassName.ace41044_flamingmagariyari,
            WeenieClassName.ace41045_frostmagariyari,
        };

        private static readonly List<WeenieClassName> Pikes = new List<WeenieClassName>()
        {
            // two-handed - spear
            WeenieClassName.ace41046_pike,
            WeenieClassName.ace41047_acidpike,
            WeenieClassName.ace41048_lightningpike,
            WeenieClassName.ace41049_flamingpike,
            WeenieClassName.ace41050_frostpike,
        };

        private static readonly List<WeenieClassName> Nodachis = new List<WeenieClassName>()
        {
            // two-handed - sword
            WeenieClassName.ace40760_nodachi,
            WeenieClassName.ace40761_acidnodachi,
            WeenieClassName.ace40762_lightningnodachi,
            WeenieClassName.ace40763_flamingnodachi,
            WeenieClassName.ace40764_frostnodachi,
        };

        private static readonly List<WeenieClassName> Shashqas = new List<WeenieClassName>()
        {
            // two-handed - sword
            WeenieClassName.ace41067_shashqa,
            WeenieClassName.ace41068_acidshashqa,
            WeenieClassName.ace41069_lightningshashqa,
            WeenieClassName.ace41070_flamingshashqa,
            WeenieClassName.ace41071_frostshashqa,
        };

        private static readonly List<WeenieClassName> Spadones = new List<WeenieClassName>()
        {
            // two-handed - sword
            WeenieClassName.ace40618_spadone,
            WeenieClassName.ace40619_acidspadone,
            WeenieClassName.ace40620_lightningspadone,
            WeenieClassName.ace40621_flamingspadone,
            WeenieClassName.ace40622_frostspadone,
        };

        private static readonly List<List<WeenieClassName>> twoHandedWeaponTables = new List<List<WeenieClassName>>()
        {
            GreatAxes,
            GreatStarMaces,
            KhandaHandledMaces,
            Quadrelles,
            Tetsubos,
            Assagais,
            Corsecas,
            MagariYaris,
            Pikes,
            Nodachis,
            Shashqas,
            Spadones,
        };

        public static WeenieClassName Roll()
        {
            // even chance of selecting each weapon type
            var weaponType = ThreadSafeRandom.Next(0, twoHandedWeaponTables.Count - 1);

            var weaponTable = twoHandedWeaponTables[weaponType];

            // 50/50 chance of selecting elemental/non-elemental
            // could have been 40/60 in retail?
            var elemental = ThreadSafeRandom.NextBool();

            if (elemental)
            {
                var elementType = ThreadSafeRandom.Next(1, 4);
                return weaponTable[elementType];
            }
            else
                return weaponTable[0];
        }
    }
}
