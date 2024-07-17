using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// House Portal Destinations
/// </summary>
public partial class HousePortal
{
    /// <summary>
    /// Unique Id of this House Portal
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Id of House
    /// </summary>
    public uint HouseId { get; set; }

    public uint ObjCellId { get; set; }

    public float OriginX { get; set; }

    public float OriginY { get; set; }

    public float OriginZ { get; set; }

    public float AnglesW { get; set; }

    public float AnglesX { get; set; }

    public float AnglesY { get; set; }

    public float AnglesZ { get; set; }

    public DateTime LastModified { get; set; }
}
