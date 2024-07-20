using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// QuestBook Properties of Weenies
/// </summary>
public partial class CharacterPropertiesQuestRegistry
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Unique Name of Quest
    /// </summary>
    public string QuestName { get; set; }

    /// <summary>
    /// Timestamp of last successful completion
    /// </summary>
    public uint LastTimeCompleted { get; set; }

    /// <summary>
    /// Number of successful completions
    /// </summary>
    public int NumTimesCompleted { get; set; }

    public virtual Character Character { get; set; }
}
