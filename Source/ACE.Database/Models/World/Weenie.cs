using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Weenies
/// </summary>
public partial class Weenie
{
    /// <summary>
    /// Weenie Class Id (wcid) / (WCID) / (weenieClassId)
    /// </summary>
    public uint ClassId { get; set; }

    /// <summary>
    /// Weenie Class Name (W_????_CLASS)
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// WeenieType
    /// </summary>
    public int Type { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<WeeniePropertiesAnimPart> WeeniePropertiesAnimPart { get; set; } = new List<WeeniePropertiesAnimPart>();

    public virtual ICollection<WeeniePropertiesAttribute> WeeniePropertiesAttribute { get; set; } = new List<WeeniePropertiesAttribute>();

    public virtual ICollection<WeeniePropertiesAttribute2nd> WeeniePropertiesAttribute2nd { get; set; } = new List<WeeniePropertiesAttribute2nd>();

    public virtual ICollection<WeeniePropertiesBodyPart> WeeniePropertiesBodyPart { get; set; } = new List<WeeniePropertiesBodyPart>();

    public virtual WeeniePropertiesBook WeeniePropertiesBook { get; set; }

    public virtual ICollection<WeeniePropertiesBookPageData> WeeniePropertiesBookPageData { get; set; } = new List<WeeniePropertiesBookPageData>();

    public virtual ICollection<WeeniePropertiesBool> WeeniePropertiesBool { get; set; } = new List<WeeniePropertiesBool>();

    public virtual ICollection<WeeniePropertiesCreateList> WeeniePropertiesCreateList { get; set; } = new List<WeeniePropertiesCreateList>();

    public virtual ICollection<WeeniePropertiesDID> WeeniePropertiesDID { get; set; } = new List<WeeniePropertiesDID>();

    public virtual ICollection<WeeniePropertiesEmote> WeeniePropertiesEmote { get; set; } = new List<WeeniePropertiesEmote>();

    public virtual ICollection<WeeniePropertiesEventFilter> WeeniePropertiesEventFilter { get; set; } = new List<WeeniePropertiesEventFilter>();

    public virtual ICollection<WeeniePropertiesFloat> WeeniePropertiesFloat { get; set; } = new List<WeeniePropertiesFloat>();

    public virtual ICollection<WeeniePropertiesGenerator> WeeniePropertiesGenerator { get; set; } = new List<WeeniePropertiesGenerator>();

    public virtual ICollection<WeeniePropertiesIID> WeeniePropertiesIID { get; set; } = new List<WeeniePropertiesIID>();

    public virtual ICollection<WeeniePropertiesInt> WeeniePropertiesInt { get; set; } = new List<WeeniePropertiesInt>();

    public virtual ICollection<WeeniePropertiesInt64> WeeniePropertiesInt64 { get; set; } = new List<WeeniePropertiesInt64>();

    public virtual ICollection<WeeniePropertiesPalette> WeeniePropertiesPalette { get; set; } = new List<WeeniePropertiesPalette>();

    public virtual ICollection<WeeniePropertiesPosition> WeeniePropertiesPosition { get; set; } = new List<WeeniePropertiesPosition>();

    public virtual ICollection<WeeniePropertiesSkill> WeeniePropertiesSkill { get; set; } = new List<WeeniePropertiesSkill>();

    public virtual ICollection<WeeniePropertiesSpellBook> WeeniePropertiesSpellBook { get; set; } = new List<WeeniePropertiesSpellBook>();

    public virtual ICollection<WeeniePropertiesString> WeeniePropertiesString { get; set; } = new List<WeeniePropertiesString>();

    public virtual ICollection<WeeniePropertiesTextureMap> WeeniePropertiesTextureMap { get; set; } = new List<WeeniePropertiesTextureMap>();
}
