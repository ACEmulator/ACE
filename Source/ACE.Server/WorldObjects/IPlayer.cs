using System;

using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// This interface is used by Player and OfflinePlayer.
    /// It allows us to maintain two separate lists for online players (Player) and offline players (OfflinePlayer) in PlayerManager and return generic IPlayer results.
    /// </summary>
    public interface IPlayer
    {
        ObjectGuid Guid { get; }


        string Name { get; }

        uint? Monarch { get; }

        uint? Patron { get; }
    }
}
