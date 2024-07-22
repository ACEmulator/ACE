using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Skill Properties of Weenies
/// </summary>
public partial class WeeniePropertiesSkill
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
    /// Type of Property the value applies to (PropertySkill.????)
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// points raised
    /// </summary>
    public ushort LevelFromPP { get; set; }

    /// <summary>
    /// skill state
    /// </summary>
    public uint SAC { get; set; }

    /// <summary>
    /// XP spent on this skill
    /// </summary>
    public uint PP { get; set; }

    /// <summary>
    /// starting point for advancement of the skill (eg bonus points)
    /// </summary>
    public uint InitLevel { get; set; }

    /// <summary>
    /// last use difficulty
    /// </summary>
    public uint ResistanceAtLastCheck { get; set; }

    /// <summary>
    /// time skill was last used
    /// </summary>
    public double LastUsedTime { get; set; }

    public virtual Weenie Object { get; set; }
}
