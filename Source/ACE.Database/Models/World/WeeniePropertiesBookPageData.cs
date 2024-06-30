using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Page Properties of Weenies
/// </summary>
public partial class WeeniePropertiesBookPageData
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the Book object this page belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Id of the page number for this page
    /// </summary>
    public uint PageId { get; set; }

    /// <summary>
    /// Id of the Author of this page
    /// </summary>
    public uint AuthorId { get; set; }

    /// <summary>
    /// Character Name of the Author of this page
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// Account Name of the Author of this page
    /// </summary>
    public string AuthorAccount { get; set; }

    /// <summary>
    /// if this is true, any character in the world can change the page
    /// </summary>
    public bool IgnoreAuthor { get; set; }

    /// <summary>
    /// Text of the Page
    /// </summary>
    public string PageText { get; set; }

    public virtual Weenie Object { get; set; }
}
