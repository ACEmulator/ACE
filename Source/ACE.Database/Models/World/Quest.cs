using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Quests
/// </summary>
public partial class Quest
{
    /// <summary>
    /// Unique Id of this Quest
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Name of Quest
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Minimum time between Quest completions
    /// </summary>
    public uint MinDelta { get; set; }

    /// <summary>
    /// Maximum number of times Quest can be completed
    /// </summary>
    public int MaxSolves { get; set; }

    /// <summary>
    /// Quest solved text - unused?
    /// </summary>
    public string Message { get; set; }

    public DateTime LastModified { get; set; }
}
