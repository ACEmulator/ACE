using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Book Properties of Weenies
/// </summary>
public partial class WeeniePropertiesBook
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

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

    public virtual Weenie Object { get; set; }
}
