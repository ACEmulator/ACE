using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class GemMaterialChance
    {
        private static readonly ChanceTable<MaterialType> class1_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.Agate, 0.13f ),
            ( MaterialType.Azurite, 0.13f ),
            ( MaterialType.LapisLazuli, 0.13f ),
            ( MaterialType.Malachite, 0.13f ),
            ( MaterialType.SmokeyQuartz, 0.12f ),
            ( MaterialType.TigerEye, 0.12f ),
            ( MaterialType.Turquoise, 0.12f ),
            ( MaterialType.WhiteQuartz, 0.12f ),
        };

        private static readonly ChanceTable<MaterialType> class2_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.Amber, 0.1f ),
            ( MaterialType.Bloodstone, 0.1f ),
            ( MaterialType.Carnelian, 0.1f ),
            ( MaterialType.Citrine, 0.1f ),
            ( MaterialType.Hematite, 0.1f ),
            ( MaterialType.Moonstone, 0.1f ),
            ( MaterialType.Onyx, 0.1f ),
            ( MaterialType.RoseQuartz, 0.1f ),
            ( MaterialType.LavenderJade, 0.1f ),
            ( MaterialType.RedJade, 0.1f ),
        };

        private static readonly ChanceTable<MaterialType> class3_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.Amethyst, 0.11f ),
            ( MaterialType.BlackGarnet, 0.11f ),
            ( MaterialType.GreenJade, 0.11f ),
            ( MaterialType.Jet, 0.11f ),
            ( MaterialType.RedGarnet, 0.11f ),
            ( MaterialType.Tourmaline, 0.11f ),
            ( MaterialType.WhiteJade, 0.11f ),
            ( MaterialType.YellowGarnet, 0.11f ),
            ( MaterialType.Zircon, 0.12f ),
        };

        private static readonly ChanceTable<MaterialType> class4_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.Aquamarine, 0.2f ),
            ( MaterialType.GreenGarnet, 0.2f ),
            ( MaterialType.Opal, 0.2f ),
            ( MaterialType.Peridot, 0.2f ),
            ( MaterialType.YellowTopaz, 0.2f ),
        };

        private static readonly ChanceTable<MaterialType> class5_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.BlackOpal, 0.2f ),
            ( MaterialType.FireOpal, 0.2f ),
            ( MaterialType.ImperialTopaz, 0.2f ),
            ( MaterialType.Sunstone, 0.2f ),
            ( MaterialType.WhiteSapphire, 0.2f ),
        };

        private static readonly ChanceTable<MaterialType> class6_materialChance = new ChanceTable<MaterialType>()
        {
            ( MaterialType.Diamond, 0.13f ),
            ( MaterialType.Emerald, 0.29f ),
            ( MaterialType.Ruby, 0.29f ),
            ( MaterialType.Sapphire, 0.29f ),
        };

        private static readonly List<ChanceTable<MaterialType>> gemMaterialChances = new List<ChanceTable<MaterialType>>()
        {
            class1_materialChance,
            class2_materialChance,
            class3_materialChance,
            class4_materialChance,
            class5_materialChance,
            class6_materialChance,
        };

        /// <summary>
        /// Rolls for a MaterialType for a gem class
        /// </summary>
        public static MaterialType Roll(int gemClass)
        {
            gemClass = Math.Clamp(gemClass, 1, 6);

            var gemMaterialChance = gemMaterialChances[gemClass - 1];

            return gemMaterialChance.Roll();
        }

        /// <summary>
        /// A factor in determining the item's monetary value
        /// </summary>
        private static readonly List<int> gemClassValue = new List<int>()
        {
            10,
            50,
            100,
            250,
            500,
            1000
        };

        private static readonly Dictionary<MaterialType, int> gemMaterialValue = new Dictionary<MaterialType, int>();

        static GemMaterialChance()
        {
            // build gemMaterialValue
            for (var i = 0; i < gemMaterialChances.Count; i++)
            {
                foreach (var material in gemMaterialChances[i].Select(i => i.result))
                    gemMaterialValue.Add(material, gemClassValue[i]);
            }
        }

        /// <summary>
        /// Returns the value for GemType
        /// </summary>
        public static int GemValue(MaterialType? gemType)
        {
            if (gemType != null && gemMaterialValue.TryGetValue(gemType.Value, out var gemValue))
                return gemValue;
            else
                return 0;   // default?
        }
    }
}
