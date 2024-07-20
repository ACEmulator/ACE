using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Events
/// </summary>
public partial class Event
{
    /// <summary>
    /// Unique Id of this Event
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Event of Quest
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Unixtime of Event Start
    /// </summary>
    public int StartTime { get; set; }

    /// <summary>
    /// Unixtime of Event End
    /// </summary>
    public int EndTime { get; set; }

    /// <summary>
    /// State of Event (GameEventState)
    /// </summary>
    public int State { get; set; }

    public DateTime LastModified { get; set; }
}
