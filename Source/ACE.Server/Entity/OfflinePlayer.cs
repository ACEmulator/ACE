using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Entity
{
    public class OfflinePlayer
    {
        /// <summary>
        /// This is object property overrides that should have come from the shard db (or init to defaults of object is new to this instance).
        /// You should not manipulate these values directly. To manipulate this use the exposed SetProperty and RemoveProperty functions instead.
        /// </summary>
        public Biota Biota { get; }

        /// <summary>
        /// This is just a wrapper around Biota.Id
        /// </summary>
        public ObjectGuid Guid => new ObjectGuid(Biota.Id);

        /// <summary>
        /// Restore a WorldObject from the database.
        /// Any properties tagged as Ephemeral will be removed from the biota.
        /// </summary>
        public OfflinePlayer(Biota biota)
        {
            Biota = biota;
        }

        public readonly ReaderWriterLockSlim BiotaDatabaseLock = new ReaderWriterLockSlim();

        #region GetProperty Functions
        public bool? GetProperty(PropertyBool property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public uint? GetProperty(PropertyDataId property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public double? GetProperty(PropertyFloat property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public uint? GetProperty(PropertyInstanceId property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public int? GetProperty(PropertyInt property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public long? GetProperty(PropertyInt64 property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        public string GetProperty(PropertyString property) { return Biota.GetProperty(property, BiotaDatabaseLock); }
        #endregion

        public uint? Monarch => GetProperty(PropertyInstanceId.Monarch);

        public uint? Patron => GetProperty(PropertyInstanceId.Patron);
    }
}
