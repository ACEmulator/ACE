using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using MaterialType = ACE.Entity.Enum.MaterialType;

namespace ACE.Server.Factories.Tables
{
    public static class GemMaterialChance
    {
        private static ChanceTable<GemResult> class1_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.gemagate,         MaterialType.Agate),         0.13f ),
            ( new GemResult(WeenieClassName.gemazurite,       MaterialType.Azurite),       0.13f ),
            ( new GemResult(WeenieClassName.gemlapislazuli,   MaterialType.LapisLazuli),   0.13f ),
            ( new GemResult(WeenieClassName.gemmalachite,     MaterialType.Malachite),     0.13f ),
            ( new GemResult(WeenieClassName.gemsmokeyquartz,  MaterialType.SmokeyQuartz),  0.12f ),
            ( new GemResult(WeenieClassName.gemtigereye,      MaterialType.TigerEye),      0.12f ),
            ( new GemResult(WeenieClassName.gemturquoise,     MaterialType.Turquoise),     0.12f ),
            ( new GemResult(WeenieClassName.gemwhitequartz,   MaterialType.WhiteQuartz),   0.12f ),
        };

        private static ChanceTable<GemResult> class2_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.gemamber,         MaterialType.Amber),         0.10f ),
            ( new GemResult(WeenieClassName.gembloodstone,    MaterialType.Bloodstone),    0.10f ),
            ( new GemResult(WeenieClassName.gemcarnelian,     MaterialType.Carnelian),     0.10f ),
            ( new GemResult(WeenieClassName.gemcitrine,       MaterialType.Citrine),       0.10f ),
            ( new GemResult(WeenieClassName.gemhematite,      MaterialType.Hematite),      0.10f ),
            ( new GemResult(WeenieClassName.gemmoonstone,     MaterialType.Moonstone),     0.10f ),
            ( new GemResult(WeenieClassName.gemonyx,          MaterialType.Onyx),          0.10f ),
            ( new GemResult(WeenieClassName.gemrosequartz,    MaterialType.RoseQuartz),    0.10f ),
            ( new GemResult(WeenieClassName.gemlavenderjade,  MaterialType.LavenderJade),  0.10f ),
            ( new GemResult(WeenieClassName.gemredjade,       MaterialType.RedJade),       0.10f ),
        };

        private static ChanceTable<GemResult> class3_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.gemamethyst,      MaterialType.Amethyst),      0.11f ),
            ( new GemResult(WeenieClassName.gemblackgarnet,   MaterialType.BlackGarnet),   0.11f ),
            ( new GemResult(WeenieClassName.gemgreenjade,     MaterialType.GreenJade),     0.11f ),
            ( new GemResult(WeenieClassName.gemjet,           MaterialType.Jet),           0.11f ),
            ( new GemResult(WeenieClassName.gemredgarnet,     MaterialType.RedGarnet),     0.11f ),
            ( new GemResult(WeenieClassName.gemtourmaline,    MaterialType.Tourmaline),    0.11f ),
            ( new GemResult(WeenieClassName.gemwhitejade,     MaterialType.WhiteJade),     0.11f ),
            ( new GemResult(WeenieClassName.gemyellowgarnet,  MaterialType.YellowGarnet),  0.11f ),
            ( new GemResult(WeenieClassName.gemzircon,        MaterialType.Zircon),        0.12f ),
        };

        private static ChanceTable<GemResult> class4_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.gemaquamarine,    MaterialType.Aquamarine),    0.20f ),
            ( new GemResult(WeenieClassName.gemgreengarnet,   MaterialType.GreenGarnet),   0.20f ),
            ( new GemResult(WeenieClassName.gemopal,          MaterialType.Opal),          0.20f ),
            ( new GemResult(WeenieClassName.gemperidot,       MaterialType.Peridot),       0.20f ),
            ( new GemResult(WeenieClassName.gemyellowtopaz,   MaterialType.YellowTopaz),   0.20f ),
        };

        private static ChanceTable<GemResult> class5_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.gemblackopal,     MaterialType.BlackOpal),     0.20f ),
            ( new GemResult(WeenieClassName.gemfireopal,      MaterialType.FireOpal),      0.20f ),
            ( new GemResult(WeenieClassName.gemimperialtopaz, MaterialType.ImperialTopaz), 0.20f ),
            ( new GemResult(WeenieClassName.gemsunstone,      MaterialType.Sunstone),      0.20f ),
            ( new GemResult(WeenieClassName.gemwhitesapphire, MaterialType.WhiteSapphire), 0.20f ),
        };

        private static ChanceTable<GemResult> class6_materialChance = new ChanceTable<GemResult>()
        {
            ( new GemResult(WeenieClassName.jeweldiamond,     MaterialType.Diamond),       0.13f ),
            ( new GemResult(WeenieClassName.jewelemerald,     MaterialType.Emerald),       0.29f ),
            ( new GemResult(WeenieClassName.jewelruby,        MaterialType.Ruby),          0.29f ),
            ( new GemResult(WeenieClassName.jewelsapphire,    MaterialType.Sapphire),      0.29f ),
        };

        private static readonly List<ChanceTable<GemResult>> gemMaterialChances = new List<ChanceTable<GemResult>>()
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
        public static GemResult Roll(int gemClass)
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

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static GemMaterialChance()
        {
            // build gemMaterialValue
            for (var i = 0; i < gemMaterialChances.Count; i++)
            {
                foreach (var material in gemMaterialChances[i].Select(i => i.result))
                    gemMaterialValue.Add(material.MaterialType, gemClassValue[i]);
            }

            // build wcid hashset for lootgen command
            foreach (var gemMaterialChance in gemMaterialChances)
            {
                foreach (var entry in gemMaterialChance)
                    _combined.Add(entry.result.ClassName);
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

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
