using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// FillCompBook Properties of Weenies
/// </summary>
public partial class CharacterPropertiesFillCompBook
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Id of Spell Component
    /// </summary>
    public int SpellComponentId { get; set; }

    /// <summary>
    /// Amount of this component to add to the buy list for repurchase
    /// </summary>
    public int QuantityToRebuy { get; set; }

    public virtual Character Character { get; set; }
}
