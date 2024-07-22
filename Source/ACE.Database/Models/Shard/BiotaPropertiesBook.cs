using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Book Properties of Weenies
/// </summary>
public partial class BiotaPropertiesBook
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Maximum number of pages per book
    /// </summary>
    public int MaxNumPages { get; set; }

    /// <summary>
    /// Maximum number of characters per page
    /// </summary>
    public int MaxNumCharsPerPage { get; set; }

    public virtual Biota Object { get; set; }
}
