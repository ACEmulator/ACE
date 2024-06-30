using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Weenie Instances for each Landblock
/// </summary>
public partial class LandblockInstance
{
    /// <summary>
    /// Unique Id of this Instance
    /// </summary>
    public uint Guid { get; set; }

    public int? Landblock { get; set; }

    /// <summary>
    /// Weenie Class Id of object to spawn
    /// </summary>
    public uint WeenieClassId { get; set; }

    public uint ObjCellId { get; set; }

    public float OriginX { get; set; }

    public float OriginY { get; set; }

    public float OriginZ { get; set; }

    public float AnglesW { get; set; }

    public float AnglesX { get; set; }

    public float AnglesY { get; set; }

    public float AnglesZ { get; set; }

    /// <summary>
    /// Is this a child link for any other instances?
    /// </summary>
    public bool IsLinkChild { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<LandblockInstanceLink> LandblockInstanceLink { get; set; } = new List<LandblockInstanceLink>();
}
