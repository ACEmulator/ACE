using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

public partial class TreasureMaterialBase
{
    public uint Id { get; set; }

    /// <summary>
    /// Derived from PropertyInt.TsysMutationData
    /// </summary>
    public uint MaterialCode { get; set; }

    /// <summary>
    /// Loot Tier
    /// </summary>
    public uint Tier { get; set; }

    public float Probability { get; set; }

    /// <summary>
    /// MaterialType
    /// </summary>
    public uint MaterialId { get; set; }
}
