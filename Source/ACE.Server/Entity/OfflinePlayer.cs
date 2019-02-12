using System;
using System.Collections.Generic;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
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

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        public OfflinePlayer(Biota biota)
        {
            Biota = biota;
            Guid = new ObjectGuid(Biota.Id);

            InitializePropertyDictionaries();
        }

        private void InitializePropertyDictionaries()
        {
            foreach (var x in Biota.BiotaPropertiesBool)
                biotaPropertyBools[(PropertyBool)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesDID)
                biotaPropertyDataIds[(PropertyDataId)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesFloat)
                biotaPropertyFloats[(PropertyFloat)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesIID)
                biotaPropertyInstanceIds[(PropertyInstanceId)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesInt)
                biotaPropertyInts[(PropertyInt)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesInt64)
                biotaPropertyInt64s[(PropertyInt64)x.Type] = x;
            foreach (var x in Biota.BiotaPropertiesString)
                biotaPropertyStrings[(PropertyString)x.Type] = x;
        }


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


        // These dictionaries should ONLY be referenced by SetEphemeralValues, GetProperty, SetProperty and RemoveProperty functions.
        // They should NOT be accessed directly to get property values.
        private readonly Dictionary<PropertyBool, BiotaPropertiesBool> biotaPropertyBools = new Dictionary<PropertyBool, BiotaPropertiesBool>();
        private readonly Dictionary<PropertyDataId, BiotaPropertiesDID> biotaPropertyDataIds = new Dictionary<PropertyDataId, BiotaPropertiesDID>();
        private readonly Dictionary<PropertyFloat, BiotaPropertiesFloat> biotaPropertyFloats = new Dictionary<PropertyFloat, BiotaPropertiesFloat>();
        private readonly Dictionary<PropertyInstanceId, BiotaPropertiesIID> biotaPropertyInstanceIds = new Dictionary<PropertyInstanceId, BiotaPropertiesIID>();
        private readonly Dictionary<PropertyInt, BiotaPropertiesInt> biotaPropertyInts = new Dictionary<PropertyInt, BiotaPropertiesInt>();
        private readonly Dictionary<PropertyInt64, BiotaPropertiesInt64> biotaPropertyInt64s = new Dictionary<PropertyInt64, BiotaPropertiesInt64>();
        private readonly Dictionary<PropertyString, BiotaPropertiesString> biotaPropertyStrings = new Dictionary<PropertyString, BiotaPropertiesString>();

        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyBools.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public uint? GetProperty(PropertyDataId property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyDataIds.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public double? GetProperty(PropertyFloat property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyFloats.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public uint? GetProperty(PropertyInstanceId property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInstanceIds.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public int? GetProperty(PropertyInt property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInts.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public long? GetProperty(PropertyInt64 property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyInt64s.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        public string GetProperty(PropertyString property)
        {
            BiotaDatabaseLock.EnterReadLock();
            try
            {
                if (biotaPropertyStrings.TryGetValue(property, out var record))
                    return record.Value;
                return null;
            }
            finally
            {
                BiotaDatabaseLock.ExitReadLock();
            }
        }
        #endregion

        #region SetProperty Functions
        public void SetProperty(PropertyBool property, bool value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyBools, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyDataId property, uint value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyDataIds, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyFloat property, double value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyFloats, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInstanceId property, uint value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInstanceIds, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInt property, int value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInts, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyInt64 property, long value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyInt64s, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        public void SetProperty(PropertyString property, string value)
        {
            Biota.SetProperty(property, value, BiotaDatabaseLock, biotaPropertyStrings, out var biotaChanged);
            if (biotaChanged)
                ChangesDetected = true;
        }
        #endregion

        #region RemoveProperty Functions
        public void RemoveProperty(PropertyBool property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyBools))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyDataId property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyDataIds))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyFloat property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyFloats))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyInstanceId property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInstanceIds))
                ChangesDetected = true;
        }

        public void RemoveProperty(PropertyInt property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInts))
                ChangesDetected = true;
        }

        public void RemoveProperty(PropertyInt64 property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyInt64s))
                ChangesDetected = true;
        }
        public void RemoveProperty(PropertyString property)
        {
            if (Biota.TryRemoveProperty(property, BiotaDatabaseLock, biotaPropertyStrings))
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
    }
}
