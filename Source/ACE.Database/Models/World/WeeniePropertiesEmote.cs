using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Emote Properties of Weenies
/// </summary>
public partial class WeeniePropertiesEmote
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
    /// EmoteCategory
    /// </summary>
    public uint Category { get; set; }

    /// <summary>
    /// Probability of this EmoteSet being chosen
    /// </summary>
    public float Probability { get; set; }

    public uint? WeenieClassId { get; set; }

    public uint? Style { get; set; }

    public uint? Substyle { get; set; }

    public string Quest { get; set; }

    public int? VendorType { get; set; }

    public float? MinHealth { get; set; }

    public float? MaxHealth { get; set; }

    public virtual Weenie Object { get; set; }

    public virtual ICollection<WeeniePropertiesEmoteAction> WeeniePropertiesEmoteAction { get; set; } = new List<WeeniePropertiesEmoteAction>();
}
