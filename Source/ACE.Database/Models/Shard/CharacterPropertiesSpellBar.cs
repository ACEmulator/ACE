using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// SpellBar Properties of Weenies
/// </summary>
public partial class CharacterPropertiesSpellBar
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Id of Spell Bar
    /// </summary>
    public uint SpellBarNumber { get; set; }

    /// <summary>
    /// Position (Slot) on this Spell Bar for this Spell
    /// </summary>
    public uint SpellBarIndex { get; set; }

    /// <summary>
    /// Id of Spell on this Spell Bar at this Slot
    /// </summary>
    public uint SpellId { get; set; }

    public virtual Character Character { get; set; }
}
