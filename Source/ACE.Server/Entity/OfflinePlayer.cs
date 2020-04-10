using System;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class OfflinePlayer : IPlayer
    {
        /// <summary>
        /// This is object property overrides that should have come from the shard db (or init to defaults of object is new to this instance).
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Biota { get; }

        /// <summary>
        /// This is just a wrapper around Biota.Id
        /// </summary>
        public ObjectGuid Guid { get; }

        public Account Account { get; }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        public OfflinePlayer(Biota biota)
        {
            Biota = biota;
            Guid = new ObjectGuid(Biota.Id);

            var character = DatabaseManager.Shard.BaseDatabase.GetCharacterStubByGuid(Guid.Full);

            if (character != null)
                Account = DatabaseManager.Authentication.GetAccountById(character.AccountId);
        }

        public bool IsDeleted => DatabaseManager.Shard.BaseDatabase.GetCharacterStubByGuid(Guid.Full).IsDeleted;
        public bool IsPendingDeletion => DatabaseManager.Shard.BaseDatabase.GetCharacterStubByGuid(Guid.Full).DeleteTime > 0 && !IsDeleted;

        public DateTime LastRequestedDatabaseSave { get; protected set; }

        public bool ChangesDetected { get; set; }

        public readonly ReaderWriterLockSlim BiotaDatabaseLock = new ReaderWriterLockSlim();

        /// <summary>
        /// This will set the LastRequestedDatabaseSave to UtcNow and ChangesDetected to false.<para />
        /// If enqueueSave is set to true, DatabaseManager.Shard.SaveBiota() will be called for the biota.<para />
        /// Set enqueueSave to false if you want to perform all the normal routines for a save but not the actual save. This is useful if you're going to collect biotas in bulk for bulk saving.
        /// </summary>
        public void SaveBiotaToDatabase(bool enqueueSave = true)
        {
            LastRequestedDatabaseSave = DateTime.UtcNow;
            ChangesDetected = false;

            if (enqueueSave)
                DatabaseManager.Shard.SaveBiota(Biota, BiotaDatabaseLock, null);
        }


        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public uint? GetProperty(PropertyDataId property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public double? GetProperty(PropertyFloat property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public uint? GetProperty(PropertyInstanceId property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public int? GetProperty(PropertyInt property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public long? GetProperty(PropertyInt64 property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        public string GetProperty(PropertyString property)
        {
            return Biota.GetProperty(property, BiotaDatabaseLock);
        }
        #endregion

        #region SetProperty Functions
        public void SetProperty(PropertyBool property, bool value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyDataId property, uint value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyFloat property, double value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInstanceId property, uint value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInt property, int value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInt64 property, long value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyString property, string value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, out var changed);
            if (changed)
                ChangesDetected = true;
        }
        #endregion

        #region RemoveProperty Functions
        public void RemoveProperty(PropertyBool property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyDataId property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyFloat property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInstanceId property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInt property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInt64 property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyString property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock))
                ChangesDetected = true;
        }
        #endregion


        public string Name => GetProperty(PropertyString.Name);

        public int? Level => GetProperty(PropertyInt.Level);

        public int? Heritage => GetProperty(PropertyInt.HeritageGroup);

        public int? Gender => GetProperty(PropertyInt.Gender);


        public uint? MonarchId
        {
            get => GetProperty(PropertyInstanceId.Monarch);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Monarch); else SetProperty(PropertyInstanceId.Monarch, value.Value); }
        }

        public uint? PatronId
        {
            get => GetProperty(PropertyInstanceId.Patron);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Patron); else SetProperty(PropertyInstanceId.Patron, value.Value); }
        }

        public ulong AllegianceXPCached
        {
            get => (ulong)(GetProperty(PropertyInt64.AllegianceXPCached) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt64.AllegianceXPCached); else SetProperty(PropertyInt64.AllegianceXPCached, (long)value); }
        }

        public ulong AllegianceXPGenerated
        {
            get => (ulong)(GetProperty(PropertyInt64.AllegianceXPGenerated) ?? 0);
            set { if (value == 0) RemoveProperty(PropertyInt64.AllegianceXPGenerated); else SetProperty(PropertyInt64.AllegianceXPGenerated, (long)value); }
        }

        public int? AllegianceRank
        {
            get => GetProperty(PropertyInt.AllegianceRank);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceRank); else SetProperty(PropertyInt.AllegianceRank, value.Value); }
        }

        public int? AllegianceOfficerRank
        {
            get => GetProperty(PropertyInt.AllegianceOfficerRank);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.AllegianceOfficerRank); else SetProperty(PropertyInt.AllegianceOfficerRank, value.Value); }
        }

        /// <summary>
        /// This flag indicates if a player can pass up allegiance XP
        /// </summary>
        public bool ExistedBeforeAllegianceXpChanges
        {
            get => GetProperty(PropertyBool.ExistedBeforeAllegianceXpChanges) ?? true;
            set { if (value) RemoveProperty(PropertyBool.ExistedBeforeAllegianceXpChanges); else SetProperty(PropertyBool.ExistedBeforeAllegianceXpChanges, value); }
        }

        /// <summary>
        /// Used for allegiance recall to monarch's mansion / villa
        /// </summary>
        public uint? HouseInstance
        {
            get => GetProperty(PropertyInstanceId.House);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.House); else SetProperty(PropertyInstanceId.House, value.Value); }
        }

        public int? HousePurchaseTimestamp
        {
            get => GetProperty(PropertyInt.HousePurchaseTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HousePurchaseTimestamp); else SetProperty(PropertyInt.HousePurchaseTimestamp, value.Value); }
        }

        public int? HouseRentTimestamp
        {
            get => GetProperty(PropertyInt.HouseRentTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.HouseRentTimestamp); else SetProperty(PropertyInt.HouseRentTimestamp, value.Value); }
        }

        public uint? HouseId
        {
            get => GetProperty(PropertyDataId.HouseId);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.HouseId); else SetProperty(PropertyDataId.HouseId, value.Value); }
        }


        public uint GetCurrentLoyalty()
        {
            return (uint?)GetProperty(PropertyInt.CurrentLoyaltyAtLastLogoff) ?? 0;
        }

        public uint GetCurrentLeadership()
        {
            return (uint?)GetProperty(PropertyInt.CurrentLeadershipAtLastLogoff) ?? 0;
        }


        public Allegiance Allegiance { get; set; }

        public AllegianceNode AllegianceNode { get; set; }

        public void UpdateProperty(PropertyInstanceId prop, uint? value, bool broadcast = false)
        {
            if (value != null)
                SetProperty(prop, value.Value);
            else
                RemoveProperty(prop);
        }
    }
}
