using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Dynamic Weenies of a Shard/World
/// </summary>
public partial class Biota
{
    /// <summary>
    /// Unique Object Id within the Shard
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Weenie Class Id of the Weenie this Biota was created from
    /// </summary>
    public uint WeenieClassId { get; set; }

    /// <summary>
    /// WeenieType for this Object
    /// </summary>
    public int WeenieType { get; set; }

    public uint PopulatedCollectionFlags { get; set; }

    public virtual ICollection<BiotaPropertiesAllegiance> BiotaPropertiesAllegiance { get; set; } = new List<BiotaPropertiesAllegiance>();

    public virtual ICollection<BiotaPropertiesAnimPart> BiotaPropertiesAnimPart { get; set; } = new List<BiotaPropertiesAnimPart>();

    public virtual ICollection<BiotaPropertiesAttribute> BiotaPropertiesAttribute { get; set; } = new List<BiotaPropertiesAttribute>();

    public virtual ICollection<BiotaPropertiesAttribute2nd> BiotaPropertiesAttribute2nd { get; set; } = new List<BiotaPropertiesAttribute2nd>();

    public virtual ICollection<BiotaPropertiesBodyPart> BiotaPropertiesBodyPart { get; set; } = new List<BiotaPropertiesBodyPart>();

    public virtual BiotaPropertiesBook BiotaPropertiesBook { get; set; }

    public virtual ICollection<BiotaPropertiesBookPageData> BiotaPropertiesBookPageData { get; set; } = new List<BiotaPropertiesBookPageData>();

    public virtual ICollection<BiotaPropertiesBool> BiotaPropertiesBool { get; set; } = new List<BiotaPropertiesBool>();

    public virtual ICollection<BiotaPropertiesCreateList> BiotaPropertiesCreateList { get; set; } = new List<BiotaPropertiesCreateList>();

    public virtual ICollection<BiotaPropertiesDID> BiotaPropertiesDID { get; set; } = new List<BiotaPropertiesDID>();

    public virtual ICollection<BiotaPropertiesEmote> BiotaPropertiesEmote { get; set; } = new List<BiotaPropertiesEmote>();

    public virtual ICollection<BiotaPropertiesEnchantmentRegistry> BiotaPropertiesEnchantmentRegistry { get; set; } = new List<BiotaPropertiesEnchantmentRegistry>();

    public virtual ICollection<BiotaPropertiesEventFilter> BiotaPropertiesEventFilter { get; set; } = new List<BiotaPropertiesEventFilter>();

    public virtual ICollection<BiotaPropertiesFloat> BiotaPropertiesFloat { get; set; } = new List<BiotaPropertiesFloat>();

    public virtual ICollection<BiotaPropertiesGenerator> BiotaPropertiesGenerator { get; set; } = new List<BiotaPropertiesGenerator>();

    public virtual ICollection<BiotaPropertiesIID> BiotaPropertiesIID { get; set; } = new List<BiotaPropertiesIID>();

    public virtual ICollection<BiotaPropertiesInt> BiotaPropertiesInt { get; set; } = new List<BiotaPropertiesInt>();

    public virtual ICollection<BiotaPropertiesInt64> BiotaPropertiesInt64 { get; set; } = new List<BiotaPropertiesInt64>();

    public virtual ICollection<BiotaPropertiesPalette> BiotaPropertiesPalette { get; set; } = new List<BiotaPropertiesPalette>();

    public virtual ICollection<BiotaPropertiesPosition> BiotaPropertiesPosition { get; set; } = new List<BiotaPropertiesPosition>();

    public virtual ICollection<BiotaPropertiesSkill> BiotaPropertiesSkill { get; set; } = new List<BiotaPropertiesSkill>();

    public virtual ICollection<BiotaPropertiesSpellBook> BiotaPropertiesSpellBook { get; set; } = new List<BiotaPropertiesSpellBook>();

    public virtual ICollection<BiotaPropertiesString> BiotaPropertiesString { get; set; } = new List<BiotaPropertiesString>();

    public virtual ICollection<BiotaPropertiesTextureMap> BiotaPropertiesTextureMap { get; set; } = new List<BiotaPropertiesTextureMap>();

    public virtual ICollection<HousePermission> HousePermission { get; set; } = new List<HousePermission>();
}
