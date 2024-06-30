using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Death Treasure
/// </summary>
public partial class TreasureDeath
{
    /// <summary>
    /// Unique Id of this Treasure
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Type of Treasure for this instance
    /// </summary>
    public uint TreasureType { get; set; }

    public int Tier { get; set; }

    public float LootQualityMod { get; set; }

    public int UnknownChances { get; set; }

    public int ItemChance { get; set; }

    public int ItemMinAmount { get; set; }

    public int ItemMaxAmount { get; set; }

    public int ItemTreasureTypeSelectionChances { get; set; }

    public int MagicItemChance { get; set; }

    public int MagicItemMinAmount { get; set; }

    public int MagicItemMaxAmount { get; set; }

    public int MagicItemTreasureTypeSelectionChances { get; set; }

    public int MundaneItemChance { get; set; }

    public int MundaneItemMinAmount { get; set; }

    public int MundaneItemMaxAmount { get; set; }

    public int MundaneItemTypeSelectionChances { get; set; }

    public DateTime LastModified { get; set; }
}
