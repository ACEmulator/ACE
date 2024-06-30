using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Weenie Instance Links
/// </summary>
public partial class LandblockInstanceLink
{
    /// <summary>
    /// Unique Id of this Instance Link
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// GUID of parent instance
    /// </summary>
    public uint ParentGuid { get; set; }

    /// <summary>
    /// GUID of child instance
    /// </summary>
    public uint ChildGuid { get; set; }

    public DateTime LastModified { get; set; }

    public virtual LandblockInstance Parent { get; set; }
}
