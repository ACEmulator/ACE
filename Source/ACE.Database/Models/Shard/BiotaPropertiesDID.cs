using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// DataID Properties of Weenies
/// </summary>
public partial class BiotaPropertiesDID
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Type of Property the value applies to (PropertyDataId.????)
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// Value of this Property
    /// </summary>
    public uint Value { get; set; }

    public virtual Biota Object { get; set; }
}
