using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ACE.Database.Models.World
{
    public partial class TreasureDeath
    {
        public uint Id { get; set; }
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

        [NotMapped]
        public bool DisableSets { get; set; }

        [NotMapped]
        public bool DisableLegendaryCantrips { get; set; }

        [NotMapped]
        public bool DisableEpicCantrips { get; set; }

        [NotMapped]
        public bool DisableAetheria { get; set; }
    }
}
