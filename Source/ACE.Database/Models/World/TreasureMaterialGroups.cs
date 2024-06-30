using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

public partial class TreasureMaterialGroups
{
    public uint Id { get; set; }

    /// <summary>
    /// MaterialType Group
    /// </summary>
    public uint MaterialGroup { get; set; }

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
