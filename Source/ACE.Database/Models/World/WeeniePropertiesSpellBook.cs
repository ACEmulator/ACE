using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// SpellBook Properties of Weenies
/// </summary>
public partial class WeeniePropertiesSpellBook
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
    /// Id of Spell
    /// </summary>
    public int Spell { get; set; }

    /// <summary>
    /// Chance to cast this spell
    /// </summary>
    public float Probability { get; set; }

    public virtual Weenie Object { get; set; }
}
