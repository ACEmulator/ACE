using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Wielded Treasure
/// </summary>
public partial class TreasureWielded
{
    /// <summary>
    /// Unique Id of this Treasure
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Type of Treasure for this instance
    /// </summary>
    public uint TreasureType { get; set; }

    /// <summary>
    /// Weenie Class Id of Treasure to Generate
    /// </summary>
    public uint WeenieClassId { get; set; }

    /// <summary>
    /// Palette Color of Object Generated
    /// </summary>
    public uint PaletteId { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown1 { get; set; }

    /// <summary>
    /// Shade of Object generated&apos;s Palette
    /// </summary>
    public float Shade { get; set; }

    /// <summary>
    /// Stack Size of object to create (-1 = infinite)
    /// </summary>
    public int StackSize { get; set; }

    public float StackSizeVariance { get; set; }

    public float Probability { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown3 { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown4 { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown5 { get; set; }

    public bool SetStart { get; set; }

    public bool HasSubSet { get; set; }

    public bool ContinuesPreviousSet { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown9 { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown10 { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown11 { get; set; }

    /// <summary>
    /// Always 0 in cache.bin
    /// </summary>
    public uint Unknown12 { get; set; }

    public DateTime LastModified { get; set; }
}
