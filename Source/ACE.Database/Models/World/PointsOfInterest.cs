using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Points of Interest for @telepoi command
/// </summary>
public partial class PointsOfInterest
{
    /// <summary>
    /// Unique Id of this POI
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Name for POI
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Weenie Class Id of portal weenie to reference for destination of POI
    /// </summary>
    public uint WeenieClassId { get; set; }

    public DateTime LastModified { get; set; }
}
