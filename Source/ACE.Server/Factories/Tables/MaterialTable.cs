using System.Collections.Generic;

using ACE.Entity.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class MaterialTable
    {
        private static readonly Dictionary<MaterialType, float> ValueMod = new Dictionary<MaterialType, float>()
        {
            { MaterialType.Ceramic,         0.2f },
            { MaterialType.Porcelain,       1.0f },
            { MaterialType.Linen,           0.0f },
            { MaterialType.Satin,           1.2f },
            { MaterialType.Silk,            1.5f },
            { MaterialType.Velvet,          1.2f },
            { MaterialType.Wool,            0.0f },
            { MaterialType.Agate,           1.4f },
            { MaterialType.Amber,           1.1f },
            { MaterialType.Amethyst,        2.3f },
            { MaterialType.Aquamarine,      2.8f },
            { MaterialType.Azurite,         2.3f },
            { MaterialType.BlackGarnet,     2.4f },
            { MaterialType.BlackOpal,       2.4f },
            { MaterialType.Bloodstone,      1.5f },
            { MaterialType.Carnelian,       2.1f },
            { MaterialType.Citrine,         2.3f },
            { MaterialType.Diamond,         6.0f },
            { MaterialType.Emerald,         4.0f },
            { MaterialType.FireOpal,        2.5f },
            { MaterialType.GreenGarnet,     4.0f },
            { MaterialType.GreenJade,       1.7f },
            { MaterialType.Hematite,        1.9f },
            { MaterialType.ImperialTopaz,   4.7f },
            { MaterialType.Jet,             2.6f },
            { MaterialType.LapisLazuli,     1.8f },
            { MaterialType.LavenderJade,    2.2f },
            { MaterialType.Malachite,       1.5f },
            { MaterialType.Moonstone,       2.0f },
            { MaterialType.Onyx,            1.2f },
            { MaterialType.Opal,            2.3f },
            { MaterialType.Peridot,         2.9f },
            { MaterialType.RedGarnet,       2.7f },
            { MaterialType.RedJade,         2.0f },
            { MaterialType.RoseQuartz,      2.1f },
            { MaterialType.Ruby,            5.0f },
            { MaterialType.Sapphire,        5.5f },
            { MaterialType.SmokeyQuartz,    1.7f },
            { MaterialType.Sunstone,        3.3f },
            { MaterialType.TigerEye,        1.3f },
            { MaterialType.Tourmaline,      3.7f },
            { MaterialType.Turquoise,       1.7f },
            { MaterialType.WhiteJade,       1.7f },
            { MaterialType.WhiteQuartz,     1.6f },
            { MaterialType.WhiteSapphire,   4.3f },
            { MaterialType.YellowGarnet,    2.8f },
            { MaterialType.YellowTopaz,     3.0f },
            { MaterialType.Zircon,          3.0f },
            { MaterialType.Ivory,           2.2f },
            { MaterialType.Leather,         0.2f },
            { MaterialType.ArmoredilloHide, 2.0f },
            { MaterialType.GromnieHide,     1.3f },
            { MaterialType.ReedSharkHide,   2.2f },
            { MaterialType.Brass,           0.5f },
            { MaterialType.Bronze,          0.0f },
            { MaterialType.Copper,          1.0f },
            { MaterialType.Gold,            3.0f },
            { MaterialType.Iron,            0.0f },
            { MaterialType.Pyreal,          5.0f },
            { MaterialType.Silver,          2.0f },
            { MaterialType.Steel,           1.0f },
            { MaterialType.Alabaster,       1.5f },
            { MaterialType.Granite,         0.0f },
            { MaterialType.Marble,          1.8f },
            { MaterialType.Obsidian,        1.2f },
            { MaterialType.Sandstone,       0.0f },
            { MaterialType.Serpentine,      2.0f },
            { MaterialType.Ebony,           2.0f },
            { MaterialType.Mahogany,        1.5f },
            { MaterialType.Oak,             0.5f },
            { MaterialType.Pine,            0.0f },
            { MaterialType.Teak,            1.5f },
        };

        public static float GetValueMod(MaterialType? materialType)
        {
            if (materialType != null && ValueMod.TryGetValue(materialType.Value, out var valueMod))
                return valueMod;
            else
                return 1.0f;    // default?
        }
    }
}
