using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Position Properties of Weenies
/// </summary>
public partial class WeeniePropertiesPosition
{
    /// <summary>
    /// Unique Id of this Position
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Type of Position the value applies to (PositionType.????)
    /// </summary>
    public ushort PositionType { get; set; }

    public uint ObjCellId { get; set; }

    public float OriginX { get; set; }

    public float OriginY { get; set; }

    public float OriginZ { get; set; }

    public float AnglesW { get; set; }

    public float AnglesX { get; set; }

    public float AnglesY { get; set; }

    public float AnglesZ { get; set; }

    public virtual Weenie Object { get; set; }
}
