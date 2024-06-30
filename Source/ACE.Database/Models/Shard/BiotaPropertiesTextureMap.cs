using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Texture Map Changes (from PCAPs) of Weenies
/// </summary>
public partial class BiotaPropertiesTextureMap
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

    public byte? Order { get; set; }

    public virtual Biota Object { get; set; }
}
