using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// ShortcutBar Properties of Weenies
/// </summary>
public partial class CharacterPropertiesShortcutBar
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Position (Slot) on the Shortcut Bar for this Object
    /// </summary>
    public uint ShortcutBarIndex { get; set; }

    /// <summary>
    /// Guid of the object at this Slot
    /// </summary>
    public uint ShortcutObjectId { get; set; }

    public virtual Character Character { get; set; }
}
