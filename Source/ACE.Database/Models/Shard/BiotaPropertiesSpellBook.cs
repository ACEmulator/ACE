using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// SpellBook Properties of Weenies
/// </summary>
public partial class BiotaPropertiesSpellBook
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Id of Spell
    /// </summary>
    public int Spell { get; set; }

    /// <summary>
    /// Chance to cast this spell
    /// </summary>
    public float Probability { get; set; }

    public virtual Biota Object { get; set; }
}
