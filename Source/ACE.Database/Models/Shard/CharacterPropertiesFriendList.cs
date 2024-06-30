using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// FriendList Properties of Weenies
/// </summary>
public partial class CharacterPropertiesFriendList
{
    /// <summary>
    /// Id of the character this property belongs to
    /// </summary>
    public uint CharacterId { get; set; }

    /// <summary>
    /// Id of Friend
    /// </summary>
    public uint FriendId { get; set; }

    public virtual Character Character { get; set; }
}
