using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Texture Map Changes (from PCAPs) of Weenies
/// </summary>
public partial class WeeniePropertiesTextureMap
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    public byte Index { get; set; }

    public uint OldId { get; set; }

    public uint NewId { get; set; }

    public virtual Weenie Object { get; set; }
}
