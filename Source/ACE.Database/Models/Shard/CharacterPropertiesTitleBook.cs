using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// TitleBook Properties of Weenies
/// </summary>
public partial class CharacterPropertiesTitleBook
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Id of Title
    /// </summary>
    public uint TitleId { get; set; }

    public virtual Character Character { get; set; }
}
