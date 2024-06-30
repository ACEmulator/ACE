using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// EmoteAction Properties of Weenies
/// </summary>
public partial class BiotaPropertiesEmoteAction
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the emote this property belongs to
    /// </summary>
    public uint EmoteId { get; set; }

    /// <summary>
    /// Emote Action Sequence Order
    /// </summary>
    public uint Order { get; set; }

    /// <summary>
    /// EmoteType
    /// </summary>
    public uint Type { get; set; }

    /// <summary>
    /// Time to wait before EmoteAction starts execution
    /// </summary>
    public float Delay { get; set; }

    /// <summary>
    /// ?
    /// </summary>
    public float Extent { get; set; }

    public uint? Motion { get; set; }

    public string Message { get; set; }

    public string TestString { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public long? Min64 { get; set; }

    public long? Max64 { get; set; }

    public double? MinDbl { get; set; }

    public double? MaxDbl { get; set; }

    public int? Stat { get; set; }

    public bool? Display { get; set; }

    public int? Amount { get; set; }

    public long? Amount64 { get; set; }

    public long? HeroXP64 { get; set; }

    public double? Percent { get; set; }

    public int? SpellId { get; set; }

    public int? WealthRating { get; set; }

    public int? TreasureClass { get; set; }

    public int? TreasureType { get; set; }

    public int? PScript { get; set; }

    public int? Sound { get; set; }

    /// <summary>
    /// Type of Destination the value applies to (DestinationType.????)
    /// </summary>
    public sbyte? DestinationType { get; set; }

    /// <summary>
    /// Weenie Class Id of object to Create
    /// </summary>
    public uint? WeenieClassId { get; set; }

    /// <summary>
    /// Stack Size of object to create (-1 = infinite)
    /// </summary>
    public int? StackSize { get; set; }

    /// <summary>
    /// Palette Color of Object
    /// </summary>
    public int? Palette { get; set; }

    /// <summary>
    /// Shade of Object&apos;s Palette
    /// </summary>
    public float? Shade { get; set; }

    /// <summary>
    /// Unused?
    /// </summary>
    public bool? TryToBond { get; set; }

    public uint? ObjCellId { get; set; }

    public float? OriginX { get; set; }

    public float? OriginY { get; set; }

    public float? OriginZ { get; set; }

    public float? AnglesW { get; set; }

    public float? AnglesX { get; set; }

    public float? AnglesY { get; set; }

    public float? AnglesZ { get; set; }

    public virtual BiotaPropertiesEmote Emote { get; set; }
}
