using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// String Properties of Weenies
/// </summary>
public partial class BiotaPropertiesString
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Type of Property the value applies to (PropertyString.????)
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// Value of this Property
    /// </summary>
    public string Value { get; set; }

    public virtual Biota Object { get; set; }
}
