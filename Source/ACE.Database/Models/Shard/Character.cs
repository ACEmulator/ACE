using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// Int Properties of Weenies
/// </summary>
public partial class Character
{
    /// <summary>
    /// Id of the Biota for this Character
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the Biota for this Character
    /// </summary>
    public uint AccountId { get; set; }

    /// <summary>
    /// Name of Character
    /// </summary>
    public string Name { get; set; }

    public bool IsPlussed { get; set; }

    /// <summary>
    /// Is this Character deleted?
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The character will be marked IsDeleted=True after this timestamp
    /// </summary>
    public ulong DeleteTime { get; set; }

    /// <summary>
    /// Timestamp the last time this character entered the world
    /// </summary>
    public double LastLoginTimestamp { get; set; }

    public int TotalLogins { get; set; }

    public int CharacterOptions1 { get; set; }

    public int CharacterOptions2 { get; set; }

    public byte[] GameplayOptions { get; set; }

    public uint SpellbookFilters { get; set; }

    public uint HairTexture { get; set; }

    public uint DefaultHairTexture { get; set; }

    public virtual ICollection<BiotaPropertiesAllegiance> BiotaPropertiesAllegiance { get; set; } = new List<BiotaPropertiesAllegiance>();

    public virtual ICollection<CharacterPropertiesContractRegistry> CharacterPropertiesContractRegistry { get; set; } = new List<CharacterPropertiesContractRegistry>();

    public virtual ICollection<CharacterPropertiesFillCompBook> CharacterPropertiesFillCompBook { get; set; } = new List<CharacterPropertiesFillCompBook>();

    public virtual ICollection<CharacterPropertiesFriendList> CharacterPropertiesFriendList { get; set; } = new List<CharacterPropertiesFriendList>();

    public virtual ICollection<CharacterPropertiesQuestRegistry> CharacterPropertiesQuestRegistry { get; set; } = new List<CharacterPropertiesQuestRegistry>();

    public virtual ICollection<CharacterPropertiesShortcutBar> CharacterPropertiesShortcutBar { get; set; } = new List<CharacterPropertiesShortcutBar>();

    public virtual ICollection<CharacterPropertiesSpellBar> CharacterPropertiesSpellBar { get; set; } = new List<CharacterPropertiesSpellBar>();

    public virtual ICollection<CharacterPropertiesSquelch> CharacterPropertiesSquelch { get; set; } = new List<CharacterPropertiesSquelch>();

    public virtual ICollection<CharacterPropertiesTitleBook> CharacterPropertiesTitleBook { get; set; } = new List<CharacterPropertiesTitleBook>();
}
