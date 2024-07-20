using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Palette Changes (from PCAPs) of Weenies
/// </summary>
public partial class BiotaPropertiesPalette
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    public uint SubPaletteId { get; set; }

    public ushort Offset { get; set; }

    public ushort Length { get; set; }

    public byte? Order { get; set; }

    public virtual Biota Object { get; set; }
}
