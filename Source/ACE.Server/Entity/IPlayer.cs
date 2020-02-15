using System;

using ACE.Database.Models.Auth;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// This interface is used by Player and OfflinePlayer.
    /// It allows us to maintain two separate lists for online players (Player) and offline players (OfflinePlayer) in PlayerManager and return generic IPlayer results.
    /// </summary>
    public interface IPlayer
    {
        ObjectGuid Guid { get; }

        Account Account { get; }

        bool? GetProperty(PropertyBool property);
        uint? GetProperty(PropertyDataId property);
        double? GetProperty(PropertyFloat property);
        uint? GetProperty(PropertyInstanceId property);
        int? GetProperty(PropertyInt property);
        long? GetProperty(PropertyInt64 property);
        string GetProperty(PropertyString property);

        void SetProperty(PropertyBool property, bool value);
        void SetProperty(PropertyDataId property, uint value);
        void SetProperty(PropertyFloat property, double value);
        void SetProperty(PropertyInstanceId property, uint value);
        void SetProperty(PropertyInt property, int value);
        void SetProperty(PropertyInt64 property, long value);
        void SetProperty(PropertyString property, string value);

        void RemoveProperty(PropertyBool property);
        void RemoveProperty(PropertyDataId property);
        void RemoveProperty(PropertyFloat property);
        void RemoveProperty(PropertyInstanceId property);
        void RemoveProperty(PropertyInt property);
        void RemoveProperty(PropertyInt64 property);
        void RemoveProperty(PropertyString property);


        string Name { get; }

        int? Level { get; }

        int? Heritage { get; }

        int? Gender { get; }


        bool IsDeleted { get; }
        bool IsPendingDeletion { get; }


        uint? MonarchId { get; set; }

        uint? PatronId { get; set; }

        ulong AllegianceXPCached { get; set; }

        ulong AllegianceXPGenerated { get; set; }

        int? AllegianceRank { get; set; }

        int? AllegianceOfficerRank { get; set; }

        bool ExistedBeforeAllegianceXpChanges { get; set; }

        uint? HouseId { get; set; }

        uint? HouseInstance { get; set; }

        int? HousePurchaseTimestamp { get; set; }

        int? HouseRentTimestamp { get; set; }


        uint GetCurrentLoyalty();

        uint GetCurrentLeadership();


        Allegiance Allegiance { get; set; }

        AllegianceNode AllegianceNode { get; set; }

        /// <summary>
        /// This method forces a player to be immediately saved to the database
        /// It should only be called in critical sections that must guarantee
        /// lock-step with other players
        /// </summary>
        void SaveBiotaToDatabase(bool enqueueSave = true);

        void UpdateProperty(PropertyInstanceId prop, uint? value, bool broadcast = false);
    }
}
