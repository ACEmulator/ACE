using System;

using ACE.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// This interface is used by Player and OfflinePlayer.
    /// It allows us to maintain two separate lists for online players (Player) and offline players (OfflinePlayer) in PlayerManager and return generic IPlayer results.
    /// </summary>
    public interface IPlayer
    {
        ObjectGuid Guid { get; }


        string Name { get; }

        int? Level { get; }

        uint? Monarch { get; set; }

        uint? Patron { get; set; }

        int? AllegianceCPPool { get; set; }


        uint GetCurrentLoyalty();

        uint GetCurrentLeadership();


        Allegiance Allegiance { get; set; }

        AllegianceNode AllegianceNode { get; set; }
    }
}
