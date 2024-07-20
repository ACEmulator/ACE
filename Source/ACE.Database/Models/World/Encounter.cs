using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Encounters
/// </summary>
public partial class Encounter
{
    /// <summary>
    /// Unique Id of this Encounter
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Landblock for this Encounter
    /// </summary>
    public int Landblock { get; set; }

    /// <summary>
    /// Weenie Class Id of generator/object to spawn for Encounter
    /// </summary>
    public uint WeenieClassId { get; set; }

    /// <summary>
    /// CellX position of this Encounter
    /// </summary>
    public int CellX { get; set; }

    /// <summary>
    /// CellY position of this Encounter
    /// </summary>
    public int CellY { get; set; }

    public DateTime LastModified { get; set; }
}
