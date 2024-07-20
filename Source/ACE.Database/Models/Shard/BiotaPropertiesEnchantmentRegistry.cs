using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Enchantment Registry Properties of Weenies
/// </summary>
public partial class BiotaPropertiesEnchantmentRegistry
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Which PackableList this Enchantment goes in (enchantmentMask)
    /// </summary>
    public uint EnchantmentCategory { get; set; }

    /// <summary>
    /// Id of Spell
    /// </summary>
    public int SpellId { get; set; }

    /// <summary>
    /// Id of Layer
    /// </summary>
    public ushort LayerId { get; set; }

    /// <summary>
    /// Has Spell Set Id?
    /// </summary>
    public bool HasSpellSetId { get; set; }

    /// <summary>
    /// Category of Spell
    /// </summary>
    public ushort SpellCategory { get; set; }

    /// <summary>
    /// Power Level of Spell
    /// </summary>
    public uint PowerLevel { get; set; }

    /// <summary>
    /// the amount of time this enchantment has been active
    /// </summary>
    public double StartTime { get; set; }

    /// <summary>
    /// the duration of the spell
    /// </summary>
    public double Duration { get; set; }

    /// <summary>
    /// Id of the object that cast this spell
    /// </summary>
    public uint CasterObjectId { get; set; }

    /// <summary>
    /// ???
    /// </summary>
    public float DegradeModifier { get; set; }

    /// <summary>
    /// ???
    /// </summary>
    public float DegradeLimit { get; set; }

    /// <summary>
    /// the time when this enchantment was cast
    /// </summary>
    public double LastTimeDegraded { get; set; }

    /// <summary>
    /// flags that indicate the type of effect the spell has
    /// </summary>
    public uint StatModType { get; set; }

    /// <summary>
    /// along with flags, indicates which attribute is affected by the spell
    /// </summary>
    public uint StatModKey { get; set; }

    /// <summary>
    /// the effect value/amount
    /// </summary>
    public float StatModValue { get; set; }

    /// <summary>
    /// Id of the Spell Set for this spell
    /// </summary>
    public uint SpellSetId { get; set; }

    public virtual Biota Object { get; set; }
}
